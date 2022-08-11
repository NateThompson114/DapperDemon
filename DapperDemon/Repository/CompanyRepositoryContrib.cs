using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper.Contrib.Extensions;
using DapperDemon.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemon.Repository
{
    public class CompanyRepositoryContrib : ICompanyRepository
    {
        private readonly IDbConnection _db;
        public CompanyRepositoryContrib(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Find(int id)
        {
            return _db.Get<Company>(id);
        }

        public List<Company> GetAll()
        {
            return _db.GetAll<Company>().ToList();
        }

        public Company Add(Company company)
        {
            var id = _db.Insert(company);
            company.CompanyId = (int) id;

            return company;
        }

        public Company Update(Company company)
        {
            var id = _db.Update(company);

            return company;
        }

        public void Remove(int id)
        {
            _db.Delete(new Company() {CompanyId = id});
        }
    }
}
