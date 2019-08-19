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
        SSModFolderActions _ModAction = (SSModFolderActions)Properties.Settings.Default.ModFoldAction;
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

        SSModFolderActions _ExploreOldLPeSSFiles = (SSModFolderActions)Properties.Settings.Default.LPessFoldAction;
        public SSModFolderActions ExploreOldLPeSSFiles
        {
            get => _ExploreOldLPeSSFiles;
            set
            {
                _ExploreOldLPeSSFiles = value;
                NotifyPropertyChanged("ExploreOldLPeSSFilesAsIgnore");
                NotifyPropertyChanged("ExploreOldLPeSSFilesAsUse");
            }

        }
        public bool ExploreOldLPeSSFilesAsIgnore
        {
            get => ExploreOldLPeSSFiles == SSModFolderActions.Ignore;
            set => ExploreOldLPeSSFiles = SSModFolderActions.Ignore;
        }
        public bool ExploreOldLPeSSFilesAsUse
        {
            get => ExploreOldLPeSSFiles == SSModFolderActions.Use;
            set => ExploreOldLPeSSFiles = SSModFolderActions.Use;
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
                    if (Properties.Settings.Default.StarsectorUrl != "")
                        _StarsectorFolderUrl.CommonUrl = Properties.Settings.Default.StarsectorUrl;
                }
                return _StarsectorFolderUrl;
            }
        }

        public ObservableCollection<ModFactionViewModel> ModCollection { get; } = new ObservableCollection<ModFactionViewModel>();

        public ObservableCollection<ModFactionViewModel> ModWithFactionCollection { get; } = new ObservableCollection<ModFactionViewModel>();
        public SSFactionDirectory FactionDirectory { get; } = new SSFactionDirectory();

        SSMod _LPeSSMod = new SSMod() { Name = "LPeSS" };
        public SSMod LPeSSMod { get => _LPeSSMod; private set { _LPeSSMod = value; NotifyPropertyChanged(); } }
        #endregion

        #region Constructors
        public SSFileExplorerViewModel()
        {
           
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
            if (StarsectorFolderUrl.UrlState==URLstate.Acceptable)
            {
                Properties.Settings.Default.StarsectorUrl = StarsectorFolderUrl.CommonUrl;
                Properties.Settings.Default.Save();
            }
            else
            {
                return;
            }
            Properties.Settings.Default.ModFoldAction = (int)ModAction;
            Properties.Settings.Default.LPessFoldAction = (int)ExploreOldLPeSSFiles;
            Properties.Settings.Default.Save();
            
            ModWithFactionCollection.Clear();
            FactionDirectory.DeleteDirectory();
            ModCollection.Clear();
            LPeSSMod.Url = new URLRelative()
            {
                CommonUrl = StarsectorFolderUrl.CommonUrl,
                LinkingUrl = Path.Combine("mods", LPeSSMod.Name)
            };
            if (ExploreOldLPeSSFiles == SSModFolderActions.Use)
            {
                if (LPeSSMod.ContainsFaction)
                {
                    ModFactionViewModel ModFolder = new ModFactionViewModel(LPeSSMod);
                    ModCollection.Add(ModFolder);
                }
            }

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
            UpdateLocalMod(new URLRelative(StarsectorFolderUrl.CommonUrl, Path.Combine("mods", "LPeSS"), null), "LPeSS");
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
                if (mod.Name == LPeSSMod.Name)
                {
                    //LPeSSMod = mod;
                }
                else
                {
                    if (mod.ContainsFaction)
                    {
                        ModFactionViewModel ModFolder = new ModFactionViewModel(mod);
                        ModCollection.Add(ModFolder);

                    }
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
                    factionGroup.Delete();
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
