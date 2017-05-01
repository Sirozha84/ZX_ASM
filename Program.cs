using System;
using System.Windows.Forms;

namespace ZXASM
{
    static class Program
    {
        public const string Version = "0.1 Beta (01.05.2017)";
        public const string UrlS = "http://www.sg-software.ru/";
        public const string UrlP = "http://www.sg-software.ru/windows/programs/zx-asm";
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
