using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DapperDemon.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemon.Repository
{
    public class CompanyRepositorySproc : ICompanyRepository
    {
        private readonly IDbConnection _db;
        public CompanyRepositorySproc(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        
        public Company Find(int id)
        {
            return _db.QueryFirstOrDefault<Company>("usp_GetCompany", new {@CompanyId = id} , commandType: CommandType.StoredProcedure);
        }

        public List<Company> GetAll()
        {
            return _db.Query<Company>("usp_GetALLCompany", commandType: CommandType.StoredProcedure).ToList();
        }

        public Company Add(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name",company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);

            _db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("@CompanyId");

            return company;
        }

        public Company Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);

            return company;
        }

        public void Remove(int id)
        {
            _db.Execute("usp_RemoveCompany", new { @CompanyId = id} ,commandType: CommandType.StoredProcedure);
        }
    }
}
