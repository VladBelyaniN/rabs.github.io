using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal partial class Authorization
    {
        public static string authorizationRole;
        public static string GetRole(string login, string password)
        {
            var account = AutodetailsEntities.GetContext().Account.FirstOrDefault(a => a.Login_ == login && a.Password_ == password);
            if (account != null) return authorizationRole = account.Role_.name_role;
            return null;
        }

    }
}
