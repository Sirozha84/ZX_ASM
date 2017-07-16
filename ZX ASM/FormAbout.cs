using System.Windows.Forms;

namespace ZXASM
{
    partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            Text = "О " + Application.ProductName;
            label1.Text = Application.ProductName;
            label2.Text = "Версия: " + Program.Version;
            label3.Text = "Автор программы: Сергей Гордеев";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }
    }
}
