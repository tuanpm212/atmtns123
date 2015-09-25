using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class AboutBO: Repository<About>
    {
        public About GetAbout()
        {
            try
            {
                using (var db = new DBEntities())
                {
                    var select = from c in db.Abouts
                                 select c;
                    return select.First();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Save(About about)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    db.Entry(about).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }catch
            {
                return false;
            }
        }
    }
}
