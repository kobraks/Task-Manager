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

        public DataTable Table
        {
            get;
            set;
        }

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
                    tasks[tasks.Count - 1].Cpu
                };

            return tmp;
        }

        public bool Uniquire(Process process)
        {
            Task wonderIfItsPresent = new Task(process);

            return !tasks.Any(item => item.Name == wonderIfItsPresent.Name);
        }

        int Index = 0;

        public void UpdateNext()
        {
            if (Index < tasks.Count && Index >= 0)
            {
                try
                {
                    Table.Rows[Index][1] = tasks[Index].Cpu;

                }
                catch(Exception)
                {}

                try
                {
                    if (tasks[Index].HasExited)
                    {
                        tasks[Index] = null;
                        tasks.RemoveAt(Index);
                        Table.Rows.RemoveAt(Index);
                        Index--;
                    }
                }
                catch(Exception)
                {}

                Index++;
            }
            else
            {
                Index = 0;
            }
        }

        public void Kill(string pName)
        {
            foreach(var task in tasks)
            {
                if (String.Compare(task.Name, pName, false) == 0)
                {
                    task.Kill();
                    return;
                }
            }
        }

        public void Clear()
        {
            tasks.Clear();
        }
    }
}
