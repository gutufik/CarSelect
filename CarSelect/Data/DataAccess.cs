using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static List<DriveType> GetDriveTypes() => CarSelectEntities.GetContext().DriveTypes.ToList();
        public static List<GBType> GetGBTypes() => CarSelectEntities.GetContext().GBTypes.ToList();
        public static List<Brand> GetBrands() => CarSelectEntities.GetContext().Brands.ToList();
        public static List<Tariff> GetTariffs() => CarSelectEntities.GetContext().Tariffs.ToList();
        public static List<Request> GetRequests() => CarSelectEntities.GetContext().Requests.ToList();
        public static List<BodyType> GetBodyTypes() => CarSelectEntities.GetContext().BodyTypes.ToList();
        public static List<Client> GetClients() => CarSelectEntities.GetContext().Clients.ToList();
        public static List<FuelType> GetFuelTypes() => CarSelectEntities.GetContext().FuelTypes.ToList();
        public static List<Role> GetRoles() => CarSelectEntities.GetContext().Roles.ToList();
        public static List<User> GetUsers() => CarSelectEntities.GetContext().Users.ToList();
        public static List<State> GetStates() => CarSelectEntities.GetContext().States.ToList();


        public static State GetStateByName(string name) => GetStates().FirstOrDefault(x => x.Name == name);

        public static User Login(string login, string password)
        {
            string hashPassword = StringToMD5(password);
            return CarSelectEntities.GetContext().Users.FirstOrDefault(x => x.Login == login && x.Password == hashPassword);
        }

        private static string StringToMD5(string password)
        {
            StringBuilder sb = new StringBuilder();
            var md5 = MD5.Create();

            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static void SaveCar(Car car)
        {
            if (car.Id == 0)
                CarSelectEntities.GetContext().Cars.Add(car);

            CarSelectEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static void SaveUser(User user, string password = null)
        {
            if (user.Id == 0)
                CarSelectEntities.GetContext().Users.Add(user);

            if (password != null)
                user.Password = StringToMD5(password);

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
