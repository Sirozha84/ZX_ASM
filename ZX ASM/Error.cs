using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXASM
{
    class Error
    {
        public static List<Error> List;

        public string File;
        public int StringNum;
        public string String;
        public string Message;

        public Error(string File, int StringNum, string String, string Message)
        {
            this.File = File;
            this.StringNum = StringNum;
            this.String = String;
            this.Message = Message;
        }

        public static void Clear()
        {
            List = new List<Error>();
        }
    }
}
