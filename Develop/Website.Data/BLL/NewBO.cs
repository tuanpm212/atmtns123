using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class NewsBO : Repository<News>
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

        public bool Save(News news)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    if (news.NewsID == -1)
                    {
                        db.News.Add(news);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.Entry(news).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
