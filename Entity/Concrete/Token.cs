using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Token
    {
        public string token { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
