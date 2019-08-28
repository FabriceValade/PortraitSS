using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
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
    /// Interaction logic for PortraitExplorerWindow.xaml
    /// </summary>
    public partial class PortraitExplorerTabView : UserControl
    {

        public PortraitExplorerTabView()
        {
            InitializeComponent();
            ViewModel.SelectedGroups = FactionGroupCollectionView.SelectedItems;
            this.Loaded += PortraitExplorerTabView_Loaded;
        }

        private void PortraitExplorerTabView_Loaded(object sender, RoutedEventArgs e)
        {
            //when the view is loaded, make sure to restore binding that could have been broken by modification of the filedirectory
            var tab = sender as PortraitExplorerTabView;
            Binding myBinding = new Binding("ViewModel.HeldView.CurrentItem.Portraits");
            myBinding.Source = tab.FactionGroupCollectionView;
            myBinding.Mode = BindingMode.OneWay;
            tab.DetailCollectionView.SetBinding(SSPortraitCollectionView.HeldCollectionProperty, myBinding);
            if (tab.FactionGroupCollectionView.ViewModel.HeldView.CurrentPosition < 0)
                tab.FactionGroupCollectionView.ViewModel.HeldView.MoveCurrentToFirst();
        }

        public static readonly DependencyProperty FactionDirectoryProperty = DependencyProperty.Register(
        "FactionDirectory", typeof(SSFactionDirectory), typeof(PortraitExplorerTabView),
        new PropertyMetadata(new SSFactionDirectory(), OnDirectoryChanged));

        public SSFactionDirectory FactionDirectory
        {
            get { return (SSFactionDirectory)GetValue(FactionDirectoryProperty); }
            set { SetValue(FactionDirectoryProperty, value); }
        }

        private static void OnDirectoryChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((PortraitExplorerTabView)obj).ViewModel.FactionDirectory = (SSFactionDirectory)args.NewValue;
        }

        public SSPortraitExplorerViewModel ViewModel
        {
            get { return (SSPortraitExplorerViewModel)Resources["PortraitExplorerVM"]; }
        }

        public static readonly DependencyProperty LocalModProperty = DependencyProperty.Register(
        "LocalMod", typeof(SSMod), typeof(PortraitExplorerTabView),
        new PropertyMetadata(new SSMod(), OnLocalModChanged));

        public SSMod LocalMod
        {
            get { return (SSMod)GetValue(LocalModProperty); }
            set { SetValue(LocalModProperty, value); }
        }

        private static void OnLocalModChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((PortraitExplorerTabView)obj).ViewModel.LocalMod = (SSMod)args.NewValue;
        }

    }
}
