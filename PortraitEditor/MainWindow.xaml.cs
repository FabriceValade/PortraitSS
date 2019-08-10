using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortraitEditor.ViewModel;
using PortraitEditor.View;
using PortraitEditor.JsonHandling;

namespace PortraitEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        public ObservableCollection<FactionFile> CoreFaction { get; set; } = new ObservableCollection<FactionFile>();
        public ObservableCollection<Portrait> AllPortraits { get; set; } = new ObservableCollection<Portrait>();
        public SSFileExplorer FileExplorer { get; set; } = new SSFileExplorer();
        public string RootPath { get; set; }
        //public SSFileSetupWindow SSFileSetup { get; set; } = new SSFileSetupWindow();
        ICommand _ExploreFolderCommand;
        public ICommand ExploreFolderCommand
        {
            get
            {
                if (_ExploreFolderCommand == null)
                {
                    _ExploreFolderCommand = new RelayCommand<object>(param => this.ChangeExplorerUrl());
                }
                return _ExploreFolderCommand;
            }
        }
        public URLViewModel uRLViewModel { get; set; }
        public EditableURLViewModel EditURLViewModel { get; set; }
        public FactionDirectoryViewModel DirectoryViewModel { get; set; }

        public MainWindow()
        {

            //FileExplorerWindow FEW = new FileExplorerWindow();
            //FEW.ShowDialog();
            uRLViewModel = new URLViewModel() { CommonUrl = @"C:\Users\fabrice\Documents\Projet\ProjetPortraitSS" };
            EditURLViewModel = new EditableURLViewModel("oui", null);
            DataContext = this;
            
            
            FileExplorerViewModel FEVM = new FileExplorerViewModel();
            FEVM.ShowDialog();
            DirectoryViewModel = new FactionDirectoryViewModel(FEVM.FactionDirectory);
            uRLViewModel.Replace(FEVM.StarsectorFolderUrl);
            AllPortraitsViewModel test = new AllPortraitsViewModel(FEVM.FactionDirectory);

            InitializeComponent();
            //PortraitsIntereaction.Visibility = Visibility.Hidden;
            //FactionIntereaction.Visibility = Visibility.Hidden;
            //AllPortraitsIntereaction.Visibility = Visibility.Hidden;
            //ListCollectionView view = new ListCollectionView(AllPortraits);
            //view.GroupDescriptions.Add(new PropertyGroupDescription("ImageGender"));
        }

        public void ChangeExplorerUrl()
        {
            SSFileSetupWindow SSFileSetup = new SSFileSetupWindow();
            if (SSFileSetup.ShowDialog() == true)
            {
                CoreFaction.Clear();
                foreach (FactionFile faction in SSFileSetup.FileExplorer.GetFactionList())
                {
                    CoreFaction.Add(faction);
                }
                AllPortraits.Clear();
                foreach (Portrait portrait in SSFileSetup.FileExplorer.AllPortraits)
                {
                    AllPortraits.Add(portrait);
                }
            }
        }

        private void UpdateFactionFileList(string SourceFolder)
        {
            string FactionDirPath = Path.Combine(SourceFolder, "data");
            FactionDirPath = Path.Combine(FactionDirPath, "world");
            FactionDirPath = Path.Combine(FactionDirPath, "factions");


            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(FactionDirPath);
            if (!CoreFactionDirectory.Exists)
                return;
            IEnumerable<FileInfo> FactionFileList = CoreFactionDirectory.EnumerateFiles();
            foreach (FileInfo DataFile in FactionFileList)
            {

                if (DataFile.Extension == ".faction")
                {
                    string RelativeUrl = SSFileUrl.ExtractRelativeUrl(SourceFolder, DataFile.FullName);
                    SSFileUrl FactionUrl = new SSFileUrl(SourceFolder,RelativeUrl);
                    //updating the faction file list will update the available portraits
                    FactionFile ExtractedFile = new FactionFile(FactionUrl,AllPortraits);
                    ExtractedFile.SetOriginal();
                    CoreFaction.Add(ExtractedFile);
                }
                    
                
            }


            //CFactionList.Items.MoveCurrentToFirst();



            AllPortraitList.Items.MoveCurrentToFirst();

            PortraitsIntereaction.Visibility = Visibility.Visible;
            FactionIntereaction.Visibility = Visibility.Visible;
            AllPortraitsIntereaction.Visibility = Visibility.Visible;
            return;
        }
        //private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    VistaFolderBrowserDialog OpenRootFolder = new VistaFolderBrowserDialog();
        //    if (OpenRootFolder.ShowDialog() == true)
        //    {
        //        RootPathDisplay.Text = OpenRootFolder.SelectedPath;
        //        FileExplorer.InstalationUrl = OpenRootFolder.SelectedPath;
        //        RootPath = OpenRootFolder.SelectedPath;                
        //        UpdateFactionFileList(Path.Combine(RootPath, "starsector-core"));
        //    }
        //    return;
        //}
        private void AddGenericPortrait_Click(object sender, RoutedEventArgs e)
        {
            Button sent = sender as Button;
            FactionFile ReceivingFaction = (FactionFile) CFactionList.Items.CurrentItem;
            Portrait Referencing = (Portrait)AllPortraitList.Items.CurrentItem;
            Portrait Transfering = new Portrait(Referencing,(String)sent.Tag);
            ReceivingFaction.AddPortrait(Transfering);
            PortraitList.Items.MoveCurrentToFirst();
            return;

        }
        private void RemoveFactionPortrait_Click(object sender, RoutedEventArgs e)
        {
            Button sent = sender as Button;
            FactionFile RemovingFaction = (FactionFile)CFactionList.Items.CurrentItem;
            int SelectedPos = (int)PortraitList.Items.CurrentPosition;
            if (SelectedPos < RemovingFaction.Portraits.Count && SelectedPos != -1)
                RemovingFaction.RemovePortrait(SelectedPos);
            return;

        }
        private void ChangeGenderFactionPortrait_Click(object sender, RoutedEventArgs e)
        {
            FactionFile CurrentFaction = (FactionFile)CFactionList.Items.CurrentItem;
            Portrait SelectedPortrait = (Portrait)PortraitList.Items.CurrentItem;
            if (SelectedPortrait!=null)
                SelectedPortrait.FlipGender();
            CurrentFaction.OrderPortrait();
            return;
        }
        private void DisplayAppend_Click(object sender, RoutedEventArgs e)
        {
            FactionFile DisplayingFaction = (FactionFile)CFactionList.Items.CurrentItem;
            var win = new PortraitListOutputWindow(DisplayingFaction.GetAppended());
            win.ShowDialog();
            return;
        }
        private void ModHierarchy_Click(object sender, RoutedEventArgs e)
        {
            FileExplorer.CreateModStructure();
            return;
        }
        private void AppendFile_Click(object sender, RoutedEventArgs e)
        {
            FactionFile DisplayingFaction = (FactionFile)CFactionList.Items.CurrentItem;
            JObject PortraitsJson = new JObject(new JProperty("portraits", DisplayingFaction.Portraits.FlattenToJson()));
            //FileExplorer.AppendFileCreation(DisplayingFaction.Portraits);
            FileExplorer.AppendFileCreation(PortraitsJson);
            return;
        }
        private void BtnAddPortraitSource_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog SelectNewFileSource = new VistaOpenFileDialog();

            if (SelectNewFileSource.ShowDialog() == true)
            {
                SSFileUrl NewUrl = new SSFileUrl(SelectNewFileSource.FileName);
                AllPortraits.Add(new Portrait(NewUrl));
            }
            return;
        }


    }
 

    
    public static class CollectionExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> collection, TSource itemTofind, Func<TSource, TSource, bool> equalizer)
        {
            foreach (var item in collection)
            {
                if (equalizer(item, itemTofind))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<int> FindAll<TSource>(this IEnumerable<TSource> collection, TSource itemTofind, Func<TSource, TSource, bool> equalizer)
        {
            List<int> Position = new List<int>();
            int PosCounter = 0;
            foreach (var item in collection)
            {
                if (equalizer(item, itemTofind))
                {
                    Position.Add(PosCounter);
                }
                PosCounter++;
            }
            return Position;
        }

        

     
    }



}
