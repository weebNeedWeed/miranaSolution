using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Systems.Slides;

namespace miranaSolution.Business.Systems.Slides
{
    public class SlideService : ISlideService
    {
        private readonly MiranaDbContext _context;
        private readonly IMapper _slideDtoMapper;

        public SlideService(MiranaDbContext context)
        {
            _context = context;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Slide, SlideDto>());
            _slideDtoMapper = config.CreateMapper();
        }

        public async Task<SlideDto> Create(SlideCreateRequest request)
        {
            var cfg = new MapperConfiguration(cfg => cfg.CreateMap<SlideCreateRequest, Slide>());
            var mapper = cfg.CreateMapper();

            var newSlide = mapper.Map<Slide>(request);

            await _context.Slides.AddAsync(newSlide);

            await _context.SaveChangesAsync();

            return _slideDtoMapper.Map<SlideDto>(newSlide);
        }

        public async Task<List<SlideDto>> GetAll()
        {
            var slides = await _context.Slides
                .OrderBy(x => x.SortOrder)
                .Select(x => _slideDtoMapper.Map<SlideDto>(x))
                .ToListAsync();

            return slides;
        }

        public async Task<SlideDto> GetById(int id)
        {
            var slide = await _context.Slides.FindAsync(id);
            return _slideDtoMapper.Map<SlideDto>(slide);
        }
    }
}