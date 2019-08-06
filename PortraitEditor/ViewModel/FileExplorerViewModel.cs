using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PortraitEditor.ViewModel
{
    public class FileExplorerViewModel : ViewModelBase
    {
        #region Field
        EditableURLViewModel _StarsectorFolderUrl;
        #endregion

        #region Command properties
        RelayCommand<object> _ExploreFolderCommand;
        public ICommand ExploreFolderCommand
        {
            get
            {
                if (_ExploreFolderCommand == null)
                {
                    _ExploreFolderCommand = new RelayCommand<object>(param => this.ExploreFolder());
                }
                return _ExploreFolderCommand;
            }
        }
        #endregion

        #region Mod radiobutton properties
        public enum SSModFolderActions { Ignore, Use }
        SSModFolderActions _ModAction = SSModFolderActions.Use;
        public SSModFolderActions ModAction
        {
            get => _ModAction;
            set
            {
                _ModAction = value;
                NotifyPropertyChanged("ModFolderRadioAsIgnore");
                NotifyPropertyChanged("ModFolderRadioAsUse");
            }

        }
        public bool ModFolderRadioAsIgnore
        {
            get => ModAction.Equals(SSModFolderActions.Ignore);
            set => ModAction = SSModFolderActions.Ignore; 
        }
        public bool ModFolderRadioAsUse
        {
            get => ModAction.Equals(SSModFolderActions.Use);
            set => ModAction = SSModFolderActions.Use;
        }
        #endregion

        #region Properties
        public EditableURLViewModel StarsectorFolderUrl
        {
            get
            {
                if (_StarsectorFolderUrl == null)
                {
                    _StarsectorFolderUrl = new EditableURLViewModel("Starsector folder", "Select path");
                    _StarsectorFolderUrl.ValidityChecker = CheckSSFolderValidity;
                }
                return _StarsectorFolderUrl;
            }
        }

        public ObservableCollection<ModViewModel> ModCollection { get; } = new ObservableCollection<ModViewModel>();

        SSFileDirectory<SSFactionGroup, SSFaction> _Directory = new SSFileDirectory<SSFactionGroup, SSFaction>(); SSFileDirectory<SSFactionGroup, SSFaction> Directory { get => _Directory; }
        #endregion

        #region Constructors
        public FileExplorerViewModel()
        {
            URLViewModel ModUrl = new URLViewModel() { CommonUrl="show"};
            ModUrl.DisplayName = null;
            ModUrl.LinkingUrl = "starsector-core";

            //ModCollection.Add(new ModViewModel() { Name = "Hello",Url= ModUrl });
        }
        #endregion

        #region Helper function
        public void ShowDialog()
        {
            FileExplorerWindow NewWindow = new FileExplorerWindow(this);
            NewWindow.ShowDialog();
            return;
        }

        public void ExploreFolder()
        {
            ExploreCoreFolder();
            if (ModAction == SSModFolderActions.Use)
                ExploreModFolder();
            return;
        }

        public bool CheckSSFolderValidity(URL url)
        {
            if (url == null)
                return false;
            if (!url.Exist())
                return false;
            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(url.FullUrl);
            IEnumerable<DirectoryInfo> DirList= CoreFactionDirectory.EnumerateDirectories();
            List<DirectoryInfo> SSCoreFolder = (from dir in DirList
                                           where dir.Name == "starsector-core"
                                           select dir).ToList();
            IEnumerable<FileInfo> FileList = CoreFactionDirectory.EnumerateFiles();
            List<FileInfo> SSExecutable = (from file in FileList
                    where file.Name == "starsector.exe"
                    select file).ToList();
            if (SSCoreFolder.Count == 1 && SSExecutable.Count == 1)
                return true;

            return false;
        }

        public void ExploreCoreFolder()
        {
            if (StarsectorFolderUrl.UrlState != URLstate.Acceptable)
                return;
            
            URLViewModel CoreModUrl = new URLViewModel()
            {
                CommonUrl = StarsectorFolderUrl.DisplayUrl,
                LinkingUrl = "starsector-core"
            };

            ModViewModel CoreFolder = new ModViewModel(PortraitEditorConfiguration.CoreModName, CoreModUrl, Directory);

            CoreFolder.ExploreFactionFile();
            ModCollection.Clear();
            ModCollection.Add(CoreFolder);
            return;
        }

        public void ExploreModFolder()
        {
            if (StarsectorFolderUrl.UrlState != URLstate.Acceptable)
                return;
            string ModFolderPath = Path.Combine(StarsectorFolderUrl.DisplayUrl, "mods");
            DirectoryInfo ModsDirectory = new DirectoryInfo(ModFolderPath);
            IEnumerable<DirectoryInfo> ModsEnumerable = ModsDirectory.EnumerateDirectories();
            foreach (DirectoryInfo ModDirectory in ModsEnumerable)
            {
                URLViewModel ModUrl = new URLViewModel()
                {
                    CommonUrl = StarsectorFolderUrl.DisplayUrl,
                    LinkingUrl = Path.Combine("mods", ModDirectory.Name)
                };
                ModViewModel ModFolder = new ModViewModel(ModDirectory.Name, ModUrl, Directory);
                if (ModFolder.ContainsFaction)
                {
                    ModFolder.ExploreFactionFile();
                    ModCollection.Add(ModFolder);
                }
            }
   
        }
      

        #endregion
    }
}
