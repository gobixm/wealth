using System.Xml.Serialization;

namespace Moex.Contracts;

public sealed record MoexData
{
    [XmlElement("rows")] public MoexRows Rows { get; set; } = new ();
}