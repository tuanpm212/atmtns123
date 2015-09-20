using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data;
using Website.Data.BLL;

namespace Website.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            ProductBO cls = new ProductBO();
            var model = cls.GetData();
            return View(model);
        }

    }
}
