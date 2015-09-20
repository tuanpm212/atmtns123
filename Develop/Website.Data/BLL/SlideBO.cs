using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class SlideBO:Repository<Slide>
    {
        public bool Save(Slide slide)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    if (slide.SlideID == -1)
                    {
                        db.Slides.Add(slide);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.Entry(slide).State = System.Data.Entity.EntityState.Modified;
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
