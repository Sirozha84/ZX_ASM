using System;
using System.Windows.Forms;

namespace ZXASM
{
    public partial class FormMain : Form
    {
        System.Diagnostics.Process Help = new System.Diagnostics.Process();
        bool User = false;

        private void FormMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Program.Site;
            Project.New();
            DrawDocument();
        }

        public FormMain()
        {
            InitializeComponent();
            Left = Properties.Settings.Default.WindowLeft;
            Top = Properties.Settings.Default.WindowTop;
            Width = Properties.Settings.Default.WindowWidth;
            Height = Properties.Settings.Default.WindowHeight;
        }



        private void menuhelp_Click(object sender, EventArgs e)
        {
            try { HelpClose(); Help.StartInfo.FileName = "help.chm"; Help.Start(); }
            catch
            {
                MessageBox.Show("Файл справки не найден.",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Project.Close()) { e.Cancel = true; return; }
            Properties.Settings.Default.WindowLeft = Left;
            Properties.Settings.Default.WindowTop = Top;
            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeight = Height;
            Properties.Settings.Default.Save();
        }

        void DrawDocument()
        {
            Text = Project.FormText;
            User = false;
            textBox1.Text = Project.Text;
            User = true;
            UndoButtons();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Project.Text = textBox1.Text;
            if (User) Project.Change();
            Text = Project.FormText;
            UndoButtons();
        }
        void UndoButtons()
        {
            menuundo.Enabled = Project.UndoEnable;
            menuredo.Enabled = Project.RedoEnable;
            toolundo.Enabled = Project.UndoEnable;
            toolredo.Enabled = Project.RedoEnable;
        }
        //Прочие процедурки
        void HelpClose() { try { Help.Kill(); } catch { } }
        //Меню и панель инструментов
        private void menunew_Click(object sender, EventArgs e) { if(Project.New()) DrawDocument(); }
        private void menuopen_Click(object sender, EventArgs e) { if(Project.Open()) DrawDocument(); }
        private void menusave_Click(object sender, EventArgs e) { if(Project.Save(false)) DrawDocument(); }
        private void menusaveas_Click(object sender, EventArgs e) { if(Project.Save(true)) DrawDocument(); }
        private void menuexit_Click(object sender, EventArgs e) { Close(); }
        private void menucut_Click(object sender, EventArgs e) { textBox1.Cut(); }
        private void menucopy_Click(object sender, EventArgs e) { textBox1.Copy(); }
        private void menupaste_Click(object sender, EventArgs e) { textBox1.Paste(); }
        private void menuundo_Click(object sender, EventArgs e) { Project.Undo(); DrawDocument(); }
        private void menuredo_Click(object sender, EventArgs e) { Project.Redo(); DrawDocument(); }
        private void menuabout_Click(object sender, EventArgs e) { FormAbout form = new FormAbout(); form.ShowDialog(); }
        private void toolnew_Click(object sender, EventArgs e) { if (Project.New()) DrawDocument(); }
        private void toolopen_Click(object sender, EventArgs e) { if(Project.Open()) DrawDocument(); }
        private void toolsave_Click(object sender, EventArgs e) { if(Project.Save(false)) DrawDocument(); }
        private void toolcut_Click(object sender, EventArgs e) { textBox1.Cut(); }
        private void toolcopy_Click(object sender, EventArgs e) { textBox1.Copy(); }
        private void toolpaste_Click(object sender, EventArgs e) { textBox1.Paste(); }
        private void toolundo_Click(object sender, EventArgs e) { Project.Undo(); DrawDocument(); }
        private void toolredo_Click(object sender, EventArgs e) { Project.Redo(); DrawDocument(); }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e) { System.Diagnostics.Process.Start(Program.Url); }
    }
}