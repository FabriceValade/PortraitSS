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
            this.Loaded += delegate { ViewModel.OnLoaded(); };
            this.Unloaded += delegate { ViewModel.OnUnloaded(); };

            Binding myBinding = new Binding("SelectedItems");
            myBinding.Source = listview;
            myBinding.Mode = BindingMode.OneWay;
            this.SetBinding(SelectedItemsProperty, myBinding);

            Binding OtherBinding = new Binding("SelectedItem");
            OtherBinding.Source = listview;
            OtherBinding.Mode = BindingMode.OneWay;
            this.SetBinding(SelectedItemProperty, OtherBinding);

        }

        public SSFactionGroupCollectionViewModel ViewModel
        {
            get { return (SSFactionGroupCollectionViewModel)Resources["FactionGroupCollectionVM"]; }
        }

        #region HeldCollection property
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
        #endregion

        #region Code that passthrough the selected items or item of the listview (bound on view construction)
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        "SelectedItems", typeof(System.Collections.IList), typeof(SSFactionGroupCollectionView));

        public System.Collections.IList SelectedItems
        {
            get { return (System.Collections.IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        "SelectedItem", typeof(SSFactionGroup), typeof(SSFactionGroupCollectionView));

        public SSFactionGroup SelectedItem
        {
            get { return (SSFactionGroup)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        #region Code for the collection filter
        public static readonly DependencyProperty CollectionFilterProperty = DependencyProperty.Register(
        "CollectionFilter", typeof(FilterEventHandler), typeof(SSFactionGroupCollectionView), new PropertyMetadata(null, OnFilterChanged));


        public FilterEventHandler CollectionFilter
        {
            get { return (FilterEventHandler)GetValue(CollectionFilterProperty); }
            set { SetValue(CollectionFilterProperty, value); }
        }

        private static void OnFilterChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSFactionGroupCollectionView)obj).ViewModel.CollectionFilter = (FilterEventHandler)args.NewValue;
        }
        #endregion
    }
}
