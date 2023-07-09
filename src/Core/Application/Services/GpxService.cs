namespace Application.Services;

using Application.Interfaces;
using Application.Readers;
using Common.Constants;
using Domain.Entities.GpxFile;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GpxService : IGpxService
{
	public async Task<Gpx> Get(string xml)
	{
		var gpx = await DeserializeAsync(xml, "gpx");
		return gpx;
	}

	private async Task<Gpx> DeserializeAsync(string inputXml, string rootName)
	{
		Gpx gpx = new Gpx();
		await Task.Run(() =>
		{
			XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Gpx), xmlRoot);

			using (var reader = new StringReader(RemoveAllXmlNamespace(inputXml)))
			{
				gpx = (Gpx)xmlSerializer.Deserialize(new IgnoreNamespaceXmlTextReader(reader))!;
			}
		});

		return gpx;
	}

	private string RemoveAllXmlNamespace(string xmlData)
	{
		MatchCollection matchCol = Regex.Matches(
			xmlData, 
			GlobalConstants.GpXFile.XmlNamespacePattern);

		foreach (Match m in matchCol)
		{
			xmlData = xmlData.Replace(m.ToString(), string.Empty);
		}
		return xmlData;
	}
}