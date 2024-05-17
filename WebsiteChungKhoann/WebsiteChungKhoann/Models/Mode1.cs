using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    public class Mode1 : DbContext
    {
        public Mode1() : base("name=connect")
        {

        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Cart> Carts_pr { get; set; }
        public virtual DbSet<Order> Orders_pr { get; set; }
      

        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Pay> Pays { get; set; }
        public virtual DbSet<History_Pay> History_Pays { get; set; }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment_Post> Comments_Post { get; set; }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Comment_Course> Comments_Course { get; set; }
        public virtual DbSet<Comment_Product> Comment_Product { get; set; }
        public virtual DbSet<Finance> Finance { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //sản phẩm
            modelBuilder.Entity<Category>()
           .HasMany(e => e.Product)
           .WithRequired(e => e.Category)
           .WillCascadeOnDelete(false);
            //tác giả
            modelBuilder.Entity<Author>()
           .HasMany(e => e.Product)
           .WithRequired(e => e.Author)
           .WillCascadeOnDelete(false);
          //bình luận sản phẩm 
            modelBuilder.Entity<Account>()
         .HasMany(e => e.Comment_Product)
         .WithRequired(e => e.Account)
         .WillCascadeOnDelete(false);
            //giỏ hàng 
            modelBuilder.Entity<Product>()
          .HasMany(e => e.Cart)
          .WithRequired(e => e.Product)
          .WillCascadeOnDelete(false);
          //phương thức thanh toán
            modelBuilder.Entity<Pay>()
           .HasMany(e => e.Order)
           .WithRequired(e => e.Pay)
           .WillCascadeOnDelete(false);
            //Trạng thái 
            modelBuilder.Entity<Status>()
            .HasMany(e => e.Order)
            .WithRequired(e => e.Status)
            .WillCascadeOnDelete(false);

            //post 
            modelBuilder.Entity<Post>()
           .HasMany(e => e.Comment_Post)
           .WithRequired(e => e.Post)
           .WillCascadeOnDelete(false);

            //Account post 
            // Thiết lập mối quan hệ một-nhiều giữa Account và Post
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Post) // Một tài khoản có thể có nhiều bài đăng
                .WithRequired(p => p.Account) // Mỗi bài đăng thuộc về một tài khoản
                .HasForeignKey(p => p.Id_Account) // Sử dụng trường Id_Account trong bảng Post làm khóa ngoại
                .WillCascadeOnDelete(false); // Không cho phép xóa tài khoản khi bài đăng liên quan tồn tại

            //Account Comment 
            modelBuilder.Entity<Account>()
           .HasMany(e => e.Comment_Post)
           .WithRequired(e => e.Account)
           .HasForeignKey(p => p.Id_Account)
           .WillCascadeOnDelete(false);

            //
            modelBuilder.Entity<Account>()
          .HasMany(e => e.Courses)
          .WithRequired(e => e.Account)
          .HasForeignKey(p => p.Id_Account)
          .WillCascadeOnDelete(false);

            //
            modelBuilder.Entity<Account>()
          .HasMany(e => e.Comment_Course)
          .WithRequired(e => e.Account)
          .HasForeignKey(p => p.Id_Account)
          .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Course>()
          .HasMany(e => e.Comment_Course)
          .WithRequired(e => e.Course)
          .WillCascadeOnDelete(false);

            // 
            modelBuilder.Entity<Product>()
           .HasMany(e => e.Order)
           .WithRequired(e => e.Product)
           .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Account>()
         .HasMany(e => e.Order)
         .WithRequired(e => e.Account)
         .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Order>()
          .HasMany(e => e.Finance)
          .WithRequired(e => e.Order)
          .WillCascadeOnDelete(false);
         //
          modelBuilder.Entity<Account>()
        .HasMany(e => e.Order)
        .WithRequired(e => e.Account)
        .WillCascadeOnDelete(false);

            //
         modelBuilder.Entity<Order>()
        .HasRequired(o => o.Account)
        .WithMany(a => a.Order)
        .HasForeignKey(o => o.Id_Account)
        .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Finance>()
            .HasRequired(o => o.Order)
            .WithMany(a => a.Finance)
            .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Account>()
          .HasMany(e => e.Comment_Product)
          .WithRequired(e => e.Account)
          .HasForeignKey(p => p.Id_Account)
          .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<WebsiteChungKhoann.Models.PayStatus> PayStatus { get; set; }
    }
}