using System;
using System.Diagnostics;

namespace Task_Manager
{
    class Task
    {
        Process process;
        PerformanceCounter cpu;

        public string Name
        {
            get;
            private set;
        }

        public double Cpu
        {
            get
            {
                return Math.Truncate(cpu.NextValue() / Environment.ProcessorCount);
            }
        }

        public bool HasExited
        {
            get
            {
                return process.HasExited;
            }
        }

        public Task(Process process)
        {
            this.process = process;
            Name = process.ProcessName;
            cpu = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
        }

        public void Kill()
        {
            process.Kill();
        }
    }
}
