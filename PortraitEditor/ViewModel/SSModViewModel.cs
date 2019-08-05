using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class SSModViewModel : ViewModelBase
    {
        #region Field
        
        
        public ObservableCollection<SSFactionViewModel> _FactionCollection = new ObservableCollection<SSFactionViewModel>();
        #endregion

        #region properties
        string _Name; public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        URLViewModel _Url; public URLViewModel Url
        {
            get => _Url;
            set
            {
                _Url = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<SSFactionViewModel> FactionCollection
        {
            get => _FactionCollection;
            set
            {
                _FactionCollection = value;
                NotifyPropertyChanged("FactionCollection");
            }
        }

        SSFileDirectory<SSFactionGroup,SSFaction> _Directory;
        #endregion

        #region Constructor
        public SSModViewModel(string name, URLViewModel url, SSFileDirectory<SSFactionGroup, SSFaction> directory)
        {
            Name = name;
            Url = new URLViewModel(url);
            _Directory = directory;
        }
        #endregion

        #region method
        public void ExploreFactionFile()
        {
            string FactionDirPath = Path.Combine("data", "world");
            FactionDirPath = Path.Combine(FactionDirPath, "factions");
            URLViewModel FactionDirUrl = new URLViewModel()
            {
                CommonUrl = Url.CommonUrl,
                LinkingUrl = Url.LinkingUrl,
                RelativeUrl = FactionDirPath
            };

            DirectoryInfo FactionDirectory = new DirectoryInfo(FactionDirUrl.DisplayUrl);
            if (!FactionDirectory.Exists)
                return;

            IEnumerable<FileInfo> FileList = FactionDirectory.EnumerateFiles();
            var Potential = from file in FileList
                            where file.Extension == ".faction"
                            select file;
            FactionCollection.Clear();
            foreach (FileInfo file in Potential)
            {
                URLRelative FactionFileUrl = new URLRelative()
                {
                    CommonUrl = Url.CommonUrl,
                    LinkingUrl = Url.LinkingUrl,
                    RelativeUrl = Path.Combine(FactionDirPath, file.Name)
                };
                SSFaction NewFaction = new SSFaction(FactionFileUrl, this.Name);
                _Directory.AddFile(NewFaction);
                SSFactionViewModel factionViewModel = new SSFactionViewModel(NewFaction);
                FactionCollection.Add(factionViewModel);
            }
            return;
        }
        #endregion
    }
}
