using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace hackinggame
{
    class DefaultCommands
    {
        [DefaultCommand("echo", "Prints a string")]
        public static void Echo(string args, Terminal Context)
        {
            Context.SendOut(args);
        }

        [DefaultCommand("clear", "Clears the console")]
        public static void Clear(string args, Terminal Context)
        {
            Context.Strings.Clear();
        }

        [DefaultCommand("shutdown", "Exits the game")]
        public static void Shutdown(string args, Terminal Context)
        {
            Context.Context.Exit();
        }

        [DefaultCommand("help", "Gives information on all commands")]
        public static void Help(string args, Terminal Context)
        {
            var Type = typeof(DefaultCommands);
            MethodInfo[] Methods = Type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo Method in Methods)
                try
                {
                    var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is DefaultCommand) as DefaultCommand;
                    Context.SendOut(SomeAttrib.name + " - " + SomeAttrib.description);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString());}
        }
    }
}
