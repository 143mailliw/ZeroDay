using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hackinggame.Widgets
{
    static class FileManager
    {
        static string JSONFileFolder{ get; set; } = Environment.SpecialFolder.ApplicationData + "/ZeroDay/Files";
        static string JSONSaveFiles { get; set; } = Environment.SpecialFolder.ApplicationData + "/ZeroDay" + "files.json";
        static string JSONSaveOther { get; set; } = Environment.SpecialFolder.ApplicationData + "/ZeroDay" + "save.json";
        static public void SetupFM()
        {
            if (!System.IO.Directory.Exists(Environment.SpecialFolder.ApplicationData + "/ZeroDay"))
            {
                System.IO.Directory.CreateDirectory(Environment.SpecialFolder.ApplicationData + "/ZeroDay");
                System.IO.Directory.CreateDirectory(Environment.SpecialFolder.ApplicationData + "/ZeroDay/Files");
            }
        }
        static public void SaveJsonFile(File FileToSave)
        {
            JsonConvert.SerializeObject(FileToSave, Formatting.Indented);
        }
    }
    
    public class FileData
    {
        string FileName { get; set; }
        FileFlags Flags { get; set; }
    }

    public class File
    {
        string FileName { get; set; }
        byte[] FileContents { get; set; }

        public void SaveJson(string filename)
        {
            System.IO.File.WriteAllText(filename + "", ToJson());
        }

        public static File Load()
        {
            return JsonConvert.DeserializeObject<File>(System.IO.File.ReadAllText(JSONFile));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
