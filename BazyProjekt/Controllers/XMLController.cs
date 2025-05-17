using BazyProjekt.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BazyProjekt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XMLController : ControllerBase
{
    private readonly IXMLService _xmlService;

    public XMLController(IXMLService xmlService)
    {
        _xmlService = xmlService;
    }

    [HttpPost]
    public async Task<IActionResult> AddXmlDocument([FromBody] string xmlContent)
    {
        try
        {
            await _xmlService.AddXmlDocumentAsync(xmlContent);
            return Ok("XML document added successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteXmlDocument(int id)
    {
        await _xmlService.DeleteXmlDocumentAsync(id);
        return Ok("XML document deleted successfully.");
    }

    [HttpGet("{id:int}/search")]
    public async Task<IActionResult> SearchXmlFragment(int id, [FromQuery] string xPath)
    {
        try
        {
            var result = await _xmlService.SearchXmlFragmentAsync(id, xPath);
            if (result == null)
                return NotFound("No matching fragment found.");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/update")]
    public async Task<IActionResult> UpdateXmlFragment(int id, [FromQuery] string xPath, [FromQuery] string newValue)
    {
        try
        {
            await _xmlService.UpdateXmlFragmentAsync(id, xPath, newValue);
            return Ok("XML fragment updated successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}