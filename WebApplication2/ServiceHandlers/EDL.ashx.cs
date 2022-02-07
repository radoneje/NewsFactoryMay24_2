using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for getServiceConfig
    /// </summary>
    public class EDL : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            

            if ((RequestContext.RouteData.Values["blockId"]) == null)
            { context.Response.Write("error"); return; }
            context.Response.ContentType = "text/xml";
            context.Response.AddHeader("Content-Type", "application/force-download");
            context.Response.AddHeader("Content-Type", "application/octet-stream");
            context.Response.AddHeader("Content-Type", "application/download");

            var blockId = Convert.ToInt64((string)RequestContext.RouteData.Values["blockId"]);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var w = "1920";
                var recS = dc.tWeb_Settings.Where(t => t.Key == "edlWidth");
                if (recS.Count() > 0)
                    w = recS.First().value;
                
                var h = "1080";
                var recd = dc.tWeb_Settings.Where(t => t.Key == "edlHeight");
                if (recd.Count() > 0)
                    h = recd.First().value;
                
                 var BREC=   dc.Blocks.Single(b => b.Id == blockId );
                 var blockText = BREC.BlockText;
                var regex= new System.Text.RegularExpressions.Regex(@"\(\(NF\:\:VIDEO\:\:([^\)]+)\)\)");
                var matches = regex.Matches(blockText);
                var lst = new List<dynamic>();
                foreach (System.Text.RegularExpressions.Match match in matches)
                {

                    var res =  System.Web.Helpers.Json.Decode((string)match.Groups[1].Value);
                    Int64 mediaId = Convert.ToInt64( (string)res.mediaId);
                    var rec = dc.Blocks.Single(b => b.Id == mediaId);
                    res.filename = rec.BlockText;

                    res.startTime = 0;
                    res.chrono = rec.BlockTime;
                    try
                    {
                        float markIn = float.Parse(res.MarkIn.Replace(".",","));
                        float markOut = float.Parse(res.MarkOut.Replace(".", ","));
                        if(markOut>markIn && markOut>0 )
                        {
                            res.chrono = (int)(markOut - markIn);
                            res.startTime =(int) markIn;
                        }


                    } catch{}
                    lst.Add(res);
                }

                //var lst2 = new List<dynamic>();
                 dc.Blocks.Where(b => b.ParentId == blockId && b.deleted==false && b.BLockType==2 && b.BlockText.Length>0).OrderBy(b => b.Name).ToList().ForEach(l => {
                 //   bool find = false;
                   // lst.ForEach(ll => { if (ll.Id == l.Id) find = true; });
                  //  if(!find)
                    {
                        var rr = new { 
                            Id = l.Id, 
                            filename = l.BlockText, 
                            BlockTime = BREC.BlockTime, 
                            chrono = BREC.BlockTime,
                            markIn=0,
                            markOut = BREC.BlockTime,
                            startTime = 0
                        };
                    //    lst.Add(rr);

                    }
                
                });

              // lst.AddRange(lst2);
                string xml = getXMLheader(BREC.Name, (Int64)lst.Sum(l=>(Int64)l.chrono), w,h);
                xml += getXMLvideo(lst, w, h);
             //   xml += getXMLvideo(lst2);
              //  xml += getXMLvideoNoLink(lst2, lst.Count()+1);
                xml += getXMLMmiddle();
                xml += getXMLaudio(lst, 1);
               // xml += getXMLaudio(lst2, 1);
                xml += getXMLmiddleAudio();
                xml += getXMLaudio(lst, 2);
              //  xml += getXMLaudio(lst2, 2);
                xml += getXMLfooter();
                context.Response.Write(xml);
                
            }

            
           
        }
        private string getXMLvideoNoLink(List<dynamic> lst, int start,string w, string h)
        {
 
            var ret = "";
            int i = start;
            Int64 time = 0;
            lst.ForEach(l => {
                i++;
               
                ret += "                	<clipitem id=\"clipitem-video-" + i.ToString() + "\" frameBlend=\"FALSE\">\n";
                ret += "						<masterclipid>masterclip-" + i.ToString() + "</masterclipid>\n";
                ret += "						<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret+="						<enabled>TRUE</enabled>\n";
ret += "						<duration>" +Convert.ToString(l.BlockTime * 25) + "</duration>\n";
ret+="						<rate>\n";
ret+="							<timebase>25</timebase>\n";
ret+="							<ntsc>FALSE</ntsc>\n";
ret+="						</rate>\n";
//ret += "						<start>"+(time*25).ToString()+"</start>\n";
ret += "						<start>" + (l.startTime * 25).ToString() + "</start>\n";
time += Convert.ToInt64( l.BlockTime);
//                            ret += "<end>" + (time*25).ToString() + "</end>\n";
ret += "<end>" + Convert.ToString((l.chrono + l.startTime) * 25) + "</end>\n";
                            ret += "<in>" + (l.startTime * 25).ToString() + "</in>\n";
                            ret += "						<out>" + Convert.ToString((l.chrono+l.startTime  )* 25 ) + "</out>\n";
ret+="						<alphatype>black</alphatype>\n";
ret+="						<pixelaspectratio>square</pixelaspectratio>\n";
ret+="						<anamorphic>FALSE</anamorphic>\n";
ret += "						<file id=\"file-" + i.ToString() + "\">\n";
ret += "							<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret += "							<pathurl>file:" + (l.filename.Replace("\\", "/").Replace("10.0.5.", "192.168.1.")) + "</pathurl>\n";
ret+="							<rate>\n";
ret+="								<timebase>25</timebase>\n";
ret+="								<ntsc>FALSE</ntsc>\n";
ret+="							</rate>\n";
ret += "							<duration>" + Convert.ToString(l.BlockTime * 25) + "</duration>\n";
ret+="							<timecode>\n";
ret+="								<rate>\n";
ret+="									<timebase>25</timebase>\n";
ret+="									<ntsc>FALSE</ntsc>\n";
ret+="								</rate>\n";
ret+="								<string>00:00:00:00</string>\n";
ret+="								<frame>0</frame>\n";
ret+="								<displayformat>NDF</displayformat>\n";
ret+="							</timecode>\n";
ret+="							<media>\n";
ret+="								<video>\n";
ret += "									<duration>" + Convert.ToString(l.BlockTime * 25) + "</duration>\n";
ret+="									<samplecharacteristics>\n";
ret+="										<rate>\n";
ret+="											<timebase>25</timebase>\n";
ret+="											<ntsc>FALSE</ntsc>\n";
ret+="										</rate>\n";
ret+="										<width>"+w+"</width>\n";
ret+="										<height>"+h+"</height>\n";
ret+="										<anamorphic>FALSE</anamorphic>\n";
ret+="										<pixelaspectratio>square</pixelaspectratio>\n";
ret+="										<fielddominance>upper</fielddominance>\n";
ret+="									</samplecharacteristics>\n";
ret+="								</video>\n";
ret+="								<audio>\n";
ret+="									<samplecharacteristics>\n";
ret+="										<depth>16</depth>\n";
ret+="										<samplerate>48000</samplerate>\n";
ret+="									</samplecharacteristics>\n";
//ret+="									<numOutputChannels>2</numOutputChannels>\n";
ret += "									<channelcount>2</channelcount>\n";
//ret+="									<audiochannel>\n";
//ret+="										<sourcechannel>1</sourcechannel>\n";
//ret+="										<channellabel>discrete</channellabel>\n";
//ret+="									</audiochannel>\n";
ret+="								</audio>\n";
ret+="							</media>\n";
ret+="						</file>\n";
ret+="					</clipitem>\n";

            });

            return ret;
        }
        private string getXMLvideo(List<dynamic> lst, string w, string h)
        {
            var ret = "";
            int i = 0;
            Int64 time = 0;
            lst.ForEach(l => {
                i++;
              
                ret += "                	<clipitem id=\"clipitem-video-" + i.ToString() + "\" frameBlend=\"FALSE\">\n";
                ret += "						<masterclipid>masterclip-" + i.ToString() + "</masterclipid>\n";
                ret += "						<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret+="						<enabled>TRUE</enabled>\n";
ret += "						<duration>" +( l.chrono*25).ToString() + "</duration>\n";
ret+="						<rate>\n";
ret+="							<timebase>25</timebase>\n";
ret+="							<ntsc>FALSE</ntsc>\n";
ret+="						</rate>\n";
//ret += "						<start>"+(time*25).ToString()+"</start>\n";
                            time += l.chrono;
//ret += "						<end>" + (time*25).ToString() + "</end>\n";
                            ret += "						<start>" + (l.startTime * 25).ToString() + "</start>\n";
                            ret += "						<end>" + ((l.chrono + l.startTime) * 25).ToString() + "</end>\n";

 ret += "						<in>" + (l.startTime * 25).ToString() + "</in>\n";
 ret += "						<out>" + ((l.chrono + l.startTime) * 25).ToString() + "</out>\n";
ret+="						<alphatype>black</alphatype>\n";
if (Convert.ToInt16(w) < 1000)
{
    ret += "						<pixelaspectratio>PAL-601</pixelaspectratio>\n";
    ret += "						<anamorphic>TRUE</anamorphic>\n";
}
else
{
    ret += "						<pixelaspectratio>square</pixelaspectratio>\n";
    ret += "						<anamorphic>FALSE</anamorphic>\n";
}
ret += "						<file id=\"file-" + i.ToString() + "\">\n";
ret += "							<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret += "							<pathurl>file:" + (l.filename.Replace("\\", "/").Replace("10.0.5.", "192.168.1.")) + "</pathurl>\n";
ret+="							<rate>\n";
ret+="								<timebase>25</timebase>\n";
ret+="								<ntsc>FALSE</ntsc>\n";
ret+="							</rate>\n";
ret += "							<duration>" + (l.chrono * 25).ToString() + "</duration>\n";
ret+="							<timecode>\n";
ret+="								<rate>\n";
ret+="									<timebase>25</timebase>\n";
ret+="									<ntsc>FALSE</ntsc>\n";
ret+="								</rate>\n";
ret+="								<string>00:00:00:00</string>\n";
ret+="								<frame>0</frame>\n";
ret+="								<displayformat>NDF</displayformat>\n";
ret+="							</timecode>\n";
ret+="							<media>\n";
ret+="								<video>\n";
ret += "									<duration>" + (l.chrono*25).ToString() + "</duration>\n";
ret+="									<samplecharacteristics>\n";
ret+="										<rate>\n";
ret+="											<timebase>25</timebase>\n";
ret+="											<ntsc>FALSE</ntsc>\n";
ret+="										</rate>\n";
ret+="										<width>"+w+"</width>\n";
ret+="										<height>"+h+"</height>\n";
if (Convert.ToInt16(w) < 1000)
{
    ret += "										<anamorphic>TRUE</anamorphic>\n";
    ret += "										<pixelaspectratio>PAL-601</pixelaspectratio>\n";
    ret += "										<fielddominance>lower</fielddominance>\n";
}
else
{
    ret += "										<anamorphic>FALSE</anamorphic>\n";
    ret += "										<pixelaspectratio>square</pixelaspectratio>\n";
    ret += "										<fielddominance>upper</fielddominance>\n";
}
ret+="									</samplecharacteristics>\n";
ret+="								</video>\n";
                /////////////
ret += "								<audio>\n";
ret += "									<samplecharacteristics>\n";
ret += "										<depth>16</depth>\n";
ret += "										<samplerate>48000</samplerate>\n";
ret += "									</samplecharacteristics>\n";
ret += "									<channelcount>1</channelcount>\n";
ret += "									<layout>mono</layout>\n";
ret += "									<audiochannel>\n";
ret += "									<sourcechannel>1</sourcechannel>\n";
ret += "									<channellabel>discrete</channellabel>\n";
ret += "									</audiochannel>\n";
ret += "								</audio>\n";
/////////////
ret += "								<audio>\n";
ret += "									<samplecharacteristics>\n";
ret += "										<depth>16</depth>\n";
ret += "										<samplerate>48000</samplerate>\n";
ret += "									</samplecharacteristics>\n";
ret += "									<channelcount>1</channelcount>\n";
ret += "									<layout>mono</layout>\n";
ret += "									<audiochannel>\n";
ret += "									<sourcechannel>2</sourcechannel>\n";
ret += "									<channellabel>discrete</channellabel>\n";
ret += "									</audiochannel>\n";
ret += "								</audio>\n";
//ret += "								<audio>\n";
                //////////////
                /*
ret+="								<audio>\n";
ret+="									<samplecharacteristics>\n";
ret+="										<depth>16</depth>\n";
ret+="										<samplerate>48000</samplerate>\n";
ret+="									</samplecharacteristics>\n";
ret+="									<channelcount>1</channelcount>\n";
ret+="									<layout>mono</layout>\n";
ret+="									<audiochannel>\n";
ret+="										<sourcechannel>1</sourcechannel>\n";
ret+="										<channellabel>discrete</channellabel>\n";
ret+="									</audiochannel>\n";
ret+="								</audio>\n";
ret+="								<audio>\n";
                
ret+="									<samplecharacteristics>\n";
ret+="										<depth>16</depth>\n";
ret+="										<samplerate>48000</samplerate>\n";
ret+="									</samplecharacteristics>\n";
ret+="									<channelcount>1</channelcount>\n";
ret+="									<layout>mono</layout>\n";
ret+="									<audiochannel>\n";
ret+="										<sourcechannel>2</sourcechannel>\n";
ret+="										<channellabel>discrete</channellabel>\n";
ret+="									</audiochannel>\n";
ret+="								</audio>\n";
                 */
ret+="							</media>\n";
ret+="						</file>\n";
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-video-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>video</mediatype>\n";
ret+="							<trackindex>1</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="						</link>\n";
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-audio1-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>1</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="							<groupindex>1</groupindex>\n";
ret+="						</link>\n";
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-audio2-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>2</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="							<groupindex>1</groupindex>\n";
ret+="						</link>\n";
ret+="					</clipitem>\n";

            });

            return ret;
        }
        private string getXMLaudio(List<dynamic> lst,int n)
        {
            var ret = "";
            int i = 0;
            Int64 time = 0;
            lst.ForEach(l => {
                i++;/*
ret+="                <clipitem id=\"clipitem-"+(lst.Count+i).ToString()+"\" >\n";
ret+="						<masterclipid>masterclip-"+i.ToString()+"</masterclipid>\n";
ret += "						<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret+="						<enabled>TRUE</enabled>\n";
ret+="						<duration>"+l.chrono.ToString()+"</duration>\n";
ret+="						<rate>\n";
ret+="							<timebase>25</timebase>\n";
ret+="							<ntsc>FALSE</ntsc>\n";
ret+="						</rate>\n";
ret += "						<start>" + time.ToString() + "</start>\n";
                                        time += l.chrono;
ret += "						<end>" + time.ToString() + "</end>\n";
ret += "						<file id=\"file-" + i.ToString() + "\"/>\n";
ret+="						<sourcetrack>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>1</trackindex>\n";
ret+="						</sourcetrack>\n";
ret+="					</clipitem>\n";
            });*/

ret += "                <clipitem id=\"clipitem-audio" + n.ToString() + "-" + i.ToString() + "\" frameBlend=\"FALSE\" PannerCurrentValue=\"0.5\" PannerKeyframes=\"\" PannerStartKeyframe=\"-91445760000000000,0.5,0,0,0,0,0,0\" PannerIsInverted=\"true\" PannerName=\"Pan\">\n";
ret += "						<masterclipid>masterclip-" + i.ToString() + "</masterclipid>\n";
ret += "						<name>" + System.IO.Path.GetFileName(l.filename) + "</name>\n";
ret+="						<enabled>TRUE</enabled>\n";
ret += "						<duration>"+( l.chrono*25).ToString()+"</duration>\n";
ret+="						<rate>\n";
ret+="							<timebase>25</timebase>\n";
ret+="							<ntsc>FALSE</ntsc>\n";
ret+="						</rate>\n";
//ret += "						<start>" + (time*25).ToString() + "</start>\n";
                                    time += l.chrono;
//ret += "						<end>" + (time*25).ToString() + "</end>\n";
                                    ret += "						<start>" + (l.startTime * 25).ToString() + "</start>\n";
                                    ret += "						<end>" + ((l.chrono + l.startTime) * 25).ToString() + "</end>\n";
ret += "						<in>" + (l.startTime * 25).ToString() + "</in>\n";
ret += "						<out>" + ((l.chrono + l.startTime) * 25).ToString() + "</out>\n";
ret += "						<file id=\"file-" + i.ToString() + "\"/>\n";
ret+="						<sourcetrack>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>"+n.ToString()+"</trackindex>\n";
ret+="						</sourcetrack>\n";
 ret+="               <outputs>\n";
ret+="					<group>\n";
ret+="						<index>1</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>1</index>\n";
ret+="						</channel>\n";
ret+="					</group>\n";
ret+="					<group>\n";
ret+="						<index>2</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>2</index>\n";
ret+="						</channel>\n";
ret += "					</group>\n";
ret += "               </outputs>\n";

/*ret+="							<filter>\n";
ret+="								<effect>\n";
ret+="									<name>Audio Pan</name>\n";
ret+="									<effectid>audiopan</effectid>\n";
ret+="									<effectcategory>audiopan</effectcategory>\n";
ret+="									<effecttype>audiopan</effecttype>\n";
ret+="									<mediatype>audio</mediatype>\n";
ret+="									<parameter authoringApp=\"PremierePro\">\n";
ret+="										<parameterid>pan</parameterid>\n";
ret+="										<name>Pan</name>\n";
ret+="										<valuemin>-1</valuemin>\n";
ret+="										<valuemax>1</valuemax>\n";
ret+="										<value>0</value>\n";
ret+="									</parameter>\n";
ret+="								</effect>\n";
ret+="							</filter>\n";*/
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-video-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>video</mediatype>\n";
ret+="							<trackindex>1</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="						</link>\n";
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-audio1-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>1</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="							<groupindex>1</groupindex>\n";
ret+="						</link>\n";
ret+="						<link>\n";
ret += "							<linkclipref>clipitem-audio2-" + i.ToString() + "</linkclipref>\n";
ret+="							<mediatype>audio</mediatype>\n";
ret+="							<trackindex>2</trackindex>\n";
ret+="							<clipindex>1</clipindex>\n";
ret+="							<groupindex>1</groupindex>\n";
ret+="						</link>\n";
ret+="					</clipitem>\n";
                 });
            return ret;
        }
        private string getXMLheader(string name, Int64 dur, string w, string h)
        {
           var ret="<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
ret+="<!DOCTYPE xmeml>\n";
ret+="<xmeml version=\"4\">\n";
ret+="	<sequence id=\"sequence-2\" TL.SQAudioVisibleBase=\"0\" TL.SQVideoVisibleBase=\"0\" TL.SQVisibleBaseTime=\"0\" TL.SQAVDividerPosition=\"0.5\" TL.SQHideShyTracks=\"0\" TL.SQHeaderWidth=\"168\" Monitor.ProgramZoomOut=\"4267468800000\" Monitor.ProgramZoomIn=\"0\" TL.SQTimePerPixel=\"0.20000000000000001\" MZ.EditLine=\"0\" MZ.Sequence.PreviewFrameSizeHeight=\""+h+"\" MZ.Sequence.PreviewFrameSizeWidth=\""+w+"\" MZ.Sequence.AudioTimeDisplayFormat=\"200\" MZ.Sequence.PreviewRenderingClassID=\"1480868673\" MZ.Sequence.PreviewRenderingPresetCodec=\"403\" MZ.Sequence.PreviewRenderingPresetPath=\"EncoderPresets\\SequencePreview\\a4535090-90ae-4ff5-8766-19dfe29b78de\\AVC-Intra Class100 1080i.epr\" MZ.Sequence.PreviewUseMaxRenderQuality=\"false\" MZ.Sequence.PreviewUseMaxBitDepth=\"false\" MZ.Sequence.EditingModeGUID=\"a4535090-90ae-4ff5-8766-19dfe29b78de\" MZ.Sequence.VideoTimeDisplayFormat=\"101\" MZ.WorkOutPoint=\"4267468800000\" MZ.WorkInPoint=\"0\">\n";
ret+="		<uuid>025324e1-3c0b-4f46-850c-c896bca377fe</uuid>\n";
ret+="		<duration>"+(dur*25).ToString()+"</duration>\n";
ret+="		<rate>\n";
ret+="			<timebase>25</timebase>\n";
ret+="			<ntsc>FALSE</ntsc>\n";
ret+="		</rate>\n";
ret += "		<name>" + name + "</name>\n";
ret+="		<media>\n";
ret+="			<video>\n";
ret+="				<format>\n";
ret+="               <samplecharacteristics>\n";
ret+="						<rate>\n";
ret+="							<timebase>25</timebase>\n";
ret+="							<ntsc>FALSE</ntsc>\n";
ret+="						</rate>\n";
ret+="						<codec>\n";
ret+="							<name>Apple ProRes 422</name>\n";
ret+="							<appspecificdata>\n";
ret+="								<appname>Final Cut Pro</appname>\n";
ret+="								<appmanufacturer>Apple Inc.</appmanufacturer>\n";
ret+="								<appversion>7.0</appversion>\n";
ret+="								<data>\n";
ret+="									<qtcodec>\n";
ret+="										<codecname>Apple ProRes 422</codecname>\n";
ret+="										<codectypename>Apple ProRes 422</codectypename>\n";
ret+="										<codectypecode>apcn</codectypecode>\n";
ret+="										<codecvendorcode>appl</codecvendorcode>\n";
ret+="										<spatialquality>1024</spatialquality>\n";
ret+="										<temporalquality>0</temporalquality>\n";
ret+="										<keyframerate>0</keyframerate>\n";
ret+="										<datarate>0</datarate>\n";
ret+="									</qtcodec>\n";
ret+="								</data>\n";
ret+="							</appspecificdata>\n";
ret+="						</codec>\n";
ret+="						<width>"+w+"</width>\n";
ret+="						<height>"+h+"</height>\n";
if (Convert.ToInt16(w) < 1000)
{

    ret += "										<pixelaspectratio>PAL-601</pixelaspectratio>\n";
    ret += "										<anamorphic>TRUE</anamorphic>\n";
    ret += "   <fielddominance>lower</fielddominance>\n";
}
else
{
    ret += "						<anamorphic>FALSE</anamorphic>\n";
    ret += "						<pixelaspectratio>square</pixelaspectratio>\n";
     ret += "   <fielddominance>upper</fielddominance>\n";
}
					
ret+="						<colordepth>24</colordepth>\n";
ret+="					</samplecharacteristics>\n";
ret+="				</format>\n";
ret+="				<track TL.SQTrackShy=\"0\" TL.SQTrackExpandedHeight=\"25\" TL.SQTrackExpanded=\"0\" MZ.TrackTargeted=\"1\">\n";
            return ret;
        }
        private string getXMLMmiddle()
        {
            var ret = "";
ret+="          <enabled>TRUE</enabled>\n";
ret+="					<locked>FALSE</locked>\n";
ret+="				</track>\n";
ret+="			</video>\n";
ret+="			<audio>\n";
ret += "	           <numOutputChannels>2</numOutputChannels>";
ret+="				<format>\n";
ret+="					<samplecharacteristics>\n";
ret+="						<depth>16</depth>\n";
ret+="						<samplerate>48000</samplerate>\n";
ret+="					</samplecharacteristics>\n";
ret+="				</format>\n";
/*ret+="				<outputs>\n";
ret+="					<group>\n";
ret+="						<index>1</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>1</index>\n";
ret+="						</channel>\n";
ret+="					</group>\n";
ret+="					<group>\n";
ret+="						<index>2</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>2</index>\n";
ret+="						</channel>\n";
ret+="					</group>\n";
ret+="				</outputs>\n";*/
//ret +="				<outputs>\n";

ret +="				<track TL.SQTrackAudioKeyframeStyle=\"0\" TL.SQTrackShy=\"0\" TL.SQTrackExpandedHeight=\"25\" TL.SQTrackExpanded=\"0\" MZ.TrackTargeted=\"1\" PannerCurrentValue=\"0.5\" PannerIsInverted=\"true\" PannerStartKeyframe=\"-91445760000000000,0.5,0,0,0,0,0,0\" PannerName=\"Balance\" currentExplodedTrackIndex=\"0\" totalExplodedTrackCount=\"1\" premiereTrackType=\"Stereo\">\n";
            return ret;
        }
        private string getXMLmiddleAudio()
        {
            var ret = "";
ret+="            <enabled>TRUE</enabled>\n";
ret+="					<locked>FALSE</locked>\n";
ret+="					<outputchannelindex>1</outputchannelindex>\n";
ret+="				</track>\n";
ret+="				<track TL.SQTrackAudioKeyframeStyle=\"0\" TL.SQTrackShy=\"0\" TL.SQTrackExpandedHeight=\"25\" TL.SQTrackExpanded=\"0\" MZ.TrackTargeted=\"1\" PannerCurrentValue=\"0.5\" PannerIsInverted=\"true\" PannerStartKeyframe=\"-91445760000000000,0.5,0,0,0,0,0,0\" PannerName=\"Balance\" currentExplodedTrackIndex=\"0\" totalExplodedTrackCount=\"1\" premiereTrackType=\"Stereo\">\n";
             return ret;
        }
         private string getXMLfooter()
        {
            var ret = "";
ret+="            <enabled>TRUE</enabled>\n";
ret+="					<locked>FALSE</locked>\n";
ret+="					<outputchannelindex>1</outputchannelindex>\n";
ret+="				</track>\n";
ret+="			</audio>\n";
/* ret+="            <audio>\n";
 ret+="                <numOutputChannels>2</numOutputChannels>\n";
ret+="				<format>\n";
ret+="					<samplecharacteristics>\n";
ret+="						<depth>16</depth>\n";
ret+="						<samplerate>48000</samplerate>\n";
ret+="					</samplecharacteristics>\n";
ret+="				</format>\n";
ret+="				<outputs>\n";
ret+="					<group>\n";
ret+="						<index>1</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>1</index>\n";
ret+="						</channel>\n";
ret+="					</group>\n";
ret+="					<group>\n";
ret+="						<index>2</index>\n";
ret+="						<numchannels>1</numchannels>\n";
ret+="						<downmix>0</downmix>\n";
ret+="						<channel>\n";
ret+="							<index>2</index>\n";
ret+="						</channel>\n";
ret+="					</group>\n";
ret+="				</outputs>\n";
ret += "                </audio>\n";
 */
ret+="		</media>\n";
ret+="		<timecode>\n";
ret+="			<rate>\n";
ret+="				<timebase>25</timebase>\n";
ret+="				<ntsc>FALSE</ntsc>\n";
ret+="			</rate>\n";
ret+="			<string>00:00:00:00</string>\n";
ret+="			<frame>0</frame>\n";
ret+="			<displayformat>NDF</displayformat>\n";
ret+="		</timecode>\n";
ret+="	</sequence>\n";
ret+="</xmeml>\n";
             return ret;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class EDLRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new EDL() { RequestContext = requestContext }; ;
        }
    }
}