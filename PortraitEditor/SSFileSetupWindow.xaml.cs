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

        public ObservableCollection<Portrait> AllPortraits = new ObservableCollection<Portrait>();
        public ObservableCollection<ModFolder> ModList = new ObservableCollection<ModFolder>();
        SSFolderActions PessFolderAction;
        SSFolderActions ModFolderAction;
        

        public SSFileSetupWindow()
        {
            InitializeComponent();
        }
    }

    public class ModFolder
    {
        public enum FactionAction { Ignore, Remove };
        public enum SourceOfFaction { Core, PeSS, Mod };
        public SSFileUrl ModUrl { get; set; }
        public ObservableCollection<FactionFile> FactionList = new ObservableCollection<FactionFile>();
        public ObservableCollection<FactionAction> FactionActionList = new ObservableCollection<FactionAction>();
        public String FactionName { get; set; }
    }

    
    
}
