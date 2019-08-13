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

namespace PortraitEditor.ViewModel.SubWindows
{
    public class SSFileExplorerViewModel : ViewModelBase
    {
        #region Field
        FileExplorerWindow WindowView;
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
        RelayCommand<object> _CloseWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (_CloseWindowCommand == null)
                {
                    _CloseWindowCommand = new RelayCommand<object>(param => this.CloseWindow());
                }
                return _CloseWindowCommand;
            }
        }
        RelayCommand<object> _ExploreFactionFileCommand;
        public ICommand ExploreFactionFileCommand
        {
            get
            {
                if (_ExploreFactionFileCommand == null)
                {
                    _ExploreFactionFileCommand = new RelayCommand<object>(param => this.ExploreFactionFile());
                }
                return _ExploreFactionFileCommand;
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

        bool _RemoveIncompleteFactionAction = true;
        public bool RemoveIncompleteFactionAction
        {
            get => _RemoveIncompleteFactionAction;
            set
            {
                _RemoveIncompleteFactionAction = value;
                NotifyPropertyChanged("RemoveIncompleteFactionActionFalse");
                NotifyPropertyChanged("RemoveIncompleteFactionActionTrue");
            }

        }
        public bool RemoveIncompleteFactionActionFalse
        {
            get => RemoveIncompleteFactionAction == false;
            set => RemoveIncompleteFactionAction = false;
        }
        public bool RemoveIncompleteFactionActionTrue
        {
            get => RemoveIncompleteFactionAction == true;
            set => RemoveIncompleteFactionAction = true;
        }
        #endregion

        #region Properties
        EditableURLViewModel _StarsectorFolderUrl;
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

        public ObservableCollection<ModFactionViewModel> ModCollection { get; } = new ObservableCollection<ModFactionViewModel>();

        public ObservableCollection<ModFactionViewModel> ModWithFactionCollection { get; } = new ObservableCollection<ModFactionViewModel>();
        public SSFileDirectory<SSFactionGroup, SSFaction> FactionDirectory { get; } = new SSFileDirectory<SSFactionGroup, SSFaction>();

        SSMod _LPeSSMod = new SSMod();
        public SSMod LPeSSMod { get => _LPeSSMod; private set { _LPeSSMod = value; NotifyPropertyChanged(); } }
        #endregion

        #region Constructors
        public SSFileExplorerViewModel()
        {
            //URLViewModel ModUrl = new URLViewModel() { CommonUrl="show"};
            //ModUrl.DisplayName = null;
            //ModUrl.LinkingUrl = "starsector-core";

            //ModCollection.Add(new ModViewModel() { Name = "Hello",Url= ModUrl });
        }
        #endregion

        #region Helper function
        public void ShowDialog()
        {
            WindowView = new FileExplorerWindow(this);
            WindowView.ShowDialog();
            return;
        }

        public void CloseWindow()
        {
            WindowView.Close();
        }

        public void ExploreFolder()
        {
            ModWithFactionCollection.Clear();
            FactionDirectory.DeleteDirectory();
            ModCollection.Clear();
            ExploreCoreFolder();
            if (ModAction == SSModFolderActions.Use)
                ExploreModFolder();
            
            return;
        }

        public bool CheckSSFolderValidity(URLRelative url)
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
            
            URLRelative CoreModUrl = new URLRelative()
            {
                CommonUrl = StarsectorFolderUrl.CommonUrl,
                LinkingUrl = "starsector-core"
            };

            SSMod CoreMod = new SSMod(CoreModUrl, PortraitEditorConfiguration.CoreModName);

            ModFactionViewModel CoreModViewModel = new ModFactionViewModel(CoreMod);
            ModCollection.Add(CoreModViewModel);
            UpdateLocalMod(new URLRelative(StarsectorFolderUrl.CommonUrl, Path.Combine("mods", "LPeSS"), null), "L_PeSS");
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
                URLRelative ModUrl = new URLRelative()
                {
                    CommonUrl = StarsectorFolderUrl.CommonUrl,
                    LinkingUrl = Path.Combine("mods", ModDirectory.Name)
                };

                SSMod mod = new SSMod(ModUrl, ModDirectory.Name);
                if (mod.ContainsFaction)
                {
                    ModFactionViewModel ModFolder = new ModFactionViewModel(mod);
                    ModCollection.Add(ModFolder);

                }
            }
            

        }

        public void ExploreFactionFile()
        {
            FactionDirectory.DeleteDirectory();
            ModWithFactionCollection.Clear();
            List<String> AvailableLinking = (from modVM in ModCollection
                                             where modVM.Mod.ContainsFaction && modVM.AllowExplore
                                             select modVM.Mod.Url.LinkingUrl).ToList();
            List<SSMod> AvailableMods = (from modVM in ModCollection
                                             where modVM.Mod.ContainsFaction && modVM.AllowExplore
                                             select modVM.Mod).ToList();
            FactionDirectory.AvailableLinkingUrl = AvailableLinking;
            foreach (ModFactionViewModel modVM in ModCollection)
            {
                if (modVM.Mod.ContainsFaction && modVM.AllowExplore)
                {
                    modVM.Mod.ExploreFactionFile(FactionDirectory, AvailableMods);
                    ModWithFactionCollection.Add(modVM);
                }
            }

            if (RemoveIncompleteFactionAction)
            {
                List<SSFactionGroup> Incomplete = (from grouped in FactionDirectory.GroupDirectory
                                                   where grouped.IsIncomplete
                                                   select grouped).ToList();
                foreach (SSFactionGroup factionGroup in Incomplete)
                {
                    factionGroup.DeleteGroup();
                }


            }
        }

        public void UpdateLocalMod(URLRelative newUrl,string newName)
        {
            LPeSSMod.Name = newName;
            LPeSSMod.Url = newUrl;
            NotifyPropertyChanged("LPeSSMod");
        }
        #endregion

        #region helper class
        #endregion
    }

    
}
