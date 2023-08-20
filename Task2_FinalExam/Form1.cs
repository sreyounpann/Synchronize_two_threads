using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2_FinalExam
{
    public partial class Form1 : Form
    {
        private AutoResetEvent thread1Event;
        private AutoResetEvent thread2Event;
        private int count;
        private bool isIncrementing;
        private bool isCountingBack;

        public Form1()
        {
            InitializeComponent();
            thread1Event = new AutoResetEvent(true);
            thread2Event = new AutoResetEvent(false);
            count = 1;
            isIncrementing = true;
            isCountingBack = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() => ThreadMethod1());
            Task.Run(() => ThreadMethod2());
        }

        private void AppendText(ListBox listBox, string text)
        {
            if (listBox.InvokeRequired)
            {
                listBox.Invoke(new Action(() => AppendText(listBox, text)));
            }
            else
            {
                listBox.Items.Add(text);
            }
        }

        private void ThreadMethod1()
        {
            while (true)
            {
                if (!isCountingBack)
                {
                    if (isIncrementing)
                    {
                        if (count <= 8)
                        {
                            thread1Event.WaitOne();
                            Thread.Sleep(1000);
                            listBox1.Invoke((Action)(() => AppendText(listBox1, count.ToString())));
                            count++;
                            thread2Event.Set();
                        }
                        else
                        {
                            isIncrementing = false;
                            isCountingBack = true;
                        }
                    }
                    else
                    {
                        if (count >= 1)
                        {
                            thread1Event.WaitOne();
                            Thread.Sleep(1000);
                            listBox1.Invoke((Action)(() => AppendText(listBox1, count.ToString())));
                            count--;
                            thread2Event.Set();
                        }
                        else
                        {
                            isIncrementing = true;
                            thread2Event.Set();
                        }
                    }
                }
                else
                {
                    if (count >= 1)
                    {
                        thread1Event.WaitOne();
                        Thread.Sleep(1000);
                        listBox1.Invoke((Action)(() => AppendText(listBox1, count.ToString())));
                        count--;
                        thread2Event.Set();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void ThreadMethod2()
        {
            while (true)
            {
                if (!isCountingBack)
                {
                    if (isIncrementing)
                    {
                        if (count <= 8)
                        {
                            thread2Event.WaitOne();
                            Thread.Sleep(1000);
                            listBox2.Invoke((Action)(() => AppendText(listBox2, count.ToString())));
                            count++;
                            thread1Event.Set();
                        }
                        else
                        {
                            isIncrementing = false;
                            isCountingBack = true;
                        }
                    }
                    else
                    {
                        if (count >= 1)
                        {
                            thread2Event.WaitOne();
                            Thread.Sleep(1000);
                            listBox2.Invoke((Action)(() => AppendText(listBox2, count.ToString())));
                            count--;
                            thread1Event.Set();
                        }
                        else
                        {
                            isIncrementing = true;
                            thread1Event.Set();
                        }
                    }
                }
                else
                {
                    if (count >= 0)
                    {
                        thread2Event.WaitOne();
                        Thread.Sleep(1000);
                        listBox2.Invoke((Action)(() => AppendText(listBox2, count.ToString())));
                        count--;
                        thread1Event.Set();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            thread1Event.Set();
            thread2Event.Set();
            base.OnFormClosing(e);
        }
    }
}
