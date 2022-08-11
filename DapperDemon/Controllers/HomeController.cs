using DapperDemon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DapperDemon.Repository;

namespace DapperDemon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBonusRepository _bonusRepository;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepository)
        {
            _logger = logger;
            _bonusRepository = bonusRepository;
        }

        public IActionResult Index()
        {
            var companies = _bonusRepository.GetAllCompaniesWithEmployees();
            return View(companies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddTestRecords()
        {
            var relatedGuid = Guid.NewGuid();
            var company = new Company()
            {
                Name = "Test" + relatedGuid,
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Email = "test Email",
                Name = "Test Name " + relatedGuid,
                Phone = " test phone",
                Title = "Test Manager"
            });

            company.Employees.Add(new Employee()
            {
                Email = "test Email 2",
                Name = "Test Name 2" + relatedGuid,
                Phone = " test phone 2",
                Title = "Test Manager 2"
            });
            _bonusRepository.AddTestCompanyWithEmployees(company);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveTestRecords()
        {
            var companyIdToRemove = _bonusRepository.FilterCompanyByName("Test").Select(i => i.CompanyId).ToArray();
            _bonusRepository.RemoveRange(companyIdToRemove);

            return RedirectToAction(nameof(Index));
        }
    }
}
