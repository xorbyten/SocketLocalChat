using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace WinFormClient
{
    public partial class Form1 : Form
    {
        ClientClass clientClass;
        public delegate void InvokeDel(string txt);
        public static Form1 form;
        public Form1()
        {
            InitializeComponent();
            clientClass = new ClientClass();
            form = this;
        }

        public void SetTextStdrt(string txt)
        {
            if(this.IsHandleCreated)
            {
                if (textBox1.InvokeRequired)
                {
                    textBox1.Invoke(new InvokeDel(SetTextStdrt), new object[] { txt });
                    return;
                }
                else
                {
                    textBox1.AppendText(txt);
                    textBox1.AppendText(Environment.NewLine);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(clientClass.SendMessage);
            thread2.Start($"{textBox3.Text} => {textBox2.Text}&msg_cmd");
            textBox2.Clear();

            Thread thread3 = new Thread( () => {
               clientClass.ReceiveMessageFromServer();
            }); thread3.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientClass.ConnectToServer("127.0.0.1", 8888);
            Thread thread1 = new Thread(clientClass.SendMessage);
            thread1.Start(textBox3.Text + "&usr_cmd");
            textBox3.Enabled = false;
            textBox1.AppendText("Connected to server.");
            textBox1.AppendText(Environment.NewLine);
            Thread thread4 = new Thread(() => {
                clientClass.ReceiveMessageFromServer();
            }); thread4.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread thread5 = new Thread(clientClass.SendMessage);
            thread5.Start($"{textBox3.Text} disconnected...&cls_cmd");
            Application.ExitThread();
            Application.Exit();
        }
    }
}
