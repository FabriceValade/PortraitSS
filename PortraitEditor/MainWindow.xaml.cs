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
        public MainWindow()
        {
            DataContext = this;
            CoreFaction = new ObservableCollection<FactionFile>() { new FactionFile("ay\\yo\\hello.faction") };
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
                UpdateFactionFileList(OpenRootFolder.SelectedPath);
            }

        }
        private void UpdateFactionFileList(string path)
        {   

            DirectoryInfo CoreFactionDirectory = new DirectoryInfo(path+ "\\starsector-core\\data\\world\\factions");
            IEnumerable<FileInfo> a = CoreFactionDirectory.EnumerateFiles();
            foreach (FileInfo DataFile in a)
            {

                if (DataFile.Extension == ".faction")
                    CoreFaction.Add(new FactionFile(DataFile.FullName));

            }

            //Data.Concat(RootDirectory.EnumerateFiles());

            return;
        }
    }
    public class FactionFile
    {
        public string Path { get; set; }
        public string FileId { get; set; }

        public FactionFile() { }
        public FactionFile(string newPath)
        {
            Path = newPath;
            Regex ExtractFactionId = new Regex(@"(?:.*\\)(.*)(?:.faction)");
            FileId = ExtractFactionId.Match(Path).Groups[1].ToString();
        }

    }
    
}
