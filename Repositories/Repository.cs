class Repository<T> : IRepository<T> where T : Entity
{
  private readonly List<T> _entities = new List<T>();
    public void Create(T entity)
    {
        _entities.Add(entity);
    }

    public T Read(int id)
    {
        return _entities.Find(entity => entity.Id == id)!;
    }

    public void Update(T entity)
    {
        _entities.RemoveAll(entity => entity.Id == entity.Id);
        _entities.Add(entity);
    }

    public void Delete(int id)
    {
        _entities.RemoveAll(entity => entity.Id == id);
    }

    public List<T> ReadAll()
    {
        return new List<T>(_entities);
    }
}
