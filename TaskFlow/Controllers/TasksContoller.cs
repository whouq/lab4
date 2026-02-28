using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? status)
        {
            var tasks = _context.Tasks.ToList();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Models.TaskStatus>(status, out var statusValue))
            {
                tasks = tasks.Where(t => t.Status == statusValue).ToList();
            }

            ViewBag.CurrentStatus = status;
            return View(tasks);
        }

        public IActionResult Details(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description,Priority,Deadline,Creator,Assignee")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedAt = DateTime.Now;
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Description,Priority,Status,Deadline,CompletedAt,Creator,Assignee")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tasks.Any(e => e.Id == task.Id))
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
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
