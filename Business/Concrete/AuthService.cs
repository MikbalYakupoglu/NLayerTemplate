using Business.Abstract;
using Business.Helpers.JWT;
using Business.Utils;
using Business.Validation;
using Core.Results;
using Core.Utils;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserDal _userDal;
        private readonly IRoleDal _roleDal;
        private readonly IUserRoleDal _userRoleDal;
        private readonly UserForRegisterValidator _validator;

        public AuthService(ITokenHelper tokenHelper, IUserDal userDal, IRoleDal roleDal, IUserRoleDal userRoleDal,  UserForRegisterValidator validator)
        {
            _tokenHelper = tokenHelper;
            _userDal = userDal;
            _roleDal = roleDal;
            _userRoleDal = userRoleDal;
            _validator = validator;
        }

        public IDataResult<Token> Login(UserForLoginDto userForLoginDto, out User userInDb)
        {
            var result = BusinessRules.Run(
             CheckIfLoginNull(userForLoginDto.Login),
             CheckIfPasswordNull(userForLoginDto.Password)
             );

            if (!result.Success)
            {
                userInDb = null;
                return new ErrorDataResult<Token>(result.Message);
            }

            var _userInDb = _userDal.GetAsync(u => u.Login == userForLoginDto.Login).Result;
            if (_userInDb == null)
            {
                userInDb = null;
                return new ErrorDataResult<Token>(Messages.UserNotFound);
            }

            var token = _tokenHelper.CreateToken(_userInDb);

            userInDb = _userInDb;
            return new SuccessDataResult<Token>(token);
        }

        public IDataResult<Token> Register(UserForRegisterDto userForRegisterDto, User newUser)
        {
            var result = BusinessRules.Run(
                CheckIfLoginNull(userForRegisterDto.Login),
                CheckIfPasswordNull(userForRegisterDto.Password),
                CheckIfUserMailAlreadyExist(userForRegisterDto.Login)
                );

            if (!result.Success)
                return new ErrorDataResult<Token>(result.Message);

            ValidationResult validationResult = _validator.Validate(userForRegisterDto);

            if (!validationResult.IsValid)
                return new ErrorDataResult<Token>(validationResult.Errors.FirstOrDefault().ErrorMessage);

            _userDal.CreateAsync(newUser);
            // CreateAsync Reader role.
            _userRoleDal.CreateAsync(new UserRole
            {
                User = newUser,
                Role = _roleDal.GetAsync(r => r.Name == AuthorizationRoles.User).Result

            });

            var token = _tokenHelper.CreateToken(newUser);
            return new SuccessDataResult<Token>(token, Messages.RegisterSuccess);
        }

        #region Helpers

        private IResult CheckIfLoginNull(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return new ErrorResult(Messages.EmailCannotBeNull);

            return new SuccessResult();
        }

        private IResult CheckIfPasswordNull(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return new ErrorResult(Messages.PasswordCannotBeNull);

            return new SuccessResult();
        }

        private IResult CheckIfUserMailAlreadyExist(string login)
        {
            var user = _userDal.GetAsync(u => u.Login == login).Result;
            if (user != null)
                return new ErrorResult(Messages.UserAlreadyExists);

            return new SuccessResult();
        }
        #endregion
    }
}
