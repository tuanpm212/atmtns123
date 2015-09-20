using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data.BLL;
using Website.Data;

namespace Website.Controllers
{
    public class GalleryController : Controller
    {
        public ActionResult Index()
        {
            List<Photo> data = new List<Photo>();
            PhotoBO _cls = new PhotoBO();
            data = _cls.GetAll();
            ViewBag.id = data[0].ID;
            ViewBag.Count = data.Count();
            return View(data);
        }

        public PartialViewResult Photo(int id)
        {
            var data = new List<PhotoDetail>();
            PhotoBO cls = new PhotoBO();
            data = cls.GetPhotoDetail(id);
            return PartialView(data);
        }

        public ActionResult Video()
        {
            VideoBO cls = new VideoBO();
            var model = cls.GetList(m => m.IsActive.Equals(true));
            return View(model);
        }

    }
}
