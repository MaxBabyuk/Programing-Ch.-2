using System.Collections.Generic;
using System.Linq;
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
}
