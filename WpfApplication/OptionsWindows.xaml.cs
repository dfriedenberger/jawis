using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoreShared.BO;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class OptionsWindows : Window
    {

        public OptionsWindows()
        {
            InitializeComponent();
        }

        public Options Options {
            get {
                return new Options()
                {
                    JavaBinary = txtJavaBinary.Text,
                    UseFromPath = cbUsePATHVariable.IsChecked.Value
                };
            }
            set
            {
                txtJavaBinary.Text = value.JavaBinary;
                cbUsePATHVariable.IsChecked = value.UseFromPath;
            }
        }

     

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void btnOpenJavaBinary_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            dialog.Multiselect = false;
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtJavaBinary.Text = dialog.FileName;
            }
        }
        private void cbUsePATHVariable_Checked(object sender, RoutedEventArgs e)
        {
            btnOpenJavaBinary.IsEnabled = false;
            txtJavaBinary.IsEnabled = false;
        }

        private void cbUsePATHVariable_Unchecked(object sender, RoutedEventArgs e)
        {
            btnOpenJavaBinary.IsEnabled = true;
            txtJavaBinary.IsEnabled = true;
        }




    }
}
