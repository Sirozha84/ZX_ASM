using System;
using System.Collections.Generic;

namespace ZXASM
{
    class Module
    {
        public string FileName;
        bool included;
        string Text;

        public static List<Module> List = new List<Module>();

        public Module(string filename)
        {
            FileName = filename;
            included = false;
        }

        public static void Add(string filename)
        {
            if (List.Find(o => o.FileName == filename) == null)
            {
                try
                {
                    Module modul = new Module(filename);
                    //Загрузка модуля
                    string file = System.IO.Path.GetDirectoryName(Project.FileName) + "\\" + filename + ".asm";
                    modul.Text = System.IO.File.ReadAllText(file);
                    List.Add(modul);
                }
                catch { throw new ArgumentException("Файл \"" + filename + "\" не найден"); }
            }
        }

        public void Include()
        {
            if (included) return;
            included = true;
            Compiler.Parsing(FileName, Text);
        }
    }
}
