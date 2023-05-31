using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Data
{
    public partial class CarSelectEntities
    {
        private static CarSelectEntities _context;
        public static CarSelectEntities GetContext()
        {
            if (_context == null)
            {
                _context = new CarSelectEntities();
            }
            return _context;
        }
    }
}
