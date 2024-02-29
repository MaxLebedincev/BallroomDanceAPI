﻿using BallroomDanceAPI.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BallroomDanceAPI.DAL.Configurations
{
    public class TypeBallroomDanceConfiguration : IEntityTypeConfiguration<TypeBallroomDance>
    {
        public void Configure(EntityTypeBuilder<TypeBallroomDance> builder)
        {
            builder.ToTable("TypesBallroomDance");
            
            builder.HasKey(x => x.Id).HasName("DanceGroupsPrimaryKey");
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        }
    }
}
