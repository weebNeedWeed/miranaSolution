namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserPasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string NewPasswordConfirmation);