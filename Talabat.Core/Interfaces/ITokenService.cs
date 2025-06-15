using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken(UserApp user, UserManager<UserApp> userManager);
    }
}
