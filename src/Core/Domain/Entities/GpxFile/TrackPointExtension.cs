namespace Domain.Entities.GpxFile;

using System.Xml.Serialization;

[XmlType("TrackPointExtension")]
public class TrackPointExtension
{
	[XmlElement("atemp")]
	public decimal? Temperature { get; set; }

	[XmlElement("hr")]
	public byte? HeartRate { get; set; }

	[XmlElement("cad")]
	public byte? Cadance { get; set; }
}
