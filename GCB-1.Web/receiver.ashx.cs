using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace GCB_1.Web
{
    /// <summary>
    /// this recives the image stream and save in server
    /// </summary>
    public class receiver : IHttpHandler
    {
        public class PictureFile
        {
            
            public string PictureName { get; set; }

            public byte[] PictureStream { get; set; }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.QueryString["filename"].ToString();

            using (FileStream fs = File.Create(context.Server.MapPath("~/Image_Files/" + filename)))
            {
                SaveFile(context.Request.InputStream, fs);
            }
           

            
        }
       

            
 
        

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}