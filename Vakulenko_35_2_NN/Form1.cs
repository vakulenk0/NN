using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vakulenko_35_2_NN
{
    public partial class Form1 : Form
    {
        int[] InputData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Form1()
        {
            InitializeComponent();
        }

        void ChangeColor(Button button, int index)
        {
            if (InputData[index] == 0)
            {
                button.BackColor = Color.Black;
                InputData[index] = 1;
            }
            else
            {
                button.BackColor = Color.Gray;
                InputData[index] = 0;
            }

        }

        private void button1_Click(object sender, EventArgs e) => ChangeColor(button1, 0);
        private void button2_Click(object sender, EventArgs e) => ChangeColor(button2, 1);
        private void button3_Click(object sender, EventArgs e) => ChangeColor(button3, 2);
        private void button4_Click(object sender, EventArgs e) => ChangeColor(button4, 3);
        private void button5_Click(object sender, EventArgs e) => ChangeColor(button5, 4);
        private void button6_Click(object sender, EventArgs e) => ChangeColor(button6, 5);
        private void button7_Click(object sender, EventArgs e) => ChangeColor(button7, 6);
        private void button8_Click(object sender, EventArgs e) => ChangeColor(button8, 7);
        private void button9_Click(object sender, EventArgs e) => ChangeColor(button9, 8);
        private void button10_Click(object sender, EventArgs e) => ChangeColor(button10, 9);
        private void button11_Click(object sender, EventArgs e) => ChangeColor(button11, 10);
        private void button12_Click(object sender, EventArgs e) => ChangeColor(button12, 11);
        private void button13_Click(object sender, EventArgs e) => ChangeColor(button13, 12);
        private void button14_Click(object sender, EventArgs e) => ChangeColor(button14, 13);
        private void button15_Click(object sender, EventArgs e) => ChangeColor(button15, 14);

        private void saveTrainDataBtn_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "trainData.txt";
            string data = null;
            foreach (int i in InputData)
            {
                data += " " + i.ToString();
            }
            data += "\n";
            File.AppendAllText(path, data);
            //logTextBox.Text = "Данные сохранены в " + path;
        }

        private void saveTestDataBtn_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "testData.txt";
            string data = null;
            foreach (int i in InputData)
            {
                data += " " + i.ToString();
            }
            data += "\n";
            File.AppendAllText(path, data);
        }
    }
}
