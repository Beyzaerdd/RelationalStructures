using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
namespace One_To_One_Relationship
{

    #region Default Convention

    //  Her iki entity'de Navigation Property ile birbirlerini tekil ederek fiziksel bir ilişkinin olacağı ifade edilir.
    //  One to One ilişki türünde, dependent entity'nin hangisi olduğunu default olarak belirleyebilmek pek kolay değildir. Bu durumda fiziksel olarak bir Foreign Key'e karşılık property/kolon tanımlayarak çözüm getirebiliyoruz.

    public class School
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Manager Manager { get; set; } // School tablosundan Manager'a erişim

    }


    public class Manager
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public int SchoolId { get; set; } // Foreign Key
        public School School { get; set; } // Manager tablosundan School'a erişim


    }

    #endregion

    #region Data Annotations

    // Navigation Property'ler tanımlanmalıdır.
    // Foreign kolonunun ismi defaul convention'ın dışında bir kolon olacaksa eğer foreign key attribute ile bunu bildirebiliriz.
    // Foreign Key kolonu oluşturmak zorunda değildir.
    // 1'e 1 ilişkide ekstradan foreign key kolonuna ihtiyaç olmayacağından dolayı dependent entity'deki id kolonunun hem foreign key hem de primary key olarak kullanmayı tercih ediyoruz ve bu duruma özen gösterilmelidir diyoruz.

    public class School
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Manager Manager { get; set; }

    }


    public class Manager

    {
        // Hem Primary Key hemde Foreign Key tanımlamasını yapmış olduk, böylece primary key olacağından dolayı bir yandan unıque oluyor diğer yandan ise foreign kolonuna gerek kalmıyor.
        // Bu şekilde 1'e 1 ilişkinin garantisini almış oluyoruz.


        [Key, ForeignKey(nameof(School))]
        public int Id { get; set; }
        public string Name { get; set; }

        public School School { get; set; }


    }

    #endregion

    #region Fluent API


    //  Navigation Propertyler tanımlanmalıdır.
    // Fluent API yönteminde entity'ler arasındaki ilişki context sınıfı içerisinde OnModelCreating fonksiyonunun override edilerek metotlar aracılığıyla tasarlanması gerekmektedir.Yani tüm sorumluluk bu fonksiyon içerisindeki çalışmalardadır.
    public class School
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Manager Manager { get; set; }

    }

    public class Manager
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public School School { get; set; }
    }

    //Model'ların(entity) veritabanında generate edilecek yapıları bu fonksiyon içerisinde konfigüre edilir.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //School tablosunun bir tane Manager'a sahip olduğunu tanımlar.
       // Bu, School ve Manager arasında bire bir ilişki olduğunu ifade eder.
        modelBuilder.Entity<School>()
            .HasOne(s => s.Manager) 
            .WithOne(m => m.School) // Manager tablosunun bir tane School ile ilişkilendirildiğini belirtir.
            .HasForeignKey<Manager>(m => m.Id);
        //dependet tablosu içinde (Manager)  foreign key olarak Id yapmış olduk

        modelBuilder.Entity<Manager>()
            .HasKey(s => s.Id);
        //Maneger tablosundaki Id hem foreign key hemde primary key olduğunu bildirmiş oldu.
    }



    #endregion
}

