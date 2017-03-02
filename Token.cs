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

        //public static int StartAdress;  //Адрес начала прграммы
        public static int CurAdress;    //Текущий адрес

        public int Adress;
        public byte[] Code;
        public bool IsComand = false;

        public string Label;    //Метка, указанная в команде
        int To;                 //Адрес, куда запишется адрес метки (+ к первому адресу команды)
        bool Rel = false;       //Относительный переход?

        public string ModuleString;   //Метаданные строки (модуль, номер строки, строка)
        public int NumString;          
        public string String;

        /// <summary>
        /// Распознание токена
        /// </summary>
        /// <param name="num"></param>
        /// <param name="Str"></param>
        public Token(string module, int num, string Str)
        {
            ModuleString = module;
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
            //Потом сделать умное разделение, что бы "( N )" было тоже что и "(N)"
            //foreach (string part in Str) Console.Write("\"" + part + "\" ");

            IsComand = true;
            ushort NN;
            ushort S;
            if (Str[0] == "org")                                                                                //ORG
            {
                if (List.Count > 0) throw new ArgumentException("Директива ORG может использоваться только в начале программы");
                ParamTest(Str, "ORG", 2);
                Compiler.StartAdress = ReadNum(Str[1]);
                CurAdress = Compiler.StartAdress;
                return;
            }
            if (Str[0] == "use")                                                                                //ORG
            {
                ParamTest(Str, "USE", 2);
                Module.Add(Str[1]);
                return;
            }
            #region LD
            if (Str[0] == "ld")
            {
                ParamTest(Str, "LD", 3);
                if (Str[1] == "a")
                {
                    if (Str[2] == "a") { Code = new byte[] { 127 }; return; }                                   //LD A,A
                    if (Str[2] == "b") { Code = new byte[] { 120 }; return; }                                   //LD A,B
                    if (Str[2] == "c") { Code = new byte[] { 121 }; return; }                                   //LD A,C
                    if (Str[2] == "d") { Code = new byte[] { 122 }; return; }                                   //LD A,D
                    if (Str[2] == "e") { Code = new byte[] { 123 }; return; }                                   //LD A,E
                    if (Str[2] == "h") { Code = new byte[] { 124 }; return; }                                   //LD A,H
                    if (Str[2] == "l") { Code = new byte[] { 125 }; return; }                                   //LD A,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 126 }; return; }                                //LD A,(HL)
                    if (Str[2] == "(bc)") { Code = new byte[] { 10 }; return; }                                 //LD A,(BC)
                    if (Str[2] == "(de)") { Code = new byte[] { 26 }; return; }                                 //LD A,(DE)
                    if (Str[2] == "i") { Code = new byte[] { 237, 87 }; return; }                               //LD A,I
                    if (Str[2] == "r") { Code = new byte[] { 237, 95 }; return; }                               //LD A,R
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 126, (byte)S }; return; }             //LD A,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 126, (byte)S }; return; }             //LD A,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 62, B1(NN) }; return; }                  //LD A,N
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 58, B1(NN), B2(NN) }; return; }          //LD A,(NN)
                }
                if (Str[1] == "b")
                {
                    if (Str[2] == "a") { Code = new byte[] { 71 }; return; }                                    //LD B,A
                    if (Str[2] == "b") { Code = new byte[] { 64 }; return; }                                    //LD B,B
                    if (Str[2] == "c") { Code = new byte[] { 65 }; return; }                                    //LD B,C
                    if (Str[2] == "d") { Code = new byte[] { 66 }; return; }                                    //LD B,D
                    if (Str[2] == "e") { Code = new byte[] { 67 }; return; }                                    //LD B,E
                    if (Str[2] == "h") { Code = new byte[] { 68 }; return; }                                    //LD B,H
                    if (Str[2] == "l") { Code = new byte[] { 69 }; return; }                                    //LD B,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 70 }; return; }                                 //LD B,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 70, (byte)S }; return; }              //LD B,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 70, (byte)S }; return; }              //LD B,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 6, B1(NN) }; return; };                  //LD B,N
                }
                if (Str[1] == "c")
                {
                    if (Str[2] == "a") { Code = new byte[] { 79 }; return; }                                    //LD C,A
                    if (Str[2] == "b") { Code = new byte[] { 72 }; return; }                                    //LD C,B
                    if (Str[2] == "c") { Code = new byte[] { 73 }; return; }                                    //LD C,C
                    if (Str[2] == "d") { Code = new byte[] { 74 }; return; }                                    //LD C,D
                    if (Str[2] == "e") { Code = new byte[] { 75 }; return; }                                    //LD C,E
                    if (Str[2] == "h") { Code = new byte[] { 76 }; return; }                                    //LD C,H
                    if (Str[2] == "l") { Code = new byte[] { 77 }; return; }                                    //LD C,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 78 }; return; }                                 //LD C,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 78, (byte)S }; return; }              //LD C,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 78, (byte)S }; return; }              //LD C,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 14, B1(NN) }; return; }                  //LD C,N
                }
                if (Str[1] == "d")
                {
                    if (Str[2] == "a") { Code = new byte[] { 87 }; return; }                                    //LD D,A
                    if (Str[2] == "b") { Code = new byte[] { 80 }; return; }                                    //LD D,B
                    if (Str[2] == "c") { Code = new byte[] { 81 }; return; }                                    //LD D,C
                    if (Str[2] == "d") { Code = new byte[] { 82 }; return; }                                    //LD D,D
                    if (Str[2] == "e") { Code = new byte[] { 83 }; return; }                                    //LD D,E
                    if (Str[2] == "h") { Code = new byte[] { 84 }; return; }                                    //LD D,H
                    if (Str[2] == "l") { Code = new byte[] { 85 }; return; }                                    //LD D,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 86 }; return; }                                 //LD D,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 86, (byte)S }; return; }              //LD D,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 86, (byte)S }; return; }              //LD D,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 22, B1(NN) }; return; }                  //LD D,N
                }
                if (Str[1] == "e")
                {
                    if (Str[2] == "a") { Code = new byte[] { 95 }; return; }                                    //LD E,A
                    if (Str[2] == "b") { Code = new byte[] { 88 }; return; }                                    //LD E,B
                    if (Str[2] == "c") { Code = new byte[] { 89 }; return; }                                    //LD E,C
                    if (Str[2] == "d") { Code = new byte[] { 90 }; return; }                                    //LD E,D
                    if (Str[2] == "e") { Code = new byte[] { 91 }; return; }                                    //LD E,E
                    if (Str[2] == "h") { Code = new byte[] { 92 }; return; }                                    //LD E,H
                    if (Str[2] == "l") { Code = new byte[] { 93 }; return; }                                    //LD E,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 94 }; return; }                                 //LD E,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 94, (byte)S }; return; }              //LD E,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 94, (byte)S }; return; }              //LD E,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 30, B1(NN) }; return; }                  //LD E,N
                }
                if (Str[1] == "h")
                {
                    if (Str[2] == "a") { Code = new byte[] { 103 }; return; }                                   //LD H,A
                    if (Str[2] == "b") { Code = new byte[] { 96 }; return; }                                    //LD H,B
                    if (Str[2] == "c") { Code = new byte[] { 97 }; return; }                                    //LD H,C
                    if (Str[2] == "d") { Code = new byte[] { 98 }; return; }                                    //LD H,D
                    if (Str[2] == "e") { Code = new byte[] { 99 }; return; }                                    //LD H,E
                    if (Str[2] == "h") { Code = new byte[] { 100 }; return; }                                   //LD H,H
                    if (Str[2] == "l") { Code = new byte[] { 101 }; return; }                                   //LD H,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 102 }; return; }                                //LD H,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 102, (byte)S }; return; }             //LD H,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 102, (byte)S }; return; }             //LD H,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 38, B1(NN) }; return; }                  //LD H,N
                }
                if (Str[1] == "l")
                {
                    if (Str[2] == "a") { Code = new byte[] { 111 }; return; }                                   //LD L,A
                    if (Str[2] == "b") { Code = new byte[] { 104 }; return; }                                   //LD L,B
                    if (Str[2] == "c") { Code = new byte[] { 105 }; return; }                                   //LD L,C
                    if (Str[2] == "d") { Code = new byte[] { 106 }; return; }                                   //LD L,D
                    if (Str[2] == "e") { Code = new byte[] { 107 }; return; }                                   //LD L,E
                    if (Str[2] == "h") { Code = new byte[] { 108 }; return; }                                   //LD L,H
                    if (Str[2] == "l") { Code = new byte[] { 109 }; return; }                                   //LD L,L
                    if (Str[2] == "(hl)") { Code = new byte[] { 110 }; return; }                                //LD L,(HL)
                    if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 110, (byte)S }; return; }             //LD L,(IX+S)
                    if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 110, (byte)S }; return; }             //LD L,(IY+S)
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 46, B1(NN) }; return; }                  //LD L,N
                }
                if (Str[1] == "de")
                {
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 17, B1(NN), B2(NN) }; return; }          //LD DE,NN
                    To = 2;
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 237, 91, B1(NN), B2(NN) }; return; }     //LD DE,(NN)
                }
                if (Str[1] == "bc")
                {
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 1, B1(NN), B2(NN) }; return; }           //LD BC,NN
                    To = 2;
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 237, 75, B1(NN), B2(NN) }; return; }     //LD BC,(NN)
                }
                if (Str[1] == "hl")
                {
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 33, B1(NN), B2(NN) }; return; }          //LD HL,NN
                    To = 1;
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 42, B1(NN), B2(NN) }; return; }          //LD HL,(NN)
                }
                if (Str[1] == "ix")
                {
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 221, 33, B1(NN), B2(NN) }; return; }     //LD IX,NN
                    To = 2;
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 221, 42, B1(NN), B2(NN) }; return; }     //LD IX,(NN)
                }
                if (Str[1] == "iy")
                {
                    To = 2;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 253, 33, B1(NN), B2(NN) }; return; }     //LD IY,NN
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 253, 42, B1(NN), B2(NN) }; return; }     //LD IY,(NN)
                }
                if (Str[1] == "sp")
                {
                    if (Str[2] == "hl") { Code = new byte[] { 249 }; return; }                                  //LD SP,HL
                    if (Str[2] == "ix") { Code = new byte[] { 221, 249 }; return; }                             //LD SP,IX
                    if (Str[2] == "iy") { Code = new byte[] { 253, 249 }; return; }                             //LD SP,IY
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 49, B1(NN), B2(NN) }; return; }          //LD SP,NN
                    To = 2;
                    if (ReadAdr(Str[2], out NN)) { Code = new byte[] { 237, 123, B1(NN), B2(NN) }; return; }    //LD SP,(NN)
                }
                if (Str[1] == "i")
                {
                    if (Str[2] == "a") { Code = new byte[] { 237, 71 }; return; }                               //LD I,A
                }
                if (Str[1] == "r")
                {
                    if (Str[2] == "a") { Code = new byte[] { 237, 79 }; return; }                               //LD R,A
                }
                if (Str[1] == "(hl)")
                {
                    if (Str[2] == "a") { Code = new byte[] { 119 }; return; }                                   //LD (HL),A
                    if (Str[2] == "h") { Code = new byte[] { 116 }; return; }                                   //LD (HL),H
                    if (Str[2] == "l") { Code = new byte[] { 117 }; return; }                                   //LD (HL),L
                    if (Str[2] == "b") { Code = new byte[] { 112 }; return; }                                   //LD (HL),B
                    if (Str[2] == "c") { Code = new byte[] { 113 }; return; }                                   //LD (HL),C
                    if (Str[2] == "d") { Code = new byte[] { 114 }; return; }                                   //LD (HL),D
                    if (Str[2] == "e") { Code = new byte[] { 115 }; return; }                                   //LD (HL),E
                    To = 1;
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 54, B1(NN), B2(NN) }; return; }          //LD (HL),NN
                }
                if (Str[2] == "a")
                {
                    if (Str[1] == "(bc)") { Code = new byte[] { 2 }; return; }                                  //LD (BC),A
                    if (Str[1] == "(de)") { Code = new byte[] { 18 }; return; }                                 //LD (DE),A
                }
                if (ReadIX(Str[1], out S))
                {
                    if (Str[2] == "a") { Code = new byte[] { 221, 119, (byte)S }; return; }                     //LD (IX+S),A
                    if (Str[2] == "h") { Code = new byte[] { 221, 116, (byte)S }; return; }                     //LD (IX+S),H
                    if (Str[2] == "l") { Code = new byte[] { 221, 117, (byte)S }; return; }                     //LD (IX+S),L
                    if (Str[2] == "b") { Code = new byte[] { 221, 112, (byte)S }; return; }                     //LD (IX+S),B
                    if (Str[2] == "c") { Code = new byte[] { 221, 113, (byte)S }; return; }                     //LD (IX+S),C
                    if (Str[2] == "d") { Code = new byte[] { 221, 114, (byte)S }; return; }                     //LD (IX+S),D
                    if (Str[2] == "e") { Code = new byte[] { 221, 115, (byte)S }; return; }                     //LD (IX+S),E
                    if (ReadNum(Str[2], out NN) ) { Code = new byte[] { 221, 54, (byte)S, B1(NN) }; return; }   //LD (IX+S),A

                }
                if (ReadIY(Str[1], out S))
                {
                    if (Str[2] == "a") { Code = new byte[] { 253, 119, (byte)S }; return; }                     //LD (IY+S),A
                    if (Str[2] == "h") { Code = new byte[] { 253, 116, (byte)S }; return; }                     //LD (IY+S),H
                    if (Str[2] == "l") { Code = new byte[] { 253, 117, (byte)S }; return; }                     //LD (IY+S),L
                    if (Str[2] == "b") { Code = new byte[] { 253, 112, (byte)S }; return; }                     //LD (IY+S),B
                    if (Str[2] == "c") { Code = new byte[] { 253, 113, (byte)S }; return; }                     //LD (IY+S),C
                    if (Str[2] == "d") { Code = new byte[] { 253, 114, (byte)S }; return; }                     //LD (IY+S),D
                    if (Str[2] == "e") { Code = new byte[] { 253, 115, (byte)S }; return; }                     //LD (IY+S),E
                    if (ReadNum(Str[2], out NN)) { Code = new byte[] { 253, 54, (byte)S, B1(NN) }; return; }    //LD (IY+S),A

                }
                if (ReadAdr(Str[1], out NN))
                {
                    To = 1;
                    if (Str[2] == "a") { Code = new byte[] { 50, B1(NN), B2(NN) }; return; }                    //LD (NN),A
                    if (Str[2] == "hl") { Code = new byte[] { 34, B1(NN), B2(NN) }; return; }                   //LD (NN),HL
                    To = 2;
                    if (Str[2] == "sp") { Code = new byte[] { 237, 115, B1(NN), B2(NN) }; return; }             //LD (NN),SP
                    if (Str[2] == "bc") { Code = new byte[] { 237, 67, B1(NN), B2(NN) }; return; }              //LD (NN),BC
                    if (Str[2] == "de") { Code = new byte[] { 237, 83, B1(NN), B2(NN) }; return; }              //LD (NN),DE
                    if (Str[2] == "ix") { Code = new byte[] { 221, 34, B1(NN), B2(NN) }; return; }              //LD (NN),IX
                    if (Str[2] == "iy") { Code = new byte[] { 253, 34, B1(NN), B2(NN) }; return; }              //LD (NN),IY
                }
                if (Code == null) throw new ArgumentException("\"" + Str[1] + "," + Str[2] + "\" не является допустимым сочетанием");
                else return;
            }
            #endregion
            if (Str[0] == "exx")                                                                                
            {
                Code = new byte[] { 217 }; return;                                                              //EXX
            }
            if (Str[0] == "ex")
            {
                if (Str[1] == "de" & Str[2] == "hl") { Code = new byte[] { 235 }; return; }                     //EX DE,HL
                if (Str[1] == "af" & Str[2] == "af'") { Code = new byte[] { 8 }; return; }                      //EX AF,AF'
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

        /// <summary>
        /// Чтение (IX+S)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        bool ReadIX(string str, out ushort res)
        {
            if (str.Length > 5 && str.Substring(0, 4) == "(ix+" & str[str.Length - 1] == ')')
                return ReadNum(str.Substring(4, str.Length - 5), out res);
            res = 0;
            return false;
        }

        /// <summary>
        /// Чтение (IY+S)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        bool ReadIY(string str, out ushort res)
        {
            if (str.Length > 5 && str.Substring(0, 4) == "(iy+" & str[str.Length - 1] == ')')
                return ReadNum(str.Substring(4, str.Length - 5), out res);
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
