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
using System.Windows.Shapes;

namespace PortraitEditor
{
    /// <summary>
    /// Interaction logic for PortraitListOutputWindow.xaml
    /// </summary>
    public partial class PortraitListOutputWindow : Window
    {
        public ObservableCollection<Portrait> DisplayPortrait { get; set; } = new ObservableCollection<Portrait>();

        public PortraitListOutputWindow(ObservableCollection<Portrait> NewPortraits)
        {
            DataContext = this;
            InitializeComponent();
            foreach (Portrait p in NewPortraits)
            {
                DisplayPortrait.Add(p);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
