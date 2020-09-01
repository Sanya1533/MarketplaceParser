using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WildberriesParser
{
    public partial class CaptchaForm : Form
    {
        private bool entered = false;

        public string CaptchaText { get; private set; } = "bad";

        public CaptchaForm(string uri)
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(new WebClient().OpenRead(uri));
            this.DialogResult = DialogResult.None;
            button1.Left = splitContainer1.Panel2.Width - button1.Width - textBox1.Left;
            textBox1.Width = button1.Left - 5 - textBox1.Left;
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;
            textBox1.Focus();
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            entered = !string.IsNullOrEmpty(textBox1.Text);
            if (!entered)
            {
                textBox1.Text = "Введите ответ";
            }
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            if (!entered)
            {
                entered = true;
                textBox1.Text = "";
            }
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            textBox1.Top = splitContainer1.Panel2.Height / 2 - textBox1.Height / 2;
            button1.Top = textBox1.Top-1;
            button1.Height = textBox1.Height+2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!entered||string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Вы ничего не ввели.\nВведите ответ","Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CaptchaText = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
