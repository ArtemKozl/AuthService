using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.Core.Models
{
    public class Users
    {
        private Users(string userName, string email, string password, string barcode)
        {

            UserName = userName;
            Email = email;
            Password = password;
            Barcode = barcode;

        }
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int RoleId { get; set; }

        public static Users Create(string userName, string email, string password, string barcode)
        {
            var user = new Users(userName, email, password, barcode);

            return user;
        }
    }
}
