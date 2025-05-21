class  CategoryRepository : Repository<Category>
{
    public List<Category> SortName()
    {
        return ReadAll().OrderBy(category => category.Name).ToList();
    }
}
