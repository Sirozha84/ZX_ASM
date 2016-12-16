using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    static class Compiler
    {
        public static int StartAdress;
        static List<byte> codes = new List<byte>();

        public static void Compile(string Text, string FileName, int Out)
        {

#if DEBUG
            Console.WriteLine("Компиляция n" + Properties.Settings.Default.Runs + ":");
#endif

            StartAdress = 25000;
            Token.CurAdress = StartAdress;
            Label.List.Clear();
            Token.List.Clear();
            Modules.List.Clear();

            Parsing(Text);
#if DEBUG
            Console.WriteLine("Бинарник (" + codes.Count + " байт):");
            foreach (byte b in codes) Console.Write(b + " ");
            Console.WriteLine();
#endif

            if (Error.List.Count == 0)
            {

#if DEBUG
                Console.WriteLine("Компиляция прошла успешно!");
#endif

            }
            else
            {
                Console.WriteLine("Компиляция завершилась ошибками:");
                foreach (Error er in Error.List)
                    Console.WriteLine(er.StringNum + ": " + er.String + "\n          " + er.Message);
            }

            //Далее делаем что-то с получившимся бинарником
            //if (Out == 0)...             //Просто бинарник
            if (Out == 1) SaveSNA(FileName);    //Сохранение снэпшота
            //if (Out == 2) SaveSNA();... //И, видимо, открытие этого снэпшота

            Properties.Settings.Default.Runs++; //Счётчик запусков, так, по приколу
            Properties.Settings.Default.Bytes += codes.Count; //И счётчик кода всего, обожаю статистику :-)
        }

        static void SaveSNA(string FileName)
        {
            byte[] SNA = new byte[49179];
            for (int i = 0; i < codes.Count; i++) SNA[StartAdress + i + 27 - 16384] = codes[i];
            SNA[23] = 253;
            SNA[24] = 255;
            SNA[49177] = (byte)(StartAdress / 256);
            SNA[49178] = (byte)(StartAdress % 256);
            System.IO.File.WriteAllBytes(FileName + ".sna", SNA);
        }

        static public void Parsing(string Text)
        {
            //Разбивка текста на строки
            string[] Strings = Text.Replace("\r\n", "\n").Split(new[] { '\r', '\n' });
            //Первый прогон (разбивка текста программы на токены)
            int num = 0;
            Error.Clear();
            foreach (string str in Strings)
            {
                num++;
                try
                {
                    Label label = new Label(str);
                    Token token = new Token(num, str);
                    //if (token.IsComand) Token.List.Add(token);

#if DEBUG
                    if (token.Code != null)
                    {
                        Console.Write(token.Adress + " - ");
                        foreach (byte b in token.Code) Console.Write(b + " ");
                        if (token.Label != null) Console.Write("   Label: " + token.Label);
                        Console.WriteLine();
                    }
#endif

                }
                catch (Exception e)
                {
                    Error.List.Add(new Error("this", num, str, e.Message));
                }
            }
            //Второй прогон (подстановка реальных адресов вместо меток)
            codes.Clear();
            foreach (Token t in Token.List)
            {
                if (t.Label != null)
                {
                    try
                    {
                        Label l = Label.List.Find(o => o.Name == t.Label);
                        if (l != null) t.SetAdress(l.Adress);
                        else throw new ArgumentException("Метка \"" + t.Label + "\" не найдена");
                    }
                    catch (Exception e)
                    {
                        Error.List.Add(new Error("this", t.NumString, t.String, e.Message));
                    }
                }
                foreach (byte b in t.Code) codes.Add(b);
            }
            //Добавление внешних модулей
            foreach (Modules module in Modules.List)
                module.Include();
        }
    }
}
