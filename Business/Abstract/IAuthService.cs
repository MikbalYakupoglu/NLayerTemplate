using Core.Results;
using Entity.Concrete;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<Token> Register(UserForRegisterDto userToRegisterDto, User newUser);
        IDataResult<Token> Login(UserForLoginDto userToLoginDto, out User userInDb);
    }
}
