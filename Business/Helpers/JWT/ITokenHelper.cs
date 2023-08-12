using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.JWT
{
    public interface ITokenHelper
    {
        Token CreateToken(User user);
    }
}
