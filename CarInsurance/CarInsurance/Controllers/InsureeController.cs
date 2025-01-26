using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;
using System.Threading.Tasks;
using System.Linq;
using CarInsurance.Data;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsureeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Insuree
        public async Task<IActionResult> Index()
        {
            var insurees = _context.Insurees.ToList();
            return View(await _context.Insurees.ToListAsync());
        }

        // GET: Insuree/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree);
                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }

        // Method to Calculate Insurance Quote
        private decimal CalculateQuote(Insuree insuree)
        {
            decimal baseQuote = 50;

            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (age <= 18) baseQuote += 100;
            else if (age >= 19 && age <= 25) baseQuote += 50;
            else baseQuote += 25;

            if (insuree.CarYear < 2000 || insuree.CarYear > 2015) baseQuote += 25;
            if (insuree.CarMake.ToLower() == "porsche") baseQuote += 25;
            if (insuree.CarMake.ToLower() == "porsche" && insuree.CarModel.ToLower() == "911 carrera") baseQuote += 25;

            baseQuote += insuree.SpeedingTickets * 10;
            if (insuree.DUI) baseQuote *= 1.25m;
            if (insuree.CoverageType) baseQuote *= 1.5m;

            return baseQuote;
        }
    }
}

