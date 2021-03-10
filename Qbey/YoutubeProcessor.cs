using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qbey
{
    class YoutubeProcessor
    {
        //yeah yeah fix later TODO
        static string apiUrl = SettDriver.Sett.youTubeAPIURL;
        static string apiKey = SettDriver.Sett.youTubeAPIToken;


        static public async Task<ChannelSearchResult> searchChannel(string channelName)
        {
            string requestUrl = apiUrl + $"search?part=snippet&q={channelName}&type=channel&key={apiKey}";
            using (HttpResponseMessage response = await ApiGeneral.ApiClient.GetAsync(requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var searchResults = await response.Content.ReadAsAsync<ChannelSearchResult>();
                    return searchResults;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        static public async Task<List<Snippet>> searchChannelByName(string channelName)
        {
            ChannelSearchResult searchResult;
            try
            {
                searchResult = await searchChannel(channelName);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
            List<Snippet> result = new List<Snippet>();
            foreach (ItemChannel res in searchResult.items)
            {
                result.Add(res.snippet);
            }
            return result;
        }

        static public async Task<string> isStreamUp(string channelId)
        {
            Console.WriteLine($"asking if stream up {channelId}");
            string allowed = "-_";
            if (!channelId.All(c => Char.IsLetterOrDigit(c) || allowed.Contains(c)))
            {
                throw new ArgumentException($"{channelId} contains not only letters and numbers.");
            }
            string requestUrl = apiUrl + $"search?part=snippet&channelId={channelId}&type=video&eventType=live&key={apiKey}";
            using (HttpResponseMessage response = await ApiGeneral.ApiClient.GetAsync(requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    VideoSearchResult videoSearchResult = await response.Content.ReadAsAsync<VideoSearchResult>();
                    return videoSearchResult.pageInfo.totalResults > 0 ? videoSearchResult.items.First().id.videoId : null;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        public async Task CheckFollows(List<string> ChannelsToCheck)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} enters CheckFollows()");
            foreach (string channelId in ChannelsToCheck)
            {
                Console.WriteLine($"ya zapustilsya {channelId}");
                string videoId = await isStreamUp(channelId);
                if (videoId != null)
                {
                    var sender = new InnerActions();
                    //await sender.streamAlert(videoId);
                }
            }
        }

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
            return root.contents.twoColumnBrowseResultsRenderer.Tabs[1].tabRenderer
                .content.sectionListRenderer.contents[0].itemSectionRenderer.contents[0]
                .gridRenderer.items[0].gridVideoRenderer.videoId;
        }
    }
}
