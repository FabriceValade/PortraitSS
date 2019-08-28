using PortraitEditor.Model;
using PortraitEditor.Model.SSFiles;
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
    /// Interaction logic for FileWriterWindow.xaml
    /// </summary>
    public partial class FileWriterTabView : UserControl
    {
        public FileWriterTabView()
        {
            InitializeComponent();
            this.Loaded += FileWriterTabView_Loaded;
            this.Loaded += delegate { ViewModel.OnLoaded(); };
            this.Unloaded += delegate { ViewModel.OnUnloaded(); };
        }

        private void FileWriterTabView_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public SSFileWriterViewModel ViewModel
        {
            get { return (SSFileWriterViewModel)Resources["FileWriterVM"]; }
        }

        #region dependencyproperty linking VM FactionDirectory
        public static readonly DependencyProperty FactionDirectoryProperty = DependencyProperty.Register(
                            "FactionDirectory",
                            typeof(SSFactionDirectory),
                            typeof(FileWriterTabView),
                            new PropertyMetadata(new SSFactionDirectory(), OnDirectoryChanged));

        public SSFactionDirectory FactionDirectory
        {
            get { return (SSFactionDirectory)GetValue(FactionDirectoryProperty); }
            set { SetValue(FactionDirectoryProperty, value); }
        }

        private static void OnDirectoryChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((FileWriterTabView)obj).ViewModel.FactionDirectory = (SSFactionDirectory)args.NewValue;
        }
        #endregion

        #region Dependencyproperty linking VM LocalMod
        public static readonly DependencyProperty LocalModProperty = DependencyProperty.Register(
        "LocalMod", typeof(SSMod), typeof(FileWriterTabView),
        new PropertyMetadata(new SSMod(), OnLocalModChanged));

        public SSMod LocalMod
        {
            get { return (SSMod)GetValue(LocalModProperty); }
            set { SetValue(LocalModProperty, value); }
        }

        private static void OnLocalModChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            ((FileWriterTabView)obj).ViewModel.LocalMod = (SSMod)args.NewValue;
        }
        #endregion
    }
}
