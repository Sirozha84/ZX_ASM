namespace ZXASM
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menunew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuopen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menusave = new System.Windows.Forms.ToolStripMenuItem();
            this.menusaveas = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuexit = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuundo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuredo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menucut = new System.Windows.Forms.ToolStripMenuItem();
            this.menucopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menupaste = new System.Windows.Forms.ToolStripMenuItem();
            this.сборкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemBin = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSNA = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSite = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuabout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolnew = new System.Windows.Forms.ToolStripButton();
            this.toolopen = new System.Windows.Forms.ToolStripButton();
            this.toolsave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolcut = new System.Windows.Forms.ToolStripButton();
            this.toolcopy = new System.Windows.Forms.ToolStripButton();
            this.toolpaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolundo = new System.Windows.Forms.ToolStripButton();
            this.toolredo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRun = new System.Windows.Forms.ToolStripButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.сборкаToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(570, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menunew,
            this.menuopen,
            this.toolStripSeparator2,
            this.menusave,
            this.menusaveas,
            this.toolStripSeparator1,
            this.menuexit});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // menunew
            // 
            this.menunew.Image = global::ZXASM.Properties.Resources.Новый;
            this.menunew.Name = "menunew";
            this.menunew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menunew.Size = new System.Drawing.Size(173, 22);
            this.menunew.Text = "Новый";
            this.menunew.Click += new System.EventHandler(this.menunew_Click);
            // 
            // menuopen
            // 
            this.menuopen.Image = global::ZXASM.Properties.Resources.Открыть;
            this.menuopen.Name = "menuopen";
            this.menuopen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuopen.Size = new System.Drawing.Size(173, 22);
            this.menuopen.Text = "Открыть...";
            this.menuopen.Click += new System.EventHandler(this.menuopen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // menusave
            // 
            this.menusave.Image = global::ZXASM.Properties.Resources.Сохранить;
            this.menusave.Name = "menusave";
            this.menusave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menusave.Size = new System.Drawing.Size(173, 22);
            this.menusave.Text = "Сохранить";
            this.menusave.Click += new System.EventHandler(this.menusave_Click);
            // 
            // menusaveas
            // 
            this.menusaveas.Name = "menusaveas";
            this.menusaveas.Size = new System.Drawing.Size(173, 22);
            this.menusaveas.Text = "Сохранить как...";
            this.menusaveas.Click += new System.EventHandler(this.menusaveas_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // menuexit
            // 
            this.menuexit.Name = "menuexit";
            this.menuexit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuexit.Size = new System.Drawing.Size(173, 22);
            this.menuexit.Text = "Выход";
            this.menuexit.Click += new System.EventHandler(this.menuexit_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuundo,
            this.menuredo,
            this.toolStripMenuItem1,
            this.menucut,
            this.menucopy,
            this.menupaste});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // menuundo
            // 
            this.menuundo.Image = global::ZXASM.Properties.Resources.Отменить;
            this.menuundo.Name = "menuundo";
            this.menuundo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.menuundo.Size = new System.Drawing.Size(181, 22);
            this.menuundo.Text = "Отменить";
            this.menuundo.Click += new System.EventHandler(this.menuundo_Click);
            // 
            // menuredo
            // 
            this.menuredo.Image = global::ZXASM.Properties.Resources.Повторить;
            this.menuredo.Name = "menuredo";
            this.menuredo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.menuredo.Size = new System.Drawing.Size(181, 22);
            this.menuredo.Text = "Повторить";
            this.menuredo.Click += new System.EventHandler(this.menuredo_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
            // 
            // menucut
            // 
            this.menucut.Image = global::ZXASM.Properties.Resources.Вырезать;
            this.menucut.Name = "menucut";
            this.menucut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menucut.Size = new System.Drawing.Size(181, 22);
            this.menucut.Text = "Вырезать";
            this.menucut.Click += new System.EventHandler(this.menucut_Click);
            // 
            // menucopy
            // 
            this.menucopy.Image = global::ZXASM.Properties.Resources.Копировать;
            this.menucopy.Name = "menucopy";
            this.menucopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menucopy.Size = new System.Drawing.Size(181, 22);
            this.menucopy.Text = "Копировать";
            this.menucopy.Click += new System.EventHandler(this.menucopy_Click);
            // 
            // menupaste
            // 
            this.menupaste.Image = global::ZXASM.Properties.Resources.Вставить;
            this.menupaste.Name = "menupaste";
            this.menupaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menupaste.Size = new System.Drawing.Size(181, 22);
            this.menupaste.Text = "Вставить";
            this.menupaste.Click += new System.EventHandler(this.menupaste_Click);
            // 
            // сборкаToolStripMenuItem
            // 
            this.сборкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemRun,
            this.ToolStripMenuItemBin,
            this.ToolStripMenuItemSNA});
            this.сборкаToolStripMenuItem.Name = "сборкаToolStripMenuItem";
            this.сборкаToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.сборкаToolStripMenuItem.Text = "Компиляция";
            // 
            // ToolStripMenuItemRun
            // 
            this.ToolStripMenuItemRun.Image = global::ZXASM.Properties.Resources.Run;
            this.ToolStripMenuItemRun.Name = "ToolStripMenuItemRun";
            this.ToolStripMenuItemRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.ToolStripMenuItemRun.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItemRun.Text = "Пуск";
            this.ToolStripMenuItemRun.Click += new System.EventHandler(this.ToolStripMenuItemRun_Click);
            // 
            // ToolStripMenuItemBin
            // 
            this.ToolStripMenuItemBin.Name = "ToolStripMenuItemBin";
            this.ToolStripMenuItemBin.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItemBin.Text = "Собрать бинарник";
            this.ToolStripMenuItemBin.Click += new System.EventHandler(this.ToolStripMenuItemBin_Click);
            // 
            // ToolStripMenuItemSNA
            // 
            this.ToolStripMenuItemSNA.Name = "ToolStripMenuItemSNA";
            this.ToolStripMenuItemSNA.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItemSNA.Text = "Собрать снимок";
            this.ToolStripMenuItemSNA.Click += new System.EventHandler(this.ToolStripMenuItemSNA_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemSite,
            this.ToolStripMenuItemPage,
            this.toolStripSeparator4,
            this.menuabout});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // ToolStripMenuItemSite
            // 
            this.ToolStripMenuItemSite.Name = "ToolStripMenuItemSite";
            this.ToolStripMenuItemSite.Size = new System.Drawing.Size(196, 22);
            this.ToolStripMenuItemSite.Text = "Сайт разработчика";
            this.ToolStripMenuItemSite.Click += new System.EventHandler(this.ToolStripMenuItemSite_Click);
            // 
            // ToolStripMenuItemPage
            // 
            this.ToolStripMenuItemPage.Name = "ToolStripMenuItemPage";
            this.ToolStripMenuItemPage.Size = new System.Drawing.Size(196, 22);
            this.ToolStripMenuItemPage.Text = "Страница программы";
            this.ToolStripMenuItemPage.Click += new System.EventHandler(this.ToolStripMenuItemPage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(193, 6);
            // 
            // menuabout
            // 
            this.menuabout.Name = "menuabout";
            this.menuabout.Size = new System.Drawing.Size(196, 22);
            this.menuabout.Text = "О программе";
            this.menuabout.Click += new System.EventHandler(this.menuabout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolnew,
            this.toolopen,
            this.toolsave,
            this.toolStripSeparator3,
            this.toolcut,
            this.toolcopy,
            this.toolpaste,
            this.toolStripSeparator5,
            this.toolundo,
            this.toolredo,
            this.toolStripSeparator6,
            this.toolStripButtonRun});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(570, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolnew
            // 
            this.toolnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolnew.Image = global::ZXASM.Properties.Resources.Новый;
            this.toolnew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolnew.Name = "toolnew";
            this.toolnew.Size = new System.Drawing.Size(23, 22);
            this.toolnew.Text = "Новый (Ctrl + N)";
            this.toolnew.Click += new System.EventHandler(this.toolnew_Click);
            // 
            // toolopen
            // 
            this.toolopen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolopen.Image = global::ZXASM.Properties.Resources.Открыть;
            this.toolopen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolopen.Name = "toolopen";
            this.toolopen.Size = new System.Drawing.Size(23, 22);
            this.toolopen.Text = "Открыть (Ctrl + O)";
            this.toolopen.Click += new System.EventHandler(this.toolopen_Click);
            // 
            // toolsave
            // 
            this.toolsave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolsave.Image = global::ZXASM.Properties.Resources.Сохранить;
            this.toolsave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsave.Name = "toolsave";
            this.toolsave.Size = new System.Drawing.Size(23, 22);
            this.toolsave.Text = "Сохранить (Ctrl + S)";
            this.toolsave.Click += new System.EventHandler(this.toolsave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolcut
            // 
            this.toolcut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolcut.Image = global::ZXASM.Properties.Resources.Вырезать;
            this.toolcut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolcut.Name = "toolcut";
            this.toolcut.Size = new System.Drawing.Size(23, 22);
            this.toolcut.Text = "Вырезать (Ctrl + X)";
            this.toolcut.Click += new System.EventHandler(this.toolcut_Click);
            // 
            // toolcopy
            // 
            this.toolcopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolcopy.Image = global::ZXASM.Properties.Resources.Копировать;
            this.toolcopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolcopy.Name = "toolcopy";
            this.toolcopy.Size = new System.Drawing.Size(23, 22);
            this.toolcopy.Text = "Копировать (Ctrl + C)";
            this.toolcopy.Click += new System.EventHandler(this.toolcopy_Click);
            // 
            // toolpaste
            // 
            this.toolpaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolpaste.Image = global::ZXASM.Properties.Resources.Вставить;
            this.toolpaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolpaste.Name = "toolpaste";
            this.toolpaste.Size = new System.Drawing.Size(23, 22);
            this.toolpaste.Text = "Вставить (Ctrl + V)";
            this.toolpaste.Click += new System.EventHandler(this.toolpaste_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolundo
            // 
            this.toolundo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolundo.Image = global::ZXASM.Properties.Resources.Отменить;
            this.toolundo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolundo.Name = "toolundo";
            this.toolundo.Size = new System.Drawing.Size(23, 22);
            this.toolundo.Text = "Отменить (Ctrl+Z)";
            this.toolundo.Click += new System.EventHandler(this.toolundo_Click);
            // 
            // toolredo
            // 
            this.toolredo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolredo.Image = global::ZXASM.Properties.Resources.Повторить;
            this.toolredo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolredo.Name = "toolredo";
            this.toolredo.Size = new System.Drawing.Size(23, 22);
            this.toolredo.Text = "Повторить (Ctrl+Y)";
            this.toolredo.Click += new System.EventHandler(this.toolredo_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRun
            // 
            this.toolStripButtonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRun.Image = global::ZXASM.Properties.Resources.Run;
            this.toolStripButtonRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRun.Name = "toolStripButtonRun";
            this.toolStripButtonRun.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRun.Text = "Запуск";
            this.toolStripButtonRun.Click += new System.EventHandler(this.toolStripButtonRun_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(0, 49);
            this.textBox1.MaxLength = 65535;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(570, 353);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 402);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menunew;
        private System.Windows.Forms.ToolStripMenuItem menuopen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menusave;
        private System.Windows.Forms.ToolStripMenuItem menusaveas;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuexit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolnew;
        private System.Windows.Forms.ToolStripButton toolopen;
        private System.Windows.Forms.ToolStripButton toolsave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuabout;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menucut;
        private System.Windows.Forms.ToolStripMenuItem menucopy;
        private System.Windows.Forms.ToolStripMenuItem menupaste;
        private System.Windows.Forms.ToolStripButton toolcut;
        private System.Windows.Forms.ToolStripButton toolcopy;
        private System.Windows.Forms.ToolStripButton toolpaste;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem menuundo;
        private System.Windows.Forms.ToolStripMenuItem menuredo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolundo;
        private System.Windows.Forms.ToolStripButton toolredo;
        private System.Windows.Forms.ToolStripMenuItem сборкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBin;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSNA;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSite;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonRun;
    }
}

