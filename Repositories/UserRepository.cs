using Microsoft.EntityFrameworkCore;
using GUIDE.Models;

namespace GUIDE.Repositories
{
    public class UserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public async Task<Users> Get(string username, string password)
        {
            var user = await _context.Users
            .Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password)
            .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("Usuário ou senha inválidos");

            return user;
        }

        public async Task<Users> Post(string username, string password)
        {
            Users user = new Users();
            user.Username = username;
            user.Password = password;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }
    }
}