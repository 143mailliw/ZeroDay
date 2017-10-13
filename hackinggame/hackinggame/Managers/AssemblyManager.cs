using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace hackinggame
{
	static class AssemblyManager
	{
		static List<Assembly> AssembliesToScan = new List<Assembly>();
		public static List<MethodInfo> CommandFunctions = new List<MethodInfo>();

		public static void SetupAssemblyManager(bool EnableModAPI)
		{
			AssembliesToScan.Add(Assembly.GetCallingAssembly());
			if (EnableModAPI)
				LoadMods();
			GrabCommands();
		}

		static void LoadMods()
		{
			List<string> ModPaths = new List<string>();
			foreach (string File in Directory.EnumerateFiles(SaveManager.ModLoadPath))
			{
				if (!File.EndsWith(".mod.dll") && File.EndsWith(".dll"))
					Assembly.LoadFile(File);
				else if (File.EndsWith(".mod.dll"))
					AssembliesToScan.Add(Assembly.LoadFile(File));
			}
		}

		static void GrabCommands()
		{
			List<Type> TypesToScan = new List<System.Type>();
			foreach (Assembly AssemblyToGet in AssembliesToScan)
				TypesToScan.AddRange(GetAllCommandClasses(AssemblyToGet));
			foreach (Type TypeToHandle in TypesToScan)
			{
				MethodInfo[] Methods = TypeToHandle.GetMethods(BindingFlags.Public | BindingFlags.Static);
				foreach (MethodInfo Method in Methods)
					try
					{
						var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
						if (SomeAttrib != null)
							CommandFunctions.Add(Method);
					}
					catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
			}
		}

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
	}
}
