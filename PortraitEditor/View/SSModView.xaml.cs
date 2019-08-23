using PortraitEditor.Model;
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
    /// Interaction logic for SSModView.xaml
    /// </summary>
    public partial class SSModView : UserControl
    {
        public SSModView()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty HeldModProperty = DependencyProperty.Register(
        "HeldMod", typeof(SSMod), typeof(SSModView),
        new PropertyMetadata(new SSMod(), OnModCollectionChanged));

        public SSMod HeldMod
        {
            get { return (SSMod)GetValue(HeldModProperty); }
            set { SetValue(HeldModProperty, value); }
        }

        private static void OnModCollectionChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSModView)obj).ViewModel.ModModel = (SSMod)args.NewValue;
        }

        public SSModViewModel ViewModel
        {
            get { return (SSModViewModel)Resources["SSModVM"]; }
        }

        public static readonly DependencyProperty EditVisibilityProperty = DependencyProperty.Register(
        "EditVisibility", typeof(Visibility), typeof(SSModView),
        new PropertyMetadata(Visibility.Visible, OnEditVisibilityChanged));

        public Visibility EditVisibility
        {
            get { return (Visibility)GetValue(EditVisibilityProperty); }
            set { SetValue(EditVisibilityProperty, value); }
        }

        private static void OnEditVisibilityChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((SSModView)obj).ViewModel.EditVisibility = (Visibility)args.NewValue;
        }
    }
}
