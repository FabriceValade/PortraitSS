using PortraitEditor.Model.SSParameters;
using PortraitEditor.Model.SSParameters.Interfaces;
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
    /// Interaction logic for PortraitCollectionView.xaml
    /// </summary>
    public partial class SSPortraitCollectionView : UserControl
    {
        public SSPortraitCollectionView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HeldCollectionProperty = DependencyProperty.Register(
        "HeldCollection", typeof(ObservableCollection<SSPortrait>), typeof(SSPortraitCollectionView),
        new PropertyMetadata(new ObservableCollection<SSPortrait>(), OnProjectChanged));

        public ObservableCollection<SSPortrait> HeldCollection
        {
            get { return (ObservableCollection<SSPortrait>)GetValue(HeldCollectionProperty); }
            set { SetValue(HeldCollectionProperty, value); }
        }

        private static void OnProjectChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSPortraitCollectionView)obj).Model.HeldCollection = (ObservableCollection<SSPortrait>)args.NewValue;
        }

        public SSExternalPortraitCollectionViewModel Model
        {
            get { return (SSExternalPortraitCollectionViewModel)Resources["PortraitCollectionVM"]; }
        }

    }
}
