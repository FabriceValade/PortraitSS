using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
using PortraitEditor.ViewModel;
using PortraitEditor.ViewModel.SubWindows;
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

namespace PortraitEditor.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FileExplorerTabView: UserControl
    {
        public FileExplorerTabView()
        {
            //DataContext = new SSFileExplorerViewModel();
            InitializeComponent();
        }
        //public FileExplorerWindow(SSFileExplorerViewModel fileExplorer)
        //{
        //    DataContext = fileExplorer;
        //    InitializeComponent();

        //}

        public static readonly DependencyProperty FactionDirectoryProperty = DependencyProperty.Register(
        "FactionDirectory", typeof(SSFactionDirectory), typeof(FileExplorerTabView),
        new PropertyMetadata(new SSFactionDirectory(), OnDirectoryChanged));

        public SSFactionDirectory FactionDirectory
        {
            get { return (SSFactionDirectory)GetValue(FactionDirectoryProperty); }
            set { SetValue(FactionDirectoryProperty, value); }
        }

        private static void OnDirectoryChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((FileExplorerTabView)obj).ViewModel.FactionDirectory = (SSFactionDirectory)args.NewValue;
        }

        public SSFileExplorerViewModel ViewModel
        {
            get { return (SSFileExplorerViewModel)Resources["FileExplorerVM"]; }
        }

        public static readonly DependencyProperty LocalModProperty = DependencyProperty.Register(
        "LocalMod", typeof(SSMod), typeof(FileExplorerTabView),
        new PropertyMetadata(new SSMod(), OnLocalModChanged));

        public SSMod LocalMod
        {
            get { return (SSMod)GetValue(LocalModProperty); }
            set { SetValue(LocalModProperty, value); }
        }

        private static void OnLocalModChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((FileExplorerTabView)obj).ViewModel.LPeSSMod = (SSMod)args.NewValue;
        }
    }
}
