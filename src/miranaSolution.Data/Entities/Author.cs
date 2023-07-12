﻿namespace miranaSolution.Data.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = new();
}