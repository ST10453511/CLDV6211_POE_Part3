﻿using PoeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoeApp.Models;

namespace PoeApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.Venue.VenueName.Contains(searchString) ||
                    b.Event.EventName.Contains(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Event"] = _context.Event.ToList();
            ViewData["Venue"] = _context.Venue.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var selectedEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventID == booking.EventID);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "selected event not found.");
                ViewData["Event"] = _context.Event.ToList();
                ViewData["Venue"] = _context.Venue.ToList();
                return View(booking);
            }

            var conflict = await _context.Booking
               .Include(b => b.Event)
               .AnyAsync(b => b.VenueID == booking.VenueID &&
                              b.Event.EventDate.Date == selectedEvent.EventDate.Date);

            if (conflict)
            {
                ModelState.AddModelError("", "This venue is already booked for that date");
            }


            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Event"] = _context.Event.ToList();
            ViewData["Venue"] = _context.Venue.ToList();
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(b => b.BookingID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewBag.EventID = new SelectList(_context.Event.ToList(), "EventID", "EventName", booking.EventID);
            ViewBag.VenueID = new SelectList(_context.Venue.ToList(), "VenueID", "VenueName", booking.VenueID);

            return View(booking);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID))
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
            ViewData["VenueID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Venue.ToListAsync(), "VenueID", "VenueName", booking.VenueID);
            ViewData["EventID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Event.ToListAsync(), "EvenetID", "EventName", booking.EventID);

            return View(booking);
        }
    }
}