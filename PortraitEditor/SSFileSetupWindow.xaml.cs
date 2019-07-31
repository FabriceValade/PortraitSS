using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PortraitEditor
{
    /// <summary>
    /// Interaction logic for SSFileSetupWindow.xaml
    /// </summary>
    public partial class SSFileSetupWindow : Window
    {
        enum SSFolderActions { Ignore, Clear, Use }

        

        public SSFileExplorer FileExplorer { get; set; } = new SSFileExplorer();

        public ICommand UrlSelectorButtonCommand { get; set; }
        public ICommand ExploreFolderCommand { get; set; }



        public SSFileSetupWindow()
        {
            DataContext = this;
            UrlSelectorButtonCommand = new RelayCommand<SSFileUrl>(new Action<object>(SelectUrl));
            ExploreFolderCommand = new RelayCommand<SSFileUrl>(new Action<object>(d => FileExplorer.ExploreCoreFolder()));
            InitializeComponent();
            
        }


        public void SelectUrl(object obj)
        {
            VistaFolderBrowserDialog OpenRootFolder = new VistaFolderBrowserDialog();
            if (OpenRootFolder.ShowDialog() == true)
            {
                SSFileUrl fileUrl = obj as SSFileUrl;
                fileUrl.ChangeUrl(OpenRootFolder.SelectedPath);

            }
            return;
        }
    }

    public class ModFolder
    {
        public enum FactionAction { Ignore, Remove };
        public enum SourceOfFaction { Core, PeSS, Mod };
        public SSFileUrl ModUrl { get; set; }
        private ObservableCollection<FactionFile> _FactionList = new ObservableCollection<FactionFile>();
        public ObservableCollection<FactionFile> FactionList
        {
            get { return _FactionList; }
            set { _FactionList = value; }
        }
        public ObservableCollection<FactionAction> FactionActionList = new ObservableCollection<FactionAction>();
        public String ModName { get; set; }

        public ModFolder(){ }
    }

    



}
