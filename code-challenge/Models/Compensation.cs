using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        //public String CompensationId { get; set; }
        // reference to the name of another navigation property in Employee class
        //[InverseProperty("EmployeeId")]
        //[ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
