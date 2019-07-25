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

        public ObservableCollection<FactionFile> CoreFaction { get; set; }
        public string RootPath { get; set; }


        public MainWindow()
        {
            DataContext = this;
            CoreFaction = new ObservableCollection<FactionFile>();
            
            InitializeComponent();

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
                RootPath = OpenRootFolder.SelectedPath;
                UpdateFactionFileList(RootPath+"\\starsector-core\\");
            }

        }
        private void UpdateFactionFileList(string path)
        {   

            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(path+ "data\\world\\factions");
            IEnumerable<FileInfo> FactionFileList = CoreFactionDirectory.EnumerateFiles();
            foreach (FileInfo DataFile in FactionFileList)
            {

                if (DataFile.Extension == ".faction")
                    CoreFaction.Add(new FactionFile(path,DataFile.FullName));

            }

            //Data.Concat(RootDirectory.EnumerateFiles());

            return;
        }

        private void PortraitList_Selected(object sender, RoutedEventArgs e)
        {
            ListView list = sender as ListView;
            if (list.Name == "MPortraitList")
            {
                MPortraitStat.Visibility = Visibility.Visible;
                FPortraitStat.Visibility = Visibility.Hidden;
            }
            else
            {
                MPortraitStat.Visibility = Visibility.Hidden;
                FPortraitStat.Visibility = Visibility.Visible;
            }
            return;
        }
    }
    public class FactionFile
    {
        public string Path { get; set; }
        public string FileId { get; set; }
        public string DisplayName { get; set; }
        public string LogoPath { get; set; }
        public string ColorRGB { get; set; }
        public ObservableCollection<Portrait> MalePortraits { get; set; } = new ObservableCollection<Portrait>();
        public ObservableCollection<Portrait> FemalePortraits { get; set; } = new ObservableCollection<Portrait>();


        public FactionFile() { }
        public FactionFile(string relativePathSource, string newPath)
        {
            Path = newPath;
            Regex ExtractFactionFileName = new Regex(@"(?:.*\\)(.*)(?:.faction)");
            FileId = ExtractFactionFileName.Match(Path).Groups[1].ToString();

            dynamic FileRessource = new JavaRessourceExtractor(Path).JavaRessource;
            DisplayName = FileRessource.displayName;

            string FormatedSource = relativePathSource.Replace("\\", "/");
            LogoPath = FormatedSource + FileRessource.logo;

            ColorRGB = "#FFFFFFFF";
            if (FileRessource.HasProperty("color"))
            {
                var ColorCode = FileRessource.color;
                if (ColorCode.Count == 4)
                {
                    List<string> ColorArray = new List<string>(4);
                    foreach (string oneCode in ColorCode)
                    {
                        string oneRgb = Int32.Parse(oneCode).ToString("X2");
                        ColorArray.Add(oneRgb);
                    }
                    ColorRGB = "#" + ColorArray[3] + ColorArray[0] + ColorArray[1] + ColorArray[2];
                }
            }
            var PortraitsMaleUrl = FileRessource.portraits.standard_male;

            if (PortraitsMaleUrl != null)
            {
                foreach (var url in PortraitsMaleUrl)
                {
                    MalePortraits.Add(new Portrait(relativePathSource, (string)url));
                }
            }
            var PortraitsFemaleUrl = FileRessource.portraits.standard_female;

            if (PortraitsFemaleUrl != null)
            {
                foreach (var url in PortraitsFemaleUrl)
                {
                    FemalePortraits.Add(new Portrait(relativePathSource, (string)url));
                }
            }
            return;
        }

    }
    public class Portrait
    {
        public string Url { get; set; }
        public bool IsCore { get; set; }
        public string Name { get; set; }
        public string FormatedSource { get; set; }
        public string FullUrl { get; set; }

        public Portrait(string relativePathSource, string url)
        {
            Url = url;
            Regex FindCore = new Regex("starsector-core");
            Match FoundCore = FindCore.Match(relativePathSource);
            if (FoundCore.Success)
                IsCore = true;
            else
                IsCore = false;
            Regex ExtractFileName = new Regex(@"(?:.*/)(.*)(?:\.)");
            Name = ExtractFileName.Match(url).Groups[1].ToString();
            FormatedSource = relativePathSource.Replace("\\", "/");
            FullUrl = FormatedSource + '/' + Url;
        }
    }

    //public class MultiValueConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (values != null)
    //            return values[0];
    //        else
    //            return null;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}
