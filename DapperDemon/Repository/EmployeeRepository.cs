using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperDemon.Data;
using DapperDemon.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemon.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnection _db;
        public EmployeeRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        
        public Employee Find(int id)
        {
            const string sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return _db.QueryFirstOrDefault<Employee>(sql, new { @Id = id});
        }

        public List<Employee> GetAll()
        {
            const string sql = "SELECT * FROM Employees";
            return _db.Query<Employee>(sql).ToList();
        }

        //Get Employees In a Company:
        //SELECT* FROM Employees WHERE companyId = @companyId

        public Employee Add(Employee Employee)
        {
            const string sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                               "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _db.Query<int>(sql,Employee).Single();
            Employee.EmployeeId = id;

            return Employee;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            const string sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                               "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await _db.QueryFirstOrDefaultAsync<int>(sql, employee);
            employee.EmployeeId = id;

            return employee;
        }

        public Employee Update(Employee Employee)
        {
            const string sql =
                "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            _db.Execute(sql, Employee);
            return Employee;
        }

        public void Remove(int id)
        {
            const string url = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
            _db.Execute(url, new {@EmployeeId = id});
        }
    }
}
