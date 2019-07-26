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
        public string RootPath { get; set; }


        public MainWindow()
        {
            DataContext = this;
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
                ExtractAllPortraits(CoreFaction);
            }
            return;
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
        private void ExtractAllPortraits(ObservableCollection<FactionFile> factionFiles)
        {
            foreach (FactionFile OneFactionFile in factionFiles)
            {
                foreach (var p in OneFactionFile.Portraits)
                {
                    bool AlreadySet = AllPortraits.Contains(p);
                    if (!AlreadySet)
                        AllPortraits.Add(p);
                }
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
        public ObservableCollection<Portrait> Portraits { get; set; } = new ObservableCollection<Portrait>();



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
                    Portraits.Add(new Portrait(relativePathSource, (string)url,Gender.Male));
                }
            }
            var PortraitsFemaleUrl = FileRessource.portraits.standard_female;

            if (PortraitsFemaleUrl != null)
            {
                foreach (var url in PortraitsFemaleUrl)
                {
                    Portraits.Add(new Portrait(relativePathSource, (string)url, Gender.Female));
                }
            }
            return;
        }

    }
    public class Portrait : IEquatable<Portrait>
    {
        public string Url { get; set; }
        public bool IsCore { get; set; }
        public string Name { get; set; }
        public string FormatedSource { get; set; }
        public string FullUrl { get; set; }
        public string ImageGender { get; set; }

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
            ImageGender = Gender.Male;
        }
        public Portrait(string relativePathSource, string url, string imageGender)
            :this( relativePathSource, url)
        {
            ImageGender = imageGender;
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            Portrait objAsPortrait = other as Portrait;
            if (objAsPortrait == null) return false;
            else return Equals(objAsPortrait);
        }
        public bool Equals(Portrait other)
        {
            if (other == null) return false;
            return (this.Url.Equals(other.Url));
        }
    }

    public  class Gender
    {
        public static string Male = "Male";
        public static string Female = "Female";
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
