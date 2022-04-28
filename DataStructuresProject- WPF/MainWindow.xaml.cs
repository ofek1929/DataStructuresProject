using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using BL;


namespace DataStructuresProject__WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Manager m;
        TimeSpan s1 = new TimeSpan(5);
        TimeSpan s2 = new TimeSpan(10);

        public MainWindow()
        {
            InitializeComponent();
            TimeSpan s1 = new TimeSpan(0,0,5,5);
            TimeSpan s2 = new TimeSpan(0, 0, 10, 10);
            checkboxSplit.Visibility = Visibility.Collapsed;
            tblAmountOfSplits.Visibility = Visibility.Collapsed;
            tblSplitInsure.Visibility = Visibility.Collapsed;
            tbxSplits.Visibility = Visibility.Collapsed;
            // comboBoxIsOKtoSplit.
            // m = new Manager(122020, 1, new TimeSpan(), new TimeSpan(20003));
            m = new Manager(122020, 1, s1, s2);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            m.AddBoxes(double.Parse( tbxX.Text), double.Parse(tbxY.Text),int.Parse(tbxAmount.Text));
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            var info = m.Info(double.Parse(tbxX.Text), double.Parse(tbxY.Text));
            showListBox.Items.Add(info);
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            checkboxSplit.Visibility = Visibility.Visible;
            tblSplitInsure.Visibility = Visibility.Visible;

            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbxY_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbxAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbxSplits_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            tblAmountOfSplits.Visibility = Visibility.Collapsed;
            tbxSplits.Visibility = Visibility.Collapsed;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            tblAmountOfSplits.Visibility = Visibility.Visible;
            tbxSplits.Visibility = Visibility.Visible;

        }

        private void btnSendToBuy_Click(object sender, RoutedEventArgs e)
        {
            string res;
            m.Buy(double.Parse(tbxX.Text), double.Parse(tbxY.Text),(bool)checkboxSplit.IsChecked, int.Parse(tbxSplits.Text),out res, int.Parse(tbxAmount.Text));
            showListBox.Items.Add(res);
        }
    }
}
