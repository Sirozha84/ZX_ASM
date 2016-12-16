﻿using System;
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

        public int NumString;          //Метаданные строки (номер, строка)
        public string String;

        public Token(int num, string Str)
        {
            NumString = num;
            String = Str;
            //Вычленяем строку с мнемоникой
            if (Str.Contains(";")) Str = Str.Split(';')[0];
            if (Str.Contains(":")) Str = Str.Split(':')[1];
            Str = Str.Trim(' ').ToLower();
            if (Str != "")
            {
                ToCode(Str);
                if (Code != null)
                {
                    Adress = CurAdress;
                    List.Add(this);
                    if (Code != null)
                        CurAdress += Code.Count();
                }
            }
        }

        void ToCode(string str)
        {
            string[] Str = str.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            IsComand = true;
            ushort NN;
            if (Str[0] == "org")                                                                            //ORG
            {
                if (List.Count > 0) throw new ArgumentException("Директива ORG может использоваться только в начале программы");
                ParamTest(Str, "ORG", 2);
                CurAdress = ReadNum(Str[1]);
                return;
            }
            if (Str[0] == "ld")
            {
                ParamTest(Str, "LD", 3);
                if (Str[1] == "a")
                {
                    if (Str[2] == "b") { Code = new byte[] { 120 }; return; }                               //LD A,B
                    if (Str[2] == "c") { Code = new byte[] { 121 }; return; }                               //LD A,C
                    if (Str[2] == "d") { Code = new byte[] { 122 }; return; }                               //LD A,D
                    if (Str[2] == "e") { Code = new byte[] { 123 }; return; }                               //LD A,E
                    if (Str[2] == "h") { Code = new byte[] { 124 }; return; }                               //LD A,H
                    if (Str[2] == "l") { Code = new byte[] { 125 }; return; }                               //LD A,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 126 }; return; }                            //LD A,(HL)
                    if (Str[2] == "a") { Code = new byte[] { 127 }; return; }                               //LD A,A
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 62, B1(NN) }; return; };             //LD A,N
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 58, B1(NN), B2(NN) }; return; };     //LD A,(NN) (только в с регистром A)
                }
                if (Str[1] == "b")
                {
                    if (Str[2] == "b") { Code = new byte[] { 64 }; return; }                                //LD B,B
                    if (Str[2] == "c") { Code = new byte[] { 65 }; return; }                                //LD B,C
                    if (Str[2] == "d") { Code = new byte[] { 66 }; return; }                                //LD B,D
                    if (Str[2] == "e") { Code = new byte[] { 67 }; return; }                                //LD B,E
                    if (Str[2] == "h") { Code = new byte[] { 68 }; return; }                                //LD B,H
                    if (Str[2] == "l") { Code = new byte[] { 69 }; return; }                                //LD B,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 70 }; return; }                             //LD B,(HL)
                    if (Str[2] == "a") { Code = new byte[] { 71 }; return; }                                //LD B,A
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 6, B1(NN) }; };                      //LD B,N
                }
                if (Str[1] == "hl")
                {
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 33, B1(NN), B2(NN) }; };             //LD HL, NN
                }
                //if (Code == null) throw new ArgumentException("\"" + Str[2] + "\" не является допустимым числом или регистром");
            }
            if (Str[0] == "exx")
            {
                Code = new byte[] { 217 };
            }
            if (Str[0] == "inc")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "b") Code = new byte[] { 4 };                                                 //INC B
                    if (Str[1] == "hl") Code = new byte[] { 35 };                                               //INC HL
                    if (Str[1] == "a") Code = new byte[] { 60 };                                                //INC A
                }
            }
            if (Str[0] == "dec")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "b") Code = new byte[] { 5 };                                                 //DEC B
                    if (Str[1] == "a") Code = new byte[] { 61 };                                                //DEC A
                }
            }
            if (Str[0] == "add")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "a")
                    {
                        if (Str[2] == "b") Code = new byte[] { 128 };                                           //ADD A,B
                        if (Str[2] == "c") Code = new byte[] { 129 };                                           //ADD A,C
                        if (Str[2] == "a") Code = new byte[] { 135 };                                           //ADD A,A
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 198, B1(NN) }; };                    //ADD A,N
                    }
                }
            }
            if (Str[0] == "adc")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "a")
                    {
                        if (Str[2] == "b") Code = new byte[] { 136 };                                           //ADC A,B
                        if (Str[2] == "c") Code = new byte[] { 137 };                                           //ADC A,C
                        if (Str[2] == "a") Code = new byte[] { 143 };                                           //ADC A,A
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 198, B1(NN) }; };                    //ADC A,N
                    }
                }
            }
            if (Str[0] == "cp")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "c") Code = new byte[] { 185 };                                               //CP C
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 254, B1(NN) }; };                        //CP N
                }
            }
            if (Str[0] == "and")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "c") Code = new byte[] { 161 };                                               //AND C
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 230, B1(NN) }; };                        //AND N
                }
            }
            if (Str[0] == "or")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "c") Code = new byte[] { 177 };                                               //OR C
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 246, B1(NN) }; };                        //OR N
                }
            }
            if (Str[0] == "xor")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "c") Code = new byte[] { 169 };                                               //XOR C
                    if (Str[1] == "a") Code = new byte[] { 175 };                                               //XOR C
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 238, B1(NN) }; };                        //XOR N
                }
            }
            if (Str[0] == "jp")
            {
                if (Str.Count() == 2)
                {
                    switch (Str[1])
                    {
                        case "(hl)": Code = new byte[] { 233 }; break;                                          //JP (HL)
                        case "(ix)": Code = new byte[] { 221, 233 }; break;                                     //JP (IX)
                        case "(iy)": Code = new byte[] { 253, 233 }; break;                                     //JP (IY)
                        default: Code = new byte[] { 195, 0, 0 }; GetLabel(1, Str[1]); break;                   //JP NN
                    }
                }
                if (Str.Count() == 3)
                {

                }
            }
            if (Str[0] == "jr")
            {
                if (Str.Count() == 2)
                {
                    Code = new byte[] { 24, 0 }; GetLabel(1, Str[1]); Rel = true;                               //JR S
                }
            }
            if (Str[0] == "djnz")
            {
                if (Str.Count() == 2)
                {
                    Code = new byte[] { 16, 0 }; GetLabel(1, Str[1]); Rel = true;                               //DJNZ S
                }
            }
            if (Str[0] == "call")
            {
                if (Str.Count() == 2)
                {
                    switch (Str[1])
                    {
                        //case "(hl)": Code = new byte[] { 233 }; break;
                        //case "(ix)": Code = new byte[] { 221, 233 }; break;
                        //case "(iy)": Code = new byte[] { 253, 233 }; break;
                        default: Code = new byte[] { 205, 0, 0 }; GetLabel(1, Str[1]); break;                   //CALL NN
                    }
                }
                if (Str.Count() == 3)
                {
                    //по условиям
                }
            }

            if (Str[0] == "nop") Code = new byte[] { 0 };
            if (Str[0] == "ret") Code = new byte[] { 201 };
            if (Str[0] == "reti") Code = new byte[] { 239, 77 };
            if (Str[0] == "di") Code = new byte[] { 243 };
            if (Str[0] == "ei") Code = new byte[] { 251 };
            if (Str[0] == "defb")
            {
                Code = new byte[Str.Length - 1];
                for (int i = 0; i < Str.Length - 1; i++)
                {
                    ReadNum(Str[i + 1], out NN);
                    Code[i] = B1(NN);
                }
            }
            if (Code == null)
                throw new ArgumentException("Не известная команда \"" + Str[0] + "\"");
        }

        /// <summary>
        /// Изъятие адреса (лейбл или прямой адрес)
        /// </summary>
        /// <param name="str"></param>
        void GetLabel(int to, string str)
        {
            To = to;
            Label = str;
        }

        /// <summary>
        /// Подсановка реального адреса вместо лейбла
        /// </summary>
        /// <param name="adr"></param>
        public void SetAdress(int adr)
        {
            if (To + 2 > Code.Count()) throw new ArgumentException("Команда не поддерживает 16-и битную адрессацию");
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

        /// <summary>
        /// Чтение NN (только числа)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ushort ReadNum(string str)
        {
            ushort res;
            if (ushort.TryParse(str, out res))
                return res;
            throw new ArgumentException(str + " не является числом");
        }

        /// <summary>
        /// Чтение NN (числа, или метки)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        bool ReadNum(string str, out ushort res)
        {
            res = 0;
            if (str[0] == '(' | str[str.Length - 1] == ')') return false;
            bool isDigit = ushort.TryParse(str, out res);
            if (str[0] == '#') { res = HexToDec(str.TrimStart('#')); isDigit = true; }
            if (!isDigit) Label = str;    //Если число не распозналось или не перевелось - считаем его меткой
            return true;
        }

        /// <summary>
        /// Чтение (NN)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        bool ReadAdr(string str, out ushort res)
        {
            if (str[0] == '(' & str[str.Length - 1] == ')')
                return ReadNum(str.Trim('(', ')'), out res);
            res = 0;
            return false;
        }

        byte B1(ushort Num) { return (byte)(Num % 256); }
        byte B2(ushort Num) { return (byte)(Num / 256); }

        /// <summary>
        /// Перевод числа из 16-и ричной системы в 10-и ричную
        /// </summary>
        /// <param name="HEX"></param>
        /// <returns></returns>
        ushort HexToDec(string HEX)
        {
            ushort dec = 0;
            ushort r = 1; //разрядность
            for (int i = HEX.Length - 1; i >= 0; i--)
            {
                if (HEX[i] == '1') dec += r;
                if (HEX[i] == '2') dec += (ushort)(r * 2);
                if (HEX[i] == '3') dec += (ushort)(r * 3);
                if (HEX[i] == '4') dec += (ushort)(r * 4);
                if (HEX[i] == '5') dec += (ushort)(r * 5);
                if (HEX[i] == '6') dec += (ushort)(r * 6);
                if (HEX[i] == '7') dec += (ushort)(r * 7);
                if (HEX[i] == '8') dec += (ushort)(r * 8);
                if (HEX[i] == '9') dec += (ushort)(r * 9);
                if (HEX[i] == 'a') dec += (ushort)(r * 10);
                if (HEX[i] == 'b') dec += (ushort)(r * 11);
                if (HEX[i] == 'c') dec += (ushort)(r * 12);
                if (HEX[i] == 'd') dec += (ushort)(r * 13);
                if (HEX[i] == 'e') dec += (ushort)(r * 14);
                if (HEX[i] == 'f') dec += (ushort)(r * 15);
                r *= 16;
            }
            return dec;
        }

        /// <summary>
        /// Тест на количество параметров
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Command"></param>
        /// <param name="Params"></param>
        void ParamTest(string[] Str, string Command, int Params)
        {
            if (Str.Count() < Params) throw new ArgumentException("Не достаточно параметров в команде " + Command);
            if (Str.Count() > Params) throw new ArgumentException("Слишком много параметров в команде " + Command);
        }

    }
}
