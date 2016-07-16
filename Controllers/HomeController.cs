using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Microsoft.ProjectOxford.Face;
using XamaMe.Extensions;

namespace XamaMe.Controllers
{
    public class HomeController : Controller
    {

        public static Image Xamagon;
        public ActionResult Runtime()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFile photo)
        {
            Xamagon = Xamagon ?? new Bitmap(Server.MapPath("~/Content/xamagon.png"));

            var faceCount = 0;

            if (Request.Files.Count == 0) return View();
            var file = Request.Files[0];

            byte[] fileBytes = GetImageBytes(file);

            Rectangle r; // = new Rectangle(43, 95, 155, 155);

            var faceService = new FaceServiceClient("510b57a61366462da7c7f6ea7da85359");
            var face = (await faceService.DetectAsync(file.InputStream)).FirstOrDefault();
            if (face == null) return View();
            r = face.GetCroppingRectangle();
            var faceCenter = r.GetCenter();

            using (var imageFactory = new ImageFactory(false))
            {
                var responseStream = new MemoryStream();

                var img = imageFactory.Load(fileBytes);

                img
                    .Crop(r)
                    .Mask(Xamagon)
                    .Format(new PngFormat())
                    .Save(responseStream);



                var responseBytes = new byte[responseStream.Length];
                responseStream.Read(responseBytes, 0, responseBytes.Length);

                ViewBag.ImageBytes = responseBytes;

                ViewBag.SeemsGood = img.Image.Width >= Xamagon.Width
                    && img.Image.Height >= Xamagon.Height;
            }
            return View("XamaMe", faceCount);

        }



        private static byte[] GetImageBytes(HttpPostedFileBase file)
        {
            var fileBytes = new byte[file.ContentLength];
            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
            file.InputStream.Seek(0, SeekOrigin.Begin);
            return fileBytes;
        }
    }

}

