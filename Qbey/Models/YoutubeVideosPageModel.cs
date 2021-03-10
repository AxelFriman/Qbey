using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{
    public class videosPage
    {
        public videosPageContents contents { get; set; }
    }

    public class videosPageContents
    {
        public TwoColumnBrowseResultsRenderer twoColumnBrowseResultsRenderer { get; set; }
    }

    public class TwoColumnBrowseResultsRenderer
    {
        public List<Tabs> Tabs { get; set; }
    }

    public class Tabs
    {
        public TabRenderer tabRenderer { get; set; }
    }

    public class TabRenderer
    {
        public string title { get; set; }
        public TabRendererContent content { get; set; }
    }

    public class TabRendererContent
    {
        public SectionListRenderer sectionListRenderer { get; set; }
    }

    public class SectionListRenderer
    {
        public List<SectionListRendererContents> contents { get; set; }
    }

    public class SectionListRendererContents
    {
        public ItemSectionRenderer itemSectionRenderer { get; set; }
    }

    public class ItemSectionRenderer
    {
        public List<ItemSectionRendererContents> contents { get; set; }
    }

    public class ItemSectionRendererContents
    {
        public GridRenderer gridRenderer { get; set; }
    }

    public class GridRenderer
    {
        public List<GridRendererItems> items { get; set; }
    }

    public class GridRendererItems
    {
        public GridVideoRenderer gridVideoRenderer { get; set; }
    }

    public class GridVideoRenderer
    {
        public string videoId { get; set; }
    }
}
