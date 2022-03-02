using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SysTk.Utils.FileZilla
{
	[XmlRoot(ElementName = "Pass")]
	public class Pass
	{
		[XmlAttribute(AttributeName = "encoding")]
		public string Encoding { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Server")]
	public class Server
	{
		[XmlElement(ElementName = "Host")]
		public string Host { get; set; }
		[XmlElement(ElementName = "Port")]
		public string Port { get; set; }
		[XmlElement(ElementName = "Protocol")]
		public string Protocol { get; set; }
		[XmlElement(ElementName = "Type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "User")]
		public string User { get; set; }
		[XmlElement(ElementName = "Pass")]
		public Pass Pass { get; set; }
		[XmlElement(ElementName = "Logontype")]
		public string Logontype { get; set; }
		[XmlElement(ElementName = "PasvMode")]
		public string PasvMode { get; set; }
		[XmlElement(ElementName = "EncodingType")]
		public string EncodingType { get; set; }
		[XmlElement(ElementName = "BypassProxy")]
		public string BypassProxy { get; set; }
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "SyncBrowsing")]
		public string SyncBrowsing { get; set; }
		[XmlElement(ElementName = "DirectoryComparison")]
		public string DirectoryComparison { get; set; }
		[XmlElement(ElementName = "RemoteDir")]
		public string RemoteDir { get; set; }
		[XmlElement(ElementName = "LocalDir")]
		public string LocalDir { get; set; }
	}

	[XmlRoot(ElementName = "Folder")]
	public class Folder
	{
		[XmlElement(ElementName = "Server")]
		public List<Server> Server { get; set; }
		[XmlAttribute(AttributeName = "expanded")]
		public string Expanded { get; set; }
		[XmlElement(ElementName = "Folder")]
		public List<Folder> Folders { get; set; }
	}

	[XmlRoot(ElementName = "Servers")]
	public class Servers
	{
		[XmlElement(ElementName = "Folder")]
		public List<Folder> Folder { get; set; }
		[XmlElement(ElementName = "Server")]
		public List<Server> Server { get; set; }
	}

	[XmlRoot(ElementName = "FileZilla3")]
	public class FileZilla3
	{
		[XmlElement(ElementName = "Servers")]
		public Servers Servers { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "platform")]
		public string Platform { get; set; }
	}

}
