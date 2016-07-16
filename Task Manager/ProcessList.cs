using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Data;

namespace Task_Manager
{
    class ProcessList
    {
        readonly Object locker = new object();
        List<Process> list = new List<Process>();
        List<PerformanceCounter> performanceList = new List<PerformanceCounter>();

        public ProcessList()
        {
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public Object[] Add(Process process)
        {
            performanceList.Add(new PerformanceCounter("Process", "% Processor Time", process.ProcessName));
            list.Add(process);

            Object[] tmp = new Object[2]
                {
                    process.ProcessName,
                    Math.Truncate(performanceList[performanceList.Count - 1].NextValue() / Environment.ProcessorCount)
                };

            return tmp;

            /*
             * list.Add(process);
            Object []values = new object[2];
            values[0] = process.ProcessName;
            var pref = new PerformanceCounter();
            pref.CategoryName = "Proces";
            pref.CounterName = "Czas procesora (%)";
            pref.InstanceName = process.ProcessName;
            list2.Add(pref);
             */
        }

        public void DeleteCancelled(ref DataTable table)
        {
            Debug.WriteLine("Atempt to delete");
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    if (list[i].HasExited)
                    {
                        table.Rows.RemoveAt(i);
                        list.RemoveAt(i);
                        performanceList.RemoveAt(i);
                        i--;
                    }
                }
                catch(Exception ext)
                {
                    Debug.WriteLine("Process: " + list[i].ProcessName);
                    Debug.WriteLine(ext.Message);
                }
            }
        }

        public void Update(ref DataTable table)
        {
            for (int i =0; i < list.Count; i++)
            {
                try
                {
                    table.Rows[i][0] = list[i].ProcessName;
                    table.Rows[i][1] = Math.Truncate(performanceList[i].NextValue() / Environment.ProcessorCount);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(list[i].ProcessName);
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public void DetectNew(ref DataTable table)
        {
            var processes = Process.GetProcesses();
            if (list.Count >= processes.Length) return;

            List<Process> news = new List<Process>();

            bool newOne = false;
            bool old = false;
            foreach (var process in processes)
            {
                foreach (var oldProcess in list)
                {
                    if (process.ProcessName == oldProcess.ProcessName)
                    {
                        old = true;
                        newOne = false;
                    }
                    else if (!old) newOne = true;
                }

                if (newOne)
                {
                    newOne = false;
                    news.Add(process);
                }
            }

            foreach (var process in news)
            {
                table.Rows.Add(this.Add(process));
            }
        }

        void Clear()
        {
            list.Clear();
            performanceList.Clear();
        }
    }
}
