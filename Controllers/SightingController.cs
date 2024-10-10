using BeltReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeltReview.Controllers;

[SessionCheck]
public class SightingController : Controller
{
    private readonly MyContext _context;

    public SightingController(MyContext context)
    {
        _context = context;
    }


    [HttpGet("sightings")]
    public ViewResult Dashboard()
    {
        List<Sighting> allSightings = _context.Sightings
                                        .Include(s => s.Owner)
                                        .Include(s => s.Believers)
                                        .ToList();
        return View(allSightings);
    }

    [HttpGet("sighting/{id}")]
    public IActionResult SightingDetails(int id)
    {
        {
            Sighting? foundSighting = _context.Sightings
                                        .Include(s => s.Owner)
                                        .Include(s => s.Believers)
                                        .ThenInclude(b => b.User)
                                        .FirstOrDefault(s => s.SightingId == id);
            if (foundSighting == null)
            {
                return RedirectToAction("Dashboard", "Sighting");
            }
            else
            {
                return View(foundSighting);
            }
        }
    }

    [HttpGet("sightings/new")]
    public ViewResult NewSightingForm()
    {
        return View();
    }

    [HttpPost("sightings/new")]
    public IActionResult NewSightingProcess(Sighting newSighting)
    {
        if (!ModelState.IsValid)
        {
            return View("NewSightingForm");
        }
        else
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId")!;
            newSighting.OwnerId = userId;
            _context.Add(newSighting);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Sighting");
        }
    }

    // This is the association post route.
    [HttpPost("sighting/believe")]
    public RedirectToActionResult BelieveSighting(int sightingId)
    {
        //get the association (UserSightingBeliever)
        int userId = (int)HttpContext.Session.GetInt32("UserId")!;
        UserSightingBeliever? foundAssociation = _context.UserSightingBelievers
                                                    .FirstOrDefault(b => b.UserId == userId && b.SightingId == sightingId);

        // if the association does not exist, add it
        if (foundAssociation == null)
        {
            UserSightingBeliever newAssociation = new() { UserId = userId, SightingId = sightingId };
            _context.UserSightingBelievers.Add(newAssociation);
            _context.SaveChanges();
        }

        //if the association exists, remove it. (Think LIKE and DISLIKE/REMOVE LIKE)
        else
        {
            _context.UserSightingBelievers.Remove(foundAssociation);
            _context.SaveChanges();
        }
        return RedirectToAction("Dashboard", "Sighting");
    }

    [HttpPost("sightings/{id}/delete")]
    public RedirectToActionResult DeleteSighting(int id)
    {
        Sighting? foundSighting = _context.Sightings.FirstOrDefault(s => s.SightingId == id);
        int userId = (int)HttpContext.Session.GetInt32("UserId")!;
        if (foundSighting == null || foundSighting.OwnerId != userId)
        {
            return RedirectToAction("Dashboard", "Sighting");
        }
        _context.Sightings.Remove(foundSighting);
        _context.SaveChanges();
        return RedirectToAction("Dashboard", "Sighting");
    }
}
