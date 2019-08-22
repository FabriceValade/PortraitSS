using PortraitEditor.Model;
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
    /// Interaction logic for SSModTreeView.xaml
    /// </summary>
    public partial class SSModTreeView : UserControl
    {
        public SSModTreeView()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty HeldModCollectionProperty = DependencyProperty.Register(
        "HeldModCollection", typeof(ObservableCollection<SSMod>), typeof(SSModTreeView),
        new PropertyMetadata(new ObservableCollection<SSMod>(), OnModCollectionChanged));

        public ObservableCollection<SSMod> HeldModCollection
        {
            get { return (ObservableCollection<SSMod>)GetValue(HeldModCollectionProperty); }
            set { SetValue(HeldModCollectionProperty, value); }
        }

        private static void OnModCollectionChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSModTreeView)obj).ViewModel.ModCollection = (ObservableCollection<SSMod>)args.NewValue;
        }

        public SSModTreeViewModel ViewModel
        {
            get { return (SSModTreeViewModel)Resources["SSModTreeVM"]; }
        }
    }
}
