using Intaker.TaskManagementSystem.domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intaker.TaskManagementSystem.domain.TaskManagement.Configurations
{
    public class TaskConfiguration : BaseConfiguration, IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Settings.NameMaxLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(Settings.DescriptionMaxLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.AssignedTo)
                .IsRequired(Settings.No)
                .HasMaxLength(Settings.AssignedToMaxLength);

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(CreatedByMaxLength);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedBy)
                .IsRequired()
                .HasMaxLength(UpdatedByMaxLength);

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }

        public class Settings
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
            public const int DescriptionMaxLength = 50;
            public const int DescriptionMinLength = 2;
            public const int AssignedToMaxLength = 50;
            public const int AssignedToMinLength = 2;
            public const bool No = false;
        }
    }
}
