using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Context;

namespace WarehouseManagement.DAL.Implementation
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly WarehouseManagementDbContext _context;
        public UserManagementRepository(WarehouseManagementDbContext context)
        {
            _context = context;
        }

        public List<IdentityUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public IdentityUser GetById(string? Id)
        {
            return _context.Users.FirstOrDefault(m => m.Id == Id);
        }

        public void EditUser(IdentityUser user)
        {
            _context.Users.Update(user);
        }

        public int CountRecordsByPredicate(Expression<Func<IdentityUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).Count();
        }

        public IQueryable<IdentityUser> FindByPredicate(Expression<Func<IdentityUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).AsQueryable();
        }
    }
}
