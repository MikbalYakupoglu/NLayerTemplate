using Core.DataAccess;
using Core.DataAccess.Extensions;
using Core.Entity;
using DataAccess.Abstract;
using Entity.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User, LoginSampleContext> , IUserDal
    {
        public override async Task DeleteAsync(User user)
        {
            using (LoginSampleContext context = new LoginSampleContext())
            {
                user.IsDeleted = true;

                var userToDelete = context.Entry(user);
                userToDelete.State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
        public override async Task<User?> GetAsync(Expression<Func<User, bool>> filter)
        {
            await using (LoginSampleContext context = new LoginSampleContext())
            {
                return await context.Users
                    .Include(u=> u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(filter);
            }
        }

        public override async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null, int page = 0, int size = 25)
        {
            await using (LoginSampleContext context = new LoginSampleContext())
            {
                var users = context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsSplitQuery();

                if (!users.Any())
                    return Enumerable.Empty<User>();

                return filter == null
                    ? await users.ToPaginateAsync(page,size)
                    : await users.Where(filter).ToPaginateAsync(page, size);
            }
        }
    }
}
