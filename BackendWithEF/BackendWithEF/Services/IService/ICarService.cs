using BackendWithEF.Models.Dtos;
using BackendWithEF.Models.Entities;
using BackendWithEF.Models.Result;
using Microsoft.AspNetCore.Mvc;

namespace BackendWithEF.Services.IService;

public interface ICarService
{
    List<Car> GetAllCars();
    Car AddCar(AddCarDTO car);
    AddCarDTO? GetCarById(Guid id);
    int UpdateCar(Guid id, AddCarDTO car);
    int DeleteCar(Guid id);
}