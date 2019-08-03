using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.ViewModel
{
    public class ModViewModel : ViewModelBase
    {
        #region Field
        string _Name;
        URLViewModel _Url;
        public ObservableCollection<FactionViewModel> _FactionCollection = new ObservableCollection<FactionViewModel>();
        #endregion

        #region properties
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        public URLViewModel Url {
            get => _Url;
            set
            {
                _Url = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<FactionViewModel> FactionCollection
        {
            get => _FactionCollection;
            set
            {
                _FactionCollection = value;
                NotifyPropertyChanged("FactionCollection");
            }
        } 
        #endregion

        #region Constructor
        public ModViewModel()
        { }
        public ModViewModel(string name, URLViewModel url)
        {
            Name = name;
            Url = new URLViewModel(url);
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
                URLViewModel FactionFileUrl = new URLViewModel()
                {
                    CommonUrl = Url.CommonUrl,
                    LinkingUrl = Url.LinkingUrl,
                    RelativeUrl = Path.Combine(FactionDirPath, file.Name)
                };
                FactionViewModel faction = new FactionViewModel(FactionFileUrl);
                FactionCollection.Add(faction);
            }
            return;
        } 
        #endregion

    }
}
