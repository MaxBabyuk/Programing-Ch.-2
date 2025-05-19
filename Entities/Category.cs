class Category : Entity
{
  public string Name { get; private set; }
  public Category(int id, string name) : base(id)
  {
    Name = name;
  }

  public override string Stringify()
  {
    return $"Category ({Id} {Name})";
  }
}
