using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    class Modules
    {
        public string FileName;
        bool included;

        public static List<Modules> List = new List<Modules>();

        public Modules(string filename)
        {
            FileName = filename;
            included = false;
        }

        public static void Add(string filename)
        {
            if (List.Find(o => o.FileName == filename) == null)
                List.Add(new Modules(filename));
        }

        public void Include()
        {
            if (included) return;
            included = true;
            try
            {
                string file = System.IO.Path.GetDirectoryName(Project.FileName) + "\\" + FileName + ".asm";
                string Module = System.IO.File.ReadAllText(file);
                Compiler.Parsing(Module);
            }
            catch { throw new ArgumentException("Файл \"" + FileName + "\" не найден"); }
        }
    }
}
