using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(User userCreate, string userId)
        {
            int result = 0;
            string phoneNumber = userCreate.PhoneNumber;
            if (phoneNumber[0] == '0')
            {
                phoneNumber = userCreate.PhoneNumber.Substring(1);
            }

            var date = DateTime.UtcNow.AddHours(3);
            User _user = new User();
            _user.UserId = Guid.NewGuid().ToString();
            _user.AccessToken = Guid.NewGuid().ToString();
            _user.Name = userCreate.UserName;
            _user.RoleID = userCreate.RoleID;
            _user.IsEnabled = userCreate.IsEnabled;
            _user.officeIdList = userCreate.officeIdList;
            _user.Email = userCreate.UserName + "@hotmail.com";
            _user.LastSignedInAt = date;
            _user.AccessFailedCount = 0;
            _user.ConcurrencyStamp = Guid.NewGuid().ToString();
            _user.EmailConfirmed = false;
            _user.LockoutEnabled = true;
            _user.NormalizedEmail = (userCreate.UserName + "@hotmail.com").ToUpper();
            _user.NormalizedUserName = "0" + phoneNumber;
            _user.PasswordHash = userCreate.PasswordHash;
            _user.PhoneNumber = phoneNumber;
            _user.SecurityStamp = "SDASDA";
            _user.UserName = userCreate.UserName;
            _user.PhoneNumberConfirmed = false;
            _user.TwoFactorEnabled = false;

            await _context.Users.AddAsync(_user);

            result = await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<UserGet>> GetAllAsync()
        {
            var list = from u in _context.Users
                       join o in _context.Offices on u.officeIdList equals o.Id                   
                       orderby u.LastSignedInAt descending
                       select new UserGet
                       {
                           Id = u.Id,
                           UserId = u.UserId,
                           AccessToken = u.AccessToken,
                           Name = u.Name,
                           RoleID = u.RoleID,
                           IsEnabled = u.IsEnabled,
                           Email = u.Email,
                           PhoneNumber = u.PhoneNumber,
                           UserName = u.UserName,
                           PasswordHash = u.PasswordHash,
                           officeIdList = u.officeIdList,
                           OfficeName = o.Name
                       };
            return await list.ToListAsync();
        }

        public async Task<List<Office>> GetAllOfficeAsync()
        {
            var list = await _context.Offices.Where(x => x.IsEnable == true).ToListAsync();
            return list;
        }

        public async Task<UserGet> GetByIdAsync(int id)
        {
            var list = from u in _context.Users
                       join o in _context.Offices on u.officeIdList equals o.Id
                       where u.Id == id
                       orderby u.LastSignedInAt descending
                       select new UserGet
                       {
                           Id = u.Id,
                           UserId = u.UserId,
                           AccessToken = u.AccessToken,
                           Name = u.Name,
                           RoleID = u.RoleID,
                           IsEnabled = u.IsEnabled,
                           Email = u.Email,
                           PhoneNumber = u.PhoneNumber,
                           UserName = u.UserName,
                           PasswordHash = u.PasswordHash,
                           officeIdList = u.officeIdList,
                           OfficeName = o.Name
                       };
            return await list.FirstOrDefaultAsync();
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {
            var _user = _context.Users.Find(id);
            int result = 0;
            if (_user != null)
            {
                var date = DateTime.UtcNow;
                _user.IsEnabled = false;
               
                result = await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> UpdateAsync(User userUpdate, string userId)
        {
            int result = 0;
            string phoneNumber = userUpdate.PhoneNumber;
            if (phoneNumber[0] == '0')
            {
                phoneNumber = userUpdate.PhoneNumber.Substring(1);
            }
            var date = DateTime.UtcNow.AddHours(3);
            var _user = _context.Users.Find(userUpdate.Id);
            _user.Name = userUpdate.UserName;
            _user.RoleID = userUpdate.RoleID;
            _user.IsEnabled = userUpdate.IsEnabled;
            _user.officeIdList = userUpdate.officeIdList;
            _user.Email = userUpdate.UserName + "@hotmail.com";
            _user.LastSignedInAt = date;       
            _user.NormalizedEmail = (userUpdate.UserName + "@hotmail.com").ToUpper();
            _user.NormalizedUserName = "0" + phoneNumber;
            _user.PasswordHash = userUpdate.PasswordHash;
            _user.PhoneNumber = phoneNumber;         
            _user.UserName = userUpdate.UserName;
          
            result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
