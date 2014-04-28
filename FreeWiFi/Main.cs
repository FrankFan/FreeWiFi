/*
 *  Description：WiFI助手 V0.0.1
 *  Author: FrankFan
 *  Email:fanyong@gmail.com
 */
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace FreeWiFi
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            string ssid = txtName.Text.Trim();
            string password = txtPassword.Text.Trim();

            bool isValid = isValidInput(ssid, password);

            if (!isValid)
                return;

            Initial(ssid, password);



        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartWiFi();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string cmd = string.Format(@"netsh wlan stop hostednetwork");
            ExecuteCmd(cmd);
        }

        private bool isValidInput(string ssid, string password)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                MessageBox.Show("SSID不能为空.");
                return false;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("密码至少需要8位.");
                return false;
            }

            return true;
        }

        private void Initial(string ssid, string password)
        {
            this.txtOutput.Text = string.Empty;

            string cmd = string.Format(@"netsh wlan set hostednetwork mode=allow ssid={0} key=""{1}""", ssid, password);

        }

        private void ExecuteCmd(string cmd)
        {
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardError = true;
            cmdStartInfo.RedirectStandardInput = true;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;

            Process cmdProcess = new Process();
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.ErrorDataReceived += cmdProcess_ErrorDataReceived;
            cmdProcess.OutputDataReceived += cmdProcess_OutputDataReceived;
            cmdProcess.EnableRaisingEvents = true;
            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();
            cmdProcess.BeginErrorReadLine();

            cmdProcess.StandardInput.WriteLine(cmd);     //Execute ping bing.com
            cmdProcess.StandardInput.WriteLine("exit");                  //Execute exit.

            cmdProcess.WaitForExit();
        }

        void cmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.txtOutput.Text += (e.Data + "\r\n");
        }

        void cmdProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.txtOutput.Text += (e.Data + "\r\n");
        }

        private void StartWiFi()
        {
            this.txtOutput.Text = string.Empty;

            string cmd = string.Format(@"netsh wlan start hostednetwork");
            ExecuteCmd(cmd);
        }

    }
}
