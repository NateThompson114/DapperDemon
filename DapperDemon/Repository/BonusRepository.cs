using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;
using Dapper;
using DapperDemon.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemon.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private readonly IDbConnection _db;
        public BonusRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public List<Employee> GetEmployeeWithCompany(int companyId)
        {
            var sql = "SELECT em.*,co.* FROM Employees AS em INNER JOIN Companies AS co ON em.CompanyId = co.CompanyId";

            if (companyId != 0)
            {
                sql += " WHERE em.CompanyId = @CompanyId";
            }

            // First Employee is the employee data, Company is the Company Data, the last Employee is telling dapper to load the first employee and the company into the last Employee
            // The Split tells dapper where to split the string of data 
            var employee = _db.Query<Employee, Company, Employee>(sql, (em, co) =>
            {
                em.Company = co;
                return em;
            }, new {companyId}, splitOn: "CompanyId");

            return employee.ToList();
        }

        public Company GetCompanyWithEmployees(int id)
        {
            var p = new {CompanyId = id};
            const string sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId;" +
                      "SELECT * FROM Employees WHERE CompanyId = @CompanyId;";

            using var list = _db.QueryMultiple(sql, p);

            var company = list.ReadFirstOrDefault<Company>();
            company.Employees = list.Read<Employee>().ToList();

            return company;
        }

        public List<Company> GetAllCompaniesWithEmployees()
        {
            const string sql = "SELECT co.*,em.* FROM Employees AS em INNER JOIN Companies AS co ON em.CompanyId = co.CompanyId";

            var companyDictionary = new Dictionary<int, Company>();

            var company = _db.Query<Company, Employee, Company>(sql, (co, em) =>
            {
                if (!companyDictionary.TryGetValue(co.CompanyId, out var currentCompany))
                {
                    currentCompany = co;
                    companyDictionary.Add(currentCompany.CompanyId, currentCompany);
                }

                currentCompany.Employees.Add(em);
                return currentCompany;
            }, splitOn: "EmployeeId");

            return company.Distinct().ToList();
        }

        public void AddTestCompanyWithEmployees(Company objCompany)
        {
            const string sql = "INSERT INTO Companies(Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);" +
                               "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _db.Query<int>(sql, objCompany).Single();
            objCompany.CompanyId = id;

            //foreach (var employee in objCompany.Employees)
            //{
            //    employee.CompanyId = objCompany.CompanyId;
            //    const string sql1 = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
            //                       "SELECT CAST(SCOPE_IDENTITY() as int);";
            //    _db.Query<int>(sql1, employee).Single();
            //}

            var employees = objCompany.Employees.Select(c =>
            {
                c.CompanyId = id;
                return c;
            }).ToList();
            const string sqlEmployees = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                                        "SELECT CAST(SCOPE_IDENTITY() as int);";
            _db.Execute(sqlEmployees, objCompany.Employees);

        }

        public void AddTestCompanyWithEmployeesWithTransactionScope(Company objCompany)
        {
            using var transaction = new TransactionScope();

            try
            {
                const string sql = "INSERT INTO Companies(Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);" +
                                   "SELECT CAST(SCOPE_IDENTITY() as int);";
                var id = _db.Query<int>(sql, objCompany).Single();
                objCompany.CompanyId = id;
                
                var employees = objCompany.Employees.Select(c =>
                {
                    c.CompanyId = id;
                    return c;
                }).ToList();
                const string sqlEmployees = "INSERT INTO Employees1 (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                                            "SELECT CAST(SCOPE_IDENTITY() as int);";
                _db.Execute(sqlEmployees, objCompany.Employees);

                transaction.Complete();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void RemoveRange(int[] companyIds)
        {
            _db.Query("DELETE FROM Companies WHERE CompanyId IN @companyIds", new {companyIds});
        }

        public List<Company> FilterCompanyByName(string name)
        {
            return _db.Query<Company>($"SELECT * FROM Companies WHERE Name LIKE '%' + @name + '%'", new {name}).ToList();
        }
    }
}
