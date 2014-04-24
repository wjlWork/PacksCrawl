using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList
{
    public class OPHelper
    {
        public static string getTimeText(string timeText)
        {
            string _time = null;
            if (timeText.IndexOf("-", 1) == 2 && timeText != null)
            {
                string year = DateTime.Now.Year.ToString();
                _time = year + "-" + timeText;
            }
            else if (timeText.Contains("今日") || timeText.Contains("今天"))
            {
                string ytime = DateTime.Now.ToString("yyyy-MM-dd");
                _time = ytime + timeText.Replace("今日", "").Replace("今天", "");
            }
            else if (timeText.Contains("时间"))
            {
                _time = null;
            }
            else
            {
                _time = timeText;
            }
            return _time.Trim();
        }




        //修正路径，将相对路径更新为绝对路径
        public static string UpdateUrl(string sour_url, string herf)
        {
            string url = "";

            if (herf.Contains("http://"))
            {
                url = herf;
            }
            else
            {
                if (herf.Substring(0, 1) == "/")
                {
                    //主页
                    string[] us = sour_url.Split('/');
                    string u = us[0] + "/" + us[1] + "/" + us[2];
                    //
                    url = u + herf;
                }
                else if (herf.Substring(0, 1) == ".")
                {
                    if (herf.Substring(0, 2) == "./")
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 1; i++)
                        {
                            u += us[i] + "/";
                        }
                        int count = herf.Length;
                        url = u + herf.Substring(2, count - 2);

                    }
                    else if (herf.Substring(0, 3) == "../")
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 2; i++)
                        {
                            u += us[i] + "/";
                        }
                        int count = herf.Length;
                        url = u + herf.Substring(3, count - 3);
                    }
                }
                else
                {
                    if (herf.Contains(".html") || herf.Contains(".php") || herf.Contains(".aspx") || herf.Contains("/"))
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 1; i++)
                        {
                            u += us[i] + "/";
                        }
                        url = u + herf;
                    }
                }
            }

            if (url != null && !string.IsNullOrEmpty(url))
            {
                if (url.Contains("&amp;"))
                {
                    string[] np = url.Split(';');
                    url = np[0].Replace("amp", np[1]);
                }
            }
            return url;
        }



    }
}
