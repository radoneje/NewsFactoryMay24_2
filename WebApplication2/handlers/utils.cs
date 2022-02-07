using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.handlers
{
    public class utils
    {
        public static string getLocalString(string strId)
        {
            string id = Guid.NewGuid().ToString();
            return "<span id=\"" + id + "\" class=\"caption caption-html\" captionId=\"" + strId + "\"></span><script>$(\"#" + id + "\").html(langTable[\"" + strId + "\"]);</script>";
        }
        public static void Streaming(HttpContext context, string filename, string mimetype)
        {

            context.Response.Buffer = false;
            if (Path.GetExtension(filename) == "webm")
                context.Response.ContentType = "video/webm";
            else
                context.Response.ContentType = mimetype;

            try
            {
                RangeDownload(filename, context);
            }
            catch (Exception ex)
            {
                // context.Response.Write("ошибка"+ex.Message);
                // context.Response.StatusCode = 500;
                //  context.Response.End();
            }



            return;
            
       
        }///////
        private static void RangeDownload(string fullpath, HttpContext context)
        {
            long size, start, end, length, fp = 0;


            using (FileStream reader = new System.IO.FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(fullpath));
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
                byte[] buffer = new byte[32768];
                int read = 0;
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fp += read;
                    if (context.Response.IsClientConnected)
                    {
                        try
                        {
                            context.Response.OutputStream.Write(buffer, 0, read);
                        }
                        catch (Exception ex) { break; }
                    }
                    else
                    { break; }
                }
                //var 
                //reader.Seek( fp, System.IO.SeekOrigin.Begin );
                //context.Response.WriteFile(fullpath, fp, length);
                context.Response.End();

            }
        }

        ////
        public static long ToUnixTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);

            return unixTimestamp;
        }
        public static DateTime ToDateTime(DateTime target, long timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);

            return dateTime.AddSeconds(timestamp);
        }
        public struct MediaChunks
        {
            public Int64 StartByte;
            public Int64 EndByte;
            public bool bReady;
            public int iNumberInFile;
            public Int64 BytesWrite;
        }
        public struct MediaFiles
        {
            public string sFileName;
            public string sFileGuid;
            public Int64 Size;
            public bool bReady;
            public Int64 UploadedBytes;
            public int iNumberInFolder;
            public Dictionary<string, utils.MediaChunks> Chunks;
            public string sSourceIp;
            public DateTime dStartDate;
            public DateTime dEndDate;
            public int iBlockType;
        }
        public struct MediaFolders
        {
            public string sFolderName;
            public string sFolderGuid;
            public bool bReady;
            public string sPath;
            public int iFilesCount;
            public string sBlockGuid;
            public Dictionary<string, utils.MediaFiles> File;
        }
        [Serializable]
        public class EncodeStack
        {
            public string FileGuid;
            public string EncoderGuid;
            public int EncoderStatus { get; set; } //0- нет, 1- готов первый проход 2- готово,
            public int Errorcount;  ///попыток выполнения
            public string Message;                        ///

        }
        public static dynamic getAjaxResp(System.IO.Stream inputStream)
        {

            inputStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(inputStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            return System.Web.Helpers.Json.Decode(rawJson);
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(string str)
        {
            var bytes = System.Text.UnicodeEncoding.Unicode.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(mso, System.IO.Compression.CompressionMode.Compress))
                {

                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(msi, System.IO.Compression.CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return System.Text.UnicodeEncoding.Unicode.GetString(mso.ToArray());
            }
        }

        public  class Lic
        {
                public  int maxUsers =1024;
                public  int maxProg = 1024;
                public  int maxNewsOnDay = 1024;
                public  bool admin = true;
                public  bool prompter =true;
                public  bool mediaFiles = true;
                public  bool lrv = true;
        }
        
    }
}

    