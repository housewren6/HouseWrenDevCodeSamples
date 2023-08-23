using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HouseWrenDevCodeSamples
{
	internal class GetRegistrySubkeyValue
	{
		//Get value at registry subkey to use in Sql Stored procedure 
		public string GetXml (string xml) { 
			string regPath = "Path\\To\\SQLServerString\\Value";
			string subkeyValue;
				try
					{
						RegistryKey key = Registry.LocalMachine.OpenSubKey (regPath);
					if (key != null)
					{
						subkeyValue = key.GetValue("subkeyName").ToString();
						key.Close();
					}
					else return ("Key value is null or not visible");
					}
				catch (XmlException e) { return e.ToString(); }

			//to not cause an error for code snippet 
			return xml;
		}
	}
}
