using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor
{
    public class SSFileExplorer
    {
        public string ModName = "L_PeSS";
        public string InstalationUrl { get; set; }
        public string RootModDirectory = "";
        public string FactionDirectory = "";

        SSFileUrl _SSUrl = new SSFileUrl();
        SSFileUrl _PeSSUrl = new SSFileUrl();
        public SSFileUrl SSUrl { get { return _SSUrl; }  }
        public SSFileUrl PeSSUrl { get { return _PeSSUrl; }  }

        Dictionary<string, string> ModInfo = new Dictionary<string, string>
        {
            { "id", "L_PeSS" },
            { "name", "Lethargie Portrait editor for StarSector" },
            { "version", "0.1" },
            { "description", "Modify or add to all ingame faction portraits" },
            { "gameVersion", "0.9.1a" }
        };


        public SSFileExplorer() { }

        public void CreateModStructure()
        {
            RootModDirectory = Path.Combine(InstalationUrl, @"mods\"+ModName);
            if (Directory.Exists(RootModDirectory))
                Directory.Delete(RootModDirectory, true);
            Directory.CreateDirectory(RootModDirectory);
            string pathString = Path.Combine(RootModDirectory, "data");
            Directory.CreateDirectory(pathString);
            pathString = Path.Combine(pathString, "world");
            Directory.CreateDirectory(pathString);
            FactionDirectory = Path.Combine(pathString, "factions");
            Directory.CreateDirectory(FactionDirectory);

            string ModInfoJson = JsonConvert.SerializeObject(ModInfo, Formatting.Indented);

            pathString = Path.Combine(RootModDirectory, "mod_info.json");
            File.WriteAllText(pathString, ModInfoJson);
            return;
        }
        public void AppendFileCreation( ObservableCollection<Portrait> Portraits)
        {
            var a = Portraits.GetType();
            Dictionary<string, Dictionary<string, List<string>>> AppendDict = new Dictionary<string, Dictionary<string, List<string>>>();
            AppendDict.Add("portrait", new Dictionary<string, List<string>>());
            AppendDict["portrait"].Add("standard_male", new List<string>());
            AppendDict["portrait"]["standard_male"].Add("hello");
            AppendDict["portrait"].Add("standard_female", new List<string>());
            AppendDict["portrait"]["standard_female"].Add("rehello");
            string DotFaction = JsonConvert.SerializeObject(AppendDict, Formatting.Indented);
            string pathString = Path.Combine(FactionDirectory, "test.faction");
            File.WriteAllText(pathString, DotFaction);
            return ;
        }
        public void AppendFileCreation(JObject PortraitJson)
        {
            string DotFaction = JsonConvert.SerializeObject(PortraitJson, Formatting.Indented);
            string pathString = Path.Combine(FactionDirectory, "test.faction");
            File.WriteAllText(pathString, DotFaction);
            return;
        }
    }
    public interface IJsonDictionary
    {

    }
}
