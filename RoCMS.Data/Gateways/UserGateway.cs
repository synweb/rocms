using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class UserGateway: BasicGateway<User>
    {
        public bool Authenticate(string username, string passHash)
        {
            return Exec<bool>(GetProcedureString(), new {username, passHash});
        }

        public User SelectByUsername(string username)
        {
            return Exec<User>(GetProcedureString(), username);
        }

        public User SelectByEmail(string email)
        {
            return Exec<User>(GetProcedureString(), email);
        }

        public void UpdatePassword(int userId, string passHash)
        {
            Exec(GetProcedureString(), new {userId, passHash});
        }
    }
}
