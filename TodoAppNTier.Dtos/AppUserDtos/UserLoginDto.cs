namespace TodoAppNTier.Dtos.AppUserDtos
{
    public class UserLoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } // "Beni Hatırla" kutucuğu için
    }
}