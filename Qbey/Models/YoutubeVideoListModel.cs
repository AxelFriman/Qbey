using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{
    public class YoutubeVideoList
    {
        public List<YoutubeVideoListItems> items { get; set; }
    }

    public class YoutubeVideoListItems
    {
        public YoutubeVideoListSnippet snippet { get; set; }
    }

    public class YoutubeVideoListSnippet
    {
        public string liveBroadcastContent { get; set; }
    }
}
