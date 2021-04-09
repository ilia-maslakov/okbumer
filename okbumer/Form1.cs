using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace okbumer
{
    public partial class Form1 : Form
    {
        private Manager mgr;
        public Form1()
        {
            InitializeComponent();
            mgr = new Manager(this);
            StartBtn.Click += async (s, e) => await this.StartBtn_ClickAsync(s, e).ConfigureAwait(false);
        }

        public void UpdateProgress(int value)
        {
            var rnd = new Random(DateTime.UtcNow.Millisecond * DateTime.UtcNow.Minute * DateTime.UtcNow.Second);
            int v = 0;
            if (CheckWinner(progressBar1, rnd.Next(13)))
            {
                v = 1;
            }
            if (CheckWinner(progressBar2, rnd.Next(13)))
            {
                v = 2;
            }
            if (CheckWinner(progressBar3, rnd.Next(13)))
            {
                v = 3;
            }
            if (CheckWinner(progressBar4, rnd.Next(13)))
            {
                v = 4;
            }
            if (CheckWinner(progressBar5, rnd.Next(13)))
            {
                v = 5;
            }
            mgr.FoundWinner(v);
            if (v > 0) {
                Int32.TryParse(textBox1.Text, out int res);
                if (v == res)
                {
                    label1.Text = "You Win!";
                }
                else
                {
                    label1.Text = "not this time, try again!";
                }
            }
        }

        private bool CheckWinner(ProgressBar pBar, int step)
        {
            bool res = false;
            var v1 = pBar.Value + step;
            if (v1 >= pBar.Maximum)
            {
                pBar.Value = pBar.Maximum;
                res = true;
            }
            else
            {
                pBar.Value = v1;
            }
            return res;
        }

        private async Task StartBtn_ClickAsync(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;
            progressBar4.Value = 0;
            progressBar5.Value = 0;
            mgr.FoundWinner(0);
            mgr.ResetJob();
            await StartProgressAsync();
        }

        private async Task StartProgressAsync()
        {
            await mgr.StartAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Your bet is: " + textBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartBtn_Click(object sender, EventArgs e)
        {

        }

        private void StartBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}
