using BackendWithEF.Data;
using BackendWithEF.Models.Dtos;
using BackendWithEF.Models.Entities;
using BackendWithEF.Repositories.IRepository;

namespace BackendWithEF.Repositories;

public class CarRepository(ApplicationDbContext dbContext) : ICarRepository
{
    public List<Car> GetAllCars()
    {
        try
        {
            return dbContext.Cars.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Car AddCar(AddCarDTO car)
    {
        try
        {
            var carEntity = new Car()
            {
                Brand = car.Brand,
                Price = car.Price,
                Year = car.Year,
            };
            dbContext.Cars.Add(carEntity);
            dbContext.SaveChanges();
            return carEntity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public AddCarDTO? GetCarById(Guid id)
    {
        try
        {
            var car = dbContext.Cars.Find(id);
            if (car == null)
                return null;
            return new AddCarDTO
            {
                Brand = car.Brand,
                Price = car.Price,
                Year = car.Year,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int UpdateCar(Guid id, AddCarDTO car)
    {
        try
        {
            var carEntity = dbContext.Cars.Find(id);

            if (carEntity == null)
                return 0;
            
            carEntity.Brand = car.Brand;
            carEntity.Price = car.Price;
            carEntity.Year = car.Year;

            dbContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int DeleteCar(Guid id)
    {
        try
        {
            var carEntity = dbContext.Cars.Find(id);
            if (carEntity == null)
                return 0;
            dbContext.Cars.Remove(carEntity);
            dbContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}