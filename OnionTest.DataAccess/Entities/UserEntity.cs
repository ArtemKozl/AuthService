using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.DataAccess.Entites
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
