using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Enums;

namespace WinFormsApp1.Models
{
    public class User
    {
    
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public UserStatus UserStatus { get; set; }
        public UserRole UserRole { get; set; }

        public User(string name,string surname,string email,string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            UserStatus = UserStatus.Active;
        }
        public User(
                string name,
                string surname,
                string email,
                string password,
                UserRole role = UserRole.Customer,
                UserStatus status = UserStatus.Active)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            UserRole = role;
            UserStatus = status;
        }

        public override string ToString()
        {
            return Name + " " + Surname + " " + Email;
        }

    }
}
