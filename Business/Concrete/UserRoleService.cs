using Business.Abstract;
using Business.Utils;
using Core.Results;
using Core.Utils;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using System.Diagnostics.Metrics;

namespace Business.Concrete
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleDal _userRoleDal;
        private readonly IRoleDal _roleDal;

        public UserRoleService(IUserRoleDal userRoleDal, IRoleDal roleDal)
        {
            _userRoleDal = userRoleDal;
            _roleDal = roleDal;
        }

        // Kullanıcıda Zaten Eklenmek İstenen Rol Bulunmuyorsa ekle.
        public async Task<IResult> AddRoleToUserAsync(Guid userId, List<Guid> roleIdsToAdd)
        {
            var result = BusinessRules.Run(
                    await CheckIfRolesExistAsync(roleIdsToAdd)
                );

            if (!result.Success)
                return new ErrorResult(result.Message);

            int addedRoleCount = await AddUserRolesAsync(userId, roleIdsToAdd);

            if (addedRoleCount == 0)
                return new SuccessResult(Messages.UserRoleNotModified);

            return new SuccessResult(Messages.UserRoleUpdateSuccess);
        }

        private async Task<int> AddUserRolesAsync(Guid userId, List<Guid> roleIdsToAdd)
        {
            var userRoles = await _userRoleDal.GetAllAsync(ur => ur.UserId == userId);
            var userRolesIds = userRoles.Select(ur => ur.RoleId).ToList();
            int addedRoleCount = 0;

            foreach (var roleToAdd in roleIdsToAdd)
            {
                if (!userRolesIds.Contains(roleToAdd))
                {
                    await _userRoleDal.CreateAsync(new UserRole
                    {
                        UserId = userId,
                        RoleId = roleToAdd
                    });
                    addedRoleCount++;
                }
            }

            return addedRoleCount;
        }

        // Kullanıcıda Silinmek İstenen Rol Bulunuyorsa Sil.
        public async Task<IResult> RemoveRoleFromUserAsync(Guid userId, List<Guid> roleIdsToRemove)
        {
            int deletedRoleCount = await RemoveUserRolesAsync(userId, roleIdsToRemove);

            if (deletedRoleCount == 0)
                return new SuccessResult(Messages.UserRoleNotModified);

            return new SuccessResult(Messages.UserRoleUpdateSuccess);
        }

        private async Task<int> RemoveUserRolesAsync(Guid userId, List<Guid> roleIdsToRemove)
        {
            var userRoles = await _userRoleDal.GetAllAsync(ur => ur.UserId == userId);
            var userRolesIds = userRoles.Select(ur => ur.RoleId).ToList();
            int deletedRoleCount = 0;
            foreach (var roleToDelete in roleIdsToRemove)
            {
                if (userRolesIds.Contains(roleToDelete))
                {
                    var userRoleToDelete = await _userRoleDal.GetAsync(ur => ur.UserId == userId && ur.RoleId == roleToDelete);
                    await _userRoleDal.DeleteAsync(userRoleToDelete);
                    deletedRoleCount++;
                }
            }

            return deletedRoleCount;
        }

        private async Task<IResult> CheckIfRolesExistAsync(List<Guid> roleIdsToAdd)
        {
            var roles = await _roleDal.GetAllAsync(null);
            var roleIds = roles.Select(r => r.Id);
            var isRolesCorrect = roleIdsToAdd.All(id => roleIds.Contains(id));

            if (!isRolesCorrect)
                return new ErrorResult(Messages.RoleNotFound);

            return new SuccessResult();
        }
    }
}
