using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace ZXASM
{
    class Project
    {
        const string NewFile = "Безымянный";
        const string Filter = "Код Assembler (*.asm)|*.asm|Все файлы|*.*";
        static bool Changed;

        //Данные проекта
        static string FileName;
        public static string Text;

        //История
        static List<string> History = new List<string>();
        static int Iteration = 0;

        public static string FormText
        {
            get
            {
                string star = Changed ? "*" : "";
                return Path.GetFileNameWithoutExtension(FileName) + star + " - " + Application.ProductName;
            }
        }
        public static bool UndoEnable { get { return Iteration > 0; } }
        public static bool RedoEnable { get { return Iteration < History.Count - 1; } }

        public static bool New()
        {
            if (!Close()) return false;
            FileName = NewFile;
            Text = "";
            ResetHistory();
            return true;
        }

        public static bool Save(bool As)
        {
            string fileName = FileName;
            if (As | FileName == NewFile)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = Filter;
                if (save.ShowDialog() == DialogResult.OK) fileName = save.FileName;
                else return false;
            }
            try
            {
                File.WriteAllText(fileName, Text);
                FileName = fileName; //Если сохранение прошло удачно, применяем новое имя
                Changed = false;
                return true;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка во время сохранения файла. Файл не сохранён.",
                  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public static bool Open()
        {
            if (!Close()) return false;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = Filter;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Text = File.ReadAllText(open.FileName);
                    FileName = open.FileName;
                    ResetHistory();
                    return true;
                }
                catch {
                    MessageBox.Show("Произошла ошибка при открытии файла.",
                      Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;
        }

        public static bool Close()
        {
            if (!Changed) return true;
            DialogResult ans = MessageBox.Show("Вы хотите сохранить изменения в файле " +
                Path.GetFileNameWithoutExtension(FileName) + "?",
                Application.ProductName, MessageBoxButtons.YesNoCancel);
            if (ans == DialogResult.No) return true;
            if (ans == DialogResult.Cancel) return false;
            return Save(false);
        }

        static void ResetHistory()
        {
            Changed = false;
            History.Clear();
            History.Add(Text);
            Iteration = 0;
        }

        public static void Change()
        {
            History.RemoveRange(Iteration + 1, History.Count() - Iteration - 1);
            History.Add(Text);
            Iteration++;
            Changed = true;
        }

        public static void Undo()
        {
            Text = History[--Iteration];
            Changed = true;
        }

        public static void Redo()
        {
            Text = History[++Iteration];
            Changed = true;
        }
    }
}
