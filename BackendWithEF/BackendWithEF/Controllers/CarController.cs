using BackendWithEF.Models.Dtos;
using BackendWithEF.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace BackendWithEF.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CarController(ICarService carService): ControllerBase
{
    
    [HttpGet]
    public IActionResult GetAllCars()
    {
        try
        {
            return Ok(carService.GetAllCars());
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult AddCar([FromBody] AddCarDTO car)
    {
        try
        {
            return Ok(carService.AddCar(car));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetCarById(Guid id)
    {
        try
        {
            return Ok(carService.GetCarById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateCar(Guid id,[FromBody] AddCarDTO car)
    {
        try
        {
           return Ok(carService.UpdateCar(id, car));

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteCar(Guid id)
    {
        try
        {
            return Ok(carService.DeleteCar(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}