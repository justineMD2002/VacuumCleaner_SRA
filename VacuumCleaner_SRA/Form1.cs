using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using VacuumCleaner_SRA.Properties;
using System.Timers;

namespace VacuumCleaner_SRA
{
    public partial class Form1 : Form
    {
        private bool shouldUpdateImage = true;
        private bool isTimerRunning = true;
        private System.Timers.Timer timer = new System.Timers.Timer(5000);
        private Random random = new Random();
        List<EnvironmentVacuum> environmentList = new List<EnvironmentVacuum>
        {
            new EnvironmentVacuum { Loc = 'A' },
            new EnvironmentVacuum { Loc = 'B' },
            new EnvironmentVacuum { Loc = 'C' },
            new EnvironmentVacuum { Loc = 'D' }
        };
        private int selectedIndex;

        public Form1()
        {
            InitializeComponent();
            environmentList[0].PicBox = pictureBox1;
            environmentList[1].PicBox = pictureBox2;
            environmentList[2].PicBox = pictureBox3;
            environmentList[3].PicBox = pictureBox4;
            randomEnvironment();
            timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
        }

        private void DisplayRandomPicture(int index)
        {
            if (environmentList[index].PicBox != null)
            {
                environmentList[index].PicBox.Image = null;
            }
            environmentList[index].PicBox.Image = Resources.amogus;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayRandomPicture(selectedIndex = random.Next(environmentList.Count));
        }

        public void randomEnvironment()
        {
            for (int i = 0; i < environmentList.Count; i++)
            {
                environmentList[i].Stat = random.Next(2);
                if (environmentList[i].PicBox != null)
                {
                    if (environmentList[i].Stat == 0) environmentList[i].PicBox.BackColor = Color.LightGreen;
                    else environmentList[i].PicBox.BackColor = Color.PaleVioletRed;
                }
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            shouldUpdateImage = true; 
            DisplayRandomPicture(selectedIndex);
            randomEnvironment();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            isTimerRunning = true;
            textBox1.AppendText("Booting up..." + Environment.NewLine);
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnRandom.Enabled = false;
            timer.Start();
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            int temp = selectedIndex;
            string action = (environmentList[selectedIndex].Stat == 0) ? "Clean" : "Dirty";
            if (textBox1.InvokeRequired) textBox1.Invoke(new Action(() => OnTimerElapsed(sender, e)));
            else
            {
                await AppendTextWithDelay(Environment.NewLine + "Current location: " + environmentList[selectedIndex].Loc);
                await AppendTextWithDelay("Status: " + action);

                if (environmentList[selectedIndex].Stat == 1)
                {
                    environmentList[selectedIndex].Stat = 0;
                    environmentList[selectedIndex].PicBox.BackColor = Color.LightGreen;
                    await AppendTextWithDelay("Cleaning...");
                }
                else await AppendTextWithDelay("No operation");
                await AppendTextWithDelay("Proceeding to the next location.");

                if (shouldUpdateImage) 
                {
                    if (environmentList[selectedIndex].Loc == 'A')
                    {
                        selectedIndex = (random.Next(2) == 0) ? 1 : 2;
                        if (selectedIndex == 1) textBox1.AppendText("Move right" + Environment.NewLine);
                        else textBox1.AppendText("Move down" + Environment.NewLine);
                    }
                    else if (environmentList[selectedIndex].Loc == 'B')
                    {
                        selectedIndex = (random.Next(2) == 0) ? 0 : 3;
                        if (selectedIndex == 0) textBox1.AppendText("Move left" + Environment.NewLine);
                        else textBox1.AppendText("Move down" + Environment.NewLine);
                    }
                    else if (environmentList[selectedIndex].Loc == 'C')
                    {
                        selectedIndex = (random.Next(2) == 0) ? 0 : 3;
                        if (selectedIndex == 0) textBox1.AppendText("Move up" + Environment.NewLine);
                        else textBox1.AppendText("Move right" + Environment.NewLine);
                    }
                    else if (environmentList[selectedIndex].Loc == 'D')
                    {
                        selectedIndex = (random.Next(2) == 0) ? 1 : 2;
                        if (selectedIndex == 1) textBox1.AppendText("Move up" + Environment.NewLine);
                        else textBox1.AppendText("Move left" + Environment.NewLine);
                    }

                    environmentList[temp].PicBox.Image = null;
                    DisplayRandomPicture(selectedIndex);
                }
            }
        }
        private async Task AppendTextWithDelay(string text)
        {
            if (!isTimerRunning) return;
            textBox1.AppendText(text + Environment.NewLine);
            textBox1.ScrollToCaret(); 
            await Task.Delay(1000); 
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            shouldUpdateImage = false;
            isTimerRunning = false;
            timer.Stop();
            btnStart.Enabled = true;
            btnRandom.Enabled = true;
            btnStop.Enabled = false;
            textBox1.AppendText("Shutting down..." + Environment.NewLine);
        }
    }
}
