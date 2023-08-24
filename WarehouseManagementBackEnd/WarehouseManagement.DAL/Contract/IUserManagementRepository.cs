using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagement.DAL.Contract
{
    public interface IUserManagementRepository
    {
        public List<IdentityUser> GetAll();
        public IdentityUser GetById(string Id);
        public void EditUser(IdentityUser user);
        int CountRecordsByPredicate(Expression<Func<IdentityUser, bool>> predicate);
        IQueryable<IdentityUser> FindByPredicate(Expression<Func<IdentityUser, bool>> predicate);
    }
}
