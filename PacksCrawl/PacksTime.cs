using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PacksModels;
using System.Text.RegularExpressions;


namespace PacksCrawl
{
    class PacksTime
    {

        //获得礼包开始时间
        public static string StratTime(HtmlDocument doc, PacksConfig config,string url)
        {
            HtmlNode node = null;
            if (config.StratTimeXpath != null && config.StratTimeXpath != "value")
            {
                node = HtmlAgilityPackHelper.GetNode(doc, config.StratTimeXpath);
            }
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }
            string StratStr = UpdateTimeStr(nodeText, url);
            return StratStr;
        }


        //获得礼包有效期(默认结束时间)
        public static string PacksValid(HtmlDocument doc, PacksConfig config,string url)
        {
            HtmlNode node = null;
            if (config.PacksValidXpath != null)
            {
                node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
            }
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }

            return nodeText;
        }


        //获得礼包有效期
        public static List<DateTime> _Final_Time(HtmlDocument doc, PacksConfig config, string url)
        {
            List<DateTime> times = new List<DateTime>();
            DateTime Strat_Time = DateTime.Parse("1000-01-01 00:00:00");
            DateTime End_Time = DateTime.Parse("1000-01-01 00:00:00");

            string _stratTime = StratTime(doc, config, url);
            string _endTime = EndTime(PacksValid(doc, config, url), url);

            if (_stratTime != null && !string.IsNullOrEmpty(_stratTime) && _stratTime != "3000-01-01 00:00:00")
            {
                _stratTime = UpdateTimeStr(_stratTime, url);
                if (_stratTime != null && !string.IsNullOrEmpty(_stratTime) && _stratTime != "")
                {
                    Strat_Time = DateTime.Parse(_stratTime);
                    times.Add(Strat_Time);
                }
                _endTime = UpdateTimeStr(_endTime, url);
                if (_endTime != null && !string.IsNullOrEmpty(_endTime) && _endTime != "")
                {
                    End_Time = DateTime.Parse(_endTime);
                    times.Add(End_Time);
                }
            }
            else
            {
                times = _Packs_Time(doc, config, url);
            }
                    
            return times;
        }

        //获得礼包有效期
        public static List<DateTime> _Packs_Time(HtmlDocument doc, PacksConfig config, string url)
        {
            HtmlNode node = null;
            List<DateTime> times = new List<DateTime>();
            if (config.PacksValidXpath != null)
            {
                node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
            }
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }

            times = ParseTime(nodeText, url);

            return times;
        }


        //将时间字符串转换成时间类型
        private static string EndTime(string timeStr, string url)
        {
            string[] timeStrs = new string[4];

            if (timeStr.Contains("至") || timeStr.Contains("到"))
            {
                if (timeStr.Contains("至"))
                {
                    timeStrs = timeStr.Split('至');
                }
                else if (timeStr.Contains("到"))
                {
                    timeStrs = timeStr.Split('到');
                }
            }

            return timeStrs[1];
        }



        //将时间字符串转换成时间类型
        private static List<DateTime> ParseTime(string timeStr, string url)
        {
            List<DateTime> times = new List<DateTime>();
            DateTime StratTime = DateTime.Parse("1000-01-01 00:00:00");
            DateTime EndTime = DateTime.Parse("1000-01-01 00:00:00");
            //分割成两个时间

            try
            {
                if (timeStr.Contains("至") || timeStr.Contains("到"))
                {
                    //Regex regex = new Regex(@"([1-9]\d*\.?\d*)|(0\.\d*[1-9])");
                    string[] timeStrs = new string[4];
                    if (timeStr.Contains("至"))
                    {
                        timeStrs = timeStr.Split('至');
                    }
                    else if (timeStr.Contains("到"))
                    {
                        timeStrs = timeStr.Split('到');
                    }

                    string StartStr = UpdateTimeStr(timeStrs[0], url);
                    if (StartStr != null && !string.IsNullOrEmpty(StartStr) && StartStr != "")
                    {
                        StratTime = DateTime.Parse(StartStr);
                    }

                    string EndStr = UpdateTimeStr(timeStrs[1], url);
                    if (EndStr != null && !string.IsNullOrEmpty(EndStr) && EndStr != "")
                    {
                        EndTime = DateTime.Parse(EndStr);
                    }
                }
                else
                {
                    if (timeStr.Contains("有效") || timeStr.Contains("结束"))
                    {
                        string EndStr = UpdateTimeStr(timeStr, url);
                        //string EndStr = timeStr.Replace("%", "").Replace("：", "").Replace("&nbsp;", "").Replace("年", "-").Replace("月", "-");
                        //EndStr = System.Text.RegularExpressions.Regex.Replace(EndStr, @"[\u4e00-\u9fa5_a-zA-Z]*", "");
                        if (EndStr != null && !string.IsNullOrEmpty(EndStr) && EndStr != "")
                        {
                            EndTime = DateTime.Parse(EndStr);
                        }
                    }
                    else
                    {
                        string StartStr = UpdateTimeStr(timeStr, url);
                        if (StartStr != null && !string.IsNullOrEmpty(StartStr) && StartStr != "")
                        {
                            StratTime = DateTime.Parse(StartStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("获取礼包有效期时出错" + ex.ToString() + "：" + url, "Log\\error.log");//
            }

            times.Add(StratTime);
            times.Add(EndTime);
            return times;
        }


        private static string UpdateTimeStr(string TimeStr, string url)
        {
            string updateStr = "";
            try
            {
                updateStr = TimeStr.Replace("年", "-").Replace("月", "-");
                int indexnum = updateStr.IndexOf("2", 0);
                if (indexnum >= 0)
                {
                    updateStr = updateStr.Substring(indexnum, updateStr.Length - indexnum);
                    updateStr = System.Text.RegularExpressions.Regex.Replace(updateStr, @"[\u4e00-\u9fa5_a-zA-Z]*", "").Replace("：", "").Replace("(", "").Replace(")", "").Replace("\n", "").Trim();
                    //如果全为数字
                    //updateStr = "2014-03-05 05:00:02";
                    if (Regex.IsMatch(updateStr.Replace(" ", ""), @"^[+-]?\d*[.]?\d*$"))
                    {
                        updateStr = "3000-01-01 00:00:00";
                    }
                }
                else
                {
                    updateStr = "3000-01-01 00:00:00";
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("修正日期格式出错" + ex.ToString() + "：" + url, "Log\\error.log");//
                updateStr = "2000-01-01 00:00:00";

            }
            return updateStr;
        }




    }
}














   ////获得礼包有效期(默认结束时间)
   //     public static string PacksValid(HtmlDocument doc, PacksConfig config)
   //     {
   //         HtmlNode node = null;
   //         if (config.PacksValidXpath != null)
   //         {
   //             node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
   //         }
   //         string nodeText = "";
   //         if (node != null)
   //         {
   //             nodeText = node.InnerText;
   //         }

   //         return nodeText;
   //     }


   //     //获得礼包有效期
   //     public static List<DateTime> PacksValid1(HtmlDocument doc, PacksConfig config,string url)
   //     {
   //         HtmlNode node = null;
   //         List<DateTime> times = new List<DateTime>();
   //         if (config.PacksValidXpath != null)
   //         {
   //             node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
   //         }
   //         string nodeText = "";
   //         if (node != null)
   //         {
   //             nodeText = node.InnerText;
   //         }

   //         times = ParseTime(nodeText,url);

   //         return times;
   //     }

   //     //获得礼包有效期
   //     public static List<DateTime> _Packs_Time(HtmlDocument doc, PacksConfig config, string url)
   //     {
   //         HtmlNode node = null;
   //         List<DateTime> times = new List<DateTime>();
   //         if (config.PacksValidXpath != null)
   //         {
   //             node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
   //         }
   //         string nodeText = "";
   //         if (node != null)
   //         {
   //             nodeText = node.InnerText;
   //         }

   //         times = ParseTime(nodeText, url);

   //         return times;
   //     }

   //     //获得礼包开始时间
   //     public static string StratTime(HtmlDocument doc, PacksConfig config)
   //     {
   //         HtmlNode node = null;
   //         if (config.PacksValidXpath != null)
   //         {
   //             node = HtmlAgilityPackHelper.GetNode(doc, config.StratTimeXpath);
   //         }
   //         string nodeText = "";
   //         if (node != null)
   //         {
   //             nodeText = node.InnerText;
   //         }

   //         return nodeText;
   //     }


   //     //将时间字符串转换成时间类型
   //     private static List<DateTime> ParseTime(string timeStr,string url)
   //     {
   //         List<DateTime> times = new List<DateTime>();
   //         DateTime StratTime = DateTime.Parse("1000-01-01 00:00:00");
   //         DateTime EndTime = DateTime.Parse("1000-01-01 00:00:00");
   //         //分割成两个时间

   //         try
   //         {
   //             if (timeStr.Contains("至") || timeStr.Contains("到"))
   //             {
   //                 //Regex regex = new Regex(@"([1-9]\d*\.?\d*)|(0\.\d*[1-9])");
   //                 string[] timeStrs = new string[4];
   //                 if (timeStr.Contains("至"))
   //                 {
   //                     timeStrs = timeStr.Split('至');
   //                 }
   //                 else if (timeStr.Contains("到"))
   //                 {
   //                     timeStrs = timeStr.Split('到');
   //                 }

   //                 string StartStr = UpdateTimeStr(timeStrs[0], url);
   //                 if (StartStr != null && !string.IsNullOrEmpty(StartStr) && StartStr != "")
   //                 {
   //                     StratTime = DateTime.Parse(StartStr);
   //                 }

   //                 string EndStr = UpdateTimeStr(timeStrs[1], url);
   //                 if (EndStr != null && !string.IsNullOrEmpty(EndStr) && EndStr != "")
   //                 {
   //                     EndTime = DateTime.Parse(EndStr);
   //                 }
   //             }
   //             else
   //             {
   //                 if (timeStr.Contains("有效") || timeStr.Contains("结束"))
   //                 {
   //                     string EndStr = UpdateTimeStr(timeStr,url);
   //                     //string EndStr = timeStr.Replace("%", "").Replace("：", "").Replace("&nbsp;", "").Replace("年", "-").Replace("月", "-");
   //                     //EndStr = System.Text.RegularExpressions.Regex.Replace(EndStr, @"[\u4e00-\u9fa5_a-zA-Z]*", "");
   //                     if (EndStr != null && !string.IsNullOrEmpty(EndStr) && EndStr != "")
   //                     {
   //                         EndTime = DateTime.Parse(EndStr);
   //                     }
   //                 }
   //                 else
   //                 {
   //                     string StartStr = UpdateTimeStr(timeStr, url);
   //                     if (StartStr != null && !string.IsNullOrEmpty(StartStr) && StartStr != "")
   //                     {
   //                         StratTime = DateTime.Parse(StartStr);
   //                     }
   //                 }
   //             }
   //         }
   //         catch (Exception ex)
   //         {
   //             Helpers.WriteLog("获取礼包有效期时出错" + ex.ToString() + "：" + url, "Log\\error.log");//
   //         }


   //         times.Add(StratTime);
   //         times.Add(EndTime);
   //         return times;
   //     }


   //     private static string UpdateTimeStr(string TimeStr,string url)
   //     {
   //         string updateStr = "";
   //         try
   //         {
   //             updateStr = TimeStr.Replace("年", "-").Replace("月", "-");
   //             int indexnum = updateStr.IndexOf("2", 0);
   //             if (indexnum >=0)
   //             {
   //                 updateStr = updateStr.Substring(indexnum, updateStr.Length - indexnum);
   //                 updateStr = System.Text.RegularExpressions.Regex.Replace(updateStr, @"[\u4e00-\u9fa5_a-zA-Z]*", "").Replace("：", "").Replace("(", "").Replace(")", "");
   //                 //如果全为数字
   //                 //updateStr = "2014-03-05 05:00:02";
   //                 if (Regex.IsMatch(updateStr.Replace(" ",""), @"^[+-]?\d*[.]?\d*$"))
   //                 {
   //                     updateStr = "3000-01-01 00:00:00";
   //                 }
   //             }
   //             else
   //             {
   //                 updateStr = "3000-01-01 00:00:00";
   //             }
   //         }
   //         catch (Exception ex)
   //         {
   //             Helpers.WriteLog("修正日期格式出错" + ex.ToString() + "：" + url, "Log\\error.log");//
   //             updateStr = "2000-01-01 00:00:00";

   //         }
   //         return updateStr;
   //     }