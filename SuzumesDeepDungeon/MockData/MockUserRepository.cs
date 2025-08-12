using SuzumesDeepDungeon.Models;

namespace SuzumesDeepDungeon.MockData
{
    public static class MockUserRepository
    {
        public static List<User> Users = new List<User>
    {
        new User { Id = 1, Username = "admin", Password = "admin123", IsAdmin = true },
        new User { Id = 2, Username = "user", Password = "user123", IsAdmin = false }
    };

        public static User? GetUser(string username, string password)
        {
            return Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);
        }
    }
}
