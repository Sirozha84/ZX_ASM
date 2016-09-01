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

#if DEBUG
            Console.WriteLine("Компиляция n" + Properties.Settings.Default.Runs +":");
#endif
            List<byte> codes = new List<byte>();
            
            //Второй прогон
            foreach (Token t in Token.List)
            {
                if (t.Label != null)
                {
                    Label l = Label.List.Find(o => o.Name == t.Label);
                    if (l != null) t.SetAdress(l.Adress);
                }
                foreach (byte b in t.Code) codes.Add(b);
#if DEBUG
                foreach (byte b in t.Code) Console.Write(b + " ");
                Console.Write(t.Label);
                Console.WriteLine();
#endif
            }
#if DEBUG

            Console.WriteLine("Бинарник:");
            foreach (byte b in codes) Console.Write(b + " ");
            Console.WriteLine();
            Console.WriteLine("Размер бинарного кода: " + codes.Count + " байт.");
            Console.WriteLine();
#endif
            Properties.Settings.Default.Runs++; //Счётчик запусков, так, по приколу
            Properties.Settings.Default.Bytes += codes.Count; //И счётчик кода всего, обожаю статистику :-)
        }
    }
}
