using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public partial class Tariff
    {
        public string ColumnWidth => App.User.Role.Name == "Консультант" ? "0" : "*";
    }
}
