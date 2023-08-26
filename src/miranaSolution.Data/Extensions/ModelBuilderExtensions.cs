using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder builder)
    {
        builder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                Name = "Trạch Trư"
            },
            new Author
            {
                Id = 2,
                Name = "Phật Tiền Hiến Hoa"
            });

        builder.Entity<Genre>().HasData(
            new Genre
            {
                Id = 1,
                Name = "Khoa Huyễn"
            },
            new Genre
            {
                Id = 2,
                Name = "Võng Du"
            });

        builder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Name = "MỤC THẦN KÝ",
                ShortDescription =
                    "Đại Khư Tàn Lão Thôn, một đứa bé được những người già nhặt được ở bờ sống, đặt tên Tần Mục, tân tân khổ khổ nuôi hắn trưởng thành. Một ngày kia bóng đem buông xuống, bóng tối bao trùm Đại Khư, Tần Mục bước ra khỏi nhà...",
                LongDescription =
                    "Đại Khư Tàn Lão Thôn, một đứa bé được những người già nhặt được ở bờ sống, đặt tên Tần Mục, tân tân khổ khổ nuôi hắn trưởng thành. Một ngày kia bóng đem buông xuống, bóng tối bao trùm Đại Khư, Tần Mục bước ra khỏi nhà...",
                ThumbnailImage =
                    "https://static.8cache.com/cover/eJzLyTDWr0r0d4svDfJwdUsMTSrKNfUzSYwqDLUsNHV0snT1zy7wcXT28PBJ9gp2MU93TC3JDwjJzC60LDZ29M-PcCtI8wwJMk2q8nXy88kqDSx3zQtKCTSwLTcyNNXNMDYyAgAc-B9d/muc-than-ky.jpg",
                IsRecommended = true,
                Slug = "muc-than-ky",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AuthorId = 1
            },
            new Book
            {
                Id = 2,
                Name = "Khủng Bố Sống Lại",
                ShortDescription =
                    "Ngũ trọc ác thế,Địa ngục đã không,ác quỷ sống lại,nhân gian như ngục.Thế giới này quỷ xuất hiện. . .Như vậy thần lại ở đâu ? Cầu thần cứu thế,có thể trên đời đã mất thần,chỉ có quỷ.",
                LongDescription =
                    "Ngũ trọc ác thế,Địa ngục đã không,ác quỷ sống lại,nhân gian như ngục.Thế giới này quỷ xuất hiện. . .Như vậy thần lại ở đâu ? Cầu thần cứu thế,có thể trên đời đã mất thần,chỉ có quỷ.",
                ThumbnailImage = "https://static.cdnno.com/poster/khung-bo-song-lai/300.jpg?1585205957",
                IsRecommended = true,
                Slug = "khung-bo-song-lai",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AuthorId = 2
            });

        builder.Entity<Chapter>()
            .HasData(new Chapter
                {
                    Id = 2,
                    Index = 1,
                    Name = "Trời tối, đừng ra ngoài",
                    ReadCount = 0,
                    WordCount = 0,
                    Content =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    BookId = 1
                },
                new Chapter
                {
                    Id = 3,
                    Index = 2,
                    Name = "Trời tối, đừng ra ngoài 2",
                    ReadCount = 0,
                    WordCount = 0,
                    Content =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    BookId = 1
                },
                new Chapter
                {
                    Id = 1,
                    Index = 1,
                    Name = "Trong diễn đàn quỷ cố sự",
                    ReadCount = 0,
                    WordCount = 0,
                    Content =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    BookId = 2
                });

        builder.Entity<ChapterRelationship>()
            .HasData(new ChapterRelationship { FromId = 2, ToId = 3});
        
        builder.Entity<BookGenre>().HasData(
            new BookGenre { BookId = 1, GenreId = 1 },
            new BookGenre { BookId = 2, GenreId = 2 },
            new BookGenre { BookId = 2, GenreId = 1 });

        var adminRoleId = Guid.NewGuid();
        var userRoleId = Guid.NewGuid();

        var adminUserId = Guid.NewGuid();

        builder.Entity<AppRole>().HasData(new AppRole
            {
                Id = adminRoleId,
                Name = RolesConstant.Administrator,
                Description = RolesConstant.Administrator,
                NormalizedName = RolesConstant.Administrator.ToUpper()
            },
            new AppRole
            {
                Id = userRoleId,
                Name = RolesConstant.User,
                Description = RolesConstant.User,
                NormalizedName = RolesConstant.User.ToUpper()
            });

        builder.Entity<AppUser>().HasData(new AppUser
        {
            Id = adminUserId,
            FirstName = "Admin",
            LastName = "Admin",
            UserName = "adminadmin",
            NormalizedUserName = "ADMINADMIN",
            PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, "@Admin123"),
            Email = "admin@admin.com",
            SecurityStamp = Guid.NewGuid().ToString()
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = adminRoleId,
            UserId = adminUserId
        });
    }
}