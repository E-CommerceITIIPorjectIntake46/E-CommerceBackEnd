using E_Commerce.Common;
using E_Commerce.Data;
using E_Commerce.Logic;
using E_Commerce.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IErrorMapper _errorMapper;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IOptions<JWTSettings> jwtSettings,
                              IValidator<RegisterDTO> registerValidator,
                              IErrorMapper errorMapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _registerValidator = registerValidator;
            _errorMapper = errorMapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<GeneralResult>> Register(RegisterDTO registerDTO)
        {
            var result = await _registerValidator.ValidateAsync(registerDTO);
            if(!result.IsValid)
            {
                var errors = _errorMapper.MapError(result);
                return GeneralResult.FailResult(errors, "Validation Failed");
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName
            };

            IdentityResult regResult = await _userManager.CreateAsync(user, registerDTO.Password);
            if(!regResult.Succeeded)
            {
                return GeneralResult.FailResult("Register Failed");
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (!roleResult.Succeeded)
            {
                return GeneralResult.FailResult("Adding Role Failed");
            }

            return GeneralResult.SuccessResult("Register Success");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<GenericGeneralResult<TokenDTO>>> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if(user is null) 
            {
                return GenericGeneralResult<TokenDTO>.FailResult("Invalid Email or Password");
            }

            var result = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);
            if(!result)
            {
                return GenericGeneralResult<TokenDTO>.FailResult("Invalid Email or Password");
            }
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(ClaimTypes.Email, user.Email!));

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDTO = GenerateToken(claims);
            return GenericGeneralResult<TokenDTO>.SuccessResult(tokenDTO);
        }
        private TokenDTO GenerateToken(List<Claim> claims)
        {
            var secertKey = _jwtSettings.SecretKey;
            var secertKeyInBytes = Convert.FromBase64String(secertKey);
            var key = new SymmetricSecurityKey(secertKeyInBytes);
            var signInCredintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDate = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            var jwt = new JwtSecurityToken
                (
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    signingCredentials: signInCredintials,
                    expires: expiryDate
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenDTO = new TokenDTO(token, _jwtSettings.DurationInMinutes);
            return tokenDTO;
        }
    }
}
