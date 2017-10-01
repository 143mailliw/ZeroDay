using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hackinggame
{
	static class SaveManager
	{
		public static string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ZeroDay/";
		public static string FileSavePath = BasePath + "Files/";
		public static string SavePath = BasePath + "save.json";
		public static string CurrentDir = "";
		public static SaveData Data;

		public static void SetupSaveSys()
		{
			EnsureSetup();
		}

		public static void EnsureSetup()
		{
			if (!Directory.Exists(BasePath))
				Directory.CreateDirectory(BasePath);
			if (Directory.Exists(FileSavePath))
			{
				Directory.CreateDirectory(FileSavePath);
				WriteFile(Encoding.ASCII.GetBytes("Memes"), "info.txt");
			}
			if (!File.Exists(SavePath))
				Data = new SaveData();
			else
				Load(SavePath);
		}

		public static void Save()
		{
			Data.InstalledPKGs = PackageManager.InstalledPKGs;
			Data.UnlockedPKGs = PackageManager.UnlockedPKGs;
			Data.DebugMode = PackageManager.DebugMode;
			File.WriteAllText(SavePath, JsonConvert.SerializeObject(Data));
		}

		public static void Load(string Path)
		{
			Data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(Path));
			PackageManager.InstalledPKGs = Data.InstalledPKGs;
			PackageManager.UnlockedPKGs = Data.UnlockedPKGs;
			PackageManager.DebugMode = Data.DebugMode;
		}

		public static string GetStuffInCurr()
		{
			string[] Files;
			string[] Folders;
			string FinishedResult = "";
			Files = Directory.GetFiles(FileSavePath + CurrentDir);
			Folders = Directory.GetDirectories(FileSavePath + CurrentDir);
			foreach (string Folder in Folders)
			{
				string Temp = Folder.Split('/')[Folder.Split('/').Length - 1];
				FinishedResult += "[" + Temp.Split('\\')[Temp.Split('\\').Length - 1] + "] ";
			}
			foreach (string File in Files)
			{
				string Temp = File.Split('/')[File.Split('/').Length - 1];
				FinishedResult += Temp.Split('\\')[Temp.Split('\\').Length - 1] + " ";
			}
			return FinishedResult;
		}

		public static void MakeFolder(string Path)
		{
			Directory.CreateDirectory(FileSavePath + Path);
		}

		public static string GetContents(string Path)
		{
			if(File.Exists(FileSavePath + Path))
			{
				try { return File.ReadAllText(FileSavePath + Path); }
				catch { return "Not a text file."; }
			}
			return "That file doesn't exist!";
		}

		public static void WriteFile(byte[] FileToWrite, string Path)
		{
			File.WriteAllBytes(FileSavePath + Path, FileToWrite);
		}
	}
}
