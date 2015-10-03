using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Website.Data;
using Website.Data.BLL;

namespace Website.Controllers
{
    public class AdminController : BaseController
    {
        #region Login

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginModel model)
        {
            UserBO _cls = new UserBO();
            var user = _cls.Login(model.LoginName, model.Password);
            if (user != null)
            {
                _session.IsLogin = true;
                _session.FullName = user.UserName;
                return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _session.IsLogin = false;
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Logout()
        {
            if (_session.IsLogin)
            {
                _session.IsLogin = false;
                _session.NickName = null;
                _session.FullName = null;
                return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Video

        public ActionResult Video()
        {
            if (_session.IsLogin)
                return View();
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult CreateVideo(int id)
        {
            ViewBag.ID = id;
            if (_session.IsLogin)
            {
                Video model = new Video();
                if (id != -1)
                {
                    VideoBO cls = new VideoBO();
                    model = cls.GetByID(id);
                    if (model == null)
                        model = new Video();
                    return View(model);
                }
                else
                    return View(model);
            }
            else
                return RedirectToAction("index", "admin");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult GetVideo()
        {
            if (_session.IsLogin)
            {
                string jsonData = "[]";
                VideoBO _cls = new VideoBO();
                var data = _cls.GetAll();
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveVideo(Video video)
        {
            VideoBO cls = new VideoBO();
            bool IsResult = cls.Save(video);
            return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteVideo(int id)
        {
            if (_session.IsLogin)
            {
                VideoBO _cls = new VideoBO();
                var IsResult = _cls.Delete(id);
                return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Photo

        public ActionResult Photo()
        {
            if (_session.IsLogin)
                return View();
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult CreatePhoto(int id)
        {
            if (_session.IsLogin)
            {
                ViewBag.ID = id;
                return View();
            }
            else
                return RedirectToAction("index", "admin");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult GetPhoto()
        {
            try
            {
                if (_session.IsLogin)
                {
                    string jsonData = "[]";
                    PhotoBO _cls = new PhotoBO();
                    var data = _cls.GetAll();
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("[]", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SavePhoto(Photo photo, List<PhotoDetail> detail)
        {
            PhotoBO cls = new PhotoBO();
            bool IsResult = cls.Save(photo, detail);
            return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePhoto(int id)
        {
            if (_session.IsLogin)
            {
                PhotoBO _cls = new PhotoBO();
                var IsResult = _cls.Delete(id);
                return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadCover()
        {
            // Validate we have a file being posted
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
            }
            // File we want to resize and save.
            var file = Request.Files[0];

            try
            {
                var filename = UploadCover(file);
                return Json(new
                {
                    statusCode = 200,
                    status = "Image uploaded.",
                    file = filename,
                }, "text/html");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image." + ex.Message,
                    file = string.Empty
                }, "text/html");
            }
        }

        [HttpPost]
        public JsonResult UploadPhoto()
        {
            // Validate we have a file being posted
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
            }
            // File we want to resize and save.
            var file = Request.Files[0];

            try
            {
                var filename = UploadPhoto(file);
                return Json(new
                {
                    statusCode = 200,
                    status = "Image uploaded.",
                    file = filename,
                    id = Guid.NewGuid().ToString(),
                }, "text/html");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image." + ex.Message,
                    file = string.Empty
                }, "text/html");
            }
        }

        private string UploadCover(HttpPostedFileBase file)
        {
            // Initialize variables we'll need for resizing and saving
            var width = 240;
            var height = 240;
            var relativeFileAndPath = "/Uploads/default";
            var fileName = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            // Build absolute path
            var absPath = HttpContext.Server.MapPath(relativeFileAndPath);
            var absFileAndPath = absPath + "\\" + fileName;

            // Create directory as necessary and save image on server
            if (!Directory.Exists(absPath))
                Directory.CreateDirectory(absPath);
            file.SaveAs(absFileAndPath);

            // Resize image using "ImageResizer" NuGet package.
            var resizeSettings = new ImageResizer.ResizeSettings
            {
                Scale = ImageResizer.ScaleMode.DownscaleOnly,
                Width = width,
                Height = height,
                Mode = ImageResizer.FitMode.Crop
            };
            var b = ImageResizer.ImageBuilder.Current.Build(absFileAndPath, resizeSettings);

            // Save resized image
            b.Save(absFileAndPath);

            // Return relative file path
            return relativeFileAndPath + "/" + fileName;
        }

        private string UploadPhoto(HttpPostedFileBase file)
        {
            // Initialize variables we'll need for resizing and saving
            var widthThumnail = 240;
            var heightThumnail = 173;
            var widthLightBox = 1074;
            var heightLightBox = 768;
            var relativeFileAndPath = "/Uploads/default";
            var idFile = Guid.NewGuid().ToString();
            var fileNameThumnail = idFile + "_thumnail" + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            var fileNameLightBox = idFile + "_lightbox" + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            var fileNameFull = idFile + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            // Build absolute path
            var absPath = HttpContext.Server.MapPath(relativeFileAndPath);
            var absFileThumnail = absPath + "\\" + fileNameThumnail;
            var absFileLightBox = absPath + "\\" + fileNameLightBox;
            var absFileFull = absPath + "\\" + fileNameFull;

            // Create directory as necessary and save image on server
            if (!Directory.Exists(absPath))
                Directory.CreateDirectory(absPath);
            file.SaveAs(absFileThumnail);
            file.SaveAs(absFileLightBox);
            file.SaveAs(absFileFull);

            // Resize image using "ImageResizer" NuGet package.
            var resizeThumnail = new ImageResizer.ResizeSettings
            {
                Scale = ImageResizer.ScaleMode.DownscaleOnly,
                Width = widthThumnail,
                Height = heightThumnail,
                Mode = ImageResizer.FitMode.Crop
            };
            var b = ImageResizer.ImageBuilder.Current.Build(absFileThumnail, resizeThumnail);
            var resizeLightBox = new ImageResizer.ResizeSettings
            {
                Scale = ImageResizer.ScaleMode.DownscaleOnly,
                Width = widthLightBox,
                Height = heightLightBox,
                Mode = ImageResizer.FitMode.Crop
            };
            var c = ImageResizer.ImageBuilder.Current.Build(absFileThumnail, resizeLightBox);
            // Save resized image
            b.Save(absFileThumnail);
            c.Save(absFileLightBox);

            // Return relative file path
            return relativeFileAndPath + "/" + fileNameFull;
        }

        public JsonResult GetPhotoByID(int id)
        {
            if (_session.IsLogin)
            {
                if (id == -1)
                {
                    string jsonData = "";
                    jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(new Photo());
                    jsonData += ",\"detail\":[]}";
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string jsonData = "";
                    PhotoBO _cls = new PhotoBO();
                    var data = _cls.GetPhotoByID(id);
                    jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(data[0]);
                    jsonData += ",\"detail\":" + new JavaScriptSerializer().Serialize(data[1]) + "}"; ;
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Slide

        public ActionResult Slide()
        {
            if (_session.IsLogin)
                return View();
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult CreateSlide(int id)
        {
            ViewBag.ID = id;
            if (_session.IsLogin)
            {
                Slide model = new Slide();
                ViewBag.Image = "~/Uploads/Default/default.png";
                if (id != -1)
                {
                    SlideBO cls = new SlideBO();
                    model = cls.GetByID(id);
                    if (model == null)
                        model = new Slide();
                    else
                        ViewBag.Image = model.Image;
                    return View(model);
                }
                else
                    return View(model);
            }
            else
                return RedirectToAction("index", "admin");
        }

        public JsonResult GetSlide()
        {
            if (_session.IsLogin)
            {
                string jsonData = "[]";
                SlideBO _cls = new SlideBO();
                var data = _cls.GetAll();
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSlide(Slide slide)
        {
            SlideBO cls = new SlideBO();
            bool IsResult = cls.Save(slide);
            return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSlide(int id)
        {
            if (_session.IsLogin)
            {
                SlideBO _cls = new SlideBO();
                var IsResult = _cls.Delete(id);
                return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadSlide()
        {
            // Validate we have a file being posted
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
            }
            // File we want to resize and save.
            var file = Request.Files[0];

            try
            {
                var filename = UploadPhoto(file);
                return Json(new
                {
                    statusCode = 200,
                    status = "Image uploaded.",
                    file = filename,
                    id = Guid.NewGuid().ToString(),
                }, "text/html");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image." + ex.Message,
                    file = string.Empty
                }, "text/html");
            }
        }

        public JsonResult GetSlideByID(int id)
        {
            if (_session.IsLogin)
            {
                if (id == -1)
                {
                    string jsonData = "";
                    jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(new Photo());
                    jsonData += "}";
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string jsonData = "";
                    SlideBO _cls = new SlideBO();
                    var data = _cls.GetByID(id);
                    jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(data);
                    jsonData += "}";
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Product

        public ActionResult Product()
        {
            if (_session.IsLogin)
                return View();
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult CreateProduct(int id)
        {
            ViewBag.ID = id;
            if (_session.IsLogin)
            {
                Product model = new Product();
                if (id != -1)
                {
                    ProductBO cls = new ProductBO();
                    model = cls.GetByID(id);
                    if (model == null)
                        model = new Product();
                    ViewBag.Image = model.ImageThumbnail;
                    ViewBag.ImageLarge = model.ImageLarge;
                    return View(model);
                }
                else
                    return View(model);
            }
            else
                return RedirectToAction("index", "admin");
        }

        public JsonResult GetProduct()
        {
            if (_session.IsLogin)
            {
                string jsonData = "[]";
                ProductBO _cls = new ProductBO();
                var data = _cls.GetList(m => m.IsActive.Equals(true));
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProduct(Product product)
        {
            ProductBO cls = new ProductBO();
            bool IsResult = cls.Save(product);
            return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region About
        public ActionResult About()
        {
            if (_session.IsLogin)
            {
                AboutBO _cls = new AboutBO();
                var model = _cls.GetAbout();
                ViewBag.ID = model.AboutID;
                ViewBag.ShortContent = model.ShortContent;

                return View(model);
            }
            else
                return RedirectToAction("index", "admin");
        }

        [HttpPost]
        public JsonResult About(About about)
        {
            if (_session.IsLogin)
            {
                AboutBO _cls = new AboutBO();
                var isResult = _cls.Save(about);
                return Json(new { IsOk = isResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                RedirectToAction("index", "admin");
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region News
        public ActionResult News()
        {
            if (_session.IsLogin)
            {
                return View();
            }
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult CreateNews(int id)
        {
            ViewBag.ID = id;
            if (_session.IsLogin)
            {
                News model = new News();
                if (id != -1)
                {
                    NewsBO cls = new NewsBO();
                    model = cls.GetNews(id);
                    if (model == null)
                        model = new News();
                    return View(model);
                }
                else
                    return View(model);
            }
            else
                return RedirectToAction("index", "admin");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult GetNews()
        {
            if (_session.IsLogin)
            {
                string jsonData = "[]";
                NewsBO _cls = new NewsBO();
                var data = _cls.GetNews();
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNews(News news)
        {
            NewsBO cls = new NewsBO();
            bool IsResult = cls.Save(news);
            return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteNews(int id)
        {
            if (_session.IsLogin)
            {
                NewsBO _cls = new NewsBO();
                var IsResult = _cls.Delete(id);
                return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
            }
            else
                RedirectToAction("index", "admin");
            return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
