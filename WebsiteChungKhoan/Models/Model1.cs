using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteChungKhoan.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Reciept> Reciepts { get; set; }
        public virtual DbSet<Star> Stars { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(e => e.Histories)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.Id_Accout);

            modelBuilder.Entity<Author>()
                .Property(e => e.Id_Author)
                .IsFixedLength();

            modelBuilder.Entity<Category>()
                .Property(e => e.Id_Category)
                .IsFixedLength();

            modelBuilder.Entity<Comment>()
                .Property(e => e.Comment1)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasOptional(e => e.Comment)
                .WithRequired(e => e.Course);

            modelBuilder.Entity<History>()
                .HasOptional(e => e.Reciept)
                .WithRequired(e => e.History);

            modelBuilder.Entity<Post>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .HasOptional(e => e.Comment)
                .WithRequired(e => e.Post);

            modelBuilder.Entity<Product>()
                .Property(e => e.Id_Product)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.Id_Category)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.Id_Author)
                .IsFixedLength();

            modelBuilder.Entity<Reciept>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Reciept)
                .HasForeignKey(e => e.Year);
        }
    }
}
