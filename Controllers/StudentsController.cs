using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* FirstOrDefaultAsync method is used to call student's id from database in order to 
             * get all information of a student.
             * Then the Include method is used to load the Student.Enrollments navigation property and then
             * the ThenInclude method would be called to load Enrollments.Course in order to get each enrollment,
             * which is related to a student's id.
             * AsNoTracking makes sure that the entity or information in the Details page will not be updated in the current context.
             * Overall Purpose: to get a single student's information from database and then connect it to courses, which are registered with
             * the student id.
             */
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,EmailAddress,YearRank,AverageGrade,EnrollmentDate")] Student student)
        {

            /* ASP.NET Core MVC model binder is used to change posted values to CLR types and then pass them
             * to action method. The model binder [Bind(...)] here is used to create an initial Student entity using values from Form collection.
             * Removing ID in Bind attribute from create method makes sure that users cannot set ID for a student
             * since ID, the primary key in SQL server, will set automatically.
             * The try-catch block displays an error message when there is an exception from DbUpateException caught
             * Purpose: The code is modified to create a Student entity based on property values in Database using
             * ASP.NET MVC model binder. Also, model binder helps enhance the security of the website.
             */
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /*FirstOrDefaultAsync method is used to call student's id from database in order to 
        * get all information of a student. In order words, it reads the existing entity from database.
        * Then TryUpdateModelAsync is used to update the retrieved entity called by FirstOrDefaultAsync method
        * based on the input in the posted form data.
        * The SaveChangeAsync method is then called to make sure that row values of the database will be updated.
        * Overall Purpose: to call the existing entity from the database, let user edit it and then save any changes
        * to the database.
        */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstName, s => s.LastName, s => s.EnrollmentDate, 
                s => s.EmailAddress, s => s.YearRank, s => s.AverageGrade))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        /* Again, FirstOrDefaultAsync retrieves the selected entity from Student based on id.
         * if an id or student is null or not found, or if any errors occuring when the database is updated, then:
         * the try-block will handle the error.
         * the HttpPost Delete method calls the HttpGet Delete, and pass a parameter that tell HttpGet that
         * there is an error. Then HttpGet will display an error message to users, and ask users to try again or cancel
         * the delete.
         * Overall purpose: display a delete view for the user. If there are any errors, ask the user to cancel or try again
         * If even there is not an error and users click the delete button, there may be nothing changed in the database
         * since this is a controller for the view.
         * The entity can be deleted properly in the database with HttpPost method.
         */
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: Students/Delete/5

        /* The FindAsync method is used to find a primary key. If a primary key is not found, then it redirects  URL to chosen name,
         * that user wants to delete. This means it fails to delete an entity
         * Then the try-catch block runs to remove an actual entity in the database and check for errors.
         * If there are any errors, then it will pass a parameter to HttpGet method to display an error message
         * Overall Purpose: to delete an actual entity in the database and check for errors. 
         */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }



    }
}
