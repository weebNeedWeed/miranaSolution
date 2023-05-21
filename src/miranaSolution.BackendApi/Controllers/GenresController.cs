using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Genres;
using miranaSolution.Dtos.Catalog.Genres;
using miranaSolution.Dtos.Common;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _genreService.GetAll();
            return Ok(new ApiSuccessResult<List<GenreDto>>(data));
        }
    }
}