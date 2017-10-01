using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackinggame
{
	class SaveData
	{
		public List<String> InstalledPKGs = new List<string>();
		public List<String> UnlockedPKGs = new List<string>();
		public bool DebugMode = true;
	}
}
