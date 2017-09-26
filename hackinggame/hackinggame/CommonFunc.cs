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
		public static IEnumerable<Type> GetAllCommandClasses(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetCustomAttributes(typeof(CommandClass), true).Length > 0)
				{
					yield return type;
				}
			}
		}

		public static string[] GetHelp(string Context)
		{
			List<string> ToOut = new List<string>();
			List<string> ToErr = new List<string>();
			IEnumerable<Type> Classes = GetAllCommandClasses(Assembly.GetCallingAssembly()); //kill me if this happens and it's not because GetAllCommandClasses' code is fucked, because then microsoft derped .net
			ToErr.Add("Help returned failure. See console for details.");
			foreach (Type TypeToHandle in Classes)
			{
				MethodInfo[] Methods = TypeToHandle.GetMethods(BindingFlags.Public | BindingFlags.Static);
				foreach (MethodInfo Method in Methods)
					try
					{
						var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
						if (SomeAttrib.context == Context)
							ToOut.Add(SomeAttrib.name + " - " + SomeAttrib.description);
					}
					catch (Exception ex) { Console.WriteLine(ex.ToString()); return ToErr.ToArray(); }
			}
			return ToOut.ToArray();
		}

		public static string[] GetAutoCompletes(string Context, string ToCheck)
		{
			List<string> Completes = new List<string>();
			IEnumerable<Type> Classes = GetAllCommandClasses(Assembly.GetCallingAssembly());
			foreach (Type TypeToHandle in Classes)
			{
				MethodInfo[] Methods = TypeToHandle.GetMethods(BindingFlags.Public | BindingFlags.Static);
				foreach (MethodInfo Method in Methods)
				{
					try
					{
						var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
						if (SomeAttrib != null && SomeAttrib.name.StartsWith(ToCheck))
						{
							Completes.Add(SomeAttrib.name);
						}
					}
					catch (Exception ex) { Console.WriteLine(ex.ToString()); }
				}
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
			IEnumerable<Type> Classes = GetAllCommandClasses(Assembly.GetCallingAssembly());
			foreach (Type TypeToHandle in Classes)
			{
				MethodInfo[] Methods = TypeToHandle.GetMethods(BindingFlags.Public | BindingFlags.Static);
				foreach (MethodInfo Method in Methods)
				{
					try
					{
						var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
						if (SomeAttrib != null)
						{
							if (SomeAttrib.name == Strings[0] && SomeAttrib.context == CMDContext)
							{
								Method.Invoke(OBJContext, new object[] { Args, Context });
								IsNF = false;
								return;
							}
						}
					}
					catch (Exception ex) { Console.WriteLine(ex.ToString()); Context.SendOut("Command not found"); }
				}
			}
			if (IsNF)
				Context.SendOut("Command not found");
		}
	}
}
