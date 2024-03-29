﻿using Microsoft.AspNetCore.Identity;

namespace miranaSolution.Data.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public int ReadChapterCount { get; set; }
    public int ReadBookCount { get; set; }

    public List<Comment> Comments { get; set; } = new();

    public List<Bookmark> Bookmarks { get; set; } = new();

    public List<CommentReaction> CommentReactions { get; set; } = new();

    public List<BookUpvote> BookUpvotes { get; set; } = new();

    public List<BookRating> BookRatings { get; set; } = new();
    
    public List<CurrentlyReading> CurrentlyReadings { get; set; } = new();
}