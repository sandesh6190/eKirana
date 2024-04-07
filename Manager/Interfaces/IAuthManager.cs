namespace eKirana.Manager.Interfaces;
public interface IAuthManager
{
    Task LogIn(string Email, string PassWord);
    Task LogOut();
    Task Register(string UserName, string Email, string PassWord);
}
