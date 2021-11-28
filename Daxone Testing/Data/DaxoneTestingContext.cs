using Daxone_Testing.Models;
using Daxone_Testing.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Daxone_Testing.Data
{

    public class DaxoneTestingContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DaxoneTestingContext(DbContextOptions<DaxoneTestingContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }

        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Item> Items { get; set; }


        public DbSet<Factor> Factors { get; set; }

        public DbSet<FactorDetail> FactorDetails { get; set; }

        public DbSet<ProductPic> ProductPics { get; set; }

        public DbSet<UserFavorites> UserFavorites { get; set; }

        public DbSet<CategoryAttr> CategoryAttrs { get; set; }

        public DbSet<FavoriteToProducts> FavoriteToProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//داریم جزئیات دیزاین مدل رو میدیم
        {
            modelBuilder.Entity<CategoryToProduct>().HasKey(k => new { k.CategoryId, k.ProductId });//ایجاد کلید اصلی دلخواه
            modelBuilder.Entity<FavoriteToProducts>().HasKey(k => new { k.UserFavoritesId, k.ProductId });

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Owner",
                RankOfRole = 1,
                NormalizedName = "OWNER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
            , new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                RankOfRole = 2,
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()

            }, new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Storekeeper",
                RankOfRole = 3,
                NormalizedName = "STOREKEEPER",
                ConcurrencyStamp = Guid.NewGuid().ToString()

            }
            , new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                RankOfRole = 4,
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()

            });

            modelBuilder.Entity<Item>(i =>
            {

                i.Property(w => w.Price).HasColumnType("Money");
                i.HasKey(w => w.Id);
            });

            #region Seed Data Category

            modelBuilder.Entity<Category>().HasData(new Category()
            {
                Id = 1,
                Name = "کالای دیجیتال",
                Describtion = "Digital devices",
            },
            new Category()
            {
                Id = 2,
                Name = "فروشگاه پوشاک",
                Describtion = "Clothing store",
            },
            new Category()
            {
                Id = 3,
                Name = "کالای سوپر مارکتی",
                Describtion = "Essentials Store",
            },
            new Category()
            {
                Id = 4,
                Name = "فروشگاه لوازم التحریر",
                Describtion = "Types of cars store",
            }
            );

            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = 1,
                    Price = 87.0M,
                    QuantityInStuck = 5
                },
            new Item()
            {
                Id = 2,
                Price = 8002.0M,
                QuantityInStuck = 8
            },
            new Item()
            {
                Id = 3,
                Price = 600.5M,
                QuantityInStuck = 3
            },
            new Item()
            {
                Id = 4,
                Price = 423.0M,
                QuantityInStuck = 2
            });




            modelBuilder.Entity<Product>().HasData(new Product()
            {
                Id = 1,
                ItemId = 1,
                Name = "Mobile Phone",
                Describtion = "لورم ایپسوم یا طرح‌نمابه متنی آزمایشی و بی‌معنی در صنعت چاپ، صفحه‌آرایی و طراحی گرافیک گفته می‌شود. طراح گرافیک از این متن به عنوان عنصری از ترکیب بندی برای پر کردن صفحه و ارایه اولیه شکل ظاهری و کلی طرح سفارش گرفته شده استفاده می نماید، تا از نظر گرافیکی نشانگر چگونگی نوع و اندازه فونت و ظاهر متن باشد. معمولا طراحان گرافیک برای صفحه‌آرایی، نخست از متن‌های آزمایشی و بی‌معنی استفاده می‌کنند تا صرفا به مشتری یا صاحب کار خود نشان دهند که صفحه طراحی یا صفحه بندی شده بعد از اینکه متن در آن قرار گیرد چگونه به نظر می‌رسد و قلم‌ها و اندازه‌بندی‌ها چگونه در نظر گرفته شده‌است. از آنجایی که طراحان عموما نویسنده متن نیستند و وظیفه رعایت حق تکثیر متون را ندارند و در همان حال کار آنها به نوعی وابسته به متن می‌باشد آنها با استفاده از محتویات ساختگی، صفحه گرافیکی خود را صفحه‌آرایی می‌کنند تا مرحله طراحی و صفحه‌بندی را به پایان برند"
            },
            new Product()
            {
                Id = 2,
                ItemId = 2,
                Name = "T Shirt",
                Describtion = "لورم ایپسوم یا طرح‌نمابه متنی آزمایشی و بی‌معنی در صنعت چاپ، صفحه‌آرایی و طراحی گرافیک گفته می‌شود. طراح گرافیک از این متن به عنوان عنصری از ترکیب بندی برای پر کردن صفحه و ارایه اولیه شکل ظاهری و کلی طرح سفارش گرفته شده استفاده می نماید، تا از نظر گرافیکی نشانگر چگونگی نوع و اندازه فونت و ظاهر متن باشد. معمولا طراحان گرافیک برای صفحه‌آرایی، نخست از متن‌های آزمایشی و بی‌معنی استفاده می‌کنند تا صرفا به مشتری یا صاحب کار خود نشان دهند که صفحه طراحی یا صفحه بندی شده بعد از اینکه متن در آن قرار گیرد چگونه به نظر می‌رسد و قلم‌ها و اندازه‌بندی‌ها چگونه در نظر گرفته شده‌است. از آنجایی که طراحان عموما نویسنده متن نیستند و وظیفه رعایت حق تکثیر متون را ندارند و در همان حال کار آنها به نوعی وابسته به متن می‌باشد آنها با استفاده از محتویات ساختگی، صفحه گرافیکی خود را صفحه‌آرایی می‌کنند تا مرحله طراحی و صفحه‌بندی را به پایان برند"
            },
            new Product()
            {

                Id = 3,
                ItemId = 3,
                Name = "Food",
                Describtion = "لورم ایپسوم یا طرح‌نمابه متنی آزمایشی و بی‌معنی در صنعت چاپ، صفحه‌آرایی و طراحی گرافیک گفته می‌شود. طراح گرافیک از این متن به عنوان عنصری از ترکیب بندی برای پر کردن صفحه و ارایه اولیه شکل ظاهری و کلی طرح سفارش گرفته شده استفاده می نماید، تا از نظر گرافیکی نشانگر چگونگی نوع و اندازه فونت و ظاهر متن باشد. معمولا طراحان گرافیک برای صفحه‌آرایی، نخست از متن‌های آزمایشی و بی‌معنی استفاده می‌کنند تا صرفا به مشتری یا صاحب کار خود نشان دهند که صفحه طراحی یا صفحه بندی شده بعد از اینکه متن در آن قرار گیرد چگونه به نظر می‌رسد و قلم‌ها و اندازه‌بندی‌ها چگونه در نظر گرفته شده‌است. از آنجایی که طراحان عموما نویسنده متن نیستند و وظیفه رعایت حق تکثیر متون را ندارند و در همان حال کار آنها به نوعی وابسته به متن می‌باشد آنها با استفاده از محتویات ساختگی، صفحه گرافیکی خود را صفحه‌آرایی می‌کنند تا مرحله طراحی و صفحه‌بندی را به پایان برند"
            },
            new Product()
            {
                Id = 4,
                ItemId = 4,
                Name = "Pride",
                Describtion = "لورم ایپسوم یا طرح‌نمابه متنی آزمایشی و بی‌معنی در صنعت چاپ، صفحه‌آرایی و طراحی گرافیک گفته می‌شود. طراح گرافیک از این متن به عنوان عنصری از ترکیب بندی برای پر کردن صفحه و ارایه اولیه شکل ظاهری و کلی طرح سفارش گرفته شده استفاده می نماید، تا از نظر گرافیکی نشانگر چگونگی نوع و اندازه فونت و ظاهر متن باشد. معمولا طراحان گرافیک برای صفحه‌آرایی، نخست از متن‌های آزمایشی و بی‌معنی استفاده می‌کنند تا صرفا به مشتری یا صاحب کار خود نشان دهند که صفحه طراحی یا صفحه بندی شده بعد از اینکه متن در آن قرار گیرد چگونه به نظر می‌رسد و قلم‌ها و اندازه‌بندی‌ها چگونه در نظر گرفته شده‌است. از آنجایی که طراحان عموما نویسنده متن نیستند و وظیفه رعایت حق تکثیر متون را ندارند و در همان حال کار آنها به نوعی وابسته به متن می‌باشد آنها با استفاده از محتویات ساختگی، صفحه گرافیکی خود را صفحه‌آرایی می‌کنند تا مرحله طراحی و صفحه‌بندی را به پایان برند"
            }
            );

            modelBuilder.Entity<CategoryToProduct>().HasData(

            new CategoryToProduct() { CategoryId = 1, ProductId = 1 },
            new CategoryToProduct() { CategoryId = 2, ProductId = 1 },
            new CategoryToProduct() { CategoryId = 3, ProductId = 1 },
            new CategoryToProduct() { CategoryId = 4, ProductId = 1 },

            new CategoryToProduct() { CategoryId = 1, ProductId = 2 },
            new CategoryToProduct() { CategoryId = 2, ProductId = 2 },
            new CategoryToProduct() { CategoryId = 3, ProductId = 2 },
            new CategoryToProduct() { CategoryId = 4, ProductId = 2 },

            new CategoryToProduct() { CategoryId = 1, ProductId = 3 },
            new CategoryToProduct() { CategoryId = 2, ProductId = 3 },
            new CategoryToProduct() { CategoryId = 3, ProductId = 3 },
            new CategoryToProduct() { CategoryId = 4, ProductId = 3 },

            new CategoryToProduct() { CategoryId = 1, ProductId = 4 },
            new CategoryToProduct() { CategoryId = 2, ProductId = 4 },
            new CategoryToProduct() { CategoryId = 3, ProductId = 4 },
            new CategoryToProduct() { CategoryId = 4, ProductId = 4 }

                );

            #endregion


            base.OnModelCreating(modelBuilder);//هنوز اطلاعات زیادی از لزوم این کد نداری
        }
    }
}
