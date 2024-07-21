using ET.Base.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ET.Data.Entities
{
    public class Transaction : BaseEntityWithId
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.InsertDate).IsRequired(true);
            builder.Property(x => x.InsertUserId).IsRequired(true);
            builder.Property(x => x.UpdateDate).IsRequired(false);
            builder.Property(x => x.UpdateUserId).IsRequired(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

            builder.Property(x => x.Amount).IsRequired(true).HasPrecision(18, 4);
            builder.Property(x => x.Description).IsRequired(true).HasMaxLength(300);
            builder.Property(x => x.TransactionDate).IsRequired(true);

            builder.HasIndex(x => x.UserId);

        }
    }
}
