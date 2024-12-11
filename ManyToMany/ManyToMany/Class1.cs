using System.Reflection.Emit;
using System.Security.Authentication.ExtendedProtection;

namespace ManyToMany
{

    #region Default Convention

    //İki Entity arasındaki ilişkiyi navigation propertyler üzerinden çoğul olarak kurmalıyız.(ICollection,List)
    //Default Convention'da cross table'ı manuel oluşturmak zorunda değiliz.Ef Core tasarımına uygun bir şekilde cross table'ı kendisi otomatik oluşturacak ve generate edecektir.
    //Oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır.
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        ICollection<Writer> Writers {  get; set; }
        
        }

        public class Writer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ResumePath { get; set; }

            ICollection<Book> Books { get; set; }

        }


        #endregion
        #region Data Annotations

        //Cross table manuel olarak oluşturmak zorundadır.
        //Entity'lerde oluşturduğumuz cross table entity'si ile bire çok ilişki kurulması gerekir.
        //Cross Table'da composite primary key'i data annotation(Attributes)lar ile manuel kuramıyoruz. Bunun için de fluent API'da çalışmamız gerekiyor.
        public class Book
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ICollection<BookWriter> Writers { get; set; }
            public class Writer
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string ResumePath { get; set; }
                public ICollection<BookWriter> Books { get; set; }
            }
            //Cross Table
            public class BookWriter
            {

                public int BookId { get; set; }
                public int WriterId { get; set; }
                public Book Book { get; set; }
                public Writer Writer { get; set; }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

                modelBuilder.Entity<BookWriter>()
                    .HasKey(bw => new { bw.BookId, bw.WriterId });

            }
            #endregion

            #region Fluent API

            //Cross Table manuel olarak oluşturulmalıdır.,
            //Composite PK HasKey metodu ile kurulmalıdır.
            public class Book
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public ICollection<BookWriter> Writers { get; set; }



                public class Writer
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                    public string ResumePath { get; set; }

                    public ICollection<BookWriter> Books { get; set; }

                }
                //Cross Table
                public class BookWriter
                {

                    public int BookId { get; set; }
                    public int WriterId { get; set; }
                    public Book Book { get; set; }
                    public Writer Writer { get; set; }
                }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
        //composite primary key
            modelBuilder.Entity<BookWriter>()
             .HasKey(bw => new { bw.BookId, bw.WriterId });

            modelBuilder.Entity<BookWriter>()
                .HasOne(bw => bw.Book)
                .WithMany(w => w.Writers)
                .HasForeignKey(bw => bw.BookId);

            modelBuilder.Entity<BookWriter>()
                .HasOne(bw => bw.Writer)
                .WithMany(b => b.Books)
                .HasForeignKey(bw => bw.WriterId);
                }

                    #endregion
                }