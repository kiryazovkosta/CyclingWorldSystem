namespace Application.Readers;

using System.Xml;

public class IgnoreNamespaceXmlTextReader : XmlTextReader
{
	public IgnoreNamespaceXmlTextReader(TextReader reader)
		: base(reader)
	{
	}

	public override string NamespaceURI => "";
}