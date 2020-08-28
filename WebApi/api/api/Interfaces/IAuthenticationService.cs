using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IAuthenticationService
    {
        AuthenticationData login(string username, string password);
        List<menuFunctionGroupView> getUserRole(string userId);
    }
}
