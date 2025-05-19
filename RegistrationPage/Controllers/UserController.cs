using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationPage.Data;
using RegistrationPage.Models;

namespace RegistrationPage.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]

        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Password = model.Password
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "User has been registered successfully.";
            return RedirectToAction("Login");

        }
        
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        public async Task<IActionResult> List()
        {
            var user = await dbContext.Users.ToListAsync();
            return View(user);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            
            user.FullName = updatedUser.FullName;
            user.MobileNumber = updatedUser.MobileNumber;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.Gender = updatedUser.Gender;
            

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List"); 
        }

        [HttpPost]

        public IActionResult Logout()
        {
            
            return RedirectToAction("Login");
        }

    }
}

        // Dashboard GET
       /** public IActionResult Dashboard()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return RedirectToAction("Login");
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            // In a real application, clear the session/cookie here
            return RedirectToAction("Login");
        }
    }
}*/
