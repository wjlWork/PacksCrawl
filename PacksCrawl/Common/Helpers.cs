using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.Common
{
    public class Helpers
    {
        //记录log
        public static void WriteLog(string mesaage, string filename)
        {
            //取得或设置当前工作目录的完整限定路径
            string rootPath = Environment.CurrentDirectory;
            //保存路径
            string path = "";
            string[] paths = filename.Split('\\');
            for (int i = 0; i < paths.Length - 1; i++)
            {
                path += paths[i] + "\\";

            }
            path = rootPath + "\\" + path;
            //判断路径是否存在，不存在则创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //文件路径
            path = Environment.CurrentDirectory + "\\" + filename;

            FileStream stream = new FileStream(path, FileMode.Append);
            string data = DateTime.Now + " : " + mesaage;
            StreamWriter writer = new StreamWriter(stream, Encoding.Default);
            writer.WriteLine(data);
            writer.Close();
            stream.Close();
        }
    }
}
