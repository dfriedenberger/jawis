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
using WpfApplication.BO;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for JobDetails.xaml
    /// </summary>
    public partial class JobDetails : Window
    {

        public JobDetails(ObservableCollection<UIHistory> History)
        {
            InitializeComponent();
            lbDetails.ItemsSource = History;
        }


        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
