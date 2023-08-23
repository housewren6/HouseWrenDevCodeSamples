using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HouseWrenDevCodeSamples
{
	internal class ParsingXML
	{
		public string GetXml (string xml)
		{
			//FIND ITEM IDs to use as Parameters for SQL stored procedure
			XmlDocument XmlDoc = new XmlDocument();
			XmlDoc.Load (xml);
			XmlNode node1ID = XmlDoc.SelectSingleNode("/Links/Link[@TypeID=1]/@ID");
			XmlNode node2ID = XmlDoc.SelectSingleNode("/Links/Link[@TypeID=2]/@ID");
			int item1ID;
			int item2ID;
			try 
			{
				item1ID = int.Parse(node1ID.InnerText);
			}
			catch { return "Unable to get item 1 ID"; }
			if (node2ID != null)
			{
				try
				{
					item2ID = int.Parse(node2ID.InnerText);
				}
				catch { return "Unable to get item 2 ID"; }
			}
			//call dataAdapter.SelectCommand.Parameters to populate SQL stored procedure parameters

			//to not cause an error for code snippet 
			return xml;
		}


	}
}
