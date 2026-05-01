using cat_facts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
public class CatFactsController : ControllerBase
{
    private readonly ICatFactService _service;

    public CatFactsController(ICatFactService service)
    {
        _service = service;
    }

    [HttpGet]
    
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("random")]
    public IActionResult GetRandom()
    {
        var fact = _service.GetRandom();
        if (fact == null) return NotFound("No facts available");

        return Ok(fact);
    }

    [HttpPost]
    public IActionResult Add([FromBody] string fact)
    {
        if (string.IsNullOrWhiteSpace(fact))
            return BadRequest("Fact cannot be empty");

        return Ok(_service.Add(fact));
    }
}