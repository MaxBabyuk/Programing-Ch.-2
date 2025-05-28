using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace DBProject;

public class UserRepository : Repository<User>
{
    public List<User> SortLogin()
    {
        return ReadAll().OrderBy(user => user.Login).ToList();
    }

    public List<User> SortId()
    {
        return ReadAll().OrderBy(user => user.Id).ToList();
    }

    public List<User> ListAdmins()
    {
        return ReadAll().Where(user => user.IsAdmin).ToList();
    }

    public User? ReadByLogin(string login)
    {
        return ReadAll().FirstOrDefault(user => user.Login == login);
    }

    public bool Register(string login, string password, bool grantAdmin = false)
    {
        if (ReadByLogin(login) != null) return false;
        var id = ReadAll().Count == 0 ? 0 : ReadAll().Max(user => user.Id) + 1;
        var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        Create(new User(id, login, hash, grantAdmin));
        return true;
    }

    public User? LogIn(string login, string password)
    {
        return ReadAll().FirstOrDefault(user => user.Login == login &&
            BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password));
    }
}
