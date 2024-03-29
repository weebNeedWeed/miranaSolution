﻿namespace miranaSolution.Data.Entities;

public class Chapter
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ReadCount { get; set; }
    public int WordCount { get; set; }
    public string Content { get; set; } = string.Empty;

    public int BookId { get; set; }
    public Book? Book { get; set; }
}