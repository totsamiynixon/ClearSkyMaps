﻿// <auto-generated />
using System;
using Readings.DAL.Implementations.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Readings.DAL.Implementations.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Admin.ApllicationConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EnableEmulation");

                    b.HasKey("Id");

                    b.ToTable("ApllicationConfiguration");
                });

            modelBuilder.Entity("Domain.Reading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("CH4");

                    b.Property<float>("CO");

                    b.Property<float>("CO2");

                    b.Property<DateTime>("Created");

                    b.Property<float>("Dust");

                    b.Property<float>("Hum");

                    b.Property<float>("LPG");

                    b.Property<float>("Preassure");

                    b.Property<int>("SensorId");

                    b.Property<float>("Temp");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("Reading");
                });

            modelBuilder.Entity("Domain.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("TrackingKey")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("TrackingKey");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("Domain.Reading", b =>
                {
                    b.HasOne("Domain.Sensor", "Sensor")
                        .WithMany("Readings")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
