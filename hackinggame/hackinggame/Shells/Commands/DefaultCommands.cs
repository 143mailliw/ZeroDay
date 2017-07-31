using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackinggame
{
    class DefaultCommands
    {
        [DefaultCommand("echo")]
        public static void Echo(string args, Terminal Context)
        {
            Context.SendOut(args);
        }

        [DefaultCommand("clear")]
        public static void Clear(string args, Terminal Context)
        {
            Context.Strings.Clear();
        }

        [DefaultCommand("shutdown")]
        public static void Shutdown(string args, Terminal Context)
        {
            Context.Context.Exit();
        }
    }
}
