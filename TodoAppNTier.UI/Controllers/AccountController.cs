using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoAppNTier.Dtos.AppUserDtos;
using TodoAppNTier.Entities.Concrete;

namespace TodoAppNTier.UI.Controllers
{
    public class AccountController : Controller
    {
        // Identity'nin kullanıcıları veritabanına güvenle kaydeden baş mimarı!
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 1. Kullanıcı siteye girdiğinde ona boş kayıt formunu (View) gösterir
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserRegisterDto());
        }

        // 2. Kullanıcı formu doldurup "Kayıt Ol" butonuna bastığında burası çalışır
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                // DTO'daki bilgileri gerçek AppUser nesnesine aktarıyoruz
                AppUser appUser = new AppUser
                {
                    UserName = dto.Username,
                    Email = dto.Email,
                    Name = "Bilinmiyor", // Şimdilik varsayılan değer veriyoruz
                    Surname = "Bilinmiyor"
                };

                // CreateAsync komutu şifreyi otomatik Hash'leyip (kriptolayıp) kaydeder!
                var result = await _userManager.CreateAsync(appUser, dto.Password);

                if (result.Succeeded)
                {
                    // Kayıt başarılıysa şimdilik anasayfaya yollayalım (İleride Login'e yollarız)
                    return RedirectToAction("Index", "Home");
                }

                // Eğer şifre kurallarına uyulmazsa (örn: çok kısa), hataları UI'a gönderir
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Hata varsa kullanıcının girdiği bilgilerle formu geri açar
            return View(dto);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserLoginDto());
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                // Önce veritabanında böyle bir kullanıcı adı var mı diye bakıyoruz
                var user = await _userManager.FindByNameAsync(dto.Username);
                
                if (user != null)
                {
                    // Kullanıcıyı bulduysak, şifresi doğru mu diye kapı şefine (SignInManager) soruyoruz
                    // false: Şifreyi 5 kere yanlış girerse hesabı kilitleme
                    var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); // Başarılıysa Anasayfaya gönder
                    }
                }

                // Kullanıcı adı yoksa veya şifre yanlışsa genel bir hata mesajı veriyoruz
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            }

            return View(dto);
        }

        // YENİ 3: ÇIKIŞ YAP (GÜVENLİ ÇIKIŞ)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Çerezleri (Cookie) temizle
            return RedirectToAction("Index", "Home"); // Çıkış yapınca anasayfaya at
        }
    }
}

        

    