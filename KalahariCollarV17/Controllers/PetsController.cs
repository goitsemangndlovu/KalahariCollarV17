using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KalahariCollarV17.Data;
using KalahariCollarV17.Models;
using KalahariCollarV17.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using PagedList;
using System.Security.Claims;
using System.Drawing;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.Data.SqlClient;

namespace KalahariCollarV17.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private readonly PetContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PetsController> _logger;
        //string UserId;
        public PetsController(PetContext context, UserManager<ApplicationUser> userManager, ILogger<PetsController> logger)
        {
            _context = context;
            _userManager = userManager;
            //UserId = User.FindFirstValue(ClaimTypes.Email);
            _context = context;
            _logger = logger;
        }

        // GET: Pets
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var pets = from p in _context.Pets
                       select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                pets = pets.Where(p => p.Name.Contains(searchString));
            }
            Console.WriteLine(_context.Database.GetConnectionString());
            var petsFromDatabase = _context.Pets.Where(p => p.OwnerId.Equals(_userManager.GetUserId(User))).ToList();

            // Map the data to the view model
            var petViewModels = petsFromDatabase.Select(p => new Pet
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Breed = p.Breed,
                Location = p.Location,
                TrackerID = p.TrackerID,
                Age = p.Age,
                // Map other properties
            }).ToList();


            switch (sortOrder)
            {
                case "name_desc":
                    pets = pets.OrderByDescending(p => p.Name);
                    break;
                default:
                    pets = pets.OrderBy(p => p.Name);
                    break;
            }
            // var petContext = _context.Pet.Include(p => p.Owner);
            //  return View(await petContext.ToListAsync());
            // Pass the view model collection to the view
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(petViewModels.ToPagedList(pageNumber, pageSize));

        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                //.Include(p => p.OwnerId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = _userManager.GetUserId(User);
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrackerID,Name,Age,Type,Breed,OwnerId,Location")] Pet pet)
        {
            if (pet.Age > 0)
            {
                if (pet.TrackerID > 100000000000000 && pet.TrackerID < 999999999999999)
                {
                    Console.WriteLine(pet.Name);
                    if (!pet.Name.Equals("") && !pet.Type.Equals("") && !pet.Breed.Equals("") && !pet.Location.Equals("") && !pet.TrackerID.Equals("") && !pet.Age.Equals(""))
                    {
                        pet.OwnerId = _userManager.GetUserId(User); ;
                        Console.WriteLine(pet.OwnerId);
                        _context.Add(pet);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }


                    else
                    {
                        // Log ModelState errors
                        foreach (var modelState in ModelState.Values)
                        {
                            foreach (var error in modelState.Errors)
                            {
                                Console.WriteLine(error.ErrorMessage);
                            }
                        }
                    }
                    ViewData["OwnerId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pet.OwnerId);
                }
                else
                {
                    ModelState.AddModelError("TrackerID", "Tracker ID is the tracking device's IMEI number and should be 15 digits long, never starting with 0");
                }
            }
            else
            {
                ModelState.AddModelError("Age", "Age Cannot be Less than 0");
            }
            return View(pet);

        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }
          //  ViewData["OwnerId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pet.OwnerId);
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrackerID,Name,Age,Type,Breed,Location")] Pet pet)
        {
            if (pet.Age > 0)
            {
                if (pet.TrackerID > 100000000000000 && pet.TrackerID < 999999999999999)
                {
                    Console.WriteLine(pet.Name);
                    if (!pet.Name.Equals("") && !pet.Type.Equals("") && !pet.Breed.Equals("") && !pet.Location.Equals("") && !pet.TrackerID.Equals("") && !pet.Age.Equals(""))
                    {
                        pet.OwnerId = _userManager.GetUserId(User); 
                        Console.WriteLine(pet.OwnerId);
                        _context.Update(pet);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }


                    else
                    {
                        // Log ModelState errors
                        foreach (var modelState in ModelState.Values)
                        {
                            foreach (var error in modelState.Errors)
                            {
                                Console.WriteLine(error.ErrorMessage);
                            }
                        }
                    }
                    ViewData["OwnerId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pet.OwnerId);
                }
                else
                {
                    ModelState.AddModelError("TrackerID", "Tracker ID is the tracking device's IMEI number and should be 15 digits long, never starting with 0");
                }
            }
            else
            {
                ModelState.AddModelError("Age", "Age Cannot be Less than 0");
            }
            return View(pet);

           /* Console.WriteLine(pet.Name);
            if (id != pet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        //    ViewData["OwnerId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pet.OwnerId);
            return View(pet);
           */
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
               // .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pets == null)
            {
                return Problem("Entity set 'PetContext.Pet'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> Location()
        {
            return View();
        }
    }
}
