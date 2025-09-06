using BackendWithEF.Models.Dtos;
using BackendWithEF.Models.Entities;
using BackendWithEF.Repositories.IRepository;
using BackendWithEF.Services.IService;

namespace BackendWithEF.Services;

public class CarService(ICarRepository carRepository) : ICarService
{
    public List<Car> GetAllCars()
    {
        return carRepository.GetAllCars();
    }

    public Car AddCar(AddCarDTO car)
    {
        return carRepository.AddCar(car);
    }

    public AddCarDTO? GetCarById(Guid id)
    {
        return carRepository.GetCarById(id);
    }

    public int UpdateCar(Guid id, AddCarDTO car)
    {
        return carRepository.UpdateCar(id, car);
    }

    public int DeleteCar(Guid id)
    {
        return carRepository.DeleteCar(id);
    }
    
    
}