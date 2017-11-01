using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestImgAreaSelect.Controllers
{
    public class ImgUploadInfo
    {
       public HttpPostedFileBase Image { get; set; }
    }

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ImageUpload(ImgUploadInfo imgInfo)
        {
            try
            {
                string dictPath = @"\images\temp\";

                string fileName = Guid.NewGuid().ToString();

                string Extension = System.IO.Path.GetExtension(imgInfo.Image.FileName);

                string savePath = string.Format("{0}{1}{2}", dictPath, fileName, Extension);

                var bitMap = new Bitmap(imgInfo.Image.InputStream);

                bitMap.Save(Server.MapPath(savePath));

                return Json(savePath);
            }catch(Exception e)
            {
                var data = e.Message;

                return Json(data);
            }
         
        }
    }
}