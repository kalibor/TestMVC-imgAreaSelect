using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TestImgAreaSelect.Tool.ImgUploadPlugin;

namespace TestImgAreaSelect.Controllers
{

    

    public class HomeController : Controller
    {
        public HomeController()
        {
            _imgUploadFactory = new ImgUploadFactory();
        }


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上傳圖片用
        /// </summary>
        /// <param name="imgInfo">AJAX該傳的參數</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImageUpload(ImgUploadInfo imgInfo)
        {
            try
            {
                var savePath = _imgUploadFactory.GetGuidImageSavePath(imgInfo.Image);
                _imgUploadFactory.SaveImage(imgInfo.Image, savePath);
                return Json(savePath);
            }
            catch (Exception e)
            {
                var data = e.Message;

                return Json(data);
            }

        }

        /// <summary>
        /// 裁剪圖片並上傳
        /// </summary>
        /// <param name="imgInfo">AJAX該傳的參數</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CropImageUpload(ImgUploadInfo imgInfo)
        {

            var savePath = "";
                _imgUploadFactory.ProcessCrop(imgInfo,out savePath);

            return Json(savePath);
        }



        /// <summary>
        /// 圖片操作工廠
        /// </summary>
        private ImgUploadFactory _imgUploadFactory { get; set; }
    

    }

}