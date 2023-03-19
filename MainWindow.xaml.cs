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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using System.Net;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace TF2ModDownloader
{
    public partial class MainWindow : Window
    {
        string? steampath;
        
        Uri pf2_full_game = new Uri("https://github.com/Pre-Fortress-2/pf2/releases/download/0.7hotfix/pf2_full_0.7.7z");
        Uri pf2_hotfix = new Uri("https://github.com/Pre-Fortress-2/pf2/releases/download/0.7hotfix/pf2_0.7-hotfix.7z");
        Uri open_fortress = new Uri("https://toast.openfortress.fun/open_fortress.zip");
        Uri tf2_classic = new Uri("https://tf2classic.org/tf2c/tf2classic-latest.zip");
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Unzip7z(string sourceFile, string destination)
        {
            System.Diagnostics.ProcessStartInfo a = new System.Diagnostics.ProcessStartInfo();
            a.FileName = "7za.exe";
            a.WindowStyle = ProcessWindowStyle.Hidden;
            a.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", sourceFile, destination);
            Process x = Process.Start(a);
            x.WaitForExit();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.ShowDialog();
            steampath = dialog.SelectedPath;
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (steampath == null)
            {
                System.Windows.MessageBox.Show("Please choose your steam path");
            }
            if(Combobox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please choose your mod");
            }
            if(Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Open Fortress")
            {
                if(File.Exists("Temp/of2.7z"))
                {
                    UnzipFile();
                    return;
                }
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(open_fortress, "Temp/of2.7z");
            }
            if(Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Team Fortress 2 Classic")
            {
                if (File.Exists("Temp/tf2c.7z"))
                {
                    UnzipFile();
                    return;
                }
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(tf2_classic, "Temp/tf2c.7z");
            }
            if(Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Pre Fortress 2")
            {
                System.Windows.MessageBox.Show("Program may freeze while downloading the hotfix. Don't close the program.");
                if (File.Exists("Temp/pf2.7z"))
                {
                    UnzipFile();
                    return;
                }
                WebClient webClient = new WebClient();
                webClient.DownloadFile(pf2_hotfix, "Temp/pf2_hotfix.7z");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(pf2_full_game, "Temp/pf2.7z");

            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadBar.Value = e.ProgressPercentage;
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            UnzipFile();
        }
        private void UnzipFile()
        {
            if(Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Open Fortress")
            {
                if(File.Exists("Temp/of2.7z") == false)
                {
                    System.Windows.MessageBox.Show("Couldnt download the file");
                    return;
                }
                Unzip7z("Temp/of2.7z", steampath + "/steamapps/sourcemods");
                System.Windows.MessageBox.Show("Download and extraction complete! Remember to restart Steam!");
            }
            if (Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Team Fortress 2 Classic")
            {
                if (File.Exists("Temp/tf2c.7z") == false)
                {
                    System.Windows.MessageBox.Show("Couldnt download the file");
                    return;
                }
                Unzip7z("Temp/tf2c.7z", steampath + "/steamapps/sourcemods");
                System.Windows.MessageBox.Show("Download and extraction complete! Remember to restart Steam!");
            }
            if(Combobox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Pre Fortress 2")
            {
                if (File.Exists("Temp/pf2.7z") == false)
                {
                    System.Windows.MessageBox.Show("Couldnt download the file");
                    return;
                }
                Unzip7z("Temp/pf2.7z", steampath + "/steamapps/sourcemods");
                Unzip7z("Temp/pf2_hotfix.7z", steampath + "steamapps/sourcemods");
                System.Windows.MessageBox.Show("Download and extraction complete! Remember to restart Steam!");
            }
        }
    }
}
