using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacksShow.Models
{
    public class ShowCounts
    {
        public int Id { get; set; }
        public int Listconfig_Id { get; set; }
        public string webName { get; set; }
        public string Total { get; set; }
        public string Surplus { get; set; }
        public string GrantCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CurrentTime { get; set; }

        public List<ShowCounts> Gets()
        {
            List<ShowCounts> scs = new List<ShowCounts>();
            ShowCounts sc = new ShowCounts ();
            List<ListConfig> lcfigs = ListConfig.GetConfigs();
            if (lcfigs != null)
            {
                foreach (var lc in lcfigs)
                {
                    List<Packs_Statistical> pcks = Packs_Statistical.GetPcks(lc.Id);

                    if (pcks != null)
                    {
                        sc.Listconfig_Id = lc.Id;
                        sc.webName = lc.WebName;
                        if (pcks[0].Total == "-1" || pcks[0].Total.Contains("%"))
                        {
                            sc.Total = "总数未知";
                        }
                        else
                        {
                            foreach (var p in pcks)
                            {
                                sc.Total += p.Total;
                                sc.GrantCount += p.GrantCount;
                            }
                        }
                        sc.StartTime = pcks[0].StartTime;
                        sc.CurrentTime = pcks[0].CurrentTime;
                        scs.Add(sc);
                    }
                }
            }
            return scs;
        }



    }
}