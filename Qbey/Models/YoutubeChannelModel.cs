using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{ 
    class ChannelSearchResult 
    {
        public List<ItemChannel> items { get; set; }
    }

    public class ItemChannel
    {
        public Snippet snippet { get; set; }
        public ChannelId id { get; set; }
    }

    public class Snippet
    {
        public string channelId { get; set; }
        public string title { get; set; }
    }

    class VideoSearchResult
    {
        public PageInfo pageInfo { get; set; }
        public List<ItemVideo> items { get; set; }
    }

    public class ItemVideo
    {
        public Snippet snippet { get; set; }
        public VideoId id { get; set; }
    }

    public class PageInfo
    {
        public int totalResults { get; set; }
    }

    public class VideoId 
    {
        public string videoId { get; set; } 
    }

    public class ChannelId
    {
        public string channelId { get; set; } 
    }
}
