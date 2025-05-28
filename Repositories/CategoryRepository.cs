using System.Collections.Generic;
using System.Linq;

namespace DBProject;

public class CategoryRepository : Repository<Category>
{
    public List<Category> SortName()
    {
        return ReadAll().OrderBy(category => category.Name).ToList();
    }
    
    public Category? ReadByName(string name)
    {
        return ReadAll().FirstOrDefault(category => category.Name == name);
    }
}
