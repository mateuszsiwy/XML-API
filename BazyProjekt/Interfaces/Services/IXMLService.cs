namespace BazyProjekt.Interfaces.Services;

public interface IXMLService
{
    Task AddXmlDocumentAsync(string xmlContent);
    Task DeleteXmlDocumentAsync(int documentId);
    Task<string?> SearchXmlFragmentAsync(int documentId, string xPath);
    Task UpdateXmlFragmentAsync(int documentId, string xPath, string newValue);
}