using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data;
using Website.Data.BLL;
using PagedList;

namespace Website.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index(int ? p)
        {
            ProductBO cls = new ProductBO();
            var model = cls.GetData();
            _session.IsLogin = false;
            int pageSize = 9;
            int pageNumber = (p ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(long i)
        {
            ProductBO cls = new ProductBO();
            var model = cls.GetProduct(i);
            _session.IsLogin = false;
            return View(model);
        }

    }
}
