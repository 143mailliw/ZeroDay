using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace hackinggame
{
    class HackingShell : IShell
    {
        List<string> History = new List<string>();
        public int Index { get; set; }
		public void ParseIn(string Command, Terminal Context)
		{
			History.Add(Command);
			CLICommon.RunCommand("pwntl", Command, this, Context);
		}

		public string[] GetTabCompletes(string ToCheck)
		{
			return CLICommon.GetAutoCompletes("pwntl", ToCheck);
		}

		public string GetFromIndex(Terminal Context)
        {
            if (Index < 0)
                Index = 0;
            if (Index > History.Count - 1)
                Index = History.Count - 1;
            if (Index == 0)
                return GetPrompt(Context) + "";
            if (History.Count != 0)
                return GetPrompt(Context) + History[History.Count - Index];
            return GetPrompt(Context) + "";
        }

        public string GetPrompt(Terminal Context)
        {
            return "pwntl> ";
        }
    }
}
