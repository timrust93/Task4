using Task4.Db;
using Task4.Library;
using Task4.Models;

namespace Task4.Services
{
    public class RegistrationServices
    {
        private readonly AppDbContext _dbContext;
        public RegistrationServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? FindRegisteredUser(string email)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User? FindRegisteredUser(string email, string password)
        {
            User? user = _dbContext.Users.Where(x => x.Email == email)
                .Where(x => x.Password == Hasher.ToSHA3256String(password))
                .SingleOrDefault();
            return user;
        }

        public bool RegisterUser(UserRegisterModel model, out string errorMessage)
        {
            if (FindRegisteredUser(model.Login) != null)
            {
                errorMessage = "Provided email is occupied";
                return false;
            }

            User user = new User();
            user.LastSeen = DateTime.Now;
            user.Name = model.Name;
            user.Password = Hasher.ToSHA3256String(model.Password);
            user.Email = model.Login;
            user.IsBlocked = false;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            errorMessage = string.Empty;
            return true;
        }

        public void UpdateLoginDate(User user)
        {
            user.LastSeen = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}
