using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels;

public class VillaNumberDeleteVm
{
    public VillaNumberDeleteVm()
    {
        VillaNumber = new VillaNumberDto();
    }

    public VillaNumberDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
    
}