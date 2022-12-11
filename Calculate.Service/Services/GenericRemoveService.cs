using Calculate.Data;
using Calculate.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class GenericRemoveService : IGenericRemoveService
    {
        private readonly DataContext _context;

        public GenericRemoveService(DataContext context)
        {
            _context = context;
        }
        public async Task<int> RemoveAsync(int id, int isMaster, string userId)
        {
            int result = 0;
            var trans = _context.Database.BeginTransaction();
            try
            {
                int _userId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                var _date = DateTime.UtcNow.AddHours(3);
                if (isMaster == (int)EnumIsMaster.OFFICE)
                {
                    var office = _context.Offices.Find(id);
                    if (office != null)
                    {
                        var date = _date;
                        office.IsEnable = false;
                        office.UpdatedBy = _userId;
                        office.UpdatedDate = date;
                    }

                    var caseList = await _context.Cases.Where(x => x.officeId == id).ToListAsync();

                    if (caseList != null)
                    {
                        foreach (var item in caseList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var accountList = await _context.Accounts.Where(x => caseList.Select(y => y.Id).Contains(x.CaseId)).ToListAsync();

                    if (accountList != null)
                    {
                        foreach (var item in accountList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var accountDetailList = await _context.AccountDetails.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (accountDetailList != null)
                    {
                        foreach (var item in accountDetailList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var operationList = await _context.Operations.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (operationList != null)
                    {
                        foreach (var item in operationList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    result = await _context.SaveChangesAsync();
                    trans.Commit();
                }
                else if(isMaster == (int)EnumIsMaster.CASE){
                    var caseList = await _context.Cases.Where(x => x.Id == id).ToListAsync();

                    if (caseList != null)
                    {
                        foreach (var item in caseList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var accountList = await _context.Accounts.Where(x => caseList.Select(y => y.Id).Contains(x.CaseId)).ToListAsync();

                    if (accountList != null)
                    {
                        foreach (var item in accountList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var accountDetailList = await _context.AccountDetails.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (accountDetailList != null)
                    {
                        foreach (var item in accountDetailList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var operationList = await _context.Operations.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (operationList != null)
                    {
                        foreach (var item in operationList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    result = await _context.SaveChangesAsync();
                    trans.Commit();
                }
                else if (isMaster == (int)EnumIsMaster.ACCOUNT)
                {
                    var accountList = await _context.Accounts.Where(x => x.Id == id).ToListAsync();

                    if (accountList != null)
                    {
                        foreach (var item in accountList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var accountDetailList = await _context.AccountDetails.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (accountDetailList != null)
                    {
                        foreach (var item in accountDetailList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var operationList = await _context.Operations.Where(x => accountList.Select(y => y.Id).Contains(x.AccountId)).ToListAsync();

                    if (operationList != null)
                    {
                        foreach (var item in operationList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    result = await _context.SaveChangesAsync();
                    trans.Commit();
                }
                else if (isMaster == (int)EnumIsMaster.ACCOUNTDETAIL)
                {
                    var accountDetailList = await _context.AccountDetails.Where(x => x.Id == id).ToListAsync();

                    if (accountDetailList != null)
                    {
                        foreach (var item in accountDetailList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    var operationList = await _context.Operations.Where(x => accountDetailList.Select(y => y.Id).Contains(x.AccountDetailId)).ToListAsync();

                    if (operationList != null)
                    {
                        foreach (var item in operationList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    result = await _context.SaveChangesAsync();
                    trans.Commit();
                }
                else if (isMaster == (int)EnumIsMaster.OPERATION)
                {
                    var operationList = await _context.Operations.Where(x => x.Id == id).ToListAsync();

                    if (operationList != null)
                    {
                        foreach (var item in operationList)
                        {
                            var date = _date;
                            item.IsEnable = false;
                            item.UpdatedBy = _userId;
                            item.UpdatedDate = date;
                        }
                    }

                    result = await _context.SaveChangesAsync();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                result = 0;
            }


            return result;
        }
    }
}
