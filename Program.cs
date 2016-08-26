using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ZXASM
{
    static class Program
    {
        public const string Site = "www.sg-software.ru";
        public const string Url = "http://www.sg-software.ru/";
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
