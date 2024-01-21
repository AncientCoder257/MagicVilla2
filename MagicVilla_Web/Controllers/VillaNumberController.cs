using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModels;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaNumberController(
        IVillaNumberService villaNumberService, 
        IMapper mapper, 
        IVillaService villaService
        )
    {
        _villaNumberService = villaNumberService;
        _mapper = mapper;
        _villaService = villaService;
    }

    // GET
    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDto> list = new();

        var response = await _villaNumberService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(
                Convert.ToString(response.Result)
            );
        }
        return View(list);
    }
    // GET
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVm villaNumberVm = new VillaNumberCreateVm();
        var response = await _villaService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
            (Convert.ToString(response.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(villaNumberVm);
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVm villaNumberCreateVm)
    {
        if (ModelState.IsValid )
        {
            var response = await _villaNumberService.CreateAsync<ApiResponse>(villaNumberCreateVm.VillaNumber);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }
        var res = await _villaService.GetAllAsync<ApiResponse>();
        if (res != null && res.IsSuccess)
        {
            villaNumberCreateVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                    (Convert.ToString(res.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(villaNumberCreateVm);
    }
    // GET
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        VillaNumberUpdateVm villaNumberVm = new VillaNumberUpdateVm();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDto villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(villaNumberDto);
        }
        
        response = await _villaService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                    (Convert.ToString(response.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            return View(villaNumberVm);
        }
        return NotFound();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVm villaNumberUpdateVm)
    {
        if (ModelState.IsValid )
        {
            var response = await _villaNumberService.UpdateAsync<ApiResponse>(villaNumberUpdateVm.VillaNumber);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }
        var res = await _villaService.GetAllAsync<ApiResponse>();
        if (res != null && res.IsSuccess)
        {
            villaNumberUpdateVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                    (Convert.ToString(res.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(villaNumberUpdateVm);
    }
    // GET
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        VillaNumberDeleteVm villaNumberVm = new VillaNumberDeleteVm();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDto villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = villaNumberDto;
        }
        
        response = await _villaService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                    (Convert.ToString(response.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            return View(villaNumberVm);
        }
        return NotFound();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVm villaNumberDeleteVm)
    {
            var response = await _villaNumberService.DeleteAsync<ApiResponse>(villaNumberDeleteVm.VillaNumber.VillaNo);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        return View(villaNumberDeleteVm);
    }
}