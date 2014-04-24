using PacksCrawl.QuantityMonitoring;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.Provider;
using Qmigh.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.Common
{
    public class Ex
    {
        public static void Init()
        {
            DataAccess.Register(new MySqlProvider(Configuration.GetConfigValue("conn")));
            DataAccess.Register(typeof(PacksConfig_Statistical), new MySqlProvider(Configuration.GetConfigValue("conn1")));
            DataAccess.Register(typeof(PacksListConfig_Statistical), new MySqlProvider(Configuration.GetConfigValue("conn1")));
            DataAccess.Register(typeof(Packs_Statistical), new MySqlProvider(Configuration.GetConfigValue("conn1")));
            DataAccess.Register(typeof(Statis), new MySqlProvider(Configuration.GetConfigValue("conn1")));
            //DataAccess.Register(typeof(Touch18.HelperSlider), new MySqlProvider(Configuration.GetConfigValue("conn18touch")));
            //DataAccess.Log = new StreamWriter(Server.MapPath("~/log/sql.log"), true);
        }
    }
}
