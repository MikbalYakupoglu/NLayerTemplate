using System.Security.Claims;
using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Entity.Temp;
using Core.Results;
using Core.Utils;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserRoleDal _userRoleDal;


        public UserService(IUserDal userDal, IUserRoleDal userRoleDal)
        {
            _userDal = userDal;
            _userRoleDal = userRoleDal;
        }

        public async Task<IDataResult<User>> GetByIdAsync(Guid id)
        {
            var user = await _userDal.GetAsync(u => u.Id == id);

            if (user == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);

            return new SuccessDataResult<User>(user);
        }
        public async Task<IDataResult<User>> GetByEmailAsync(string email)
        {
            var user = await _userDal.GetAsync(u => u.Login == email);

            if (user == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);

            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<IEnumerable<User>>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userDal.GetAllAsync(null,page,size);

            return new SuccessDataResult<IEnumerable<User>>(users);       
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var result = BusinessRules.Run(
                await CheckIfUserExistInDbAsync(id)
            );

            if (!result.Success)
                return new ErrorResult(result.Message);

            var userToDelete = await _userDal.GetAsync(u => id == u.Id);

            await _userDal.DeleteAsync(userToDelete);
            return new SuccessResult(Messages.RemoveSuccess);
        }

        private int GetLoginedUserId()
        {
            var loginedUserId = LoginedUser.ClaimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Name);
            return int.Parse(loginedUserId);
        }

        private async Task<IResult >CheckIfUserExistInDbAsync(Guid userId)
        {
            var user = await _userDal.GetAsync(u => u.Id == userId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            return new SuccessResult();
        }
    }
}
