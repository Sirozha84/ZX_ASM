using System;
using System.Collections.Generic;
using System.Linq;

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
            
            #region Директивы компилятора
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
            if (Str[0] == "defb")
            {
                Code = new byte[Str.Length - 1];
                for (int i = 0; i < Str.Length - 1; i++)
                {
                    ReadNum(Str[i + 1], out NN);
                    Code[i] = B1(NN);
                }
            }
            #endregion

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
            
            #region ADD, ADC, INC
            if (Str[0] == "add")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "a")
                    {
                        if (Str[2] == "a") { Code = new byte[] { 135 }; return; }                               //ADD A,A
                        if (Str[2] == "h") { Code = new byte[] { 132 }; return; }                               //ADD A,H
                        if (Str[2] == "l") { Code = new byte[] { 133 }; return; }                               //ADD A,L
                        if (Str[2] == "b") { Code = new byte[] { 128 }; return; }                               //ADD A,B
                        if (Str[2] == "c") { Code = new byte[] { 129 }; return; }                               //ADD A,C
                        if (Str[2] == "d") { Code = new byte[] { 130 }; return; }                               //ADD A,D
                        if (Str[2] == "e") { Code = new byte[] { 131 }; return; }                               //ADD A,E
                        if (Str[2] == "(hl)") { Code = new byte[] { 134 }; return; }                            //ADD A,(HL)
                        if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 134, (byte)S }; return; }         //ADD A,(IX+S)
                        if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 134, (byte)S }; return; }         //ADD A,(IY+S)
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 198, B1(NN) }; return; }             //ADD A,N
                    }
                    if (Str[1] == "hl")
                    {
                        if (Str[2] == "hl") { Code = new byte[] { 41 }; return; }                               //ADD HL,HL
                        if (Str[2] == "bc") { Code = new byte[] { 9 }; return; }                                //ADD HL,BC
                        if (Str[2] == "de") { Code = new byte[] { 25 }; return; }                               //ADD HL,DE
                        if (Str[2] == "sp") { Code = new byte[] { 57 }; return; }                               //ADD HL,SP
                    }
                    if (Str[1] == "ix")
                    {
                        if (Str[2] == "ix") { Code = new byte[] { 221, 41 }; return; }                          //ADD IX,IX
                        if (Str[2] == "bc") { Code = new byte[] { 221, 9 }; return; }                           //ADD IX,BC
                        if (Str[2] == "de") { Code = new byte[] { 221, 25 }; return; }                          //ADD IX,DE
                        if (Str[2] == "sp") { Code = new byte[] { 221, 57 }; return; }                          //ADD IX,SP
                    }
                    if (Str[1] == "iy")
                    {
                        if (Str[2] == "iy") { Code = new byte[] { 253, 41 }; return; }                          //ADD IY,IY
                        if (Str[2] == "bc") { Code = new byte[] { 253, 9 }; return; }                           //ADD IY,BC
                        if (Str[2] == "de") { Code = new byte[] { 253, 25 }; return; }                          //ADD IY,DE
                        if (Str[2] == "sp") { Code = new byte[] { 253, 57 }; return; }                          //ADD IY,SP
                    }
                }
            }
            if (Str[0] == "adc")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "a")
                    {
                        if (Str[2] == "a") { Code = new byte[] { 143 }; return; }                               //ADC A,A
                        if (Str[2] == "h") { Code = new byte[] { 140 }; return; }                               //ADC A,H
                        if (Str[2] == "l") { Code = new byte[] { 141 }; return; }                               //ADC A,L
                        if (Str[2] == "b") { Code = new byte[] { 136 }; return; }                               //ADC A,B
                        if (Str[2] == "c") { Code = new byte[] { 137 }; return; }                               //ADC A,C
                        if (Str[2] == "d") { Code = new byte[] { 138 }; return; }                               //ADC A,D
                        if (Str[2] == "e") { Code = new byte[] { 139 }; return; }                               //ADC A,E
                        if (Str[2] == "(hl)") { Code = new byte[] { 142 }; return; }                            //ADC A,(HL)
                        if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 142, (byte)S }; return; }         //ADC A,(IX+S)
                        if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 142, (byte)S }; return; }         //ADC A,(IY+S)
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 206, B1(NN) }; return; }             //ADC A,N
                    }
                    if (Str[1] == "hl")
                    {
                        if (Str[2] == "hl") { Code = new byte[] { 237, 106 }; return; }                         //ADC HL,HL
                        if (Str[2] == "bc") { Code = new byte[] { 237, 74 }; return; }                          //ADC HL,BC
                        if (Str[2] == "de") { Code = new byte[] { 237, 90 }; return; }                          //ADC HL,DE
                        if (Str[2] == "sp") { Code = new byte[] { 237, 122 }; return; }                         //ADC HL,SP
                    }
                }
            }
            if (Str[0] == "inc")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") Code = new byte[] { 60 };                                                //INC A
                    if (Str[1] == "h") Code = new byte[] { 36 };                                                //INC H
                    if (Str[1] == "l") Code = new byte[] { 44 };                                                //INC L
                    if (Str[1] == "b") Code = new byte[] { 4 };                                                 //INC B
                    if (Str[1] == "c") Code = new byte[] { 12 };                                                //INC C
                    if (Str[1] == "d") Code = new byte[] { 20 };                                                //INC D
                    if (Str[1] == "e") Code = new byte[] { 28 };                                                //INC E
                    if (Str[1] == "hl") Code = new byte[] { 35 };                                               //INC HL
                    if (Str[1] == "bc") Code = new byte[] { 3 };                                                //INC BC
                    if (Str[1] == "de") Code = new byte[] { 19 };                                               //INC DE
                    if (Str[1] == "sp") Code = new byte[] { 51 };                                               //INC SP
                    if (Str[1] == "ix") Code = new byte[] { 221, 35 };                                          //INC IX
                    if (Str[1] == "iy") Code = new byte[] { 253, 35 };                                          //INC IY
                    if (Str[1] == "(hl)") Code = new byte[] { 52 };                                             //INC (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 52, (byte)S }; return; }              //INC (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 52, (byte)S }; return; }              //INC (IY+S)
                }
            }
            #endregion

            #region SUB, SBC, DEC
            if (Str[0] == "sub")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 151 }; return; }                                   //SUB A
                    if (Str[1] == "h") { Code = new byte[] { 148 }; return; }                                   //SUB H
                    if (Str[1] == "l") { Code = new byte[] { 149 }; return; }                                   //SUB L
                    if (Str[1] == "b") { Code = new byte[] { 144 }; return; }                                   //SUB B
                    if (Str[1] == "c") { Code = new byte[] { 145 }; return; }                                   //SUB C
                    if (Str[1] == "d") { Code = new byte[] { 146 }; return; }                                   //SUB D
                    if (Str[1] == "e") { Code = new byte[] { 147 }; return; }                                   //SUB E
                    if (Str[1] == "(hl)") { Code = new byte[] { 150 }; return; }                                //SUB (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 150, (byte)S }; return; }             //SUB (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 150, (byte)S }; return; }             //SUB (IY+S)
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 214, B1(NN) }; return; }                 //SUB A,N
                }
            }
            if (Str[0] == "sbc")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "a")
                    {
                        if (Str[2] == "a") { Code = new byte[] { 159 }; return; }                               //SBC A,A
                        if (Str[2] == "h") { Code = new byte[] { 156 }; return; }                               //SBC A,H
                        if (Str[2] == "l") { Code = new byte[] { 157 }; return; }                               //SBC A,L
                        if (Str[2] == "b") { Code = new byte[] { 152 }; return; }                               //SBC A,B
                        if (Str[2] == "c") { Code = new byte[] { 153 }; return; }                               //SBC A,C
                        if (Str[2] == "d") { Code = new byte[] { 154 }; return; }                               //SBC A,D
                        if (Str[2] == "e") { Code = new byte[] { 155 }; return; }                               //SBC A,E
                        if (Str[2] == "(hl)") { Code = new byte[] { 158 }; return; }                            //SBC A,(HL)
                        if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 158, (byte)S }; return; }         //SBC A,(IX+S)
                        if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 158, (byte)S }; return; }         //SBC A,(IY+S)
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 222, B1(NN) }; return; }             //SBC A,N
                    }
                    if (Str[1] == "hl")
                    {
                        if (Str[2] == "hl") { Code = new byte[] { 237, 98 }; return; }                          //SBC HL,HL
                        if (Str[2] == "bc") { Code = new byte[] { 237, 66 }; return; }                          //SBC HL,BC
                        if (Str[2] == "de") { Code = new byte[] { 237, 82 }; return; }                          //SBC HL,DE
                        if (Str[2] == "sp") { Code = new byte[] { 237, 114 }; return; }                         //SBC HL,SP
                    }
                }
            }
            if (Str[0] == "dec")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") Code = new byte[] { 61 };                                                //DEC A
                    if (Str[1] == "h") Code = new byte[] { 37 };                                                //DEC H
                    if (Str[1] == "l") Code = new byte[] { 45 };                                                //DEC L
                    if (Str[1] == "b") Code = new byte[] { 5 };                                                 //DEC B
                    if (Str[1] == "c") Code = new byte[] { 13 };                                                //DEC C
                    if (Str[1] == "d") Code = new byte[] { 21 };                                                //DEC D
                    if (Str[1] == "e") Code = new byte[] { 29 };                                                //DEC E
                    if (Str[1] == "hl") Code = new byte[] { 43 };                                               //DEC HL
                    if (Str[1] == "bc") Code = new byte[] { 11 };                                               //DEC BC
                    if (Str[1] == "de") Code = new byte[] { 27 };                                               //DEC DE
                    if (Str[1] == "sp") Code = new byte[] { 59 };                                               //DEC SP
                    if (Str[1] == "ix") Code = new byte[] { 221, 43 };                                          //DEC IX
                    if (Str[1] == "iy") Code = new byte[] { 253, 43 };                                          //DEC IY
                    if (Str[1] == "(hl)") Code = new byte[] { 53 };                                             //DEC (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 53, (byte)S }; return; }              //DEC (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 53, (byte)S }; return; }              //DEC (IY+S)
                }
            }
            #endregion

            #region CP, AND, OR, XOR
            if (Str[0] == "cp")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 191 }; return; }                                   //CP A
                    if (Str[1] == "h") { Code = new byte[] { 188 }; return; }                                   //CP H
                    if (Str[1] == "l") { Code = new byte[] { 189 }; return; }                                   //CP L
                    if (Str[1] == "b") { Code = new byte[] { 184 }; return; }                                   //CP B
                    if (Str[1] == "c") { Code = new byte[] { 185 }; return; }                                   //CP C
                    if (Str[1] == "d") { Code = new byte[] { 186 }; return; }                                   //CP D
                    if (Str[1] == "e") { Code = new byte[] { 187 }; return; }                                   //CP E
                    if (Str[1] == "(hl)") { Code = new byte[] { 190 }; return; }                                //CP (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 190, (byte)S }; return; }             //CP (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 190, (byte)S }; return; }             //CP (IY+S)
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 254, B1(NN) }; return; }                 //CP A,N
                }
            }
            if (Str[0] == "and")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 167 }; return; }                                   //AND A
                    if (Str[1] == "h") { Code = new byte[] { 164 }; return; }                                   //AND H
                    if (Str[1] == "l") { Code = new byte[] { 165 }; return; }                                   //AND L
                    if (Str[1] == "b") { Code = new byte[] { 160 }; return; }                                   //AND B
                    if (Str[1] == "c") { Code = new byte[] { 161 }; return; }                                   //AND C
                    if (Str[1] == "d") { Code = new byte[] { 162 }; return; }                                   //AND D
                    if (Str[1] == "e") { Code = new byte[] { 163 }; return; }                                   //AND E
                    if (Str[1] == "(hl)") { Code = new byte[] { 166 }; return; }                                //AND (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 166, (byte)S }; return; }             //AND (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 166, (byte)S }; return; }             //AND (IY+S)
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 230, B1(NN) }; return; }                 //AND A,N
                }
            }
            if (Str[0] == "or")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 183 }; return; }                                   //OR A
                    if (Str[1] == "h") { Code = new byte[] { 180 }; return; }                                   //OR H
                    if (Str[1] == "l") { Code = new byte[] { 181 }; return; }                                   //OR L
                    if (Str[1] == "b") { Code = new byte[] { 176 }; return; }                                   //OR B
                    if (Str[1] == "c") { Code = new byte[] { 177 }; return; }                                   //OR C
                    if (Str[1] == "d") { Code = new byte[] { 178 }; return; }                                   //OR D
                    if (Str[1] == "e") { Code = new byte[] { 179 }; return; }                                   //OR E
                    if (Str[1] == "(hl)") { Code = new byte[] { 182 }; return; }                                //OR (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 182, (byte)S }; return; }             //OR (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 182, (byte)S }; return; }             //OR (IY+S)
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 246, B1(NN) }; return; }                 //OR A,N
                }
            }
            if (Str[0] == "xor")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 175 }; return; }                                   //XOR A
                    if (Str[1] == "h") { Code = new byte[] { 172 }; return; }                                   //XOR H
                    if (Str[1] == "l") { Code = new byte[] { 173 }; return; }                                   //XOR L
                    if (Str[1] == "b") { Code = new byte[] { 168 }; return; }                                   //XOR B
                    if (Str[1] == "c") { Code = new byte[] { 169 }; return; }                                   //XOR C
                    if (Str[1] == "d") { Code = new byte[] { 170 }; return; }                                   //XOR D
                    if (Str[1] == "e") { Code = new byte[] { 171 }; return; }                                   //XOR E
                    if (Str[1] == "(hl)") { Code = new byte[] { 174 }; return; }                                //XOR (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 174, (byte)S }; return; }             //XOR (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 174, (byte)S }; return; }             //XOR (IY+S)
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 238, B1(NN) }; return; }                 //XOR A,N
                }
            }
            #endregion

            #region JP, JR
            if (Str[0] == "jp")
            {
                if (Str.Count() == 2)
                {
                    if (Str[1] == "(hl)") { Code = new byte[] { 233 }; return; }                                //JP (HL)
                    if (Str[1] == "(ix)") { Code = new byte[] { 221, 233 }; return; }                           //JP (IX)
                    if (Str[1] == "(iy)") { Code = new byte[] { 253, 233 }; return; }                           //JP (IY)
                    To = 1;
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 195, B1(NN), B2(NN) }; return; }         //JP NN
                }
                if (Str.Count() == 3)
                {
                    if (Str[1] == "c")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 218, B1(NN), B2(NN) }; return; }     //JP С,NN
                    if (Str[1] == "nc")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 210, B1(NN), B2(NN) }; return; }     //JP NС,NN
                    if (Str[1] == "z")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 202, B1(NN), B2(NN) }; return; }     //JP Z,NN
                    if (Str[1] == "nz")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 194, B1(NN), B2(NN) }; return; }     //JP NZ,NN
                    if (Str[1] == "p")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 242, B1(NN), B2(NN) }; return; }     //JP P,NN
                    if (Str[1] == "m")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 250, B1(NN), B2(NN) }; return; }     //JP M,NN
                    if (Str[1] == "pe")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 234, B1(NN), B2(NN) }; return; }     //JP PE,NN
                    if (Str[1] == "po")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 226, B1(NN), B2(NN) }; return; }     //JP PO,NN
                }
            }
            if (Str[0] == "jr")
            {
                To = 1;
                Rel = true;
                if (Str.Count() == 2)
                {
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 24, B1(NN) }; return; }                  //JR S
                }
                if (Str.Count() == 3)
                {
                    if (Str[1] == "c")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 56, B1(NN) }; return; }              //JR C,S
                    if (Str[1] == "z")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 40, B1(NN) }; return; }              //JR Z,S
                    if (Str[1] == "nz")
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 32, B1(NN) }; return; }              //JR NZ,S
                }
            }
            if (Str[0] == "djnz")
            {
                To = 1;
                Rel = true;
                if (Str.Count() == 2)
                {
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 16, B1(NN) }; return; }                  //DJNZ S
                }
            }
            #endregion

            #region PUSH, POP
            if (Str[0] == "push")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "af") { Code = new byte[] { 245 }; return; }                                  //PUSH AF
                    if (Str[1] == "bc") { Code = new byte[] { 197 }; return; }                                  //PUSH BC
                    if (Str[1] == "de") { Code = new byte[] { 213 }; return; }                                  //PUSH DE
                    if (Str[1] == "hl") { Code = new byte[] { 229 }; return; }                                  //PUSH HL
                    if (Str[1] == "ix") { Code = new byte[] { 221, 229 }; return; }                             //PUSH IX
                    if (Str[1] == "iy") { Code = new byte[] { 253, 229 }; return; }                             //PUSH IY
                }
            }
            if (Str[0] == "pop")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "af") { Code = new byte[] { 241 }; return; }                                  //POP AF
                    if (Str[1] == "bc") { Code = new byte[] { 193 }; return; }                                  //POP BC
                    if (Str[1] == "de") { Code = new byte[] { 209 }; return; }                                  //POP DE
                    if (Str[1] == "hl") { Code = new byte[] { 225 }; return; }                                  //POP HL
                    if (Str[1] == "ix") { Code = new byte[] { 221, 225 }; return; }                             //POP IX
                    if (Str[1] == "iy") { Code = new byte[] { 253, 225 }; return; }                             //POP IY
                }
            }
            #endregion

            #region RST, CALL, RET
            if (Str[0] == "rst")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "0") { Code = new byte[] { 199 }; return; }                                   //RST 0
                    if (Str[1] == "8") { Code = new byte[] { 207 }; return; }                                   //RST 8
                    if (Str[1] == "10") { Code = new byte[] { 215 }; return; }                                  //RST 10
                    if (Str[1] == "18") { Code = new byte[] { 223 }; return; }                                  //RST 18
                    if (Str[1] == "20") { Code = new byte[] { 231 }; return; }                                  //RST 20
                    if (Str[1] == "28") { Code = new byte[] { 239 }; return; }                                  //RST 28
                    if (Str[1] == "30") { Code = new byte[] { 247 }; return; }                                  //RST 30
                    if (Str[1] == "38") { Code = new byte[] { 255 }; return; }                                  //RST 38
                }
            }
            if (Str[0] == "call")
            {
                if (Str.Length == 2)
                {
                    if (ReadNum(Str[1], out NN)) { Code = new byte[] { 205, B1(NN), B2(NN) }; return; }         //CALL NN
                }
                else if (Str.Length == 3)
                {
                    if (Str[1] == "c" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 220, B1(NN), B2(NN) }; return; }                                  //CALL C,NN
                    if (Str[1] == "nc" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 212, B1(NN), B2(NN) }; return; }                                  //CALL NC,NN
                    if (Str[1] == "z" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 204, B1(NN), B2(NN) }; return; }                                  //CALL Z,NN
                    if (Str[1] == "nz" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 196, B1(NN), B2(NN) }; return; }                                  //CALL NZ,NN
                    if (Str[1] == "m" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 252, B1(NN), B2(NN) }; return; }                                  //CALL M,NN
                    if (Str[1] == "p" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 244, B1(NN), B2(NN) }; return; }                                  //CALL P,NN
                    if (Str[1] == "pe" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 236, B1(NN), B2(NN) }; return; }                                  //CALL PE,NN
                    if (Str[1] == "po" && ReadNum(Str[2], out NN))
                        { Code = new byte[] { 228, B1(NN), B2(NN) }; return; }                                  //CALL PO,NN
                }
            }
            if (Str[0] == "ret")
            {
                if (Str.Length == 1) { Code = new byte[] { 201 }; return; }                                     //RET
                else if (Str.Length == 2)
                {
                    if (Str[1] == "c" ) { Code = new byte[] { 216 }; return; }                                  //RET C
                    if (Str[1] == "nc" ) { Code = new byte[] { 208 }; return; }                                 //RET NC
                    if (Str[1] == "z" ) { Code = new byte[] { 200 }; return; }                                  //RET Z
                    if (Str[1] == "nz" ) { Code = new byte[] { 192 }; return; }                                 //RET NZ
                    if (Str[1] == "m" ) { Code = new byte[] { 248 }; return; }                                  //RET M
                    if (Str[1] == "p" ) { Code = new byte[] { 240 }; return; }                                  //RET P
                    if (Str[1] == "pe" ) { Code = new byte[] { 232 }; return; }                                 //RET PE
                    if (Str[1] == "po" ) { Code = new byte[] { 224 }; return; }                                 //RET PO
                }
            }
            #endregion

            #region SRL, SRA, SLA
            if (Str[0] == "srl")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 63 }; return; }                               //SRL A
                    if (Str[1] == "h") { Code = new byte[] { 203, 60 }; return; }                               //SRL H
                    if (Str[1] == "l") { Code = new byte[] { 203, 61 }; return; }                               //SRL L
                    if (Str[1] == "b") { Code = new byte[] { 203, 56 }; return; }                               //SRL B
                    if (Str[1] == "c") { Code = new byte[] { 203, 57 }; return; }                               //SRL C
                    if (Str[1] == "d") { Code = new byte[] { 203, 58 }; return; }                               //SRL D
                    if (Str[1] == "e") { Code = new byte[] { 203, 59 }; return; }                               //SRL E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 62 }; return; }                            //SRL (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 62 }; return; }         //SRL (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 62 }; return; }         //SRL (IY+S)
                }
            }
            if (Str[0] == "sra")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 47 }; return; }                               //SRA A
                    if (Str[1] == "h") { Code = new byte[] { 203, 44 }; return; }                               //SRA H
                    if (Str[1] == "l") { Code = new byte[] { 203, 45 }; return; }                               //SRA L
                    if (Str[1] == "b") { Code = new byte[] { 203, 40 }; return; }                               //SRA B
                    if (Str[1] == "c") { Code = new byte[] { 203, 41 }; return; }                               //SRA C
                    if (Str[1] == "d") { Code = new byte[] { 203, 42 }; return; }                               //SRA D
                    if (Str[1] == "e") { Code = new byte[] { 203, 43 }; return; }                               //SRA E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 46 }; return; }                            //SRA (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 46 }; return; }         //SRA (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 46 }; return; }         //SRA (IY+S)
                }
            }
            if (Str[0] == "sla")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 39 }; return; }                               //SLA A
                    if (Str[1] == "h") { Code = new byte[] { 203, 36 }; return; }                               //SLA H
                    if (Str[1] == "l") { Code = new byte[] { 203, 37 }; return; }                               //SLA L
                    if (Str[1] == "b") { Code = new byte[] { 203, 32 }; return; }                               //SLA B
                    if (Str[1] == "c") { Code = new byte[] { 203, 33 }; return; }                               //SLA C
                    if (Str[1] == "d") { Code = new byte[] { 203, 34 }; return; }                               //SLA D
                    if (Str[1] == "e") { Code = new byte[] { 203, 35 }; return; }                               //SLA E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 38 }; return; }                            //SLA (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 38 }; return; }         //SLA (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 38 }; return; }         //SLA (IY+S)
                }
            }
            #endregion

            #region RL, RR, RLC, RRC, RLA, RRA, RLCA, RRCA, RLD, RRD
            if (Str[0] == "rl")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 23 }; return; }                               //RL A
                    if (Str[1] == "h") { Code = new byte[] { 203, 20 }; return; }                               //RL H
                    if (Str[1] == "l") { Code = new byte[] { 203, 21 }; return; }                               //RL L
                    if (Str[1] == "b") { Code = new byte[] { 203, 16 }; return; }                               //RL B
                    if (Str[1] == "c") { Code = new byte[] { 203, 17 }; return; }                               //RL C
                    if (Str[1] == "d") { Code = new byte[] { 203, 18 }; return; }                               //RL D
                    if (Str[1] == "e") { Code = new byte[] { 203, 19 }; return; }                               //RL E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 22 }; return; }                            //RL (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 22 }; return; }         //RL (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 22 }; return; }         //RL (IY+S)
                }
            }
            if (Str[0] == "rr")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 31 }; return; }                               //RR A
                    if (Str[1] == "h") { Code = new byte[] { 203, 28 }; return; }                               //RR H
                    if (Str[1] == "l") { Code = new byte[] { 203, 29 }; return; }                               //RR L
                    if (Str[1] == "b") { Code = new byte[] { 203, 24 }; return; }                               //RR B
                    if (Str[1] == "c") { Code = new byte[] { 203, 25 }; return; }                               //RR C
                    if (Str[1] == "d") { Code = new byte[] { 203, 26 }; return; }                               //RR D
                    if (Str[1] == "e") { Code = new byte[] { 203, 27 }; return; }                               //RR E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 30 }; return; }                            //RR (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 30 }; return; }         //RR (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 30 }; return; }         //RR (IY+S)
                }
            }
            if (Str[0] == "rlc")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 7 }; return; }                                //RLC A
                    if (Str[1] == "h") { Code = new byte[] { 203, 4 }; return; }                                //RLC H
                    if (Str[1] == "l") { Code = new byte[] { 203, 5 }; return; }                                //RLC L
                    if (Str[1] == "b") { Code = new byte[] { 203, 0 }; return; }                                //RLC B
                    if (Str[1] == "c") { Code = new byte[] { 203, 1 }; return; }                                //RLC C
                    if (Str[1] == "d") { Code = new byte[] { 203, 2 }; return; }                                //RLC D
                    if (Str[1] == "e") { Code = new byte[] { 203, 3 }; return; }                                //RLC E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 6 }; return; }                             //RLC (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 6 }; return; }          //RLC (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 6 }; return; }          //RLC (IY+S)
                }
            }
            if (Str[0] == "rrc")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "a") { Code = new byte[] { 203, 15 }; return; }                               //RRC A
                    if (Str[1] == "h") { Code = new byte[] { 203, 12 }; return; }                               //RRC H
                    if (Str[1] == "l") { Code = new byte[] { 203, 13 }; return; }                               //RRC L
                    if (Str[1] == "b") { Code = new byte[] { 203, 8 }; return; }                                //RRC B
                    if (Str[1] == "c") { Code = new byte[] { 203, 9 }; return; }                                //RRC C
                    if (Str[1] == "d") { Code = new byte[] { 203, 10 }; return; }                               //RRC D
                    if (Str[1] == "e") { Code = new byte[] { 203, 11 }; return; }                               //RRC E
                    if (Str[1] == "(hl)") { Code = new byte[] { 203, 14 }; return; }                            //RRC (HL)
                    if (ReadIX(Str[1], out S)) { Code = new byte[] { 221, 203, (byte)S, 14 }; return; }         //RRC (IX+S)
                    if (ReadIY(Str[1], out S)) { Code = new byte[] { 253, 203, (byte)S, 14 }; return; }         //RRC (IY+S)
                }
            }
            if (Str[0] == "rla") { Code = new byte[] { 23 }; return; }                                          //RLA
            if (Str[0] == "rra") { Code = new byte[] { 31 }; return; }                                          //RRA
            if (Str[0] == "rlca") { Code = new byte[] { 7 }; return; }                                          //RLCA
            if (Str[0] == "rrca") { Code = new byte[] { 15 }; return; }                                         //RRCA
            if (Str[0] == "rld") { Code = new byte[] { 237, 111 }; return; }                                    //RLD
            if (Str[0] == "rrd") { Code = new byte[] { 237, 103 }; return; }                                    //RRD
            #endregion

            #region SET, RES, BIT, LDIR, LDDR, LDI, LDD, CPIR, CPDR, CPI, CPD
            if (Str[0] == "set" | Str[0] == "res" | Str[0] == "bit")
            {
                if (Str.Length == 3)
                {
                    byte first = 0;
                    try { first = Convert.ToByte(Str[1]); } catch { }
                    if (first >= 0 & first <= 7)
                    {
                        if (Str[0] == "set") first = (byte)(192 + first * 8);
                        if (Str[0] == "res") first = (byte)(128 + first * 8);
                        if (Str[0] == "bit") first = (byte)(64 + first * 8);
                        if (Str[2] == "a") { Code = new byte[] { 203, (byte)(first + 7) }; return; }
                        if (Str[2] == "h") { Code = new byte[] { 203, (byte)(first + 4) }; return; }
                        if (Str[2] == "l") { Code = new byte[] { 203, (byte)(first + 5) }; return; }
                        if (Str[2] == "b") { Code = new byte[] { 203, first }; return; }
                        if (Str[2] == "c") { Code = new byte[] { 203, (byte)(first + 1) }; return; }
                        if (Str[2] == "d") { Code = new byte[] { 203, (byte)(first + 2) }; return; }
                        if (Str[2] == "e") { Code = new byte[] { 203, (byte)(first + 3) }; return; }
                        if (Str[2] == "(hl)") { Code = new byte[] { 203, (byte)(first + 6) }; return; }
                        if (ReadIX(Str[2], out S)) { Code = new byte[] { 221, 203, (byte)S, (byte)(first + 6) }; return; }
                        if (ReadIY(Str[2], out S)) { Code = new byte[] { 253, 203, (byte)S, (byte)(first + 6) }; return; }
                    }
                }
            }
            if (Str[0] == "ldir") { Code = new byte[] { 237, 176 }; return; }                                   //LDIR
            if (Str[0] == "lddr") { Code = new byte[] { 237, 184 }; return; }                                   //LDDR
            if (Str[0] == "ldi") { Code = new byte[] { 237, 160 }; return; }                                    //LDI
            if (Str[0] == "ldd") { Code = new byte[] { 237, 168 }; return; }                                    //LDD
            if (Str[0] == "cpir") { Code = new byte[] { 237, 177 }; return; }                                   //CPIR
            if (Str[0] == "cpdr") { Code = new byte[] { 237, 185 }; return; }                                   //CPDR
            if (Str[0] == "cpi") { Code = new byte[] { 237, 161 }; return; }                                    //CPI
            if (Str[0] == "cpd") { Code = new byte[] { 237, 169 }; return; }                                    //CPD
            #endregion

            #region IN, INIR, INDR, INI, IND, OUT, OTIR, ITDR, OUTI, OUTD
            if (Str[0] == "in")
            {
                if (Str.Length == 3)
                {
                    if (Str[2] == "(c)")
                    {
                        if (Str[1] == "a") { Code = new byte[] { 237, 120 }; return; }                          //IN A,(C)
                        if (Str[1] == "h") { Code = new byte[] { 237, 96 }; return; }                           //IN H,(C)
                        if (Str[1] == "l") { Code = new byte[] { 237, 104 }; return; }                          //IN L,(C)
                        if (Str[1] == "b") { Code = new byte[] { 237, 64 }; return; }                           //IN B,(C)
                        if (Str[1] == "c") { Code = new byte[] { 237, 72 }; return; }                           //IN C,(C)
                        if (Str[1] == "d") { Code = new byte[] { 237, 80 }; return; }                           //IN D,(C)
                        if (Str[1] == "e") { Code = new byte[] { 237, 88 }; return; }                           //IN E,(C)
                        if (Str[1] == "f") { Code = new byte[] { 237, 112 }; return; }                          //IN F,(C)
                    }
                    if (Str[1] == "a" && Str[2][0] == '(' & Str[2][Str[2].Length - 1] == ')')
                    {
                        Str[2] = Str[2].Trim('(', ')');
                        if (ReadNum(Str[2], out NN)) { Code = new byte[] { 219, B1(NN) }; return; }             //IN A,(N)
                    }
                }
            }
            if (Str[0] == "inir") { Code = new byte[] { 237, 178 }; return; }                                   //INIR
            if (Str[0] == "indr") { Code = new byte[] { 237, 186 }; return; }                                   //INDR
            if (Str[0] == "ini") { Code = new byte[] { 237, 162 }; return; }                                    //INI
            if (Str[0] == "ind") { Code = new byte[] { 237, 170 }; return; }                                    //IND
            if (Str[0] == "out")
            {
                if (Str.Length == 3)
                {
                    if (Str[1] == "(c)")
                    {
                        if (Str[2] == "a") { Code = new byte[] { 237, 121 }; return; }                          //OUT (C),A
                        if (Str[2] == "h") { Code = new byte[] { 237, 97 }; return; }                           //OUT (C),H
                        if (Str[2] == "l") { Code = new byte[] { 237, 105 }; return; }                          //OUT (C),L
                        if (Str[2] == "b") { Code = new byte[] { 237, 65 }; return; }                           //OUT (C),B
                        if (Str[2] == "c") { Code = new byte[] { 237, 73 }; return; }                           //OUT (C),C
                        if (Str[2] == "d") { Code = new byte[] { 237, 81 }; return; }                           //OUT (C),D
                        if (Str[2] == "e") { Code = new byte[] { 237, 89 }; return; }                           //OUT (C),E
                    }
                    if (Str[2] == "a" && Str[1][0] == '(' & Str[1][Str[1].Length - 1] == ')')
                    {
                        Str[1] = Str[1].Trim('(', ')');
                        if (ReadNum(Str[1], out NN)) { Code = new byte[] { 211, B1(NN) }; return; }             //OUT (N),A
                    }
                }
            }
            if (Str[0] == "otir") { Code = new byte[] { 237, 179 }; return; }                                   //OTIR
            if (Str[0] == "otdr") { Code = new byte[] { 237, 187 }; return; }                                   //OTDR
            if (Str[0] == "outi") { Code = new byte[] { 237, 163 }; return; }                                   //OUTI
            if (Str[0] == "outd") { Code = new byte[] { 237, 171 }; return; }                                   //OUTD

            #endregion

            #region EI, DI, IM, MOP, CLP, NEG, SCF, CCF, HALT, DAA
            if (Str[0] == "ei") { Code = new byte[] { 251 }; return; }                                          //EI
            if (Str[0] == "di") { Code = new byte[] { 243 }; return; }                                          //DI
            if (Str[0] == "im")
            {
                if (Str.Length == 2)
                {
                    if (Str[1] == "0") { Code = new byte[] { 237, 70 }; return; }                               //IM 0
                    if (Str[1] == "1") { Code = new byte[] { 237, 86 }; return; }                               //IM 1
                    if (Str[1] == "2") { Code = new byte[] { 237, 94 }; return; }                               //IM 2
                }
            }
            if (Str[0] == "reti") { Code = new byte[] { 237, 77 }; return; }                                    //RETI
            if (Str[0] == "retn") { Code = new byte[] { 237, 69 }; return; }                                    //RETN
            if (Str[0] == "nop") { Code = new byte[] { 0 }; return; }                                           //NOP
            if (Str[0] == "cpl") { Code = new byte[] { 47 }; return; }                                          //CPL
            if (Str[0] == "neg") { Code = new byte[] { 237, 68 }; return; }                                     //NEG
            if (Str[0] == "scf") { Code = new byte[] { 55 }; return; }                                          //SCF
            if (Str[0] == "ccf") { Code = new byte[] { 63 }; return; }                                          //CCF
            if (Str[0] == "halt") { Code = new byte[] { 118 }; return; }                                        //HALT
            if (Str[0] == "daa") { Code = new byte[] { 39 }; return; }                                          //DAA
            #endregion

            /*if (Str[0] == "exx") { Code = new byte[] { 217 }; return; }                                         //EXX
            if (Str[0] == "ex")
            {
                if (Str.Count() == 3)
                {
                    if (Str[1] == "de" & Str[2] == "hl") { Code = new byte[] { 235 }; return; }                 //EX DE,HL
                    if (Str[1] == "af" & Str[2] == "af'") { Code = new byte[] { 8 }; return; }                  //EX AF,AF'
                    if (Str[1] == "(sp)" & Str[2] == "hl") { Code = new byte[] { 227 }; return; }               //EX (SP),HL
                    if (Str[1] == "(sp)" & Str[2] == "ix") { Code = new byte[] { 221, 227 }; return; }          //EX (SP),IX
                    if (Str[1] == "(sp)" & Str[2] == "iy") { Code = new byte[] { 253, 227 }; return; }          //EX (SP),IY
                }
            }

            //if (Str[0] == "reti") { Code = new byte[] { 239, 77 }; }                                            //RETI*/
            if (Code == null)
                throw new ArgumentException("Не известная команда \"" + Str[0] + "\"");
        }

        /// <summary>
        /// Подсановка реального адреса вместо лейбла
        /// </summary>
        /// <param name="adr"></param>
        public void SetAdress(int adr)
        {
            //if (To + 2 > Code.Count()) throw new ArgumentException("Команда не поддерживает 16-и битную адрессацию");
            if (Rel)
            {
                int jr = adr - Adress - 2;
                if (jr >= 0)
                {
                    if (jr > 127) throw new AccessViolationException("Относительный переход не не может быть таким большим");
                    Code[To] = (byte)jr;
                }
                else
                {
                    if (jr < -128) throw new AccessViolationException("Относительный переход не не может быть таким большим");
                    Code[To] = (byte)(256 + jr);
                }
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
