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
                {
                    FactionFile ExtractedFile = new FactionFile(path, DataFile.FullName);
                    ExtractedFile.SetOriginal();
                    CoreFaction.Add(ExtractedFile);
                }
                    
                
            }
            CFactionList.Items.MoveCurrentToFirst();
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
        private void DisplayAppend_Click(object sender, RoutedEventArgs e)
        {
            FactionFile DisplayingFaction = (FactionFile)CFactionList.Items.CurrentItem;
            var win = new PortraitListOutputWindow(DisplayingFaction.GetAppended());
            win.ShowDialog();
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
        public ObservableCollection<Portrait> OriginalPortraits { get; set; } = new ObservableCollection<Portrait>();


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

        public void SetOriginal()
        {
            foreach (Portrait p in Portraits)
            {
                Portrait Originaling = new Portrait(p.RelativePathSource, p.Url, p.ImageGender);
                OriginalPortraits.Add(Originaling);
            }
        }
        public void AddPortrait(Portrait adding)
        {
            Portraits.Add(adding);
            OrderPortrait();
            return;
        }
        public void RemovePortrait(int index)
        {
            if (Portraits.Count>index)
                Portraits.RemoveAt(index);
            return;
        }
        public void OrderPortrait()
        {
            ObservableCollection<Portrait> temp;
            temp = new ObservableCollection<Portrait>(Portraits.OrderBy(Portrait => Portrait));
            Portraits.Clear();
            foreach (Portrait j in temp) Portraits.Add(j);
            return;
        }
        public ObservableCollection<Portrait> GetAppended()
        {
            ObservableCollection<Portrait> Appended = new ObservableCollection<Portrait>();
            bool[] originalUsed = new bool[OriginalPortraits.Count];
            for (int i = 0; i < originalUsed.Length; i++) originalUsed[i] = false;
            
            foreach ( Portrait p in Portraits)
            {
                List<int> PosOriginal = OriginalPortraits.FindAll(p, Portrait.EqualsWithGender);
                int PosFound = -1;
                for (int i = 0; i < PosOriginal.Count && PosFound == -1; i++)
                {
                    if (!originalUsed[PosOriginal[i]])
                    {
                        PosFound = PosOriginal[i];
                        originalUsed[PosOriginal[i]] = true;
                    }
                }
                if (PosFound == -1)
                    Appended.Add(p);
            }

            return Appended;
        }
    }
    public class Portrait : IEquatable<Portrait>, IComparable
    {
        public string Url { get; set; }
        public bool IsCore { get; set; }
        public string Name { get; set; }
        public string RelativePathSource { get; set; }
        public string FormatedSource { get; set; }
        public string FullUrl { get; set; }
        public string ImageGender { get; set; }
        public string OriginalGender { get; set; } = null;
        public bool IsChanged { get; set; } = true;

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
            RelativePathSource = relativePathSource;
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
            return (this.Url.Equals(other.Url) );
        }
        public static bool EqualsWithGender(Portrait other1, Portrait other2)
        {
            if (other1 == null || other2 == null) return false;
            return (other1.Url.Equals(other2.Url) && other1.ImageGender.Equals(other2.ImageGender));
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Portrait otherPortrait)
            {
                if (this.ImageGender == otherPortrait.ImageGender)
                {
                    return this.Name.CompareTo(otherPortrait.Name);
                }
                else
                {
                    if (this.ImageGender == Gender.Male) return -1;
                    else return 1;
                }
            }
            else
                throw new ArgumentException("Object is not a Portrait");
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
