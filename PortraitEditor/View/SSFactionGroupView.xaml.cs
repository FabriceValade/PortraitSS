using PortraitEditor.Model.SSFiles;
using PortraitEditor.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortraitEditor.View
{
    /// <summary>
    /// Interaction logic for SSFactionGroupView.xaml
    /// </summary>
    public partial class SSFactionGroupView : UserControl
    {
        public SSFactionGroupView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FactionGroupModelProperty = DependencyProperty.Register(
        "FactionGroupModel", typeof(SSFactionGroup), typeof(SSFactionGroupView),
        new PropertyMetadata(null, OnProjectChanged));

        public SSFactionGroup FactionGroupModel
        {
            get { return (SSFactionGroup)GetValue(FactionGroupModelProperty); }
            set { SetValue(FactionGroupModelProperty, value); }
        }

        private static void OnProjectChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSFactionGroupView)obj).ViewModel.FactionGroupModel = (SSFactionGroup)args.NewValue;
        }

        public SSFactionGroupViewModel ViewModel
        {
            get { return (SSFactionGroupViewModel)Resources["FactionGroupVM"]; }
        }
    }
}
