using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class VideoBO : Repository<Video>
    {
        public bool Save(Video video)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    if(video.VideoID==-1)
                    {
                        db.Videos.Add(video);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.Entry(video).State = System.Data.Entity.EntityState.Modified;
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
