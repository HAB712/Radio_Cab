using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RadioCab.Models;
using System.Security.Claims;


namespace RadioCab.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext context;

        public AccountController(AppDbContext context)
        {
           this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAdmin()
        {
            var check = context.Users.FirstOrDefault(x => x.Email == "admin@gmail.com");
            if (check == null)
            {
                var register = new User
                {
                    FullName = "Super Admin",
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Phone = "89856427890",
                    CreatedAt = DateTime.Now,
                    Status = "Active",
                    Role = "Admin"
                };

                context.Users.Add(register);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return Content("Admin already exists in database.");
        }



        public IActionResult CompanyRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CompanyRegister(UserValidate vm)
        {

            var userExists = context.Users.FirstOrDefault(u => u.Email == vm.Email || u.Phone == vm.Phone);

            if (userExists != null)
            {
                if (userExists.Email == vm.Email)
                    ModelState.AddModelError("Email", "Email already exists");

                if (userExists.Phone == vm.Phone)
                    ModelState.AddModelError("Phone", "Phone number already exists");

                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                Phone = vm.Phone,
                Role = "Company",
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("Login");
        }


        public IActionResult DriverRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DriverRegister(UserValidate vm)
        {

            var userExists = context.Users.FirstOrDefault(u => u.Email == vm.Email || u.Phone == vm.Phone);

            if (userExists != null)
            {
                if (userExists.Email == vm.Email)
                    ModelState.AddModelError("Email", "Email already exists");

                if (userExists.Phone == vm.Phone)
                    ModelState.AddModelError("Phone", "Phone number already exists");

                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                Phone = vm.Phone,
                Role = "Driver",
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserValidate vm)
        {

            var existingEmail = context.Users.Any(u => u.Email == vm.Email);
            var existingPhone = context.Users.Any(p => p.Phone == p.Phone);

            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(vm);
            }

            if (existingPhone != null)
            {
                ModelState.AddModelError("Phone", "Phone Number already exists");
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),   
                Phone = vm.Phone,
                Role = "User",
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User vm)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == vm.Email);

            if (user == null)
            {
                ViewBag.Error = "Email Not Exits";
                return View();
            }

            if (!BCrypt.Net.BCrypt.Verify(vm.Password, user.Password))
            {
                ViewBag.Error = "Invalid  password";
                return View();
            }

      
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.FullName),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.MobilePhone, user.Phone),
               new Claim("UserID", user.UserID.ToString()),
               new Claim(ClaimTypes.Role, user.Role)

            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identity));

            
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.Role == "Driver")
            {
                return RedirectToAction("Index", "Driver");
            }
            else if (user.Role == "Company")
            {
                return RedirectToAction("Index", "Company");
            }
            else if (user.Role == "User")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid Credantials";
                return View();
            }
        }


        public IActionResult RegCard() { 
        return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

    }
}
