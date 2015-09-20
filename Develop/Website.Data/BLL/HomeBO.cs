using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class HomeBO: Repository<About>
    {
        public ModelHome GetDataForHome()
        {
            try
            {
                ModelHome model = new ModelHome();
                using (var db = new DBEntities())
                {
                    var _slider = from c in db.Slides
                                  where c.IsActive && c.Type == "SLIDE_TOP"
                                  orderby c.Sort
                                  select c;
                    if (_slider.Count() > 0)
                        model.Sliders = _slider.ToList();

                    var _about = from c in db.Abouts
                                select c;
                    if (_about.Count() > 0)
                        model.about = _about.First();

                    var _product = from c in db.Products
                                   where c.IsActive
                                   orderby c.Sort
                                   select c;
                    if (_product.Count() > 0)
                        model.Products = _product.ToList();

                    var _service = from c in db.Services
                                   where c.IsActive
                                   orderby c.Sort
                                   select c;
                    if (_service.Count() > 0)
                        model.Services = _service.ToList();
                    return model;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
