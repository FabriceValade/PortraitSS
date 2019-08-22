using PortraitEditor.Extensions;
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
            ((SSPortraitCollectionView)obj).ViewModel.HeldCollection = (ObservableCollection<SSPortrait>)args.NewValue;
        }

        public SSExternalPortraitCollectionViewModel ViewModel
        {
            get { return (SSExternalPortraitCollectionViewModel)Resources["PortraitCollectionVM"]; }
        }

        #region Button 1 text/command
        public static readonly DependencyProperty Button1NameProperty = DependencyProperty.Register(
       "Button1Name", typeof(string), typeof(SSPortraitCollectionView),
       new PropertyMetadata(null, OnButton1Changed));

        public string Button1Name
        {
            get { return (string)GetValue(Button1NameProperty); }
            set { SetValue(Button1NameProperty, value); }
        }
        private static void OnButton1Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button1Text = (string)args.NewValue; }

        public static readonly DependencyProperty Button1CommandProperty = DependencyProperty.Register(
      "Button1Command", typeof(ICommand), typeof(SSPortraitCollectionView),
      new PropertyMetadata(null, OnButton1CommandChanged));

        public ICommand Button1Command
        {
            get { return (ICommand)GetValue(Button1CommandProperty); }
            set { SetValue(Button1CommandProperty, value); }
        }
        private static void OnButton1CommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button1Command = (ICommand)args.NewValue; }
        #endregion

        #region Button 2 text/command
        public static readonly DependencyProperty Button2NameProperty = DependencyProperty.Register(
       "Button2Name", typeof(string), typeof(SSPortraitCollectionView),
       new PropertyMetadata(null, OnButton2Changed));

        public string Button2Name
        {
            get { return (string)GetValue(Button2NameProperty); }
            set { SetValue(Button2NameProperty, value); }
        }
        private static void OnButton2Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button2Text = (string)args.NewValue; }

        public static readonly DependencyProperty Button2CommandProperty = DependencyProperty.Register(
      "Button2Command", typeof(ICommand), typeof(SSPortraitCollectionView),
      new PropertyMetadata(null, OnButton2CommandChanged));

        public ICommand Button2Command
        {
            get { return (ICommand)GetValue(Button2CommandProperty); }
            set { SetValue(Button2CommandProperty, value); }
        }
        private static void OnButton2CommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button2Command = (ICommand)args.NewValue; }
        #endregion

        #region Button 3 text/command
        public static readonly DependencyProperty Button3NameProperty = DependencyProperty.Register(
       "Button3Name", typeof(string), typeof(SSPortraitCollectionView),
       new PropertyMetadata(null, OnButton3Changed));

        public string Button3Name
        {
            get { return (string)GetValue(Button3NameProperty); }
            set { SetValue(Button3NameProperty, value); }
        }
        private static void OnButton3Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button3Text = (string)args.NewValue; }

        public static readonly DependencyProperty Button3CommandProperty = DependencyProperty.Register(
      "Button3Command", typeof(ICommand), typeof(SSPortraitCollectionView),
      new PropertyMetadata(null, OnButton3CommandChanged));

        public ICommand Button3Command
        {
            get { return (ICommand)GetValue(Button3CommandProperty); }
            set { SetValue(Button3CommandProperty, value); }
        }
        private static void OnButton3CommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((SSPortraitCollectionView)obj).ViewModel.Button3Command = (ICommand)args.NewValue; }
        #endregion

        #region grouping possibilities
        public static readonly DependencyProperty GroupDescriptionComboBoxProperty = DependencyProperty.Register(
        "GroupDescriptionComboBox", typeof(List<ComboboxContent<PropertyGroupDescription>>), typeof(SSPortraitCollectionView),
        new PropertyMetadata(new List<ComboboxContent<PropertyGroupDescription>>() { new ComboboxContent<PropertyGroupDescription>()}, OnGroupDescriptionComboBoxChanged));

        public List<ComboboxContent<PropertyGroupDescription>> GroupDescriptionComboBox
        {
            get { return (List<ComboboxContent<PropertyGroupDescription>>)GetValue(GroupDescriptionComboBoxProperty); }
            set { SetValue(GroupDescriptionComboBoxProperty, value); }
        }
        private static void OnGroupDescriptionComboBoxChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((SSPortraitCollectionView)obj).ViewModel.GroupDescriptionComboBox = (List<ComboboxContent<PropertyGroupDescription>>)args.NewValue;
        }
        #endregion
    }
}
