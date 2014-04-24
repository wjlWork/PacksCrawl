using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.QuantityMonitoring
{
    [Map("statis")]
    public class Statis:TracableObject
    {
        [Map("id")]
        public int Id {get;set;}
        [Map("packslistconfig_id")]
        public int ListConfigId { get; set; }
        [Map("send_count")]
        public string SendCount { get; set; }
        [Map("star_time")]
        public DateTime StarTime { get; set; }
        [Map("current_time")]
        public DateTime CurrentTime { get; set; }

        private string SendAmount(int listconfigid)
        {
            var pcks = Table.Object<Packs_Statistical>()
                            .Where(m => m.ListConfigId, listconfigid)
                            .SelectList();

            int count = 0;
            foreach (var pck in pcks)
            {
                if (!string.IsNullOrWhiteSpace(pck.GrantCount))
                {
                    count += int.Parse(pck.GrantCount);
                }
            }
            return count.ToString();
        }

        //public DateTime GetStarTime()
        //{

        //}

        public Statis CreateStatis(int listconfigid)
        {
            var st = Table.Object<Statis>()
                          .Where(m => m.ListConfigId, listconfigid)
                          .SelectList().FirstOrDefault();

            if (st != null)
            {
                st.SendCount = SendAmount(listconfigid);
                st.StarTime = st.CurrentTime;
                st.CurrentTime = DateTime.Now;
                DataAccess.Update(st);

            }
            else
            {
                Statis sta = new Statis
                {
                    ListConfigId = listconfigid,
                    SendCount = SendAmount(listconfigid),
                    StarTime = DateTime.Now,
                    CurrentTime = DateTime.Now,
                };
                DataAccess.Insert(sta);

                st = sta;
            }

            return st;
        }

        public List<Statis> CreateStatisLs(List<PacksListConfig_Statistical> configs)
        {
            Statis sta = new Statis();
            List<Statis> StaList = new List<Statis>();
            foreach (var c in configs)
            {               
                sta = CreateStatis(c.Id);
                StaList.Add(sta);
            }
            return StaList;
        }


    }
}
