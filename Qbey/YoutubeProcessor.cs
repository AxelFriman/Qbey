using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace Qbey
{
    class YoutubeProcessor
    {
        static string apiUrl = SettDriver.Sett.youTubeAPIURL;
        static string apiKey = SettDriver.Sett.youTubeAPIToken;

        //Asks API if the given video is a live broadcast. Costs 1 quota
        public static async Task<bool> isStream(string videoId)
        {
            Boolean isLive = false;
            string requestUrl = apiUrl + $"videos?part=snippet&id={videoId}&type=channel&key={apiKey}";
            using (HttpResponseMessage response = await ApiGeneral.ApiClient.GetAsync(requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var searchResults = await response.Content.ReadAsAsync<YoutubeVideoList>();
                    isLive = searchResults.items[0].snippet.liveBroadcastContent.Equals("live");
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
            return isLive;
        }

        //Gets "/videos" page with webclient and parses response. Returns last video ID.
        public static async Task<string> getLastVideoFromWeb(string linkToChannelVideosTab)
        {
            string htmlCode = "";
            string startString = "var ytInitialData = ";
            string endString = ";</script>";
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = await client.DownloadStringTaskAsync(new Uri(linkToChannelVideosTab));
            }
            int startIndex = htmlCode.IndexOf(startString) + startString.Length;
            int endIndex = htmlCode.IndexOf(endString, startIndex);
            string jsonTxt = htmlCode.Substring(startIndex, endIndex - startIndex);
            videosPage root = Newtonsoft.Json.JsonConvert.DeserializeObject<videosPage>(jsonTxt); 
            GridRenderer videos = null;
            try
            {
                 videos = root.contents.twoColumnBrowseResultsRenderer.Tabs[1].tabRenderer
                    .content.sectionListRenderer.contents[0].itemSectionRenderer.contents[0]
                    .gridRenderer;
            }
            catch (NullReferenceException e)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Error, "YoutubeProcessor.getLastVideoFromWeb", $"Unable to serialize a videos page.\nChannel: {linkToChannelVideosTab}\nResponsed json:{jsonTxt}"));
            }
            return videos?.items[0].gridVideoRenderer.videoId;
        }

        public static event Func<LogMessage, Task> ErrorEvent;
    }
}
