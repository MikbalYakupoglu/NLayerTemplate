using Core.Results;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<IEnumerable<User>>> GetAllUsersAsync(int page, int size);
        Task<IDataResult<User>> GetByIdAsync(Guid id);
        Task<IDataResult<User>> GetByEmailAsync(string email);
        Task<IResult> DeleteAsync(Guid id);
    }
}
