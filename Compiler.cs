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
            Module.List.Clear();
            Error.Clear();

            Parsing("*", Text);

            InsertAdress();



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
                FormErrors form = new FormErrors();
                form.ShowDialog();
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

        /// <summary>
        /// Парсинг текста
        /// </summary>
        /// <param name="module"></param>
        /// <param name="Text"></param>
        static public void Parsing(string module, string Text)
        {
            //Разбивка текста на строки
            string[] Strings = Text.Replace("\r\n", "\n").Split(new[] { '\r', '\n' });
            int num = 0;
            foreach (string str in Strings)
            {
                num++;
                try
                {
                    Label label = new Label(module, str);
                    Token token = new Token(module, num, str);
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
                    Error.List.Add(new Error(module, num, str, e.Message));
                }
            }
            //Добавление внешних модулей
            foreach (Module mod in Module.List)
                mod.Include();
        }

        /// <summary>
        /// Подстановка адресов вместо меток
        /// </summary>
        static void InsertAdress()
        {
            codes.Clear();
            foreach (Token t in Token.List)
            {
                if (t.Label != null)
                {
                    try
                    {
                        if (!t.Label.Contains(".")) t.Label = t.ModuleString + "." + t.Label;
                        //Здесь надо проверить есть ли точка, и если нет - подставить с названием текущего модуля
                        Label l = Label.List.Find(o => o.Name == t.Label);
                        if (l != null) t.SetAdress(l.Adress);
                        else
                        {
                            string lb = t.Label;
                            if (lb.Contains("*.")) lb = lb.Replace("*.","");
                            throw new ArgumentException("Метка \"" + lb + "\" не найдена");
                        }
                    }
                    catch (Exception e)
                    {
                        Error.List.Add(new Error(t.ModuleString, t.NumString, t.String, e.Message));
                    }
                }
                foreach (byte b in t.Code) codes.Add(b);
            }
        }
    }
}
