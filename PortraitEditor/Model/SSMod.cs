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
        public URLRelative Url { get; private set; }
        public string Name { get; private set; }
        public bool ContainsFaction
        {
            get
            {
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
        #endregion


        public SSMod(URLRelative url, string name)
        {
            Url = url ?? throw new ArgumentNullException("The Mod url cannot be null.");
            Name = name ?? throw new ArgumentNullException("The Mod name cannot be null.");
        }

        #region method
        public List<SSFaction> ExploreFactionFile()
        {
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
                return null;

            IEnumerable<FileInfo> FileInfoList = FactionDirectory.EnumerateFiles();
            var Potential = from file in FileInfoList
                            where file.Extension == ".faction"
                            select file;
            foreach (SSFile file in FileList)
            {
                if (file is SSFaction)
                {
                    FileList.Remove(file);
                }
            }
            List<SSFaction> ExploreResult= new List<SSFaction>();
            foreach (FileInfo file in Potential)
            {
                URLRelative FactionFileUrl = new URLRelative()
                {
                    CommonUrl = Url.CommonUrl,
                    LinkingUrl = Url.LinkingUrl,
                    RelativeUrl = Path.Combine(FactionDirPath, file.Name)
                };
                SSFaction NewFaction = new SSFaction(FactionFileUrl, this);
                ExploreResult.Add(NewFaction);
                FileList.Add(NewFaction);
            }
            return ExploreResult;
        }
        
        #endregion

    }
}
