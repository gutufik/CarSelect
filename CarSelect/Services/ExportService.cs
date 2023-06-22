using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSelect.Data;
using IronXL;


namespace CarSelect.Services
{
    public class ExportService
    {
        public static void ExportUserStatistics(List<Request> requests)
        {
            WorkBook wb = WorkBook.Create();
            wb.CreateWorkSheet("Заявки");
            WorkSheet workSheet = wb.WorkSheets[0];

            var states = DataAccess.GetStates();

            var user = requests[0].User.ToString();

            workSheet["A1"].Value = user;

            workSheet[$"A3"].Value = "Статус";
            workSheet[$"B3"].Value = "Кол-во";


            var row = 4;
            foreach (var state in states)
            {
                workSheet[$"A{row}"].Value = state.Name;
                workSheet[$"B{row}"].Value = requests.Count(x => x.State == state);
                row++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //workSheet.AutoSizeColumn(0);
            wb.SaveAs($"{desktopPath}/Отчет по сотруднику {user}.xlsx");
        }

        public static void ExportUsersStatistics(List<User> users)
        {
            WorkBook wb = WorkBook.Create();

            
            foreach (var user in users)
            {
                WorkSheet workSheet = wb.CreateWorkSheet(user.ToString());

                workSheet["A1"].Value = "Заявки";
                workSheet[$"A{2}"].Value = "Дата начала";
                workSheet[$"B{2}"].Value = "Дата окончания";
                workSheet[$"C{2}"].Value = "Тариф";
                workSheet[$"D{2}"].Value = "Стоимость тарифа";
                workSheet[$"E{2}"].Value = "Автомобиль";
                workSheet[$"F{2}"].Value = "Стоимость автомобиля";


                var row = 3;
                foreach(var request in user.Requests)
                {
                    workSheet[$"A{row}"].Value = request.StartDate.ToString();
                    workSheet[$"B{row}"].Value = request.EndDate == null? "" : ((DateTime)request.EndDate).Date.ToString("d");
                    workSheet[$"C{row}"].Value = request.Tariff.Name;
                    workSheet[$"D{row}"].Value = request.Tariff.Price;
                    if (request.Car == null)
                    {
                        workSheet[$"E{row}"].Value = $"";
                        workSheet[$"F{row}"].Value = $"";
                    }
                    else
                    {
                        workSheet[$"E{row}"].Value = $"{request.Car.Model.Brand.Name} {request.Car.Model.Name}";
                        workSheet[$"F{row}"].Value = $"{request.Car.Price} ₽";
                    }
                }
                //workSheet.AutoSizeColumn(0);
                //workSheet.AutoSizeColumn(1);
                //workSheet.AutoSizeColumn(2);
                //workSheet.AutoSizeColumn(3);
                //workSheet.AutoSizeColumn(4);
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            wb.SaveAs($"{desktopPath}/Отчет по всем сотрудникам.xlsx");
        }

        public static void ExportTariffsStatistics(List<Tariff> tariffs)
        {
            WorkBook wb = WorkBook.Create();

            WorkSheet workSheet = wb.CreateWorkSheet("Отчет по тарифам");

            workSheet["A1"].Value = "Тариф";
            workSheet["B1"].Value = "Количество заказов";
            workSheet["C1"].Value = "Общая выручка";

            var row = 2;
            foreach (var tariff in tariffs)
            {
                int totalRequests = tariff.Requests.Count;
                decimal totalRevenue = tariff.Price * (decimal)tariff.Requests.Count;

                workSheet[$"A{row}"].Value = tariff.Name;
                workSheet[$"B{row}"].Value = totalRequests;
                workSheet[$"C{row}"].Value = totalRevenue.ToString("C");

                row++;
            }

            //workSheet.AutoSizeColumn(0);
            //workSheet.AutoSizeColumn(1);
            //workSheet.AutoSizeColumn(2);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            wb.SaveAs($"{desktopPath}/Отчет по тарифам.xlsx");
        }
    }
}