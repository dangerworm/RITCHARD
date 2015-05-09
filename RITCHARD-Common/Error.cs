using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RITCHARD_Common
{
    public class Error
    {
        private string _message;
        private string _stackTrace;

        public Error(Exception e)
        {
            _message = e.Message;
            _stackTrace = e.StackTrace;
        }

        public Error(string message, string stackTrace)
        {
            _message = message;
            _stackTrace = stackTrace;
        }
    }
}
