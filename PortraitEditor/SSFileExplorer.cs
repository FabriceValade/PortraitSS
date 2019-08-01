using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor
{
    public class SSFileExplorer : INotifyPropertyChanged
    {
        public string ModName = "L_PeSS";
        public string InstalationUrl { get; set; }
        public string RootModDirectory = "";
        public string FactionDirectory = "";
        public event PropertyChangedEventHandler PropertyChanged;

        SSFileUrl _SSUrl = new SSFileUrl();
        SSFileUrl _PeSSUrl = new SSFileUrl();
        public SSFileUrl SSUrl { get { return _SSUrl; } set { _SSUrl = value; NotifyPropertyChanged(); } }
        public SSFileUrl PeSSUrl { get { return _PeSSUrl; } set { _PeSSUrl = value; NotifyPropertyChanged(); } }

        Dictionary<string, string> ModInfo = new Dictionary<string, string>
        {
            { "id", "L_PeSS" },
            { "name", "Lethargie Portrait editor for StarSector" },
            { "version", "0.1" },
            { "description", "Modify or add to all ingame faction portraits" },
            { "gameVersion", "0.9.1a" }
        };
        public ObservableCollection<Portrait> AllPortraits = new ObservableCollection<Portrait>();

        public ObservableCollection<ModFolder> ModList { get; set; } = new ObservableCollection<ModFolder>();

        public SSFileExplorer()
        {        }

        public void ExploreCoreFolder()
        {
            if (SSUrl == new SSFileUrl())
                return;
            string FactionDirPath = Path.Combine("data", "world");
            FactionDirPath = Path.Combine(FactionDirPath, "factions");
            SSFileUrl CoreFactionUrl = new SSFileUrl(SSUrl, "starsector-core", FactionDirPath);

            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(CoreFactionUrl.FullUrl);
            if (!CoreFactionDirectory.Exists)
                return;

            ModFolder coreFolder = new ModFolder()
            {
                ModName = "Core",
                ModFactionUrl = CoreFactionUrl
            };

            IEnumerable<FileInfo> FileList = CoreFactionDirectory.EnumerateFiles();
            var Potential = from file in FileList
                            where file.Extension == ".faction"
                            select file;
            AllPortraits.Clear();
            ModList.Clear();
            foreach (FileInfo file in Potential)
            {
                string RelativeUrl = SSFileUrl.ExtractRelativeUrl(CoreFactionUrl.LinkedUrl, file.FullName);
                SSFileUrl FactionUrl = new SSFileUrl(SSUrl,"starsector-core", RelativeUrl);
                // warning this modify allportraits
                FactionFile ExtractedFile = new FactionFile(FactionUrl, AllPortraits);
                ExtractedFile.SetOriginal();
                ExtractedFile.ActionToMake = FactionFile.FactionAction.Modify;
                ExtractedFile.FactionSource = FactionFile.SourceOfFaction.Core;
                coreFolder.FactionList.Add(ExtractedFile);
            }
            ModList.Add(coreFolder);
            return;
        }

        public ObservableCollection<FactionFile> GetFactionList ()
        {
            ObservableCollection<FactionFile> Result = new ObservableCollection<FactionFile>();
            foreach(ModFolder mod in ModList)
            {
                foreach(FactionFile faction in mod.FactionList)
                {
                    if (faction.ActionToMake == FactionFile.FactionAction.Modify)
                        Result.Add(faction);
                }
            }
            return Result;
        }
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
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPassThrough(object sender, Object e, string property)
        {
            NotifyPropertyChanged(property);
        }
    }
    public class ModFolder
    {

        public SSFileUrl ModFactionUrl { get; set; }
        private ObservableCollection<FactionFile> _FactionList = new ObservableCollection<FactionFile>();
        public ObservableCollection<FactionFile> FactionList
        {
            get { return _FactionList; }
            set { _FactionList = value; }
        }
        public String ModName { get; set; }

        public ModFolder() { }
    }

}
