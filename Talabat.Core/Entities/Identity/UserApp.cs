using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class UserApp:IdentityUser
    {
        public string FName {  get; set; }
        public string LName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

    }
}
