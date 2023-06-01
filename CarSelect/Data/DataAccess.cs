using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public class DataAccess
    {
        public delegate void RefreshListDelegate();
        public static event RefreshListDelegate RefreshList;

        public static List<Car> GetCars() => CarSelectEntities.GetContext().Cars.ToList();
        public static List<Model> GetModels() => CarSelectEntities.GetContext().Models.ToList();
        public static List<Brand> GetBrands() => CarSelectEntities.GetContext().Brands.ToList();
        public static List<Tariff> GetTariffs() => CarSelectEntities.GetContext().Tariffs.ToList();
        public static List<Request> GetRequests() => CarSelectEntities.GetContext().Requests.ToList();
        public static List<BodyType> GetBodyTypes() => CarSelectEntities.GetContext().BodyTypes.ToList();
        public static List<Client> GetClients() => CarSelectEntities.GetContext().Clients.ToList();
        public static List<FuelType> GetFuelTypes() => CarSelectEntities.GetContext().FuelTypes.ToList();

        public static void SaveCar(Car car)
        {
            if (car.Id == 0)
                CarSelectEntities.GetContext().Cars.Add(car);

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }


        public static void SaveModel(Model model)
        {
            if (model.Id == 0)
            {
                CarSelectEntities.GetContext().Models.Add(model);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveBrand(Brand brand)
        {
            if (brand.Id == 0)
            {
                CarSelectEntities.GetContext().Brands.Add(brand);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveTariff(Tariff tariff)
        {
            if (tariff.Id == 0)
            {
                CarSelectEntities.GetContext().Tariffs.Add(tariff);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveRequest(Request request)
        {
            if (request.Id == 0)
            {
                CarSelectEntities.GetContext().Requests.Add(request);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveBodyType(BodyType bodyType)
        {
            if (bodyType.Id == 0)
            {
                CarSelectEntities.GetContext().BodyTypes.Add(bodyType);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveClient(Client client)
        {
            if (client.Id == 0)
            {
                CarSelectEntities.GetContext().Clients.Add(client);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveFuelType(FuelType fuelType)
        {
            if (fuelType.Id == 0)
            {
                CarSelectEntities.GetContext().FuelTypes.Add(fuelType);
            }

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }
    }
}
