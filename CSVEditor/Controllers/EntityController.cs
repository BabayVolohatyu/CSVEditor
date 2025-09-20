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
        [HttpGet("Index")]
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("UploadCsv")]
        public async Task<IActionResult> UploadCsv(IFormFile file, string mode)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File wasn't loaded");

            try
            {
                if (mode == "rewrite")
                {
                    await _context.Database.ExecuteSqlRawAsync("DELETE FROM Entities");
                }

                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                const int batchSize = 1000;
                var batch = new List<Entity>();

                await foreach (var record in csv.GetRecordsAsync<Entity>())
                {
                    record.Id = 0;
                    batch.Add(record);

                    if (batch.Count >= batchSize)
                    {
                        _context.Entities.AddRange(batch);
                        await _context.SaveChangesAsync();
                        batch.Clear();
                    }
                }

                if (batch.Count > 0)
                {
                    _context.Entities.AddRange(batch);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                
                var inner = ex.InnerException != null ? ex.InnerException.Message : "";
                return BadRequest("Error processing CSV: " + ex.Message + " | Inner: " + inner);
            }

            return RedirectToAction("Table");
        }


        [HttpGet("LoadFromDb")]
        public IActionResult LoadFromDb()
        {
            return RedirectToAction("Table");
        }

        [HttpGet("Table")]
        public async Task<IActionResult> Table()
        {
            var entities = await _context.Entities.ToListAsync();
            return View(entities);
        }

        [HttpPost("UpdateField")]
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
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Entity entity)
        {
            if (ModelState.IsValid)
            {
                _context.Entities.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Table");
            }
            return View(entity);
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
