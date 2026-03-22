using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Dtos.WorkDtos;
using TodoAppNTier.Dtos.WorkUpdateDtos;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.Services.WorkService;
using TodoAppNTier.UI.Models;

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
     var workList = await  _workService.GetAll();
        return View(workList);
    }


    public IActionResult Create()
    {
        return View(new WorkCreateDto());
    }


    [HttpPost]
    public async Task<IActionResult> Create(WorkCreateDto dto)
    {
        if (ModelState.IsValid)
        {
             await _workService.Create(dto);

            return RedirectToAction("Index");
        }

        
        return View(dto);
    }


       [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        // 1. Service katmanı AutoMapper sayesinde bize zaten içi dolu, makyajlı bir DTO gönderiyor.
        var dto = await _workService.GetById<WorkUpdateDto>(id);
        
        // 2. Biz de o kutuyu hiç bozmadan doğrudan ekrana (View'a) basıyoruz!
        return View(dto);
    }

        [HttpPost]

        public async Task<IActionResult> Update(WorkUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _workService.Update(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task <IActionResult> Remove (int id)
        {
            await _workService.Remove(id);
            return RedirectToAction("Index");
         }




}
