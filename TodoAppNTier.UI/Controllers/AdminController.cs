using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.DataAccess.Contexts;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.UI.Models; // Rol Modeli için gerekli

namespace TodoAppNTier.UI.Controllers
{
    // Sadece Admin rolü olanlar girebilir
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager; // ROL YÖNETİCİSİ EKLENDİ
        private readonly TodoContext _context;

        // DI (Bağımlılık Enjeksiyonu)
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, TodoContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // --- ANA KONTROL MERKEZİ (DASHBOARD) ---
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalWorks = await _context.Works.CountAsync();
            ViewBag.CompletedWorks = await _context.Works.CountAsync(x => x.IsCompleted);
            ViewBag.PendingWorks = ViewBag.TotalWorks - ViewBag.CompletedWorks;
            return View();
        }

        // --- KULLANICI YÖNETİMİ ---
        public IActionResult UserList()
        {
            var users = _userManager.Users.Where(x => x.UserName != "admin").ToList();
            return View(users);
        }

        // ESKİ MakeAdmin METODUNUN YERİNİ ALAN YENİ ROL ATAMA METOTLARI
        
        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()); // Kullanıcıyı ID'sine göre bul
            if (user == null) return NotFound();

            var roles = _roleManager.Roles.ToList();  // Sistemdeki tüm rolleri çek
            var userRoles = await _userManager.GetRolesAsync(user); // Kullanıcının sahip olduğu rolleri çek

            var roleAssignViewModels = new List<RoleAssignViewModel>(); 
            foreach (var role in roles) // Tüm roller arasında dön
            {
                roleAssignViewModels.Add(new RoleAssignViewModel // Her rol için bir RoleAssignViewModel oluştur
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name) // Rol adamda varsa Checkbox tikli gelsin
                });
            }

            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(int userId, List<RoleAssignViewModel> model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            foreach (var item in model)
            {
                if (item.HasRole && !await _userManager.IsInRoleAsync(user, item.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName); // Tiklendi -> Rolü Ver
                }
                else if (!item.HasRole && await _userManager.IsInRoleAsync(user, item.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName); // Tik Kalktı -> Rolü Al
                }
            }
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null && user.UserName != User.Identity.Name) 
                await _userManager.DeleteAsync(user); 
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> BanUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null && user.UserName != User.Identity.Name)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            return RedirectToAction("UserList");
        }

        // --- SİSTEMDEKİ TÜM GÖREVLERİN YÖNETİMİ ---
        public async Task<IActionResult> AllWorks(string searchString, string statusFilter)
        {
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentFilter = statusFilter;

            var worksQuery = _context.Works.Include(x => x.AppUser).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); 
                worksQuery = worksQuery.Where(w => w.Definition.ToLower().Contains(searchString) || 
                                                   (w.AppUser != null && w.AppUser.UserName.ToLower().Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "completed") worksQuery = worksQuery.Where(w => w.IsCompleted == true);
                else if (statusFilter == "pending") worksQuery = worksQuery.Where(w => w.IsCompleted == false);
            }

            var filteredWorks = await worksQuery.ToListAsync();
            return View(filteredWorks);
        }

        public async Task<IActionResult> DeleteWork(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work != null)
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AllWorks");
        }
    }
}