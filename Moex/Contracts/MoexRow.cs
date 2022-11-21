using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Moex.Contracts;

public class MoexRow : IXmlSerializable
{
    private readonly Dictionary<string, string> _attributes = new();

    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        while (reader.MoveToNextAttribute()) _attributes[reader.Name] = reader.Value;

        reader.Read();
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }

    public string? GetAttributeValue(string name)
    {
        return _attributes.TryGetValue(name, out var value) ? value : null;
    }
}