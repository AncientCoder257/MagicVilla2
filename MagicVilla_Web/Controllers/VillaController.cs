using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }

    // GET
    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDto> list = new();

        var response = await _villaService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(
                Convert.ToString(response.Result)
                );
        }
        return View(list);
    }
    // GET
    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDto villaCreateDto)
    {
        if (ModelState.IsValid )
        {
            var response = await _villaService.CreateAsync<ApiResponse>(villaCreateDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Villa encountered";
        return View(villaCreateDto);
    }
    // GET
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaService.GetAsync<ApiResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDto villaDto = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDto>(villaDto));
        }
        return NotFound();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDto villaUpdateDto)
    {
        if (ModelState.IsValid )
        {
            TempData["success"] = "Villa updated successfully";
            var response = await _villaService.UpdateAsync<ApiResponse>(villaUpdateDto);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Villa encountered";
        return View(villaUpdateDto);
    }
    // GET
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaService.GetAsync<ApiResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDto villaDto = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(villaDto);
        }
        return NotFound();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDto villaDto)
    {
            var response = await _villaService.DeleteAsync<ApiResponse>(villaDto.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Villa encountered";
        return View(villaDto);
    }
    
}