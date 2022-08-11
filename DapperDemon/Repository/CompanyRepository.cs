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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnection _db;
        public CompanyRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        
        public Company Find(int id)
        {
            const string sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return _db.QueryFirstOrDefault<Company>(sql, new {@CompanyId = id});
        }

        public List<Company> GetAll()
        {
            const string sql = "SELECT * FROM Companies";
            return _db.Query<Company>(sql).ToList();
        }

        public Company Add(Company company)
        {
            const string sql = "INSERT INTO Companies(Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);" +
                               "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _db.Query<int>(sql,company).Single();
            company.CompanyId = id;

            return company;
        }

        public Company Update(Company company)
        {
            const string sql =
                "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            _db.Execute(sql, company);
            return company;
        }

        public void Remove(int id)
        {
            const string url = "DELETE FROM Companies WHERE CompanyId = @CompanyId";
            _db.Execute(url, new {@CompanyId = id});
        }
    }
}
