using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace hackinggame
{
	static class PackageManager
	{
		public static List<String> InstalledPKGs = new List<string>();
		public static List<String> UnlockedPKGs = new List<string>();
		public static List<String> ExistingPKGs = new List<string>();
		public static bool DebugMode = true;

		public static int InstallPKG(string PKG, bool IgnoreUnlocks)
		{
			if (!UnlockedPKGs.Contains(PKG) && !IgnoreUnlocks)
				return 1;
			if (!ExistingPKGs.Contains(PKG))
				return 1;
			if (InstalledPKGs.Contains(PKG))
				return 2;
			InstalledPKGs.Add(PKG);
			SaveManager.Save();
			return 0;
		}

		public static int RemovePKG(string PKG)
		{
			if (!InstalledPKGs.Contains(PKG))
				return 1;
			InstalledPKGs.Remove(PKG);
			SaveManager.Save();
			return 0;
		}

		public static void SetupPKGSys()
		{
			if (!InstalledPKGs.Contains("core"))
				InstalledPKGs.Add("core");
			if (!UnlockedPKGs.Contains("core"))
			{
				UnlockedPKGs.Add("core");
				UnlockedPKGs.Add("pwntl");
			}
			SetupUsedPackages();
		}

		static void SetupUsedPackages()
		{
			foreach (MethodInfo Method in AssemblyManager.CommandFunctions)
			{
				try
				{
					var SomeAttrib = Method.GetCustomAttributes(false).FirstOrDefault(x => x is Command) as Command;
					if (SomeAttrib != null && !ExistingPKGs.Contains(SomeAttrib.package))
					{
						ExistingPKGs.Add(SomeAttrib.package);
					}
				}
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			}
		}
	}
}
