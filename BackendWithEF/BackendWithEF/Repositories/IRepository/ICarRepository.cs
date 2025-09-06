using BackendWithEF.Models.Dtos;
using BackendWithEF.Models.Entities;
using BackendWithEF.Models.Result;

namespace BackendWithEF.Repositories.IRepository;

public interface ICarRepository
{
    List<Car> GetAllCars();
    Car AddCar(AddCarDTO car);
    AddCarDTO? GetCarById(Guid id);
    int UpdateCar(Guid id, AddCarDTO car);
    int DeleteCar(Guid id);
}