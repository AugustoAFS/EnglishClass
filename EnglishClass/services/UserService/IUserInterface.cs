using EnglishClass.Models;

namespace EnglishClass.services.UserService
{
    public interface IUserInterface
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int Userid);
        Task<IEnumerable<User>> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(int Userid);
        Task<string> Login(string username, string password);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
