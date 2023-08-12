using Azure;
using Core.Results;
using Entity.Concrete;

namespace Business.Abstract;

public interface IRoleService
{
    Task<IResult> CreateAsync(Role role);
    Task<IResult> DeleteAsync(Guid roleId);
    Task<IResult> UpdateAsync(Guid roleId, Role newRole);
    Task<IDataResult<Role>> GetAsync(Guid roleId);
    Task<IDataResult<IEnumerable<Role>>> GetAllAsync();

}