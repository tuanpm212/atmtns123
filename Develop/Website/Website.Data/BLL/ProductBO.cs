
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data.BLL
{
    public class ProductBO: Repository<Product>
    {
        public List<Product> GetData()
        {
            try
            {
                using(var db= new DBEntities())
                {
                    var select = from c in db.Products
                                 where c.IsActive
                                 orderby c.Sort
                                 select c;
                    return select.ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Save(Product product)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    if (product.ID == -1)
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.Entry(product).State = System.Data.Entity.EntityState.Modified;
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
