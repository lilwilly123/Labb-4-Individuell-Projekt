using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Staff> Staff { get; set; } = new List<Staff>();
    }
}
