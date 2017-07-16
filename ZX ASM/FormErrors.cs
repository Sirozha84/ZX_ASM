using System;
using System.Windows.Forms;

namespace ZXASM
{
    public partial class FormErrors : Form
    {
        public FormErrors()
        {
            InitializeComponent();
        }

        private void FormErrors_Load(object sender, EventArgs e)
        {
            foreach (Error er in Error.List)
            {
                ListViewItem item = new ListViewItem(new string[] { er.File, er.StringNum.ToString(), er.String, er.Message });
                listView1.Items.Add(item);
            }
        }
    }
}
