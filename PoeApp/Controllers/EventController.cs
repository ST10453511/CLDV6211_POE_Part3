using PoeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PoeApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                events = events.Where(e => e.EventType.Name == searchType);

            if (venueId.HasValue)
                events = events.Where(e => e.VenueID == venueId);

            if (startDate.HasValue && endDate.HasValue)
                events = events.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);

            ViewData["Venue"] = _context.Venue.ToList();

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(await events.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Venue = _context.Venue.ToList();

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@events);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Venue = _context.Venue.ToList();

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(@events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @events = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@events == null) return NotFound();

            return View(@events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @events = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@events == null) return NotFound();

            return View(@events);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @events = await _context.Event.FindAsync(id);
            if (@events == null) return NotFound();

            var isBooked = await _context.Booking.AnyAsync(b => b.EventID == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Event.Remove(@events);
            await _context.SaveChangesAsync();
            TempData["ErrorMessage"] = "Event deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @events = await _context.Event.FindAsync(id);
            if (@events == null) return NotFound();

            ViewBag.VenueID = new SelectList(await _context.Venue.ToListAsync(), "VenueID", "VenueName", events.VenueID);

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(@events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @events)
        {
            if (id != @events.EventID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(@events);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueID = new SelectList(await _context.Venue.ToListAsync(), "VenueID", "VenueName", events.VenueID);

            ViewData["EventTypes"] = _context.EventType.ToList();
            return View(@events);
        }
    }
}