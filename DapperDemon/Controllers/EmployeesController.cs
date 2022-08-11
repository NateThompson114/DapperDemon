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
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBonusRepository _bonusRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IBonusRepository bonusRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _bonusRepository = bonusRepository;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        // GET: Employees
        public async Task<IActionResult> Index(int companyId = 0)
        {
            var employees = _bonusRepository.GetEmployeeWithCompany(companyId);

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeRepository.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["CompanyData"] = new SelectList(_companyRepository.GetAll(), "CompanyId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,Email,Phone,Title,CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyData"] = GetCompanySelectList(employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeRepository.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CompanyData"] = GetCompanySelectList(employee.CompanyId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Email,Phone,Title,CompanyId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _employeeRepository.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyData"] = GetCompanySelectList(employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeRepository.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var employee = await _employeeRepository.Employees.FindAsync(id);
            //_employeeRepository.Employees.Remove(employee);
            //await _employeeRepository.SaveChangesAsync();
            _employeeRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool EmployeeExists(int id)
        //{
        //    return _employeeRepository.Employees.Any(e => e.EmployeeId == id);
        //}

        private SelectList GetCompanySelectList(int currentCompanyId)
        {
            return new(_companyRepository.GetAll()
                .Select(i => new CompanySelectListItem(i.CompanyId, i.Name)).ToList(),"CompanyId", "Name", currentCompanyId); 
        }
    }
}
