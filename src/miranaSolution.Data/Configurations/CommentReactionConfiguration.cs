using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
{
    public void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        builder.ToTable("CommmentReactions");

        builder.HasKey(x => new
        {
            x.UserId,
            x.CommentId
        });

        builder.HasOne(x => x.Comment)
            .WithMany(x => x.CommentReactions)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.CommentReactions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}