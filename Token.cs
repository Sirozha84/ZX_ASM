using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    class Token
    {
        public static List<Token> List = new List<Token>();

        public static int Adress;
        public byte[] Code;
        public bool IsComand = false;

        public string Label;   //Метка, указанная в команде
        int To;         //Адрес, куда запишется адрес метки (+ к первому адресу команды)

        public Token(string Str)
        {
            //Вычленяем строку с мнемоникой
            if (Str.Contains(";")) Str = Str.Split(';')[0];
            if (Str.Contains(":")) Str = Str.Split(':')[1];
            Str = Str.Trim(' ').ToLower();
            if (Str != "")
            {
                ToCode(Str);
                if (Code != null)
                {
                    List.Add(this);
                    if (Code != null)
                        Adress += Code.Count();
                }
            }
        }

        void ToCode(string str)
        {
            string[] Str = str.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            IsComand = true;
            switch (Str[0])
            {
                case "nop": Code = new byte[] { 0 }; break;
                case "ret": Code = new byte[] { 201 }; break;
                case "exx": Code = new byte[] { 217 }; break;
                case "reti": Code = new byte[] { 239, 77 }; break;
                case "di": Code = new byte[] { 243 }; break;
                case "ei": Code = new byte[] { 251 }; break;
                case "jp":
                    if (Str.Count() == 2) { Code = new byte[] { 195, 0, 0 }; To = 1; Label = Str[1]; }
                    break;
            }
        }
    }
}
