namespace miranaSolution.Dtos.Auth.Users;

public class UserUpdatePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirmation { get; set; }
}