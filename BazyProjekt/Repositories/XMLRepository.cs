using System.Data.SqlClient;
using BazyProjekt.Interfaces.Repositories;

namespace BazyProjekt.Repositories;

public class XMLRepository : IXMLRepository
{
    private readonly string _connectionString;

    public XMLRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddXmlDocumentAsync(string xmlContent)
    {
        const string query = "INSERT INTO XmlDocuments (XmlContent) VALUES (@XmlContent)";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@XmlContent", xmlContent);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteXmlDocumentAsync(int documentId)
    {
        const string query = "DELETE FROM XmlDocuments WHERE Id = @DocumentId";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@DocumentId", documentId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<string?> SearchXmlFragmentAsync(int documentId, string xPath)
    {
        // Ensure the XPath is properly escaped to prevent SQL injection
        var escapedXPath = xPath.Replace("'", "''");
    
        // Construct the query dynamically with the escaped XPath
        var query = $"SELECT XmlContent.query('{escapedXPath}') AS Fragment FROM XmlDocuments WHERE Id = @DocumentId";
    
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@DocumentId", documentId);
    
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return result?.ToString();
        }
    }

    public async Task UpdateXmlFragmentAsync(int documentId, string xPath, string newValue)
    {
        // Escape znaków cudzysłowu w newValue (możesz dodać też inne znaki jeśli trzeba)
        var escapedValue = newValue.Replace("\"", "&quot;");

        // Jeśli XPath nie kończy się na text() lub nie zawiera, dodaj /text()[1] i potem [1] na zewnątrz
        if (!xPath.EndsWith("text()") && !xPath.Contains("text()"))
        {
            // Dodaj /text()[1] i "opakuj" całość w nawiasy + [1]
            xPath = $"({xPath}/text()[1])[1]";
        }
        else
        {
            // Jeśli xpath już zawiera text(), ale nie ma zewnętrznego [1], to dodaj go
            if (!xPath.EndsWith("][1]") && !xPath.EndsWith(")[1]"))
            {
                xPath = $"({xPath})[1]";
            }
        }

        var modifyStatement = $"replace value of {xPath} with (\"{escapedValue}\")";

        var query = $"UPDATE XmlDocuments SET XmlContent.modify('{modifyStatement}') WHERE Id = @DocumentId";

        Console.WriteLine(query);

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@DocumentId", documentId);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }


}