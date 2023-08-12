using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Role : IEntity
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; private set; }
        public string Name { get; set; }
    }
}
