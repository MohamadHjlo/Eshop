using System;//تمامی اجناس
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{//ساختن یه شیئ و نمونه سازی کردنش توی یه کلاس دیگه ریلیشن ایجاد میکنه
    public class Item
    {

        public int Id { get; set; }

        [AllowNull]
        [MaxLength(100)]
        [Range(0,100)]
        public float Discount { get; set; }

        public decimal Price { get; set; }//دسیمال اعشاری هم میپذیره

        public int QuantityInStuck { get; set; }//تعداد

        public Product Product { get; set; }//relation 1v1 with item tabel
    }
}
