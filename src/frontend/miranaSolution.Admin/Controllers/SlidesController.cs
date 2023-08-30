using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Slides;
using Newtonsoft.Json;
using Refit;

namespace miranaSolution.Admin.Controllers;

[AutoValidateAntiforgeryToken]
[Microsoft.AspNetCore.Authorization.Authorize]
public class SlidesController : Controller
{
    private readonly ISlidesApiService _slidesApiService;

    public SlidesController(ISlidesApiService slidesApiService)
    {
        _slidesApiService = slidesApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var getSlidesResult = await _slidesApiService.GetAllSlidesAsync();
        return View(getSlidesResult.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApiCreateSlideRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        var streamPart = new StreamPart(
            request.ThumbnailImage.OpenReadStream(), request.ThumbnailImage.FileName);

        var response = await _slidesApiService.CreateSlideAsync(
            request.Name,
            request.ShortDescription,
            request.Genres,
            request.SortOrder,
            request.Url,
            streamPart);
        
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _slidesApiService.DeleteSlideAsync(id);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        ViewData["SlideId"] = id;
        var getSlideResult = await _slidesApiService.GetSlideByIdAsync(id);

        var viewModel = new ApiUpdateSlideRequest(
            getSlideResult.Data.Name,
            getSlideResult.Data.ShortDescription,
            getSlideResult.Data.Genres,
            getSlideResult.Data.SortOrder,
            getSlideResult.Data.Url,
            null);

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ApiUpdateSlideRequest request)
    {
        ViewData["SlideId"] = id;
        if (!ModelState.IsValid) return View(request);

        StreamPart? streamPart = null;
        if (request.ThumbnailImage is not null)
        {
            streamPart = new StreamPart(
                request.ThumbnailImage.OpenReadStream(), request.ThumbnailImage.FileName);
        }
        
        var response = await _slidesApiService.UpdateSlideAsync(
            id,
            request.Name,
            request.ShortDescription,
            request.Genres,
            request.SortOrder,
            request.Url,
            streamPart);
        
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("Index");
    }
}
    