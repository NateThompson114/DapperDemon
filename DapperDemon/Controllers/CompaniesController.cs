using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperDemon.Data;
using DapperDemon.Models;
using DapperDemon.Repository;

namespace DapperDemon.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBonusRepository _bonusRepository;
        private readonly IDapperSprocRepository _dapperSprocRepository;

        public CompaniesController(ICompanyRepository companyRepository, IBonusRepository bonusRepository, IDapperSprocRepository dapperSprocRepository)
        {
            _companyRepository = companyRepository;
            _bonusRepository = bonusRepository;
            _dapperSprocRepository = dapperSprocRepository;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //return View(_companyRepository.GetAll());
            return View(_dapperSprocRepository.List<Company>("usp_GetALLCompany"));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _bonusRepository.GetCompanyWithEmployees(id.GetValueOrDefault());
            //var company = _companyRepository.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _companyRepository.Add(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = _companyRepository.Find(id.GetValueOrDefault());
            var company = _dapperSprocRepository.Single<Company>("usp_GetCompany", new {@CompanyId = id});
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _companyRepository.Update(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _companyRepository.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var company = await _companyRepository.Companies.FindAsync(id);
            //_companyRepository.Companies.Remove(company);
            //await _companyRepository.SaveChangesAsync();
            _companyRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool CompanyExists(int id)
        //{
        //    return _companyRepository.Companies.Any(e => e.CompanyId == id);
        //}
    }
}
