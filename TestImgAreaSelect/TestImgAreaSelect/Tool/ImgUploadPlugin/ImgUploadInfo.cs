using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestImgAreaSelect.Tool.ImgUploadPlugin
{
    public class ImgUploadInfo
    {
        public HttpPostedFileBase Image { get; set; }

        public string Section { get; set; }

        public string ImageUrl { get; set; }

    }
}