using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public partial class User
    {
        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}." + (Patronymic != null ? $" {Patronymic[0]}." : "");
        }
    }
}
