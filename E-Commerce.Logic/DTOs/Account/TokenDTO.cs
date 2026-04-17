namespace E_Commerce.Logic
{
    public record TokenDTO
    (
        string AccessToken,
        int DurationInMinutes,
        string TokenType = "Bearer"
    );
}
