using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools
{
    public static class ExceptionExtension
    {

        public static string GetInnerStackTraces(this Exception? e)
        {
            if (e == null) return "";
            return e.StackTrace + "\n" + e.InnerException.GetInnerStackTraces();
        }
        public static string GetInnerMessages(this Exception? e)
        {
            if (e == null) return "";
            return e.Message + "\n" + e.InnerException.GetInnerMessages();
        }
    }
}
