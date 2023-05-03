namespace miranaSolution.Dtos.Auth.Users
{
    public class UserAuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}