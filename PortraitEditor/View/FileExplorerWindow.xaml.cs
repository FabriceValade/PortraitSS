using PortraitEditor.ViewModel;
using PortraitEditor.ViewModel.SubWindows;
using System;
using System.Collections.Generic;
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

namespace PortraitEditor.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FileExplorerWindow : Window
    {
        public FileExplorerWindow()
        {
            DataContext = new SSFileExplorerViewModel();
            InitializeComponent();
        }
        public FileExplorerWindow(SSFileExplorerViewModel fileExplorer)
        {
            DataContext = fileExplorer;
            InitializeComponent();

        }
    }
}
