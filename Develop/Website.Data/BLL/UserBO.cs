using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class UserBO : Repository<User>
    {
        public User Login(string LoginName, string Password)
        {
            using (var db = new DBEntities())
            {
                var select = from c in db.Users
                             where c.LoginName == LoginName && c.Password == Password && c.IsAdmin
                             select c;
                if (select.Count() >0)
                    return select.First();
                return null;
            }
        }
    }
}
