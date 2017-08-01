using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace hackinggame
{
    class DefaultShell : IShell
    {
        List<string> History = new List<string>();
        public int Index { get; set; }
        public void ParseIn(string Command, Terminal Context)
        {
            History.Add(Command);
            string[] Strings = Command.Split(' ');
            string Args = "";
            bool IsNF = true;
            try
            {
                Args = Command.Substring(Strings[0].Length + 1);
            }
            catch { }
            var Type = typeof(DefaultCommands);
            MethodInfo[] Methods = Type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo Method in Methods)
            {
                try
                {
                    var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is DefaultCommand) as DefaultCommand;
                    if (SomeAttrib != null)
                    {
                        if (SomeAttrib.name == Strings[0])
                        {
                            Method.Invoke(this, new object[] { Args, Context });
                            IsNF = false;
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); Context.SendOut("Command not found"); }
            }
            if (IsNF)
                Context.SendOut("Command not found");
        }

        public string[] GetTabCompletes(string ToCheck)
        {
            List<string> Completes = new List<string>();
            var Type = typeof(DefaultCommands);
            MethodInfo[] Methods = Type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo Method in Methods)
            {
                try
                {
                    var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is DefaultCommand) as DefaultCommand;
                    if (SomeAttrib != null && SomeAttrib.name.StartsWith(ToCheck))
                    {
                        Completes.Add(SomeAttrib.name);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            return Completes.ToArray();
        }
        public string GetFromIndex(Terminal Context)
        {
            if (Index < 0)
                Index = 0;
            if (Index > History.Count - 1)
                Index = History.Count - 1;
            if (Index == 0)
                return GetPrompt() + "";
            if (History.Count != 0)
                return GetPrompt() + History[History.Count - Index];
            return GetPrompt() + "";
        }

        public string GetPrompt()
        {
            return "Memes~$ ";
        }
    }
}
