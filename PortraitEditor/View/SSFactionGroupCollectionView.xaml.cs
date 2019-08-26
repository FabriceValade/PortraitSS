using PortraitEditor.Model.SSFiles;
using PortraitEditor.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortraitEditor.View
{
    /// <summary>
    /// Interaction logic for SSFactionGroupCollectionView.xaml
    /// </summary>
    public partial class SSFactionGroupCollectionView : UserControl
    {
        public SSFactionGroupCollectionView()
        {
            InitializeComponent();
            ViewModel.SelectedStuff = listview.SelectedItems;
        }

        public static readonly DependencyProperty HeldCollectionProperty = DependencyProperty.Register(
        "HeldCollection", typeof(ObservableCollection<SSFactionGroup>), typeof(SSFactionGroupCollectionView),
        new PropertyMetadata(new ObservableCollection<SSFactionGroup>(), OnProjectChanged));

        public ObservableCollection<SSFactionGroup> HeldCollection
        {
            get { return (ObservableCollection<SSFactionGroup>)GetValue(HeldCollectionProperty); }
            set { SetValue(HeldCollectionProperty, value); }
        }

        private static void OnProjectChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSFactionGroupCollectionView)obj).ViewModel.HeldCollection = (ObservableCollection<SSFactionGroup>)args.NewValue;
        }

        public SSFactionGroupCollectionViewModel ViewModel
        {
            get { return (SSFactionGroupCollectionViewModel)Resources["FactionGroupCollectionVM"]; }
        }



    }
}
