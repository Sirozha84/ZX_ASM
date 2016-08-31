using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    static class Compiler
    {

        public static void Compile(string Text)
        {
            Label.List.Clear();
            Token.List.Clear();

            Token.Adress = 25000;
            Parsing(Text);
        }

        static void Parsing(string Text)
        {
            string[] Strings = Text.Replace("\r\n", "\n").Split(new[] { '\r', '\n' });
            //Первый прогон
            foreach (string str in Strings)
            {
                Label label = new Label(str);
                Token token = new Token(str);
                //if (token.IsComand) Token.List.Add(token);
            }

            foreach (Token t in Token.List)
            {
                foreach (byte b in t.Code) Console.Write(b + " ");
                Console.Write(t.Label);
                Console.WriteLine();
            }

        }
    }
}
