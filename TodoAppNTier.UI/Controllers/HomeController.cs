using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // KULLANICILAR İÇİN EKLENDİ
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Common.ResponseObjects;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.UI.Extensions;

namespace TodoAppNTier.UI.Controllers
{
    [Authorize] // 1. GÜVENLİK KİLİDİ: Ziyaretçiler giremez!
    public class HomeController : Controller
    {
        private readonly IWorkService _workService;
        private readonly UserManager<AppUser> _userManager; // 2. KULLANICI UZMANI EKLENDİ

        public HomeController(IWorkService workService, UserManager<AppUser> userManager)
        {
            _workService = workService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // A) O an giriş yapan kişiyi sistemden çek
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // B) Bütün görevleri getir
            var response = await _workService.GetAll();

            // C) SADECE BU KULLANICIYA AİT OLANLARI FİLTRELE (Başkalarının görevlerini gizle)
            if (response.ResponseType == ResponseType.Success && response.Data != null)
            {
                response.Data = response.Data.Where(x => x.AppUserId == user.Id).ToList();
            }

            return this.ResponseView(response);
        }

        public IActionResult Create()
        {
            return View(new WorkCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkCreateDto dto)
        {
            // A) Görevi ekleyen kişiyi bul
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            // B) Görevi o kişiye zimmetle! (Veritabanına "Sahibi Budur" diyoruz)
            dto.AppUserId = user.Id;

            var response = await _workService.Create(dto);
            
            if (response.ResponseType == ResponseType.ValidationError)
                return this.ResponseViewError(response, dto);

            return this.ResponseRedirectToAction(response, "Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _workService.GetById<WorkUpdateDto>(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // GÜVENLİK (IDOR Koruması): Başkasının görev ID'sini yazarak açmaya çalışırsa engelle!
            if (response.ResponseType == ResponseType.Success && response.Data.AppUserId != user.Id)
            {
                return RedirectToAction("Index"); // Çaktırmadan anasayfaya geri yolla
            }

            return this.ResponseView(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(WorkUpdateDto dto)
        {
            // Hacklenmemek için Update ederken kullanıcının ID'sini zorla kendimiz basıyoruz
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            dto.AppUserId = user.Id;

            var response = await _workService.Update(dto);
            
            if (response.ResponseType == ResponseType.ValidationError)
                return this.ResponseViewError(response, dto);

            return this.ResponseRedirectToAction(response, "Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            // Silinmek istenen görevi ve o anki kullanıcıyı bul
            var response = await _workService.GetById<WorkListDto>(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // GÜVENLİK: Eğer görev başkasına aitse silmesine KESİNLİKLE izin verme!
            if (response.ResponseType == ResponseType.Success && response.Data.AppUserId != user.Id)
            {
                return RedirectToAction("Index"); 
            }

            var removeResponse = await _workService.Remove(id);

            if (removeResponse.ResponseType == ResponseType.NotFound)
            {
                TempData["ErrorMessage"] = removeResponse.Message; 
                return NotFound(); 
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}