using CsvHelper;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Globalization;

namespace HouseWrenDevCodeSamples
{
	//Custom Microsoft Graph call to set one SharePoint site ID and recursively get all files in all folders on that site.
	//Returns the name and webURL written to a csv file
	internal class GraphCallGETAllSharepointFilesInfo
	{
		//// Settings object
		//private static Settings? _settings;
		//// User auth token credential
		//private static DeviceCodeCredential? _deviceCodeCredential;
		//// Client configured with user authentication
		private static GraphServiceClient? _userClient;
		//SHAREPOINT SITE TO GET FILES FROM
		private static string SharepointSiteId = ""; 
		
		//PATH FOR CSV FILE DOCUMENT INFO OUTPUT  
		//EACH FILE NAME/URL PAIR WILL BE SAVED INTO AN OBJECT AND ADDED TO LIST TO WRITE TO CSV
		private static string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "docName.csv"; //current users desktop+new filename
																																						 
		private static List<O365DocDetails> ItemNamesUrls = new List<O365DocDetails>();
		public async static Task GetAllFilesAsync()
		{
			_ = _userClient ??
				throw new System.NullReferenceException("Graph has not been initialized for user auth");

			Console.WriteLine("GET sharepoint");
			try
			{
				//GET SITE
				//hardcoded site ID at top of file to get sharepoint site, ID obtained from Graph http API call 
				var resultSite = await _userClient.Sites[SharepointSiteId].GetAsync();
				Console.WriteLine("SITE NAME: " + resultSite.Name);

				//GET DOCUMENT LIBRARY(IES)
				//Console.WriteLine("GET all document libraries in "+resultSite.Name);  
				var resultDrives = await _userClient.Sites[resultSite.Id].Drives.GetAsync();
				foreach (Drive drive in resultDrives.Value)
				{
					//returns all document libraries for site, preferred document storage/sharing for sites
					if (drive.DriveType == "documentLibrary")
					{
						//Console.WriteLine("DOCUMENT LIBRARY NAME: "+drive.Name);
						//GET DRIVE ITEM ID FOR DOCUMENT LIBRARY
						var resultDriveItem = await _userClient.Drives[drive.Id].Root.GetAsync();
						string driveItemId = resultDriveItem.Id;//		Id	"01Z52HCSV6Y2GOVW7725BZO354PWSELRRZ"
						Console.WriteLine("Getting directory and file info...this may take a minute.");
						await GetChildrenAsync(drive.Id, driveItemId);
					}
				}
				//WRITE LIST OF NAME/URLS TO CSV FILE
				using (var writer = new StreamWriter(path))
				using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					csv.WriteRecords(ItemNamesUrls);
				}
				Console.WriteLine("Successfully written file names and urls to file at " + path);
			}
			catch (Exception e) { Console.WriteLine(e.Message); }
		}
		public async static Task GetChildrenAsync(string driveId, string itemId)
		{
			if (_userClient != null)
			{
				var resultChildren = await _userClient.Drives[driveId].Items[itemId].Children.GetAsync();
				if (resultChildren != null)
				{
					foreach (var x in resultChildren.Value)
					{
						string type = x.Folder == null ? "File" : "Folder";
						//Console.WriteLine(type + " " + x.Name);
						//BUILD LIST OF NAME/URLS THAT WILL BE WRITTEN TO FILE
						if (type == "Folder") { ItemNamesUrls.Add(new O365DocDetails { Name = "FOLDER", webURL = x.Name.ToUpper() }); }
						ItemNamesUrls.Add(new O365DocDetails { Name = x.Name, webURL = x.WebUrl });
						if (x.Id != null)
						{
							await GetChildrenAsync(driveId, x.Id);
						}
					}
				}
			}
		}
		public class O365DocDetails
		{
			public string Name { get; set; }
			public string webURL { get; set; }
		}
	}
}
