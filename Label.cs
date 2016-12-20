using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    class Label
    {
        public static List<Label> List = new List<Label>();
        public string Module;
        public string Name;
        public int Adress;

        /// <summary>
        /// Парсинг метки
        /// </summary>
        /// <param name="module">Имя модуля</param>
        /// <param name="Str">Метка</param>
        public Label(string module, string Str)
        {
            //Вычленяем строку с меткой
            if (Str.Contains(":"))
            {
                Str = Str.Split(':')[0];
                Str = Str.Trim(' ', ':');
                if (Str != "" & List.Find(o => o.Name == Str) == null)
                {
                    //Module = module;
                    Name = module + "." + Str;
                    Adress = Token.CurAdress;
                    List.Add(this);
                }
                else
                    throw new ArgumentException("Метка \"" + Str + "\" уже существует");
            }
        }
    }
}
