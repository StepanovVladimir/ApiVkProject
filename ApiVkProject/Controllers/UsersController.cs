using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApiVkProject.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace ApiVkProject.Controllers
{
    public class UsersController : Controller
    {
        public UsersController(ApplicationContext context)
        {
            _context = context;
            _vkApi = new VkApi();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            _vkApi.Authorize(new VkNet.Model.ApiAuthParams
            {
                AccessToken = "8c4dd8cf5b8e9a2990dc1f9ff82d4d0494325d019469dddcec425e63e6871b45baf7e3e57452de5f2c427"
            });

            var subscribers = _vkApi.Users.SearchAsync(new UserSearchParams
            {
                GroupId = 183744420,
                Count = 100,
                Fields = ProfileFields.FirstName | ProfileFields.LastName | ProfileFields.Photo100
            });

            return View(await subscribers);

            //return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Photo")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Photo")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private readonly ApplicationContext _context;
        private VkApi _vkApi;

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
