using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Context
{
    public class TalabatContextIdentity:IdentityDbContext<UserApp>
    {
        public TalabatContextIdentity(DbContextOptions<TalabatContextIdentity> options):base(options) 
        {
            
        }

    }
}
