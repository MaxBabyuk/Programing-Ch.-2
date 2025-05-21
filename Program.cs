var userRepository = new UserRepository();

var categoryRepository = new CategoryRepository();
var bookRepository = new BookRepository();
var borrowRepository = new BorrowRepository();
userRepository.Create(new User(1, "admin", "admin", true));
userRepository.Create(new User(2, "user", "user", false));

foreach (var user in userRepository.SortLogin())
{
  Console.WriteLine(user.Stringify());
}

userRepository.Delete(1);
userRepository.Update(new User(1, "admin", "admin", true));
userRepository.Create(new User(3, "user", "user", false));
userRepository.Create(new User(4, "a_bit_admin", "P@$$w0rd", false));

Console.WriteLine("After update:");
foreach (var user in userRepository.SortId())
{
  Console.WriteLine(user.Stringify());
}

Console.WriteLine("Admins:");
foreach (var user in userRepository.ListAdmins())
{
  Console.WriteLine(user.Stringify());
}
