using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackinggame
{
    class DefaultShell : Shell
    {
        public void ParseIn(string Command, Terminal Context)
        {
            Context.SendOut(Command);
            Context.SendOut(GetPrompt());
        }
        public string GetPrompt()
        {
            return "Memes~$";
        }
    }
}
