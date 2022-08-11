using System.Collections.Generic;
using System.Linq;
using DapperDemon.Data;
using DapperDemon.Models;

namespace DapperDemon.Repository
{
    public class CompanyRepositoryEF : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepositoryEF(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Company Find(int id)
        {
            return _db.Companies.FirstOrDefault(r => r.CompanyId == id);
        }

        public List<Company> GetAll()
        {
            return _db.Companies.ToList();
        }

        public Company Add(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
            return company;
        }

        public Company Update(Company company)
        {
            _db.Companies.Update(company);
            _db.SaveChanges();
            return company;
        }

        public void Remove(int id)
        {
            var company = _db.Companies.FirstOrDefault(r => r.CompanyId == id);

            if(company != null)
                _db.Companies.Remove(company);
            _db.SaveChanges();
        }
    }
}
