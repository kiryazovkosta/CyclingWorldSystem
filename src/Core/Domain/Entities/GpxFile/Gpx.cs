namespace Domain.Entities.GpxFile;

using System.Xml.Serialization;

[XmlType("gpx")]
public class Gpx
{

	[XmlElement("metadata")]
	public GpxMetadata Metadata { get; set; } = null!;

	[XmlElement("trk")]
	public GpxTrk Trk { get; set; } = null!;
}
