using PortraitEditor.Model;
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
                    _ExploreFolderCommand = new RelayCommand<object>(param => this.ExploreCoreFolder());
                }
                return _ExploreFolderCommand;
            }
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
        #endregion

        #region Constructors
        public FileExplorerViewModel()
        {
            URLViewModel ModUrl = new URLViewModel() { CommonUrl="show"};
            ModUrl.DisplayName = null;
            ModUrl.LinkingUrl = "starsector-core";

            ModCollection.Add(new ModViewModel() { Name = "Hello",Url= ModUrl });
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
            string FactionDirPath = Path.Combine("data", "world");
            FactionDirPath = Path.Combine(FactionDirPath, "factions");
            URLViewModel CoreFactionUrl = new URLViewModel()
            {
                CommonUrl = StarsectorFolderUrl.DisplayUrl,
                LinkingUrl = "starsector-core",
                RelativeUrl = FactionDirPath
            };
            URLViewModel CoreModUrl = new URLViewModel()
            {
                CommonUrl = StarsectorFolderUrl.DisplayUrl,
                LinkingUrl = "starsector-core"
            };

            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(CoreFactionUrl.DisplayUrl);
            if (!CoreFactionDirectory.Exists)
                return;

            ModViewModel CoreFolder = new ModViewModel()
            {
                Name = "Core",
                Url = CoreFactionUrl
            };
            ModCollection.Clear();
            ModCollection.Add(CoreFolder);
            //IEnumerable<FileInfo> FileList = CoreFactionDirectory.EnumerateFiles();
            //var Potential = from file in FileList
            //                where file.Extension == ".faction"
            //                select file;
            //AllPortraits.Clear();
            //ModList.Clear();
            //foreach (FileInfo file in Potential)
            //{
            //    string RelativeUrl = SSFileUrl.ExtractRelativeUrl(CoreFactionUrl.LinkedUrl, file.FullName);
            //    SSFileUrl FactionUrl = new SSFileUrl(SSUrl, "starsector-core", RelativeUrl);
            //    // warning this modify allportraits
            //    FactionFile ExtractedFile = new FactionFile(FactionUrl, AllPortraits);
            //    ExtractedFile.SetOriginal();
            //    ExtractedFile.ActionToMake = FactionFile.FactionAction.Modify;
            //    ExtractedFile.FactionSource = FactionFile.SourceOfFaction.Core;
            //    coreFolder.FactionList.Add(ExtractedFile);
            //}
            //ModList.Add(coreFolder);
            return;
        }
        #endregion
    }
}
