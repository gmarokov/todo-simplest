using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using todo_app.Data;
using todo_app.Models;

namespace todo_app.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly DefaultDbContext _context;
        private readonly UserManager<User> _userManager;

        public ToDosController(DefaultDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDos
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDos.Where(x =>
                !x.IsDone && x.UserId == _userManager.GetUserId(HttpContext.User))
            .ToListAsync());
        }

        // GET: ToDos/Archive
        public async Task<IActionResult> Archive()
        {
            return View(await _context.ToDos.Where(x =>
                x.IsDone && x.UserId == _userManager.GetUserId(HttpContext.User))
            .ToListAsync());
        }

        // GET: ToDos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // GET: ToDos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsDone")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                toDo.UserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
        }

        // GET: ToDos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }
            return View(toDo);
        }

        // POST: ToDos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsDone")] ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDo.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.Id))
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
            return View(toDo);
        }

        // GET: ToDos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // POST: ToDos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDo = await _context.ToDos
                .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                .FirstOrDefaultAsync(x => x.Id == id);
                
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }
    }
}
