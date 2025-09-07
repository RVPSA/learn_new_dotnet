using BackendWithGraphQL.DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BackendWithGraphQL.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController: ControllerBase
{
    private readonly ApplicationDBContext _context;
    public TestController(ApplicationDBContext context)=> _context = context;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Employees.ToList());
    }
}