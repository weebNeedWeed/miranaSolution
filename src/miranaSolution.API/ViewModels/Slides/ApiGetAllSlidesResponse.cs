using miranaSolution.DTOs.Core.Slides;

namespace miranaSolution.API.ViewModels.Slides;

public record ApiGetAllSlidesResponse(
    List<SlideVm> Slides);