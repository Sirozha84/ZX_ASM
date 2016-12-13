using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    static class Compiler
    {
        static int Adress;
        static List<byte> codes = new List<byte>();

        public static void Compile(string Text, string FileName, int Out)
        {
            Adress = 25000;
            Label.List.Clear();
            Token.List.Clear();

            Token.CurAdress = Adress;
            Parsing(Text);
            //if (Out == 0)...             //Просто бинарник
            if (Out == 1) SaveSNA(FileName);    //Сохранение снэпшота
            //if (Out == 2) SaveSNA();... //И, видимо, открытие этого снэпшота
        }

        static void SaveSNA(string FileName)
        {
            byte[] SNA = new byte[49179];
            for (int i = 0; i < codes.Count; i++) SNA[Adress + i + 27 - 16384] = codes[i];
            SNA[23] = 253;
            SNA[24] = 255;
            SNA[49177] = (byte)(Adress / 256);
            SNA[49178] = (byte)(Adress % 256);
            System.IO.File.WriteAllBytes(FileName + ".sna", SNA);
        }

        static void Parsing(string Text)
        {
            string[] Strings = Text.Replace("\r\n", "\n").Split(new[] { '\r', '\n' });
            //Первый прогон
            int num = 0;
            bool isOK = true;
            Error.Clear();
            foreach (string str in Strings)
            {
                num++;
                Label label = new Label(str);
                try
                {
                    Token token = new Token(str);
                    //if (token.IsComand) Token.List.Add(token);
                }
                catch (Exception e)
                {
                    isOK = false;
                    Error.List.Add(new Error("this", num, e.Message));
                }
            }

#if DEBUG
            Console.WriteLine("Компиляция n" + Properties.Settings.Default.Runs + ":");
#endif

            codes.Clear();

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
                Console.Write(t.Adress + " - ");
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

            //Результат компиляции
            if (isOK) System.Windows.Forms.MessageBox.Show("Компиляция прошла успешно!", "Компиляция");
            else
            {
                string str = "Компиляция завершилась ошибками:\n";
                foreach (Error er in Error.List)
                {
                    str += er.StringNum.ToString("0:   " + er.Message + "\n");
                }
                System.Windows.Forms.MessageBox.Show(str, "Компиляция");
            }
        }
    }
}

