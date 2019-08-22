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
    /// Interaction logic for SSFactionView.xaml
    /// </summary>
    public partial class SSFactionView : UserControl
    {
        public SSFactionView()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty HeldFactionProperty = DependencyProperty.Register(
        "HeldFaction", typeof(SSFaction), typeof(SSFactionView),
        new PropertyMetadata(null, OnFactionChanged));

        public SSFaction HeldFaction
        {
            get { return (SSFaction)GetValue(HeldFactionProperty); }
            set { SetValue(HeldFactionProperty, value); }
        }

        private static void OnFactionChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSFactionView)obj).ViewModel.FactionModel = (SSFaction)args.NewValue;
        }

        public SSFactionViewModel ViewModel
        {
            get { return (SSFactionViewModel)Resources["SSFactionVM"]; }
        }
    }
}
