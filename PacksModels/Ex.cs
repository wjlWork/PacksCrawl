using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.Provider;
using Qmigh.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksModels
{
    public class Ex
    {
        public static void Init()
        {
            DataAccess.Register(new MySqlProvider(Configuration.GetConfigValue("conn")));
            //DataAccess.Register(typeof(Touch18.HelperSlider), new MySqlProvider(Configuration.GetConfigValue("conn18touch")));
            //DataAccess.Log = new StreamWriter(Server.MapPath("~/log/sql.log"), true);
        }
    }
}
