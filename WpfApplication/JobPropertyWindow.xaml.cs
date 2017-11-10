using CoreShared.BO;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication.BO;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for JobPropertyWindow.xaml
    /// </summary>
    public partial class JobPropertyWindow : Window
    {
        private UIJob _job;

        public JobPropertyWindow()
        {
            InitializeComponent();
        }

        public UIJob Job {
            get {
                return _job;
            }
            set
            {
                _job = value;
                InitializeForm();
            }

        }


        private void InitializeForm()
        {
            txtExecutable.Text = Job.Config.Executable;
            txtArguments.Text = Job.Config.Arguments;
            txtFolder.Text = Job.Config.WorkingDirectory;


            cbEnabled.IsChecked = Job.Config.Enabled;

            //cycle
            cbEver.IsChecked = Job.Config.Schedule.Ever;
            cycleValue.Text = string.Format("{0}",Job.Config.Schedule.CycleValue);
            foreach (var e in Enum.GetValues(typeof(CycleUnit)).Cast<CycleUnit>())
            {
                cycleUnit.Items.Add(e);
            }
            cycleUnit.SelectedItem = Job.Config.Schedule.CycleUnit;


            //Range
            cbContinuous.IsChecked = Job.Config.Schedule.Continuous;
            timeHourFrom.Text = string.Format("{0:00}",Job.Config.Schedule.TimeHourFrom);
            timeMinuteFrom.Text = string.Format("{0:00}", Job.Config.Schedule.TimeMinuteFrom);
            timeHourTo.Text = string.Format("{0:00}", Job.Config.Schedule.TimeHourTo);
            timeMinuteTo.Text = string.Format("{0:00}", Job.Config.Schedule.TimeMinuteTo);

            CheckedChanged(null, null);


        }

        private void CheckedChanged(object sender, RoutedEventArgs e)
        {


            if(cbEnabled.IsChecked.Value == false)
            {
                //disable all
                cycleValue.IsEnabled = false;
                cycleUnit.IsEnabled = false;
                cbEver.IsEnabled = false;


                timeHourFrom.IsEnabled = false;
                timeMinuteFrom.IsEnabled = false;
                timeHourTo.IsEnabled = false;
                timeMinuteTo.IsEnabled = false;
                cbContinuous.IsEnabled = false;

            }
            else
            {
                //enable all
                cbEver.IsEnabled = true;

                if (cbEver.IsChecked.Value == true)
                {
                    //disable
                    cycleValue.IsEnabled = false;
                    cycleUnit.IsEnabled = false;
                }
                else
                {
                    //enable
                    cycleValue.IsEnabled = true;
                    cycleUnit.IsEnabled = true;
                }

                cbContinuous.IsEnabled = true;
                if (cbContinuous.IsChecked.Value == true)
                {
                    //disable
                    timeHourFrom.IsEnabled = false;
                    timeMinuteFrom.IsEnabled = false;
                    timeHourTo.IsEnabled = false;
                    timeMinuteTo.IsEnabled = false;
                }
                else
                {
                    //enable
                    timeHourFrom.IsEnabled = true;
                    timeMinuteFrom.IsEnabled = true;
                    timeHourTo.IsEnabled = true;
                    timeMinuteTo.IsEnabled = true;
                }

            }


        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Job.Config.Type = JobType.Java;
            Job.Config.Executable = txtExecutable.Text;
            Job.Config.Arguments = txtArguments.Text;
            Job.Config.WorkingDirectory = txtFolder.Text;

            Job.Config.Enabled = cbEnabled.IsChecked.Value;

            //Cycle
            Job.Config.Schedule.Ever = cbEver.IsChecked.Value;
            Job.Config.Schedule.CycleValue = int.Parse(cycleValue.Text);
            Job.Config.Schedule.CycleUnit = (CycleUnit)Enum.Parse(typeof(CycleUnit), cycleUnit.SelectedItem.ToString());

            //Range
            Job.Config.Schedule.Continuous = cbContinuous.IsChecked.Value;
            Job.Config.Schedule.TimeHourFrom = int.Parse(timeHourFrom.Text);
            Job.Config.Schedule.TimeMinuteFrom = int.Parse(timeMinuteFrom.Text);
            Job.Config.Schedule.TimeHourTo = int.Parse(timeHourTo.Text);
            Job.Config.Schedule.TimeMinuteTo = int.Parse(timeMinuteTo.Text);
            this.DialogResult = true;
        }

      

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnOpenExecutable_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "jar files (*.jar)|*.jar|All files (*.*)|*.*";
            dialog.Multiselect = false;
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtExecutable.Text = dialog.FileName;

                if (txtFolder.Text.Equals(""))
                    txtFolder.Text = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
        }
    }
}
