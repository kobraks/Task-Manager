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

        List<Task> tasks = new List<Task>();

        public ProcessList()
        {
        }

        public int Count
        {
            get
            {
                return tasks.Count;
            }
        }

        public Object[] Add(Process process)
        {
            tasks.Add(new Task(process));

            Object[] tmp = new Object[2]
                {
                    tasks[tasks.Count - 1].Name,
                    tasks[tasks.Count - 1].CpuUsage
                };

            return tmp;
        }

        public bool Uniquire(Process process)
        {
            return !tasks.Any(item => item.Name.Equals(process.ProcessName));
        }

        public void Kill(string pName)
        {
            var task = tasks.Where(item => item.Name.Equals(pName));
            task.ToList()[0].Kill();
        }

        int index = 0;

        public void Update(ref DataTable table)
        {
            try
            {
                if (index >= 0 && index < tasks.Count)
                {
                    try
                    {
                        table.Rows[index][1] = tasks[index].CpuUsage;

                        if (tasks[index].HasExited)
                        {
                            table.Rows.RemoveAt(index);
                            tasks.RemoveAt(index);
                            index--;
                        }
                    }
                    catch(Exception)
                    {}

                    index++;
                }
                else
                {
                    index = 0;
                }
            }
            catch (Exception)
            {}
        }

        void Clear()
        {
            tasks.Clear();
        }
    }
}
