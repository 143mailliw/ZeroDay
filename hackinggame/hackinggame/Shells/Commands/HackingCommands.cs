using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace hackinggame
{
	[CommandClass]
    class HackingCommands
    {
        [Command("exit", "Exits this shell", "pwntl", "pwntl")]
        public static void Exit(string args, Terminal Context)
        {
            Context.ShellToUse = new DefaultShell();
        }

        [Command("help", "List of commands", "pwntl", "pwntl")]
        public static void Help(string args, Terminal Context)
        {
            var Type = typeof(HackingCommands);
            MethodInfo[] Methods = Type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo Method in Methods)
                try
                {
                    var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
                    Context.SendOut(SomeAttrib.name + " - " + SomeAttrib.description);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString());}
        }
    }
}
