using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels;

public class VillaNumberUpdateVm
{
    public VillaNumberUpdateVm()
    {
        VillaNumber = new VillaNumberUpdateDto();
    }

    public VillaNumberUpdateDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
    
}