using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Data
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }
}
