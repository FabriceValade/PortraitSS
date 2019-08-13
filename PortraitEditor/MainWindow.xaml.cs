using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortraitEditor.ViewModel;
using PortraitEditor.View;
using PortraitEditor.JsonHandling;
using PortraitEditor.ViewModel.SubWindows;

namespace PortraitEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {


        public SSFileExplorerViewModel FileExplorer { get; set; } = new SSFileExplorerViewModel();
        public SSPortraitExplorerViewModel PortraitExplorer { get; set; }

        ICommand _ExploreFileCommand;
        public ICommand ExploreFileCommand
        {
            get
            {
                if (_ExploreFileCommand == null)
                {
                    _ExploreFileCommand = new RelayCommand<object>(param => this.ExploreFiles());
                }
                return _ExploreFileCommand;
            }
        }
        ICommand _ExplorePortraitCommand;
        public ICommand ExplorePortraitCommand
        {
            get
            {
                if (_ExplorePortraitCommand == null)
                {
                    _ExplorePortraitCommand = new RelayCommand<object>(param => this.ExplorePortraits());
                }
                return _ExplorePortraitCommand;
            }
        }


        public MainWindow()
        {
            //FileExplorer.ShowDialog();
            //PortraitExplorer = new SSPortraitExplorerViewModel(FileExplorer.FactionDirectory,FileExplorer.LPeSSMod);
            //PortraitExplorer.ShowDialog();
            DataContext = this;
            InitializeComponent();
        }

        

 
       
        private void ExploreFiles()
        {
            this.Hide();
            FileExplorer.ShowDialog();
            this.Show();
            return;
        }
        private void ExplorePortraits()
        {
            if (FileExplorer.LPeSSMod == null)
                return;
            if (PortraitExplorer==null)
                PortraitExplorer = new SSPortraitExplorerViewModel(FileExplorer.FactionDirectory, FileExplorer.LPeSSMod);
            //PortraitExplorer.L_PeSSMod = FileExplorer.LPeSSMod;
            this.Hide();
            PortraitExplorer.ShowDialog();
            this.Show();
            return;
        }



    }
 

    




}
