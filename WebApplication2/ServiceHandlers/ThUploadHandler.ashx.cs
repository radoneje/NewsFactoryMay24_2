using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for ThUploadHandler
    /// </summary>
    public class ThUploadHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                String EncoderGuid = (string)RequestContext.RouteData.Values["EncoderGuid"];
                // Dictionary<string, handlers.utils.EncodeStatus> encsatus = (Dictionary<string, handlers.utils.EncodeStatus>)context.Application["EncoderStatus"];
                //////////    
                ////////////

                // handlers.utils.EncodeStatus stat = encsatus[EncoderGuid];
                int Number = Convert.ToInt32((string)RequestContext.RouteData.Values["ThNumber"]);
                using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                {
                    byte[] buffer = new byte[context.Request.InputStream.Length];
                    Stream st = new MemoryStream();
                    context.Request.InputStream.Read(buffer, 0, buffer.Length);
                    System.Data.Linq.Binary bt = new System.Data.Linq.Binary(buffer);

                    st.Seek(0, SeekOrigin.Begin);
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(st);
                    int thHeight = Convert.ToInt32(((float)((float)64 / (float)bm.Width)) * (float)bm.Height);
                    System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap((System.Drawing.Image)bm, new System.Drawing.Size(640, thHeight));
                    thHeight = Convert.ToInt32(((float)((float)32 / (float)bm.Width)) * (float)bm.Height);
                    System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap((System.Drawing.Image)bm, new System.Drawing.Size(32, thHeight));

                    System.IO.Stream st1 = new System.IO.MemoryStream();
                    System.IO.Stream st2 = new System.IO.MemoryStream();

                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder =
                        System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bm1.Save(st1, jgpEncoder, myEncoderParameters);
                    bm1.Save(st2, jgpEncoder, myEncoderParameters);

                    st1.Seek(0, System.IO.SeekOrigin.Begin);
                    st2.Seek(0, System.IO.SeekOrigin.Begin);
                    byte[] bt1 = new byte[st1.Length];
                    st1.Read(bt1, 0, bt1.Length);
                    byte[] bt2 = new byte[st2.Length];
                    st2.Read(bt2, 0, bt2.Length);

                    dc.pMedia_ThumbnailInsert(EncoderGuid, bt, Number, new System.Data.Linq.Binary(bt1), new System.Data.Linq.Binary(bt2));
                }
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 505;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error: Th is no uploaded"+ex.Message);
                return;
            }
              

            context.Response.ContentType = "text/plain";
            context.Response.Write("Th is uploaded");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
    public class ThUploadRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ThUploadHandler() { RequestContext = requestContext }; ;
        }
    }
}