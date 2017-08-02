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
            Context.CurrentY = 32;
            Context.ValuesChecked = 0;
            Context.Strings.Clear();
        }

		[DefaultCommand("mkdir", "Creates a directory.")]
		public static void MakeDir(string args, Terminal Context)
		{
			SaveManager.MakeFolder(SaveManager.CurrentDir + "/" + args);
		}

		[DefaultCommand("ls", "Lists all the files/directories in the current directory")]
		public static void List(string args, Terminal Context)
		{
			Context.SendOut(SaveManager.GetStuffInCurr());
		}

		[DefaultCommand("cd", "Changes directory.")]
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

		[DefaultCommand("cat", "Reads text from a file.")]
		public static void GetText(string args, Terminal Context)
		{
			Context.SendOut(SaveManager.GetContents(SaveManager.CurrentDir + "/" + args));
		}

		[DefaultCommand("shutdown", "Exits the game")]
        public static void Shutdown(string args, Terminal Context)
        {
            Context.Context.Exit();
        }

        [DefaultCommand("metasploit", "Temp, opens hacking shell")]
        public static void Metasploit(string args, Terminal Context)
        {
            Context.ShellToUse = new HackingShell();
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
