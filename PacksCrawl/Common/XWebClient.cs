using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.Common
{
    public class XWebClient : WebClient
    {
        private int _timeOut = 10;
        public int Timeout
        {
            get
            {
                return _timeOut;
            }
            set
            {
                if (value <= 0)
                    _timeOut = 10;
                _timeOut = value;
            }
        }

        //重写Webclient的GetWebRequest方法，实现解压缩，为HttpWebRequest添加请求超时及读写超时
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            //实现解压缩
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            //为HttpWebRequest添加请求超时及读写超时
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;

            return request;
        }


    }
}
