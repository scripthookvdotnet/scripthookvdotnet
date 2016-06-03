using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
	static void Main(string[] args)
	{
		var inputFiles = new List<string>();
		var outputFile = string.Empty;
		var assemblyName = string.Empty;

		foreach (var arg in args)
		{
			if (arg.Length < 2)
			{
				continue;
			}

			if (arg[0] == '@')
			{
				string cmdline = File.ReadAllText(arg.Substring(1));
				args = Regex.Matches(cmdline, @"\G(""((""""|[^""])+)""|(\S+)) *")
					.Cast<Match>()
					.Select(m => Regex.Replace(m.Groups[2].Success ? m.Groups[2].Value : m.Groups[4].Value, @"([-/].*)""(.*)""", "$1$2"))
					.ToArray();

				Main(args);
				return;
			}

			if (arg[0] != '/' && arg[0] != '-')
			{
				assemblyName = arg;
				continue;
			}

			string id = arg.Substring(1);
			string param = string.Empty;

			if (id[0] == 'F')
			{
				if (id.Length > 2)
				{
					param = id.Substring(2);
					id = id.Substring(0, 2);
				}
			}

			switch (id)
			{
				case "Fs":
					inputFiles.Add(param);
					break;
				case "Fo":
					outputFile = param;
					break;
			}
		}

		if (outputFile.Length == 0)
		{
			Console.WriteLine("error XDC0007 : No output file specified");
			return;
		}

		if (assemblyName.Length == 0)
		{
			assemblyName = Path.GetFileNameWithoutExtension(outputFile);
		}

		var outputDocument = new XmlDocument();
		outputDocument.LoadXml("<doc><assembly>" + assemblyName + "</assembly><members/></doc>");
		XmlNode outputNode = outputDocument.DocumentElement.SelectSingleNode("members");

		foreach (var file in inputFiles)
		{
			var doc = new XmlDocument();

			try
			{
				doc.Load(file);
			}
			catch (Exception)
			{
				Console.WriteLine(file + " : warning XDC0004 : Unable to load XML fragment");
				continue;
			}

			XmlNode members = doc.SelectSingleNode("doc/members");

			if (members == null)
			{
				Console.WriteLine(file + " : warning XDC0022 : File contains no <doc><members> tag");
				continue;
			}

			foreach (XmlNode member in members.ChildNodes)
			{
				member.Attributes.RemoveNamedItem("decl");
				member.Attributes.RemoveNamedItem("source");
				member.Attributes.RemoveNamedItem("line");

				outputNode.AppendChild(outputDocument.ImportNode(member, true));
			}
		}

		try
		{
			outputDocument.Save(outputFile);
		}
		catch (Exception)
		{
			Console.WriteLine("error XDC0002 : Cannot write to output file");
		}
	}
}
