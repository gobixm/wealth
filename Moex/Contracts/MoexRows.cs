using System.Xml.Serialization;

namespace Moex.Contracts;

public sealed record MoexRows
{
    [XmlElement("row")] public MoexRow[] Rows { get; set; } = Array.Empty<MoexRow>();
}