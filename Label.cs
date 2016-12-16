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
        public string Name;
        public int Adress;

        public Label(string Str)
        {
            //Вычленяем строку с меткой
            if (Str.Contains(":"))
            {
                Str = Str.Split(':')[0];
                Str = Str.Trim(' ', ':');
                if (Str != "" & List.Find(o => o.Name == Str) == null)
                {
                    Name = Str;
                    Adress = Token.CurAdress;
                    List.Add(this);
                }
                else
                    throw new ArgumentException("Метка \"" + Str + "\" уже существует");
            }
        }
    }
}
