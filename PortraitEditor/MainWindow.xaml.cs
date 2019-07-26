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
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;

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


        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            PortraitsIntereaction.Visibility = Visibility.Hidden;
            FactionIntereaction.Visibility = Visibility.Hidden;
            AllPortraitsIntereaction.Visibility = Visibility.Hidden;
        }

        private void UpdateFactionFileList(string path)
        {   

            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(path+ "data\\world\\factions");
            IEnumerable<FileInfo> FactionFileList = CoreFactionDirectory.EnumerateFiles();
            foreach (FileInfo DataFile in FactionFileList)
            {

                if (DataFile.Extension == ".faction")
                {
                    FactionFile ExtractedFile = new FactionFile(path, DataFile.FullName);
                    ExtractedFile.SetOriginal();
                    CoreFaction.Add(ExtractedFile);
                }
                    
                
            }
            CFactionList.Items.MoveCurrentToFirst();
            PortraitsIntereaction.Visibility = Visibility.Visible;
            FactionIntereaction.Visibility = Visibility.Visible;
            //Data.Concat(RootDirectory.EnumerateFiles());

            return;
        }
        private void ExtractAllPortraits(ObservableCollection<FactionFile> factionFiles)
        {
            foreach (FactionFile OneFactionFile in factionFiles)
            {
                foreach (var p in OneFactionFile.Portraits)
                {
                    //bool AlreadySet = AllPortraits.Contains(p);
                    bool AlreadySet = AllPortraits.Contains(p,Portrait.Equals);
                    if (!AlreadySet)
                    {
                        Portrait ExtractedPortrait = new Portrait(p.RelativePathSource, p.Url, p.ImageGender);
                        AllPortraits.Add(ExtractedPortrait);
                    }
                }
            }           
            AllPortraitList.Items.MoveCurrentToFirst();
            AllPortraitsIntereaction.Visibility = Visibility.Visible;
            return;
        }


        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "faction files (*.faction)|*.faction";
            //if (openFileDialog.ShowDialog() == true)
            //    txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
            VistaFolderBrowserDialog OpenRootFolder = new VistaFolderBrowserDialog();
            if (OpenRootFolder.ShowDialog() == true)
            {
                RootPathDisplay.Text = OpenRootFolder.SelectedPath;
                FileExplorer.InstalationUrl = OpenRootFolder.SelectedPath;
                RootPath = OpenRootFolder.SelectedPath;
                UpdateFactionFileList(RootPath+"\\starsector-core\\");
                ExtractAllPortraits(CoreFaction);
            }
            return;
        }
        private void AddGenericPortrait_Click(object sender, RoutedEventArgs e)
        {
            Button sent = sender as Button;
            FactionFile ReceivingFaction = (FactionFile) CFactionList.Items.CurrentItem;
            Portrait Referencing = (Portrait)AllPortraitList.Items.CurrentItem;
            Portrait Transfering = new Portrait(Referencing.RelativePathSource, Referencing.Url, (String)sent.Tag);
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
            FileExplorer.AppendFileCreation();
            return;
        }
    }
 

    public  class Gender
    {
        public static string Male = "Male";
        public static string Female = "Female";
    }

    public static class CollectionExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> collection, TSource itemTofind, Func<TSource, TSource, bool> equalizer)
        {
            foreach (var item in collection)
            {
                if (equalizer(item,itemTofind))
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
