using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;

namespace Entity.Concrete
{
    public class User : IEntity
    {
        public User()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public bool IsDeleted { get; set; }
        public Guid Id { get; }
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
