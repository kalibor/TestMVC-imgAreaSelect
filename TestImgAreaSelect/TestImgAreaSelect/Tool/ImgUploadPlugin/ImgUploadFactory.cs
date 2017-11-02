using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TestImgAreaSelect.Tool.ImgUploadPlugin
{
    public class ImgUploadFactory
    {
        /// <summary>
        /// 取得不重複的GUID檔名
        /// </summary>
        /// <param name="Image">由使用者上傳的圖片檔</param>
        /// <returns></returns>
        public string GetGuidImageSavePath(HttpPostedFileBase Image)
        {
            string dictPath = @"\images\temp\";

            string fileName = Guid.NewGuid().ToString();

            string Extension = System.IO.Path.GetExtension(Image.FileName);

            string savePath = string.Format("{0}{1}{2}", dictPath, fileName, Extension);
            return savePath;
        }

        /// <summary>
        /// 取得不重複的GUID檔名
        /// </summary>
        /// <param name="Url">圖片網址</param>
        /// <returns></returns>
        public string GetGuidImageSavePath(string Url)
        {
            string dictPath = @"\images\temp\";

            string fileName = Guid.NewGuid().ToString();

            string Extension = System.IO.Path.GetExtension(Url);

            string savePath = string.Format("{0}{1}{2}", dictPath, fileName, Extension);
            return savePath;
        }



        /// <summary>
        /// 儲存圖片
        /// </summary>
        /// <param name="file">由使用者上傳的圖片</param>
        /// <param name="savePath">儲存路徑(虛擬路徑)</param>
        public void SaveImage(HttpPostedFileBase file, string savePath)
        {
            file.SaveAs(HttpContext.Current.Server.MapPath(savePath));
        }


        /// <summary>
        /// 透過圖片網址取得該圖片的串流
        /// </summary>
        /// <param name="Url">圖片網址</param>
        /// <returns></returns>
        public MemoryStream GetUrlImgStream(string Url)
        {
            var memoryStream = new MemoryStream();
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(Url).Result)
                    {


                        using (var stream = response.Content.ReadAsStreamAsync().Result)
                        {
                            stream.CopyTo(memoryStream);
                        }
                        memoryStream.Seek(0, SeekOrigin.Begin);

                    }
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            return memoryStream;

        }


        /// <summary>
        /// 執行裁減
        /// </summary>
        /// <param name="imgInfo">AJAX傳過來的參數</param>
        /// <param name="savePath">儲存路徑(虛擬)</param>
        public void ProcessCrop(ImgUploadInfo imgInfo, out string savePath)
        {
            Stream imageStream = null;

            try
            {
                if (imgInfo.Image != null)
                {
                    savePath = GetGuidImageSavePath(imgInfo.Image);
                    imageStream = imgInfo.Image.InputStream;

                }
                else
                {
                    savePath = GetGuidImageSavePath(imgInfo.ImageUrl);
                    imageStream = GetUrlImgStream(imgInfo.ImageUrl);
                }

                using (var sourceImage = Image.FromStream(imageStream))
                {
                    if (!string.IsNullOrEmpty(imgInfo.Section))
                    {
                        DoCrop(imgInfo, savePath, sourceImage);
                    }
                    else
                    {
                        sourceImage.Save(HttpContext.Current.Server.MapPath(savePath));
                    }
                }

            }
            catch (Exception e)
            {
                savePath = null;
            }
            finally
            {
                if (imageStream != null)
                {
                    imageStream.Close();
                    imageStream.Dispose();
                    imageStream = null;
                }
            }

        }

        /// <summary>
        /// 執行裁減
        /// </summary>
        /// <param name="imgInfo">AJAX傳過來的參數</param>
        /// <param name="savePath">儲存路徑(虛擬)</param>
        /// <param name="sourceImage">原圖片</param>
        private static void DoCrop(ImgUploadInfo imgInfo, string savePath, Image sourceImage)
        {
            var sectionModal = new
            {
                x1 = 0,
                y1 = 0,
                x2 = 0,
                y2 = 0,
                width = 0,
                height = 0,
                imgControlWidth = 0.0f,
                imgControlHeight = 0.0f
            };
            var section = JsonConvert.DeserializeAnonymousType(imgInfo.Section, sectionModal);

            //判斷使用者是否有選取切割範圍
            if (section.height == 0 && section.width == 0)
            {
                sourceImage.Save(HttpContext.Current.Server.MapPath(savePath));
            }
            else
            {
                //由於控制項的大小與圖片實際大小會有出入，因此取得其差異;
                var ScaleX = sourceImage.Width / section.imgControlWidth;
                var ScaleY = sourceImage.Height / section.imgControlHeight;

                //設定實際儲存用的參數
                int ActualX = (int)(section.x1 * ScaleX);
                int ActualY = (int)(section.y1 * ScaleY);
                int ActualWidth = (int)(section.width * ScaleX);
                int ActualHeight = (int)(section.height * ScaleY);

                //建立新的BitMap物件
                Bitmap bitmap = new Bitmap(ActualWidth, ActualHeight);
                var fromR = new Rectangle(ActualX, ActualY, ActualWidth, ActualHeight);
                var toR = new Rectangle(0, 0, ActualWidth, ActualHeight);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.White);
                GraphicsUnit units = GraphicsUnit.Pixel;

                g.DrawImage(sourceImage, toR, fromR, units);
                bitmap.Save(HttpContext.Current.Server.MapPath(savePath));
            }

        }


    }
}