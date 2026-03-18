using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoAppNTier.Business.Interfaces;
using TodoAppNTier.Dtos.WorkDtos;
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
}
