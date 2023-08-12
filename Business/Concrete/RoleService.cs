using Business.Abstract;
using Business.Utils;
using Core.Results;
using Core.Utils;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Data;

namespace Business.Concrete;

public class RoleService : IRoleService
{
    private readonly IRoleDal _roleDal;
    private readonly IUserRoleDal _userRoleDal;


    public RoleService(IRoleDal roleDal, IUserRoleDal userRoleDal)
    {
        _roleDal = roleDal;
        _userRoleDal = userRoleDal;
    }

    public async Task<IResult> CreateAsync(Role role)
    {
        var result = BusinessRules.Run(
            CheckIfInputsNull(role.Name),
            await CheckIfRoleAlreadyExistAsync(role.Name)
        );

        if (!result.Success)
            return new ErrorResult(result.Message);

        await _roleDal.CreateAsync(role);
        return new SuccessResult(Messages.RoleCreateSuccess);
    }

    public async Task<IResult> DeleteAsync(Guid roleId)
    {
        var result = BusinessRules.Run(
            await CheckIfRoleExistInDbForModifyAsync(roleId),
            await CheckIfRoleNotUsedByUser(roleId)
        );

        if (!result.Success)
            return new ErrorResult(result.Message);

        var role = await _roleDal.GetAsync(r => r.Id == roleId);
        await _roleDal.DeleteAsync(role);
        return new SuccessResult(Messages.RoleDeleteSuccess);
    }

    public async Task<IResult> UpdateAsync(Guid roleId, Role newRole)
    {
        var result = BusinessRules.Run(
            await CheckIfRoleExistInDbForModifyAsync(roleId)
        );

        if (!result.Success)
            return new ErrorResult(result.Message);

        var role = await _roleDal.GetAsync(r => r.Id == roleId);
        role.Name = newRole.Name;
        await _roleDal.UpdateAsync(role);
        return new SuccessResult(Messages.RoleUpdateSuccess);
    }

    public async Task<IDataResult<Role>> GetAsync(Guid roleId)
    {
        var role = await _roleDal.GetAsync(r => r.Id == roleId);

        if (role == null)
            return new ErrorDataResult<Role>(Messages.RoleNotFound);

        return new SuccessDataResult<Role>(role);
    }

    public async Task<IDataResult<IEnumerable<Role>>> GetAllAsync()
    {
        var roles = await _roleDal.GetAllAsync(null);

        return new SuccessDataResult<IEnumerable<Role>>(roles);
    }

    private async Task<IResult> CheckIfRoleAlreadyExistAsync(string roleName)
    {
        var role = await _roleDal.GetAsync(r => r.Name == roleName);

        if (role != null)
            return new ErrorResult(Messages.RoleAlreadyExist);

        return new SuccessResult();
    }

    private async Task<IResult> CheckIfRoleExistInDbForModifyAsync(Guid roleId)
    {
        var role = await _roleDal.GetAsync(r => r.Id == roleId);

        if (role == null)
            return new ErrorResult(Messages.RoleNotFound);

        return new SuccessResult();
    }

    private IResult CheckIfInputsNull(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return new ErrorResult(Messages.RoleNameCannotBeNull);

        return new SuccessResult();
    }

    private async Task<IResult> CheckIfRoleNotUsedByUser(Guid roleId)
    {
        var userRoles = await _userRoleDal.GetAllAsync(ur=> ur.RoleId == roleId);
        if (userRoles.Any())
            return new ErrorResult(Messages.RoleCannotBeDeleteWhileUsing);

        return new SuccessResult();
    }
}