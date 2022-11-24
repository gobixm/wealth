using System.Xml.Serialization;

namespace Moex.Contracts;

[XmlRoot("document")]
public sealed record MoexResponse
{
    [XmlElement("data")] public MoexData Data { get; set; } = new();
}