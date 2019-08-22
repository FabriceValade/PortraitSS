using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortraitEditor.Model.SSFiles;
namespace PortraitEditor.Model
{
    public class SSMod
    {
        #region Event
        #endregion

        #region properties
        SSFileLister<SSFile> _FileLister=new SSFileLister<SSFile>();
        public ObservableCollection<SSFile> FileList { get => _FileLister.Files; }
        public URLRelative Url { get; set; }
        public string Name { get; set; }
        public bool ContainsFaction
        {
            get
            {
                if (Url == null)
                    return false;
                string FactionDirPath = Path.Combine("data", "world");
                FactionDirPath = Path.Combine(FactionDirPath, "factions");
                URLRelative FactionDirUrl = new URLRelative()
                {
                    CommonUrl = Url.CommonUrl,
                    LinkingUrl = Url.LinkingUrl,
                    RelativeUrl = FactionDirPath
                };

                DirectoryInfo FactionDirectory = new DirectoryInfo(FactionDirUrl.FullUrl);
                if (!FactionDirectory.Exists)
                    return false;

                IEnumerable<FileInfo> FileList = FactionDirectory.EnumerateFiles();
                if (FileList.Count() == 0)
                    return false;

                return true;
            }
        }
        public bool AllowExplore { get; set; } = true;
        #endregion

        public SSMod()
        { }
        public SSMod(URLRelative url, string name)
        {
            Url = url ?? throw new ArgumentNullException("The Mod url cannot be null.");
            Name = name ?? throw new ArgumentNullException("The Mod name cannot be null.");
        }

        #region method
        public void ExploreFactionFile(SSFileDirectory<SSFactionGroup,SSFaction> directory, List<SSMod> AvailableMods)
        {
            if (Url == null || Name == null)
                throw new InvalidOperationException();
            if (!AllowExplore || !ContainsFaction)
                return;
            string FactionDirPath = Path.Combine("data", "world");
            FactionDirPath = Path.Combine(FactionDirPath, "factions");
            URLRelative FactionDirUrl = new URLRelative()
            {
                CommonUrl = Url.CommonUrl,
                LinkingUrl = Url.LinkingUrl,
                RelativeUrl = FactionDirPath
            };

            DirectoryInfo FactionDirectory = new DirectoryInfo(FactionDirUrl.FullUrl);
            if (!FactionDirectory.Exists)
                return;

            IEnumerable<FileInfo> FileInfoList = FactionDirectory.EnumerateFiles();
            var Potential = from file in FileInfoList
                            where file.Extension == ".faction"
                            select file;
        
            List<SSFile> tempList = new List<SSFile>(FileList);
            foreach (SSFile file in tempList)
            {
                if (file is SSFaction)
                {
                    file.Delete();
                }
            }
            //List<SSFaction> ExploreResult= new List<SSFaction>();
            foreach (FileInfo file in Potential)
            {
                URLRelative FactionFileUrl = new URLRelative()
                {
                    CommonUrl = Url.CommonUrl,
                    LinkingUrl = Url.LinkingUrl,
                    RelativeUrl = Path.Combine(FactionDirPath, file.Name)
                };
                SSFaction NewFaction = new SSFaction(FactionFileUrl, this, AvailableMods);
                directory.AddFile(NewFaction);
                FileList.Add(NewFaction);
            }
            return;
        }
        
        public void RemoveFactionFile()
        {
            while (FileList.Count >0)
            {
                FileList[0].Delete();
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion

    }
}
