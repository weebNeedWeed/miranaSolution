using System.ComponentModel.DataAnnotations;

namespace miranaSolution.Admin.Models.Auth;

public class LoginViewModel
{
    public string UserName { get; set; }

    [DataType(DataType.Password)] public string Password { get; set; }
}