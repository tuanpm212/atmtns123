using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Website.Data;
using Website.Data.BLL;
using PagedList;

namespace Website.Controllers
{
    public class HomeController : BaseController
    {
        #region ActionResult

            public ActionResult Index()
            {
                HomeBO iHome = new HomeBO();
                ModelHome model = new ModelHome();
                model = iHome.GetDataForHome();
                _session.IsLogin = false;
                return View(model);
            }

            [HttpGet]
            public ActionResult Contact()
            {
                Contact model = new Contact();
                return View(model);
            }

            [HttpPost]
            public ActionResult Contact(Contact model)
            {
                ContactBO _contact = new ContactBO();
                if (model != null)
                {
                    model.CreatedDate = DateTime.Now;
                    if (_contact.Add(model))
                    {
                        string body = string.Format(Resources.EmailTemplate.feedbackBody, model.UserName);
                        this.SendEmailMessage(model.Email, Resources.EmailTemplate.feedbackSubject, Resources.EmailTemplate.feedbackBody);

                        body = string.Format(Resources.EmailTemplate.fromBody, model.UserName, model.Email, model.Title, model.Content);
                        this.SendEmailMessage(model.Email, Resources.EmailTemplate.fromSubject, body);

                        this.ModelState.Clear();
                        model = new Contact();
                    }
                }
                return View(model);
            }

            public ActionResult About()
            {
                AboutBO iAbout = new AboutBO();
                var model = iAbout.GetAbout();
                return View(model);
            }

            public ActionResult News(int ? p)
            {
                NewsBO cls = new NewsBO();
                var model = cls.GetNews();
                _session.IsLogin = false;
                int pageSize = 9;
                int pageNumber = (p ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }

            public ActionResult NewsDetail(long i)
            {
                NewsBO cls = new NewsBO();
                var model = cls.GetNews(i);
                _session.IsLogin = false;
                return View(model);
            }

          #endregion
    }
}