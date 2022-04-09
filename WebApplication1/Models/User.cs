using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public DateTimeOffset LastLoginDate { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
    }

}