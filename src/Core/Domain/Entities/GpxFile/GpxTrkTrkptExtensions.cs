namespace Domain.Entities.GpxFile;

using System.Xml.Serialization;

[XmlType("extensions")]
public class GpxTrkTrkptExtensions
{
	[XmlElement("power")]
	public ushort? Power { get; set; }

	[XmlElement("TrackPointExtension")]
	public TrackPointExtension TrackPointExtension { get; set; } = null!;
}
