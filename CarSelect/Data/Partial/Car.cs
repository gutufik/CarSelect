using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public partial class Car
    {
        public override string ToString()
        {
            return $"{Model.Brand.Name} {Model.Name}";
        }
    }
}
