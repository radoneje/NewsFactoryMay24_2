using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.PrintTemplates
{
    public class BlockScriptClass
    {
        public static string GenerateHTMLNewsTemplate(Int64 NewsId, Int32 TemplateId)
        {
            Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
            string t = dc.PrintTemplates.Where(tb => tb.id == TemplateId).First().news;

            PrintTemplates.PrintTemplatesDataClassesDataContext DCBL = new PrintTemplatesDataClassesDataContext();
            var b = DCBL.News.Where(bl => bl.id  == NewsId).First();

            t = t.Replace("$NewsId", b.id.ToString());
            t = t.Replace("$NewsName", replaceSpecialCh(b.Name));
            t = t.Replace("$NewsOwner", Utils.GetUserName(b.EditorId ));
            t = t.Replace("$NewsDate", b.NewsDate.ToString("dd:MM:yyyy HH:mm:ss"));
            t = t.Replace("$NewsDuration", Utils.GetTextTimeFromSeconds(b.Duration ));
            t = t.Replace("$NewsCronoTask", Utils.GetTextTimeFromSeconds(b.TaskTime  ));
            t = t.Replace("$NewsCronoCalk", Utils.GetTextTimeFromSeconds(b.CalcTime  ));
            t = t.Replace("$NewsChronoReal", Utils.GetTextTimeFromSeconds(b.NewsTime ));
            t = t.Replace("$NewsName", b.id.ToString() );
            t = t.Replace("$NewsSourceLabel", replaceSpecialCh(b.Cassete));
            t = t.Replace("$NewsTimeCode", Utils.GetTextTimeFromSeconds(b.Time_Code ));


            if (t.IndexOf("@BLOCK") >= 0)
            {
                string subblocktext = "";
                foreach (var chaildbl in dc.Blocks.Where(pb => pb.ParentId == 0 && pb.NewsId==b.id  && pb.deleted == false).OrderBy(bl=>bl.Sort))
                    subblocktext = subblocktext + GenerateHTMLBlockTemplate(chaildbl.Id, TemplateId);

                t = t.Replace("@BLOCK", subblocktext);

            }

         return t;
            /*
             * Name	Description
$NewsName	название выпуска
$NewsOwner	выпускающий редактор
$NewsDate	дата и время выхода выпуска в эфир
$NewsDuration	продолжительность выпуска
$NewsCronoTask	заданный хронометраж (сумма заданных хронометражей сюжетов)
$NewsCronoCalk	расчетный хронометраж (сумма расчетных хронометражей сюжетов)
$NewsChronoReal	реальный хронометраж (сумма реальных хронометражей сюжетов)
$NewsId	ID выпуска
$NewsSourceLabel	номер кассеты
$NewsTimeCode	таймкод
             * */
        }
        public static string replaceSpecialCh(string str)
        {
             var ret= str.Replace("\"", "&#34").Replace("'", "&#39;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br>").Replace("\r", "<br>").Replace("\n", "<br>");
            return ret;
            //  return  (new System.Text.RegularExpressions.Regex(@"\(\(NF\:\:[^\)]+\)\)")).Replace(ret, "((MEDIA))");
       }
        public static string GenerateHTMLBlockTemplate(Int64 BlockId, Int32 TemplateId)
        {
            try
            {
                Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
                string t = dc.PrintTemplates.Where(tb => tb.id == TemplateId).First().block;
                PrintTemplates.PrintTemplatesDataClassesDataContext DCBL= new PrintTemplatesDataClassesDataContext();
                var b =DCBL.vWeb_BlockForPrintTemplates.Where(bl => bl.Id  == BlockId).First();
                t = t.Replace("$BlockName", replaceSpecialCh(b.Name));
                t = t.Replace("$BlockText", replaceSpecialCh(b.BlockText));
                t = t.Replace("$BlockAutor", Utils.GetUserName(b.CreatorId));
                t = t.Replace("$BlockOperator", Utils.GetUserName(b.OperatorId ));
                t = t.Replace("$BlockJockey", Utils.GetUserName(b.JockeyId ));
                t = t.Replace("$BlockCutter", Utils.GetUserName(b.CutterId ));
                t = t.Replace("$BlockCronoReal", Utils.GetTextTimeFromSeconds(b.BlockTime));
                t = t.Replace("$BlockCronoTask", Utils.GetTextTimeFromSeconds(b.TaskTime ));
                t = t.Replace("$BlockCronoCalc", Utils.GetTextTimeFromSeconds(b.CalcTime ));
                t = t.Replace("$BlockReady", b.Ready.ToString());
                t = t.Replace("$BlockApprove", b.Approve.ToString());
                t = t.Replace("$BlockTypeId", b.BLockType.ToString());
                t = t.Replace("$BlockType", b.TypeName);
                t = t.Replace("$BlockDescription", replaceSpecialCh(b.Description));
                t = t.Replace("$BlockNumber", b.Sort.ToString());
                t = t.Replace("$BlockId", b.Id.ToString());
                t = t.Replace("$TextLang1", b.TextLang1);
                t = t.Replace("$TextLang2", b.TextLang2);
                t = t.Replace("$TextLang3", b.TextLang3);

                t = t.Replace("@BLOCKFLAG", "");

                if (t.IndexOf("@BLOCKDEPEND") >= 0)
                {
                    string subblocktext="";
                    foreach (var chaildbl in dc.Blocks.Where(pb => pb.ParentId == b.Id && pb.deleted == false).OrderBy(bl => bl.Sort))
                         subblocktext=subblocktext+GenerateHTMLSubBlockTemplate(chaildbl.Id , TemplateId);
                            
                     t = t.Replace("@BLOCKDEPEND", subblocktext);
              
                }
                return t;
            }
            catch (Exception ex)
            {
                return "Произошла ошибка: " + ex.Message;
            }
            return "";
            /*
             * Name	Description
@BLOCK	эта переменная заменяется
$BlockName	название сюжета
$BlockText	текст сюжета
$BlockAutor	автор
$BlockOperator	оператор
$BlockJockey	ведущий
$BlockCutter монтежер
$BlockCronoReal	реальный хронометраж
$BlockCronoTask	заданный хронометраж
$BlockCronoCalc	расчетный хронометраж
$BlockId	ID блока
$BlockNumber	Порядковый номер сюжета
$BlockReady	Галочка <готов>
$BlockApprove	галочка <принят>
$BlockTypeId	тип сюжета - ID
$BlockType	тип сюжета
$BlockDescription	комментарий
             * */
        }
        public static string GenerateHTMLSubBlockTemplate(Int64 BlockId, Int32 TemplateId)
        {
            Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
            string t = dc.PrintTemplates.Where(tb => tb.id == TemplateId).First().block;
            PrintTemplates.PrintTemplatesDataClassesDataContext DCBL = new PrintTemplatesDataClassesDataContext();
            var bb = DCBL.vWeb_BlockForPrintTemplates.Where(bl => bl.Id == BlockId);
            if (bb.Count() > 0)
            {
                var b = bb.First();
                t = t.Replace("$BlockDependName", replaceSpecialCh(b.Name));
                t = t.Replace("$BlockDependFile", replaceSpecialCh(b.BlockText));
                t = t.Replace("$BlockDependReady", b.Ready.ToString());
                t = t.Replace("$BlockDependApprove", b.Approve.ToString());
                t = t.Replace("$BlockDependNumber", b.Sort.ToString());
                t = t.Replace("$BlockDependSourceLabel", replaceSpecialCh(b.Description));
                t = t.Replace("$BlockDependTimeCode", Utils.GetTextTimeFromSeconds(b.BlockTime));
            }
            else
            {
                t = replaceSpecialCh(t);
                t = replaceSpecialCh(t);
                t = t.Replace("$BlockDependReady", "");
                t = t.Replace("$BlockDependApprove", "");
                t = t.Replace("$BlockDependNumber", "");
                t = t.Replace("$BlockDependSourceLabel", "");
                t = t.Replace("$BlockDependTimeCode", "");

            }


            return "";
            /*
             $BlockDependName
$BlockDependFile
$BlockDependReady
$BlockDependApprove
$BlockDependId
$BlockDependNumber
$BlockDependSourceLabel
$BlockDependTimeCode
             */
        }
    }
}