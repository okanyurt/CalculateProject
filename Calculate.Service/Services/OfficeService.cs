using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly DataContext _context;

        public OfficeService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Office OfficeCreate, string userId)
        {
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            var date = DateTime.UtcNow.AddHours(3);
            Office office = new Office();
            office.Name = OfficeCreate.Name;
            office.CreatedBy = currentUserId;
            office.CreatedDate = date;
            office.UpdatedBy = currentUserId;
            office.UpdatedDate = date;
            office.IsEnable = true;

            await _context.Offices.AddAsync(office);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<Office>> GetAllAsync()
        {
            var list = await _context.Offices.Where(x => x.IsEnable == true).ToListAsync();
            return list;
        }

        public async Task<Office> GetByIdAsync(int id)
        {
            var list = await _context.Offices.Where(x => x.Id == id && x.IsEnable == true).FirstOrDefaultAsync();
            return list;
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {
            var office = _context.Offices.Find(id);
            if (office != null)
            {
                var date = DateTime.UtcNow;
                office.IsEnable = false;
                office.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                office.UpdatedDate = date;

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateAsync(Office OfficeUpdate, string userId)
        {
            var date = DateTime.UtcNow.AddHours(3);
            var office = _context.Offices.Find(OfficeUpdate.Id);
            office.Name = OfficeUpdate.Name;
            office.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            office.UpdatedDate = date;
            office.IsEnable = true;
            return await _context.SaveChangesAsync();
        }
    }
}
