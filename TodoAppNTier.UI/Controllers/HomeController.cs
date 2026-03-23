using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Common.ResponseObjects;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.Services.WorkService;
using TodoAppNTier.UI.Models;
using TodoAppNTier.UI.Extensions;

namespace TodoAppNTier.UI.Controllers;

public class HomeController : Controller
{
    private readonly IWorkService _workService;

    public HomeController(IWorkService workService)
    {
        _workService = workService;
    }

    public async Task<IActionResult> Index()
    {



     var response = await  _workService.GetAll();
        return this.ResponseView(response);
    }


    public IActionResult Create()
    {
        return View(new WorkCreateDto());
    }


    [HttpPost]
        public async Task<IActionResult> Create(WorkCreateDto dto)
        {
            var response = await _workService.Create(dto);
            
            // DÜZELTME 2: Hata varsa form verilerini (dto) silmeden geri döndür.
            if (response.ResponseType == ResponseType.ValidationError)
                return this.ResponseViewError(response, dto);

            // Başarılıysa Index'e git
            return this.ResponseRedirectToAction(response, "Index");
        }


       [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        // 1. Service katmanı AutoMapper sayesinde bize zaten içi dolu, makyajlı bir DTO gönderiyor.
        var response = await _workService.GetById<WorkUpdateDto>(id);
        
        return this.ResponseView(response);
    }

        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> Update(WorkUpdateDto dto)
        {
            var response = await _workService.Update(dto);
            
            // Hata varsa hataları basıp DTO'yu geri dön
            if (response.ResponseType == ResponseType.ValidationError)
                return this.ResponseViewError(response, dto);

            // NotFound veya Başarılı durumlarını tek satırda hallet
            return this.ResponseRedirectToAction(response, "Index");
        }

        public async Task <IActionResult> Remove (int id)
        {

            
          var response = await _workService.Remove(id);

          if (response.ResponseType == ResponseType.NotFound)
    {
        TempData["ErrorMessage"] = response.Message; // "Silinmek istenen görev bulunamadı."
        return NotFound(); // 404 sayfasına yönlendir
    }

            
            return RedirectToAction("Index");
         }


    public IActionResult PageNotFound()
{
    return View();
}


}
