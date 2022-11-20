using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class CaseService : ICaseService
    {
        private readonly DataContext _context;

        public CaseService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Case CaseCreate, string userId)
        {
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;   
            var date = DateTime.UtcNow.AddHours(3);
            Case _case = new Case();
            _case.officeId = CaseCreate.officeId;
            _case.Name = CaseCreate.Name;
            _case.CreatedBy = currentUserId;
            _case.CreatedDate = date;
            _case.UpdatedBy = currentUserId;
            _case.UpdatedDate = date;
            _case.IsEnable = true;

            await _context.Cases.AddAsync(_case);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<Office>> GetAllOfficeAsync()
        {
            var list = await _context.Offices.Where(x => x.IsEnable == true).ToListAsync();
            return list;
        }
        public async Task<List<Case>> GetAllAsync()
        {
            var list = from c in _context.Cases
                       join o in _context.Offices on c.officeId equals o.Id
                       where c.IsEnable == true
                       orderby c.UpdatedDate descending
                       select new Case
                       {
                           Id = c.Id,
                           Name = c.Name,
                           officeId = c.officeId,
                           IsEnable = c.IsEnable,
                           CreatedBy = c.CreatedBy,
                           CreatedDate = c.CreatedDate,
                           UpdatedBy = c.UpdatedBy,
                           UpdatedDate = c.UpdatedDate,
                           OfficeName = o.Name
                       };

            return await list.ToListAsync();
        }

        public async Task<Case> GetByIdAsync(int id)
        {
            var list = await _context.Cases.Where(x => x.Id == id && x.IsEnable == true).FirstOrDefaultAsync();
            return list;
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {
            var _case = _context.Cases.Find(id);
            if (_case != null)
            {
                var date = DateTime.UtcNow;
                _case.IsEnable = false;
                _case.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                _case.UpdatedDate = date;

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateAsync(Case CaseUpdate, string userId)
        {
            var date = DateTime.UtcNow.AddHours(3);
            var _case = _context.Cases.Find(CaseUpdate.Id);
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            _case.officeId = CaseUpdate.officeId;
            _case.Name = CaseUpdate.Name;
            _case.UpdatedBy = user.Id;
            _case.UpdatedDate = date;
            _case.IsEnable = true;
            return await _context.SaveChangesAsync();
        }
    }
}
