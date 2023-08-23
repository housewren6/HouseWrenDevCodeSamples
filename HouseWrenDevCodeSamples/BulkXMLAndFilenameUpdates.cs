using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HouseWrenDevCodeSamples
{
	internal class BulkXMLAndFilenameUpdates
	{
		//Loop through all files in a directory and if filenames match set patterns,
		//alter XML nodes' values to reflect pattern updates
		static void Main(string[] args)
		{
			//user inputs path of files to change names
			//all file names to change should be in one base directory, only xml files name will be changed
			Console.WriteLine("Input base directory");
			string inputDirectory = Console.ReadLine();
			Console.WriteLine("Input directory: " + inputDirectory);
			Console.WriteLine("Input project type name");
			string inputProjectName = Console.ReadLine();
			Console.WriteLine("Input project name: " + inputProjectName);

			string filename;
			//output directory files from given input path
			Console.WriteLine("Files in directory: ");
			foreach (string file in Directory.GetFiles(inputDirectory))
			{
				Console.WriteLine(Path.GetFileName(file));
				filename = Path.GetFileName(file);
				//Console.WriteLine(Path.GetDirectoryName(file));

				int first_index = filename.IndexOf("_");
				//Console.WriteLine(first_index);
				string namestart = filename.Substring(0, first_index + 1);
				//Console.WriteLine(namestart);
				int Pattern1Index = filename.IndexOf("Pattern1");
				if (Pattern1Index != -1)
				{
					//Console.WriteLine(Pattern1Index);
					string packageType = filename.Substring(first_index + 1, (Pattern1Index - first_index - 1));
					//Console.WriteLine(packageType);
					int second_index = filename.IndexOf("_", first_index + 1);
					//Console.WriteLine(second_index);
					string Type = filename.Substring(Pattern1Index + 8, (second_index - Pattern1Index - 8));
					//Console.WriteLine(tokenType);
					string newFileName = namestart + inputProjectName + Type + "_" + packageType + ".XML";
					Console.WriteLine(newFileName);

					if (packageType == "Pattern2")
					{
						Console.WriteLine("This is Pattern2.");
						XmlDocument xmldoc = new XmlDocument();
						xmldoc.Load(file);
						XmlNode Pattern3XmlFile = xmldoc.SelectSingleNode("/root/definition/items/item[@type='pattern3']/@location");
						//Console.WriteLine(scriptXmlFile.InnerText);
						Pattern3XmlFile.InnerText = namestart + inputProjectName + Type + "_" + "Pattern3.XML";
						XmlNode Pattern4XmlFile = xmldoc.SelectSingleNode("/root/definition/items/item[@type='pattern4']/@location");
						//Console.WriteLine(tokenXmlFile.InnerText);
						Pattern4XmlFile.InnerText = namestart + inputProjectName + Type + "_" + "Pattern4.XML";
						xmldoc.Save(file);
						Console.WriteLine(Pattern3XmlFile.InnerText);
						Console.WriteLine(Pattern4XmlFile.InnerText);

					}
					//Console.WriteLine(Path.GetDirectoryName(file) + "\\" + newFileName);
					File.Move(file, Path.GetDirectoryName(file) + "\\" + newFileName);
				}
				filename = "";
			}
			Console.WriteLine("Press enter key...");
			Console.ReadLine();
		}
	}
}
