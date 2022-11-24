using System.Xml.Serialization;
using FluentAssertions;
using Moex.Contracts;

namespace Moex.Tests;

public sealed class SecurityTests
{
    [Fact]
    public void Deserialize_Security_Deserialized()
    {
        // arrange
        var serializer = new XmlSerializer(typeof(MoexResponse));
        using var source =
            typeof(SecurityTests).Assembly.GetManifestResourceStream(@"Moex.Tests.Resources.securities.xml");

        // act
        var securities = serializer.Deserialize(source!) as MoexResponse;

        // assert
        securities!.Data.Rows.Rows.Should().HaveCount(100);
        securities.Data.Rows.Rows.First().GetAttributeValue("secid").Should().Be("A-RM");
        securities.Data.Rows.Rows.Last().GetAttributeValue("name").Should().Be("Builders FirstSource ORD SHS");
    }

    [Fact]
    public void Deserialize_Index_Deserialized()
    {
        // arrange
        var serializer = new XmlSerializer(typeof(MoexResponse));
        using var source =
            typeof(SecurityTests).Assembly.GetManifestResourceStream(@"Moex.Tests.Resources.index-imoex.xml");

        // act
        var index = serializer.Deserialize(source!) as MoexResponse;

        // assert
        index!.Data.Rows.Rows.Should().HaveCount(40);
        index.Data.Rows.Rows.First().GetAttributeValue("secids").Should().Be("AFKS");
        index.Data.Rows.Rows.Last().GetAttributeValue("weight").Should().Be("6.03");
    }
}