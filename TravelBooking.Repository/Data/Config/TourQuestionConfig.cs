using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Configurations;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data.Config
{
    public class TourQuestionConfig : BaseEntityConfiguration<TourQuestion>
    {
        public override void Configure(EntityTypeBuilder<TourQuestion> builder)
        {
            base.Configure(builder);

            builder.Property(q => q.QuestionText).HasMaxLength(500).IsRequired();
            builder.Property(q => q.AnswerText).HasMaxLength(1000).IsRequired();
        }
    }
}
