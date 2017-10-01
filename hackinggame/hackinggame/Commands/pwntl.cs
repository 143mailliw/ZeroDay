using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace hackinggame
{
	[CommandClass]
    class pwntl
    {
		[Command("pwntl", "Temp, opens hacking shell", "pwntl", "default")]
		public static void Metasploit(string args, Terminal Context)
		{
			Context.ShellToUse = new HackingShell();
		}

		[Command("exit", "Exits this shell", "pwntl", "pwntl")]
        public static void Exit(string args, Terminal Context)
        {
            Context.ShellToUse = new DefaultShell();
        }

        [Command("help", "List of commands", "pwntl", "pwntl")]
        public static void Help(string args, Terminal Context)
        {
			foreach (string StringToOut in CLICommon.GetHelp("pwntl"))
			{
				Context.SendOut(StringToOut);
			}
		}
    }
}
