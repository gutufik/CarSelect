using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public partial class Role
    {
        public string Color => Name == "Администратор" ? "DeepSkyBlue" : Name == "Менеджер" ? "Silver" : "MediumSpringGreen";
    }
}
