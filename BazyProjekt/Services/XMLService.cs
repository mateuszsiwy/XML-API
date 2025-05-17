using System.Xml;
using BazyProjekt.Interfaces.Repositories;
using BazyProjekt.Interfaces.Services;

namespace BazyProjekt.Services;

public class XMLService : IXMLService
{
    private readonly IXMLRepository _xmlRepository;

    public XMLService(IXMLRepository xmlRepository)
    {
        _xmlRepository = xmlRepository;
    }

    public async Task AddXmlDocumentAsync(string xmlContent)
    {
        ValidateXml(xmlContent); 
        await _xmlRepository.AddXmlDocumentAsync(xmlContent);
    }

    public async Task DeleteXmlDocumentAsync(int documentId)
    {
        await _xmlRepository.DeleteXmlDocumentAsync(documentId);
    }

    public async Task<string?> SearchXmlFragmentAsync(int documentId, string xPath)
    {
        if (string.IsNullOrWhiteSpace(xPath))
            throw new ArgumentException("XPath cannot be null or empty.", nameof(xPath));

        return await _xmlRepository.SearchXmlFragmentAsync(documentId, xPath);
    }

    public async Task UpdateXmlFragmentAsync(int documentId, string xPath, string newValue)
    {
        if (string.IsNullOrWhiteSpace(xPath))
            throw new ArgumentException("XPath cannot be null or empty.", nameof(xPath));

        if (string.IsNullOrWhiteSpace(newValue))
            throw new ArgumentException("New value cannot be null or empty.", nameof(newValue));

        await _xmlRepository.UpdateXmlFragmentAsync(documentId, xPath, newValue);
    }

    private void ValidateXml(string xmlContent)
    {
        if (string.IsNullOrWhiteSpace(xmlContent))
            throw new ArgumentException("XML content cannot be null or empty.", nameof(xmlContent));

        try
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent); // This will throw an exception if the XML is invalid
        }
        catch (XmlException ex)
        {
            throw new ArgumentException("Invalid XML content.", ex);
        }
    }
}