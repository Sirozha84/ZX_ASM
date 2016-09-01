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
            ushort NN;
            switch (Str[0])
            {
                case "ld":
                    if (Str.Count() == 3)
                    {
                        if (Str[1] == "a")
                        {
                            if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 58, B1(NN), B2(NN) }; };  //LD A,(NN)
                        }
                        if (Str[1] == "b")
                        {
                            if (Str[2] == "b") Code = new byte[] { 64 };
                            if (Str[2] == "c") Code = new byte[] { 65 };
                            if (Str[2] == "d") Code = new byte[] { 66 };
                            if (Str[2] == "e") Code = new byte[] { 67 };
                            if (Str[2] == "h") Code = new byte[] { 68 };
                            if (Str[2] == "l") Code = new byte[] { 69 };
                            if (Str[2] == "(hl)") Code = new byte[] { 70 };
                            if (Str[2] == "a") Code = new byte[] { 71 };
                            if (ReadNum(Str[2], out NN)) { Code = new byte[] { 6, B1(NN) }; };  //LD B,NN
                        }
                        if (Str[1] == "hl")
                        {
                            if (ReadNum(Str[2], out NN)) { Code = new byte[] { 33, B1(NN), B2(NN) }; };
                        }
                    }
                    break;
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

        public void SetAdress(int adr)
        {
            Code[To] = (byte)(adr % 256);
            Code[To + 1] = (byte)(adr / 256);
        }

        bool ReadNum(string str, out ushort res)
        {
            //if (str[0] == '#') //
            return (ushort.TryParse(str, out res));
            //else throw Exception.
        }

        bool ReadAdr(string str, out ushort res)
        {
            res = 0;
            //if (str[0] == '#') //
            if (str[0] == '(' & str[str.Length - 1] == ')')
                return (ushort.TryParse(str.Trim('(', ')'), out res));
            return false;
            
            //else throw Exception.
        }

        byte B1(ushort Num) { return (byte)(Num % 256); }
        byte B2(ushort Num) { return (byte)(Num / 256); }

    }
}
