using CSVEditor.Data;
using CSVEditor.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CSVEditor.Controllers
{
    public class EntityController : Controller
    {
        private readonly CsvAppDbContext _context;
        public EntityController(CsvAppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile file, string mode)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File wasn't loaded");

            List<Entity> records;
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Entity>().ToList();
            }

            if (mode == "rewrite")
            {
                _context.Entities.RemoveRange(_context.Entities);
                await _context.SaveChangesAsync();
            }

            _context.Entities.AddRange(records);
            await _context.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table()
        {
            var entities = await _context.Entities.ToListAsync();
            return View(entities);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateField([FromBody] UpdateFieldRequest request)
        {
            var entity = await _context.Entities.FindAsync(request.Id);
            if (entity == null) return NotFound();

            switch (request.Field)
            {
                case "Name":
                    entity.Name = request.Value;
                    break;
                case "DateOfBirth":
                    if (DateTimeOffset.TryParse(request.Value, out var dob))
                        entity.DateOfBirth = dob;
                    break;
                case "Married":
                    if (bool.TryParse(request.Value, out var married))
                        entity.Married = married;
                    break;
                case "Phone":
                    entity.Phone = request.Value;
                    break;
                case "Salary":
                    if (float.TryParse(request.Value, out var salary))
                        entity.Salary = salary;
                    break;
                default:
                    return BadRequest("Unknown");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
