// ------------------------------------------------------------------------------------------------
//  <copyright file="IgnoreNamespaceXmlTextReaderTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Readers;

using Application.Readers;

public class IgnoreNamespaceXmlTextReaderTests
{
    [Fact]
    public void Constructor_ShouldInitializeBaseReader()
    {
        // Arrange
        var reader = new StringReader("<root>Test</root>");

        // Act
        var ignoreNamespaceReader = new IgnoreNamespaceXmlTextReader(reader);

        // Assert
        Assert.NotNull(ignoreNamespaceReader);
    }

    [Fact]
    public void NamespaceURI_ShouldReturnEmptyString()
    {
        // Arrange
        var reader = new StringReader("<root>Test</root>");
        var ignoreNamespaceReader = new IgnoreNamespaceXmlTextReader(reader);

        // Act
        var namespaceUri = ignoreNamespaceReader.NamespaceURI;

        // Assert
        Assert.Equal("", namespaceUri);
    }
}