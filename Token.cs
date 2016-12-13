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

        public static int CurAdress;

        public int Adress;
        public byte[] Code;
        public bool IsComand = false;

        public string Label;    //Метка, указанная в команде
        int To;                 //Адрес, куда запишется адрес метки (+ к первому адресу команды)
        bool Rel = false;       //Относительный переход?

        public Token(string Str)
        {
            //Вычленяем строку с мнемоникой
            if (Str.Contains(";")) Str = Str.Split(';')[0];
            if (Str.Contains(":")) Str = Str.Split(':')[1];
            Str = Str.Trim(' ').ToLower();
            if (Str != "")
            {
                //try
                //{
                    ToCode(Str);
                    if (Code != null)
                    {
                        Adress = CurAdress;
                        List.Add(this);
                        if (Code != null)
                            CurAdress += Code.Count();
                    }
                //}
                //catch (Exception e) { System.Windows.Forms.MessageBox.Show("Ошибка в строке \n" + e.Message, "Ошибка компиляции"); }
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
                        switch (Str[1])
                        {
                            case "a":
                                if (ReadNum(Str[2], out NN)) { Code = new byte[] { 62, B1(NN) }; };                 //LD A,N
                                if (Str[2] == "b") Code = new byte[] { 120 };                                       //LD A,B
                                if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 58, B1(NN), B2(NN) }; };         //LD A,(NN)
                                break;
                            case "b":
                                if (ReadNum(Str[2], out NN)) { Code = new byte[] { 6, B1(NN) }; };                  //LD B,NN
                                if (Str[2] == "b") Code = new byte[] { 64 };                                        //LD B,B
                                if (Str[2] == "c") Code = new byte[] { 65 };                                        //LD B,C
                                if (Str[2] == "d") Code = new byte[] { 66 };                                        //LD B,D
                                if (Str[2] == "e") Code = new byte[] { 67 };                                        //LD B,E
                                if (Str[2] == "h") Code = new byte[] { 68 };                                        //LD B,H
                                if (Str[2] == "l") Code = new byte[] { 69 };                                        //LD B,L
                                if (Str[2] == "(hl)") Code = new byte[] { 70 };                                     //LD B,(HL)
                                if (Str[2] == "a") Code = new byte[] { 71 };                                        //LD B,A
                                break;
                            case "hl":
                                if (ReadNum(Str[2], out NN)) { Code = new byte[] { 33, B1(NN), B2(NN) }; };         //...........
                                break;
                            default:
                                throw new ArgumentException("\"" + Str[1] + "\" не является допустимым регистром");
                        }
                        if (Code == null) throw new ArgumentException("\"" + Str[2] + "\" не является допустимым числом или регистром");
                    }
                    if (Str.Count() < 3) throw new ArgumentException("Не достаточно параметров в команде LD");
                    if (Str.Count() > 3) throw new ArgumentException("Слишком много параметров в команде LD");
                    break;
                case "exx": Code = new byte[] { 217 }; break;
                case "inc":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "b") Code = new byte[] { 4 };                                             //INC B
                        if (Str[1] == "hl") Code = new byte[] { 35 };                                           //INC HL
                        if (Str[1] == "a") Code = new byte[] { 60 };                                            //INC A
                    }
                    break;
                case "dec":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "b") Code = new byte[] { 5 };                                             //DEC B
                        if (Str[1] == "a") Code = new byte[] { 61 };                                            //DEC A
                    }
                    break;
                case "add":
                    if (Str.Count() == 3)
                    {
                        if (Str[1] == "a")
                        {
                            if (Str[2] == "b") Code = new byte[] { 128 };                                       //ADD A,B
                            if (Str[2] == "c") Code = new byte[] { 129 };                                       //ADD A,C
                            if (Str[2] == "a") Code = new byte[] { 135 };                                       //ADD A,A
                            if (ReadNum(Str[2], out NN)) { Code = new byte[] { 198, B1(NN) }; };                //ADD A,N
                        }
                    }
                    break;
                case "adc":
                    if (Str.Count() == 3)
                    {
                        if (Str[1] == "a")
                        {
                            if (Str[2] == "b") Code = new byte[] { 136 };                                       //ADC A,B
                            if (Str[2] == "c") Code = new byte[] { 137 };                                       //ADC A,C
                            if (Str[2] == "a") Code = new byte[] { 143 };                                       //ADC A,A
                            if (ReadNum(Str[2], out NN)) { Code = new byte[] { 198, B1(NN) }; };                //ADC A,N
                        }
                    }
                    break;
                case "cp":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "c") Code = new byte[] { 185 };                                           //CP C
                        if (ReadNum(Str[1], out NN)) { Code = new byte[] { 254, B1(NN) }; };                    //CP N
                    }
                    break;
                case "and":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "c") Code = new byte[] { 161 };                                           //AND C
                        if (ReadNum(Str[1], out NN)) { Code = new byte[] { 230, B1(NN) }; };                    //AND N
                    }
                    break;
                case "or":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "c") Code = new byte[] { 177 };                                           //OR C
                        if (ReadNum(Str[1], out NN)) { Code = new byte[] { 246, B1(NN) }; };                    //OR N
                    }
                    break;
                case "xor":
                    if (Str.Count() == 2)
                    {
                        if (Str[1] == "c") Code = new byte[] { 169 };                                           //XOR C
                        if (Str[1] == "a") Code = new byte[] { 175 };                                           //XOR C
                        if (ReadNum(Str[1], out NN)) { Code = new byte[] { 238, B1(NN) }; };                    //XOR N
                    }
                    break;
                case "jp":
                    if (Str.Count() == 2)
                    {
                        switch (Str[1])
                        {
                            case "(hl)": Code = new byte[] { 233 }; break;                                      //JP (HL)
                            case "(ix)": Code = new byte[] { 221, 233 }; break;                                 //JP (IX)
                            case "(iy)": Code = new byte[] { 253, 233 }; break;                                 //JP (IY)
                            default: Code = new byte[] { 195, 0, 0 }; To = 1; Label = Str[1]; break;            //JP NN
                        }
                    }
                    if (Str.Count() == 3)
                    {

                    }
                    break;
                case "jr":
                    if (Str.Count() == 2)
                    {
                        Code = new byte[] { 24, 0 }; To = 1; Label = Str[1]; Rel = true; break;                 //JR S
                    }
                    break;
                case "djnz":
                    if (Str.Count() == 2)
                    {
                        Code = new byte[] { 16, 0 }; To = 1; Label = Str[1]; Rel = true; break;                 //DJNZ S
                    }
                    break;
                case "call":
                    if (Str.Count() == 2)
                    {
                        switch (Str[1])
                        {
                            //case "(hl)": Code = new byte[] { 233 }; break;
                            //case "(ix)": Code = new byte[] { 221, 233 }; break;
                            //case "(iy)": Code = new byte[] { 253, 233 }; break;
                            default: Code = new byte[] { 205, 0, 0 }; To = 1; Label = Str[1]; break;            //CALL NN
                        }
                    }
                    if (Str.Count() == 3)
                    {
                        //по условиям
                    }
                    break;

                case "nop": Code = new byte[] { 0 }; break;
                case "ret": Code = new byte[] { 201 }; break;
                case "reti": Code = new byte[] { 239, 77 }; break;
                case "di": Code = new byte[] { 243 }; break;
                case "ei": Code = new byte[] { 251 }; break;
                case "defb":
                    Code = new byte[Str.Length - 1];
                    for (int i = 0; i < Str.Length - 1; i++)
                    {
                        ReadNum(Str[i + 1], out NN);
                        Code[i] = B1(NN);
                    }
                    break;
                default:
                    throw new ArgumentException("Не известная команда \"" + Str[0] + "\"");
            }
        }

        public void SetAdress(int adr)
        {
            if (Rel)
            {
                int jr = adr - Adress - 2;
                if (jr >= 0)
                    Code[To] = (byte)jr;
                else
                    Code[To] = (byte)(256 + jr);
            }
            else
            {
                Code[To] = (byte)(adr % 256);
                Code[To + 1] = (byte)(adr / 256);
            }
        }

        bool ReadNum(string str, out ushort res)
        {
            bool isNum = (ushort.TryParse(str, out res));
            if (str[0] == '#') { isNum = true; } //Тут переведём из 16-и в 10
            return isNum;
        }

        bool ReadAdr(string str, out ushort res)
        {
            res = 0;
            //if (str[0] == '#') //
            if (str[0] == '(' & str[str.Length - 1] == ')')
                return (ushort.TryParse(str.Trim('(', ')'), out res));
            return false;
            
            //else throw Exception.

            //Ещё надо проверить не метка ли указана в адресе
        }

        byte B1(ushort Num) { return (byte)(Num % 256); }
        byte B2(ushort Num) { return (byte)(Num / 256); }

    }
}
