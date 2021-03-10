using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{
    static class HistoryDriver
    {
        static public string PathToHistory { get; set; } = Directory.GetCurrentDirectory() + "/checkHistory.json";
        static public List<CheckHistory> History { get; private set; }
        static public void setStatusById(int followId, bool isOnline)
        {
            History.Find(x => x.followId == followId).isOnline = isOnline;
            saveHistory();
        }
        static public bool getStatusById(int followId)
        {
            return History.Find(x => x.followId == followId).isOnline;
        }

        static public void makeNewHistoryRecords()
        {
            try
            {
                loadHistory();
                foreach (var follow in SettDriver.Sett.follows)
                {
                    if (follow.followId > History.Last().followId)
                    {
                        CheckHistory newFollow = new CheckHistory();
                        newFollow.followId = follow.followId;
                        newFollow.isOnline = false;
                        History.Add(newFollow);
                    }
                }
                saveHistory();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static public void loadHistory() => History = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckHistory>>(File.ReadAllText(HistoryDriver.PathToHistory));
        static public void saveHistory() => File.WriteAllText(PathToHistory, Newtonsoft.Json.JsonConvert.SerializeObject(History));
    }

    internal class CheckHistory
    {
        public int followId { get; set; }
        public bool isOnline { get; set; }
    }
}
