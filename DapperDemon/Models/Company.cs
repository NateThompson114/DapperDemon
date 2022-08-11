using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DapperDemon.Models
{
    [Table("Companies")] // This tells Dapper.Contrib that the name of the table is Companies and not 'Companys'
    public class Company
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
        // This key is added to tell Dapper.Contrib that the CompanyId is the Id for the table
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        [Write(false)] // Tells Dapper.Contrib that is should not write to the table
        public List<Employee> Employees { get; set; }
    }
}
