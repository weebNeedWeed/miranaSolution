﻿using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Genres;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.Genres;

public class GenreService : IGenreService
{
    private readonly MiranaDbContext _context;

    public GenreService(MiranaDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetAllGenresResponse> GetAllGenresAsync()
    {
        var genres = await _context.Genres
            .Select(x => new GenreVm(x.Id, x.Name))
            .ToListAsync();

        var response = new GetAllGenresResponse(genres);

        return response;
    }

    public async Task<GetGenreByIdResponse> GetGenreByIdAsync(GetGenreByIdRequest request)
    {
        var genre = await _context.Genres.FindAsync(request.GenreId);
        if (genre is null)
        {
            return new GetGenreByIdResponse(null);
        }

        var response = new GetGenreByIdResponse(
            new GenreVm(
                genre.Id,
                genre.Name));

        return response;
    }

    public async Task<CreateGenreResponse> CreateGenreAsync(CreateGenreRequest request)
    {
        var genre = new Genre
        {
            Name = request.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        
        var response = new CreateGenreResponse(
            new GenreVm(
                genre.Id,
                genre.Name));

        return response;
    }

    public async Task DeleteGenreAsync(DeleteGenreRequest request)
    {
        var genre = await _context.Genres.FindAsync(request.GenreId);
        if (genre is null)
        {
            throw new GenreNotFoundException("The genre with given Id does not exist.");
        }

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }

    public async Task<UpdateGenreResponse> UpdateGenreAsync(UpdateGenreRequest request)
    {
        var genre = await _context.Genres.FindAsync(request.GenreId);
        if (genre is null)
        {
            throw new GenreNotFoundException("The genre with given Id does not exist.");
        }
        
        genre.Name = request.Name;

        await _context.SaveChangesAsync();
        
        var response = new UpdateGenreResponse(
            new GenreVm(
                genre.Id,
                genre.Name));

        return response;
    }
}