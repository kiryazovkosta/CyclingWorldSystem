namespace Web.Models.Users;

public sealed record ChangeUserPasswordModel( 
    string UserName,   
    string OldPassword, 
    string NewPassword, 
    string ConfirmNewPassword);