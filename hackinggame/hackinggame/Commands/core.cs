using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace hackinggame
{
	[CommandClass()]
    class core
    {
        [Command("echo", "Prints a string", "core", "default")]
        public static void Echo(string args, Terminal Context)
        {
            Context.SendOut(args);
        }

        [Command("clear", "Clears the console", "core", "default")]
        public static void Clear(string args, Terminal Context)
        {
            Context.CurrentY = 32;
            Context.ValuesChecked = 0;
            Context.Strings.Clear();
        }

		[Command("mkdir", "Creates a directory.", "core", "default")]
		public static void MakeDir(string args, Terminal Context)
		{
			SaveManager.MakeFolder(SaveManager.CurrentDir + "/" + args);
		}

		[Command("ls", "Lists all the files/directories in the current directory", "core", "default")]
		public static void List(string args, Terminal Context)
		{
			Context.SendOut(SaveManager.GetStuffInCurr());
		}

		[Command("cd", "Changes directory.", "core", "default")]
		public static void ChangeDir(string args, Terminal Context)
		{
			if (args == "..")
			{
				if (SaveManager.CurrentDir == "")
				{
					Context.SendOut("ZDPS Permission Err: The system administrator has blocked you from leaving your allocated home directory.");
					return;
				}
				List<string> DirSploit = new List<string>();
				DirSploit.AddRange(SaveManager.CurrentDir.Split('/'));
				SaveManager.CurrentDir = "/";
				if (DirSploit.Count == 2 || DirSploit.Count == 0 || DirSploit.Count == 1)
				{
					SaveManager.CurrentDir = "";
				}
				else
				{
					SaveManager.CurrentDir = "";
					DirSploit.RemoveAt(DirSploit.Count - 1);
					foreach (string Dir in DirSploit)
						SaveManager.CurrentDir += Dir + "/";
					SaveManager.CurrentDir = SaveManager.CurrentDir.Substring(0, SaveManager.CurrentDir.Length - 1);
				}
				return;
			}
			if (args == "./")
				return;
			if (System.IO.Directory.Exists(SaveManager.FileSavePath + SaveManager.CurrentDir + "/" + args))
			{
				SaveManager.CurrentDir += "/" + args;
				return;
			}
			Context.SendOut("Invalid directory");
		}

		[Command("cat", "Reads text from a file.", "core", "default")]
		public static void GetText(string args, Terminal Context)
		{
			Context.SendOut(SaveManager.GetContents(SaveManager.CurrentDir + "/" + args));
		}

		[Command("shutdown", "Exits the game", "core", "default")]
        public static void Shutdown(string args, Terminal Context)
        {
            Context.Context.Exit();
        }

        [Command("help", "Gives information on all commands", "core", "default")]
        public static void Help(string args, Terminal Context)
        {
			foreach(string StringToOut in CLICommon.GetHelp("default"))
			{
				Context.SendOut(StringToOut);
			}
        }

		[Command("zpm", "The Zero Package Manager", "core", "default")]
		public static void PackageManagerCMD(string args, Terminal Context)
		{
			string[] Arguments = args.Split(' ');
			int RetVal = 0;
			if (Arguments.Length == 0 | Arguments.Length == 1)
			{
				Context.SendOut("Missing arguments.");
				return;
			}
			if (Arguments[0] == "remove")
			{
				RetVal = PackageManager.RemovePKG(Arguments[1]);
				if (RetVal == 1)
					Context.SendOut("Package not installed.");
				if (RetVal == 0)
					Context.SendOut("Package removed.");
			}
			if (Arguments[0] == "install")
			{
				RetVal = PackageManager.InstallPKG(Arguments[1], PackageManager.DebugMode);
				if (RetVal == 1)
					Context.SendOut("Package not available.");
				if (RetVal == 2)
					Context.SendOut("Package already installed.");
				if (RetVal == 0)
					Context.SendOut("Package installed.");
			}
		}
	}
}
