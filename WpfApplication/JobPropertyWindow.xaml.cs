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
        private IList<System.Windows.Controls.RadioButton> rbGrpSchedule;

        public JobPropertyWindow()
        {
            InitializeComponent();
            rbGrpSchedule = new List<System.Windows.Controls.RadioButton>()
            {
                rbEver,
                rbEveryXMinute,
                rbDailyAtX
            };
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

            cycleValue.Text = string.Format("{0}",Job.Config.Schedule.CycleValue);


            foreach (var e in Enum.GetValues(typeof(CycleUnit)).Cast<CycleUnit>())
            {
                cycleUnit.Items.Add(e);
            }
            cycleUnit.SelectedItem = Job.Config.Schedule.CycleUnit;


            timeHour.Text = string.Format("{0:00}",Job.Config.Schedule.TimeHour);
            timeMinute.Text = string.Format("{0:00}",Job.Config.Schedule.TimeMinute);
            rbGrpSchedule.Single(rb => rb.Name.EndsWith(Job.Config.Schedule.Type.ToString())).IsChecked = true;
            
        }



        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Job.Config.Type = JobType.Java;
            Job.Config.Executable = txtExecutable.Text;
            Job.Config.Arguments = txtArguments.Text;
            Job.Config.WorkingDirectory = txtFolder.Text;

            Job.Config.Enabled = cbEnabled.IsChecked.Value;

            var type = rbGrpSchedule.Single(rb => rb.IsChecked == true).Name.Substring(2);
            Job.Config.Schedule.Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), type);
            Job.Config.Schedule.CycleValue = int.Parse(cycleValue.Text);
            Job.Config.Schedule.CycleUnit = (CycleUnit)Enum.Parse(typeof(CycleUnit), cycleUnit.SelectedItem.ToString());
            Job.Config.Schedule.TimeHour = int.Parse(timeHour.Text);
            Job.Config.Schedule.TimeMinute = int.Parse(timeMinute.Text);
           
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
