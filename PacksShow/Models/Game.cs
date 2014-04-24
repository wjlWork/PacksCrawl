using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacksShow.Models
{
    [Map("game")]
    public class Game : DataFieldBase
    {

        [Map("id")]
        public int Id { get; set; }

        [Map("name")]
        public string Name { get; set; }

        [Map("icon")]
        public string Icon { get; set; }

        [Map("ios_game_url")]
        public string IosGameUrl { get; set; }

        [Map("android_game_url")]
        public string AndroidGameUrl { get; set; }

        [Map("ios_helper_url")]
        public string IosHelperUrl { get; set; }

        [Map("android_helper_url")]
        public string AndroidHelperUrl { get; set; }

        [Map("wait_for_review")]
        public int WaitForReview { get; set; }

        [Map("alias")]
        public string Alias { get; set; }

        [Map("like_count")]
        public int LikeCount { get; set; }

        [Map("status")]
        public int Status { get; set; }

        [Map("group_prompt")]
        public string GroupPrompt { get; set; }

        [Map("is_block")]
        public int IsBlock { get; set; }

        [Map("block_id")]
        public int BlockId { get; set; }

        [Map("zone_image")]
        public string ZoneImage { get; set; }

        [Map("zone_type")]
        public int ZoneType { get; set; }

        [Map("zone_version")]
        public int ZoneVersion { get; set; }

        [Map("zone_url")]
        public string ZoneUrl { get; set; }

        [Map("zone_define")]
        public string ZoneDefine { get; set; }

        [Map("zone_header_image")]
        public string ZoneHeaderImage { get; set; }

        [Map("zone_list_image")]
        public string ZoneListImage { get; set; }


        public static Game Get(string name)
        {
            return Table.Object<Game>()
                        .Where(m => m.Name, name)
                        .SelectList().FirstOrDefault();
        }
    }
}