interface IRepository<T>
{
  void Create(T entity);
  T Read(int id);
  void Update(T entity);
  void Delete(int id);
}
