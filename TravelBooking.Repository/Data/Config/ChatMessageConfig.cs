using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Configurations;
using TravelBooking.Core.Models.ChatBot;
using static Grpc.Core.Metadata;

namespace TravelBooking.Repository.Data.Config
{
    public class ChatMessageConfig : BaseEntityConfiguration<ChatMessage>
    {
        public override void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.Response).IsRequired().HasMaxLength(4000);
            builder.Property(e => e.SessionId).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Context).HasColumnType("nvarchar(max)");
            builder.HasIndex(e => e.SessionId);
            builder.HasIndex(e => e.Timestamp);
            builder.HasIndex(e => e.MessageType);
            builder.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
