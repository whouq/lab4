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

        public async Task<IActionResult> Index(string? status, string? priority)
        {
            var tasks = _context.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Models.TaskStatus>(status, out var statusValue))
            {
                tasks = tasks.Where(t => t.Status == statusValue);
            }

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskPriority>(priority, out var priorityValue))
            {
                tasks = tasks.Where(t => t.Priority == priorityValue);
            }

            var taskList = await tasks
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.Deadline)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPriority = priority;

            return View(taskList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);

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
        public async Task<IActionResult> Create([Bind("Title,Description,Priority,Status,Deadline,Creator,Assignee,Progress")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedAt = DateTime.Now;
                _context.Add(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Задача успешно создана!";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Priority,Status,Deadline,CompletedAt,Creator,Assignee,Progress")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    if (task.Status == Models.TaskStatus.Completed && task.CompletedAt == null)
                    {
                        task.CompletedAt = DateTime.Now;
                    }
                    else if (task.Status != Models.TaskStatus.Completed)
                    {
                        task.CompletedAt = null;
                    }

                    _context.Update(task);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Задача успешно обновлена!";
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Задача удалена!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, Models.TaskStatus status)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.Status = status;

                if (status == Models.TaskStatus.Completed)
                {
                    task.CompletedAt = DateTime.Now;
                    task.Progress = 100;
                }
                else if (status == Models.TaskStatus.Cancelled)
                {
                    task.CompletedAt = null;
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int id, int progress)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.Progress = progress;

                if (progress == 100 && task.Status != Models.TaskStatus.Completed)
                {
                    task.Status = Models.TaskStatus.Completed;
                    task.CompletedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
