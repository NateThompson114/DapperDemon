using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDemon.Models;

namespace DapperDemon.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompany(int companyId);

        Company GetCompanyWithEmployees(int id);

        List<Company> GetAllCompaniesWithEmployees();

        void AddTestCompanyWithEmployees(Company objCompany);

        void AddTestCompanyWithEmployeesWithTransactionScope(Company objCompany);

        void RemoveRange(int[] companyIds);

        List<Company> FilterCompanyByName(string name);
    }
}
