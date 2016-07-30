using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Task_Manager
{
    class Task
    {
        PerformanceCounter cpuCounter;
        Process process;

        public String Name
        {
            get
            {
                return process.ProcessName;
            }
        }

        public double CpuUsage
        {
            get
            {
                return Math.Truncate(cpuCounter.NextValue() / Environment.ProcessorCount);
            }
        }

        public bool HasExited
        {
            get
            {
                return process.HasExited;
            }
        }

        public Task(Process _process)
        {
            process = _process;
            cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
        }

        public void Kill()
        {
            process.Kill();
        }
    }
}
