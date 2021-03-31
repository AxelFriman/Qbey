using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{
    abstract class BaseSettings<TSettType> where TSettType : new()
    {
        public FileInfo JsonFile { get; protected set; }
        protected TSettType _sett;
        public virtual TSettType Sett
        {
            get { loadSett(); return _sett; }
            protected set => _sett = value;
        } //TODO teach models how to smart initialize
        public BaseSettings(string jsonPath)
        {
            JsonFile = new FileInfo(jsonPath);
            loadSett();
        }
        public BaseSettings(string jsonPath, TSettType sett)
        {
            JsonFile = new FileInfo(jsonPath);
            saveSett(sett);
            loadSett();
        }
        public void loadSett()
        {
            string jsonTxt;
            try
            {
                jsonTxt = File.ReadAllText(JsonFile.FullName);
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                jsonTxt = JsonConvert.SerializeObject(Sett);
                saveSett();
                OnError($"File {JsonFile.FullName} wasn't found. An empty one has been created.");
            }
            try
            {
                Sett = JsonConvert.DeserializeObject<TSettType>(jsonTxt);
            }
            catch (ArgumentException e)
            {
                OnError(e.Message);
                throw;
            }

            if (Sett is null)
            {
                throw (new NullReferenceException("Invalid config.json format."));
            }
        }
        public void saveSett() => saveSett(_sett);
        public void saveSett(TSettType settToSave)
        {
            JsonFile?.Directory.Create();
            File.WriteAllText(JsonFile.FullName, JsonConvert.SerializeObject(settToSave));
        }

        protected virtual void OnError(string msg)
        {
            ErrorEvent?.Invoke(msg);
        }
        public event Func<string, Task> ErrorEvent;
    }
}
