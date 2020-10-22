using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Identity
{
    public class User : IdentityUser
    {
        public DateTime dateBirth { get; set; }

        public DateTime dateRegistration { get; set; }

        public byte[] ProfileImage { get; set; }

        public string Name { get; set; }
        public string Family { get; set; }

        public User()
        {
            dateRegistration = DateTime.Now;
        }
    }
}
