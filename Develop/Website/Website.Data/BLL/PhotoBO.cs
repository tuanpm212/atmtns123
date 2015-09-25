using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Website.Data.BLL
{
    public class PhotoBO: Repository<Photo>
    {
        public bool Save(Photo photo, List<PhotoDetail> detail)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    using (TransactionScope myTran = new TransactionScope())
                    {
                        try
                        {
                            if (photo.ID == -1)
                            {
                                db.Photos.Add(photo);
                                db.SaveChanges();
                                foreach (PhotoDetail row in detail)
                                {
                                    row.PhotoID = photo.ID;
                                }
                                db.PhotoDetails.AddRange(detail);
                                db.SaveChanges();
                                myTran.Complete();
                                return true;
                            }
                            else
                            {
                                db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();

                                db.PhotoDetails.RemoveRange(db.PhotoDetails.Where(c => c.PhotoID == photo.ID));
                                db.SaveChanges();
                                foreach (PhotoDetail row in detail)
                                {
                                    row.PhotoID = photo.ID;
                                }
                                db.PhotoDetails.AddRange(detail);
                                db.SaveChanges();
                                myTran.Complete();
                                return true;
                            }
                        }
                        catch
                        {
                            myTran.Dispose();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public ArrayList GetPhotoByID(int id)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    ArrayList arrResult = new ArrayList();
                    var photos = from c in db.Photos
                                 where c.ID == id
                                 select c;
                    if (photos.Count() > 0)
                        arrResult.Add(photos.First());

                    var detail = from c in db.PhotoDetails
                                 where c.PhotoID == id
                                 select c;
                    if (detail.Count() > 0)
                        arrResult.Add(detail.ToList());
                    return arrResult;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<PhotoDetail> GetPhotoDetail(int id)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    var select = from c in db.PhotoDetails
                                 where c.PhotoID == id && c.IsActive
                                 orderby c.Sort
                                 select c;
                    if (select.Count() > 0)
                        return select.ToList();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
