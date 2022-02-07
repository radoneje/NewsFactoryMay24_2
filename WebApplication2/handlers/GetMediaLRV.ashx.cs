using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetMediaLRV
    /// </summary>
    ///
    public class GetMediaLRV : IHttpHandler
    {
      private  void Download(string fullpath, HttpContext context)
       {
            long size, start, end, length, fp = 0;


            using (FileStream  reader = new System.IO.FileStream(fullpath, FileMode.Open  , FileAccess.Read , FileShare.ReadWrite  ))
            {

                size = reader.Length;
                start = 0;
                end = size - 1;
                length = size;
                // Now that we've gotten so far without errors we send the accept range header
                /* At the moment we only support single ranges.
                 * Multiple ranges requires some more work to ensure it works correctly
                 * and comply with the spesifications: http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2
                 *
                 * Multirange support annouces itself with:
                 * header('Accept-Ranges: bytes');
                 *
                 * Multirange content must be sent with multipart/byteranges mediatype,
                 * (mediatype = mimetype)
                 * as well as a boundry header to indicate the various chunks of data.
                 */
                context.Response.AddHeader("Accept-Ranges", "0-" + size);
                // header('Accept-Ranges: bytes');
                // multipart/byteranges
                // http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2

                if (!String.IsNullOrEmpty(context.Request.ServerVariables["HTTP_RANGE"]))
                {
                    long anotherStart = start;
                    long anotherEnd = end;
                    string[] arr_split = context.Request.ServerVariables["HTTP_RANGE"].Split(new char[] { Convert.ToChar("=") });
                    string range = arr_split[1];

                    // Make sure the client hasn't sent us a multibyte range
                    if (range.IndexOf(",") > -1)
                    {
                        // (?) Shoud this be issued here, or should the first
                        // range be used? Or should the header be ignored and
                        // we output the whole content?
                        context.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");

                    }

                    // If the range starts with an '-' we start from the beginning
                    // If not, we forward the file pointer
                    // And make sure to get the end byte if spesified
                    if (range.StartsWith("-"))
                    {
                        // The n-number of the last bytes is requested
                        anotherStart = size - Convert.ToInt64(range.Substring(1));
                    }
                    else
                    {
                        arr_split = range.Split(new char[] { Convert.ToChar("-") });
                        anotherStart = Convert.ToInt64(arr_split[0]);
                        long temp = 0;
                        anotherEnd = (arr_split.Length > 1 && Int64.TryParse(arr_split[1].ToString(), out temp)) ? Convert.ToInt64(arr_split[1]) : size;
                    }
                    /* Check the range and make sure it's treated according to the specs.
                     * http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
                     */
                    // End bytes can not be larger than $end.
                    anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                    // Validate the requested range and return an error if it's not correct.
                    if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                    {

                        context.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");
                    }
                    start = anotherStart;
                    end = anotherEnd;

                    length = end - start + 1; // Calculate new content length
                    fp = reader.Seek(start, SeekOrigin.Begin);
                    context.Response.StatusCode = 206;
                }
           
            
            // Notify the client the byte range we'll be outputting
            context.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
            context.Response.AddHeader("Content-Length", length.ToString());

            //context.Response.AddHeader("Content-Type");
            // Start buffered download
            byte[] buffer = new byte[1024];
            int read=0;
            int send = 0;
            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                fp += read;
                send += read;
                if (context.Response.IsClientConnected)
                {
                    try
                    {
                        context.Response.OutputStream.Write(buffer, 0, read);
                    }
                    catch (Exception ex) {
                        DubugWriter("Exeption .OutputStream.Write, send bytes: " + send.ToString()+" Exeption"+ex.Message );
                        break;
                    }

                }
                else
                {
                    DubugWriter("Exeption .client disconnected, send bytes: " + send.ToString() + " start " + start.ToString() + " end " + end.ToString());
                       
                    break; }
            }
            DubugWriter("exit , send bytes: " + send.ToString() + " " + " start " + start.ToString() + " end " + end.ToString());
                //var 
            //reader.Seek( fp, System.IO.SeekOrigin.Begin );
            //context.Response.WriteFile(fullpath, fp, length);
            context.Response.End();
            
            }
        }

        private void DubugWriter(string s)
      {
          using (Stream fs = new FileStream(@"c:\\tmp\\12.txt", FileMode.Create | FileMode.Append))
          {
              StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII, s.Length );
              sw.Write(s);
              sw.Write("\r\n");
              sw.Close();
          }
      }
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            string filename = "c:\\tmp\\1.mp4";
            for (int i = 0; i < context.Request.Headers.Keys.Count; i++)
            {

                DubugWriter(context.Request.Headers.Keys[i] + "->" + context.Request.Headers.Get(i) + "\r\n");


            }

            DubugWriter("\r\n_______\r\n");
           //
            
           Download(filename, context);
           //
            
           /* using (FileStream reader = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
           {

                string path = filename;
                FileInfo file = new FileInfo(path);
                int len = (int)file.Length, bytes;
                context.Response.AppendHeader("content-length", len.ToString());
                context.Response.ContentType = /*System.Web.MimeMapping.GetMimeMappingMimeExtensionHelper.GetMimeType(filename);
                byte[] buffer = new byte[1024];
                Stream outStream = context.Response.OutputStream;
                int i = 0;
                using (Stream stream = File.OpenRead(path))
                {
                    while (len > 0 && (bytes =
                        stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, bytes);
                        len -= bytes;
                        i += bytes;
                        
                    }
                }
                DubugWriter("total bytes read bytes: " + i.ToString());
            }
            */
            

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
    public class MediaLRVRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetMediaLRV() { RequestContext = requestContext }; ;
        }
    }
}