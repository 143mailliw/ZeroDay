using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace hackinggame
{
	static class CLICommon
	{

		public static string[] GetHelp(string Context)
		{
			List<string> ToOut = new List<string>();
			List<string> ToErr = new List<string>();
			ToErr.Add("Help returned failure. See console for details.");
			foreach (MethodInfo Method in AssemblyManager.CommandFunctions)
				try
				{
					var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
					if (SomeAttrib.context == Context && PackageManager.InstalledPKGs.Contains(SomeAttrib.package))
						ToOut.Add(SomeAttrib.name + " - " + SomeAttrib.description);
				}
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			return ToOut.ToArray();
		}

		public static string[] GetAutoCompletes(string Context, string ToCheck)
		{
			List<string> Completes = new List<string>();
			foreach (MethodInfo Method in AssemblyManager.CommandFunctions)
			{
				try
				{
					var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
					if (SomeAttrib != null && SomeAttrib.name.StartsWith(ToCheck) && PackageManager.InstalledPKGs.Contains(SomeAttrib.package) && SomeAttrib.context == Context)
					{
						Completes.Add(SomeAttrib.name);
					}
				}
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			}
			return Completes.ToArray();
		}

		public static void RunCommand(string CMDContext, string Command, object OBJContext, Terminal Context)
		{
			string[] Strings = Command.Split(' ');
			string Args = "";
			bool IsNF = true;
			try
			{
				Args = Command.Substring(Strings[0].Length + 1);
			}
			catch { }
			foreach (MethodInfo Method in AssemblyManager.CommandFunctions)
			{
				try
				{
					var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
					if (SomeAttrib != null)
					{
						if (SomeAttrib.name == Strings[0] && SomeAttrib.context == CMDContext && PackageManager.InstalledPKGs.Contains(SomeAttrib.package))
						{
							Method.Invoke(OBJContext, new object[] { Args, Context });
							IsNF = false;
							return;
						}
					}
				}
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			}
			if (IsNF)
				Context.SendOut("Command not found");
		}
	}
}
