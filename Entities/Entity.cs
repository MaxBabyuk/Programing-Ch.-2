public abstract class Entity
{
  public int Id { get; }
  
  public Entity(int id)
  {
    Id = id;
  }

  abstract public string Stringify();

  public virtual bool IsSame(object obj)
  {
    return obj is Entity entity && entity.Id == Id;
  }

}
