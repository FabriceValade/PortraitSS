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
            IEnumerable<FileInfo> a = CoreFactionDirectory.EnumerateFiles();
            foreach (FileInfo DataFile in a)
            {

                if (DataFile.Extension == ".faction")
                    CoreFaction.Add(new FactionFile(path,DataFile.FullName));

            }

            //Data.Concat(RootDirectory.EnumerateFiles());

            return;
        }
    }
    public class FactionFile
    {
        public string Path { get; set; }
        public string FileId { get; set; }
        public string LogoPath { get; set; }
        public string ColorRGB { get; set; }

        public FactionFile() { }
        public FactionFile(string relativePathSource, string newPath)
        {
            Path = newPath;
            Regex ExtractFactionFileName = new Regex(@"(?:.*\\)(.*)(?:.faction)");
            FileId = ExtractFactionFileName.Match(Path).Groups[1].ToString();

            dynamic FileRessource = new JavaRessourceExtractor(Path).JavaRessource;

            string ReadResult = File.ReadAllText(Path);

            Regex ExtractLogoPath = new Regex("(?:\"logo\":\")(.*)(?:\".*,)");
            string FormatedSource = relativePathSource.Replace("\\", "/");
            LogoPath = FormatedSource + ExtractLogoPath.Match(ReadResult).Groups[1].ToString();

            Regex ExtractColor = new Regex(@"(?:""color"":\s*\[\s*)(.*)(?:\s*\])");
            string[] ColorCode = ExtractColor.Match(ReadResult).Groups[1].ToString().Split(',');
            if (ColorCode.Count() == 4)
            {
                ColorRGB = "#";
                List<string> ColorArray = new List<string>(4);
                foreach (string oneCode in ColorCode)
                {
                    string oneRgb = Int32.Parse(oneCode).ToString("X2");
                    ColorArray.Add(oneRgb);
                }
                ColorRGB = "#" + ColorArray[3] + ColorArray[0] + ColorArray[1] + ColorArray[2];
            }
            else
            {
                ColorRGB = "#FFFFFFFF";
            }

            string cleanstring = ReadResult.Replace('\n', ' ');
            cleanstring = cleanstring.Replace('\t', ' ');
            cleanstring = cleanstring.Replace('\r', ' ');
            Regex ExtractPortrait = new Regex(@"(?:""portraits"":)(.*)");
            Match a = ExtractPortrait.Match(cleanstring);
            string PortraitStuff = ExtractPortrait.Match(ReadResult).Groups[1].ToString();

            return;
        }

    }

}
