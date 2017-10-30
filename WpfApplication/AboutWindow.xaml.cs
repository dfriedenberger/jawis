﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            GetProductInfo();
        }

        private void GetProductInfo()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("ProductInfo.xml");

            var version = doc.DocumentElement.SelectSingleNode("/info/version").InnerText;
            var productname = doc.DocumentElement.SelectSingleNode("/info/productname").InnerText;
            var url = doc.DocumentElement.SelectSingleNode("/info/url").InnerText;
            hyperlink.NavigateUri = new Uri(url);
            tbLink.Text = tbLink.Text.Replace("URL", url);
            tbVersion.Text = tbVersion.Text.Replace("PRODUCTNAME", productname).Replace("VERSION", version);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
