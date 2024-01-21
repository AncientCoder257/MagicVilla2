using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels;

public class VillaNumberCreateVm
{
    public VillaNumberCreateVm()
    {
        VillaNumber = new VillaNumberCreateDto();
    }

    public VillaNumberCreateDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
    
}