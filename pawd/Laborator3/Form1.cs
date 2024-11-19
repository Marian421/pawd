using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laborator3
{
    public partial class Form1 : Form
    {
        private List<float> cpuValues = new List<float>();
        private const int MaxCpuValues = 5;
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        PerformanceCounter gpuCounter;  // Adăugăm contorul pentru GPU

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            // Inițializarea contorului pentru GPU
            gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage", "GPU0");

            timer1.Start();
        }

        private float GetCpuUsage()
        {
            float cpuUsage = 0;

            using (var searcher = new ManagementObjectSearcher("SELECT LoadPercentage FROM Win32_Processor"))
            {
                foreach (var obj in searcher.Get())
                {
                    cpuUsage += Convert.ToSingle(obj["LoadPercentage"]);
                }
            }

            return cpuUsage;
        }

        private float GetTotalMemoryInMB()
        {
            float totalMemory = 0;

            using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
            {
                foreach (var obj in searcher.Get())
                {
                    totalMemory += Convert.ToSingle(obj["TotalPhysicalMemory"]) / (1024 * 1024);
                }
            }

            return totalMemory;
        }

        private float GetAvailabeMemoryInMB()
        {
            float availabeMemory = 0;

            using (var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem"))
            {
                foreach (var obj in searcher.Get())
                {
                    availabeMemory += Convert.ToSingle(obj["FreePhysicalMemory"]) / 1024;
                }
            }

            return availabeMemory;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Obținem valorile de utilizare CPU, RAM și GPU
            float cpuUsage = GetCpuUsage();
            float availableMemory = GetAvailabeMemoryInMB();
            float totalMemory = GetTotalMemoryInMB();
            float usedMemory = totalMemory - availableMemory;
            float memoryUsagePercentage = (usedMemory / totalMemory) * 100;
            float gpuUsage = gpuCounter.NextValue();  // Obținem utilizarea GPU-ului

            // Actualizăm progresul și etichetele pentru CPU, RAM și GPU
            progressBar1.Value = (int)cpuUsage;
            label1.Text = $"Utilizare CPU: {cpuUsage:F2}%";

            progressBar2.Value = (int)memoryUsagePercentage;
            label2.Text = $"Utilizare RAM: {memoryUsagePercentage:F2}%";

            // Adăugăm progresul și eticheta pentru GPU
            progressBar3.Value = (int)gpuUsage;
            label3.Text = $"Utilizare GPU: {gpuUsage:F2}%";
        }
    }
}
