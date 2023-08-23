using Renci.SshNet;
using Renci.SshNet.Sftp;
//Used to test getting files from remote site to be integrating in SSIS package
namespace HouseWrenDevCodeSamples
{
	internal class CSConsolSFTPTransfer
	{
		static void Main()
		{
			string host="";
			string user = "";
			string pass = "";
			int port = 0; ;

			var client = new SftpClient(host, port, user, pass);
			try
			{
				Console.WriteLine("about to connect");
				client.Connect();
				Console.WriteLine("connected");
				foreach (SftpFile sf in client.ListDirectory("/"))
				{
					if (sf.IsDirectory)
					{
						Console.WriteLine(sf.Name);
					}
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
			finally
			{
				Console.WriteLine("disconnecting");
				client.Disconnect();
			}
			Console.ReadLine();
		}
	}
}