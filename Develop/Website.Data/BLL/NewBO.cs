using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class NewsBO : Repository<About>
    {
        public List<News> GetNews()
        {
            try
            {
                using (var db = new DBEntities())
                {
                    var select = from c in db.News
                                 select c;
                    return select.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public News GetNews(long id) {
            try
            {
                using (var db = new DBEntities())
                {
                    var select = from c in db.News
                                  where c.NewsID == id
                                     select c;
                    return select.First();
                }
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
