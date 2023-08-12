using Core.DataAccess;
using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfUserRolesDal : EfEntityRepositoryBase<UserRole, LoginSampleContext>,IUserRoleDal
    {
        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            await using (var context = new LoginSampleContext())
            {
                var userWithRoles = await context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                List<string> userRoles = new List<string>();

                foreach (var roles in userWithRoles.UserRoles)
                {
                    userRoles.Add(roles.Role.Name);
                }

                return userRoles;
            }
        }
    }
}
