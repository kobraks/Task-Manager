using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Diagnostics;

namespace Task_Manager
{
    class Table
    {
        readonly Object locker = new object();
        bool update = false;
        bool isRun = true;
        DataTable _table = new DataTable();

        ProcessList processList = new ProcessList();

        Thread thread;

        void exec()
        {
            while (isRun)
            {
                lock(locker)
                {
                    while (!update) Monitor.Wait(locker);

                    update = false;
                }
                processList.Update(ref _table);
            }
        }

        public DataTable DTable
        {
            get
            {
                return _table;
            }

            private set
            {
                _table = value;
            }
        }

        public Table()
        {
            DTable.Columns.Add("Process Name", typeof(string));
            DTable.Columns.Add("CPU", typeof(float));
            DataColumn column = new DataColumn();

            thread = new Thread(exec);
            thread.Start();
            thread.Priority = ThreadPriority.Lowest;
        }

        public void Add(Process process)
        {
            lock(locker)
            {
                if (processList.Uniquire(process))
                {
                    DTable.Rows.Add(processList.Add(process));
                }
            }
        }

        public void Update()
        {
            lock (locker)
            {
                update = true;
                Monitor.Pulse(locker);
            }
        }

        public void Kill(string pName)
        {
            processList.Kill(pName);
        }

        public void Destroy()
        {
            try
            {
                isRun = false;
                thread.Abort();
            }
            catch(Exception)
            { }
        }

        ~Table()
        {
            Destroy();
        }
    }
}
