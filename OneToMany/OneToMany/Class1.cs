using System.ComponentModel.DataAnnotations.Schema;

namespace OneToMany
{
    #region Default Convention

    // 1 çalışan 1 Departmana sahipken 1 Departmanın birden fazla çalışanı olabilir(one to many)

    public class Employeer
    {
        public int Id { get; set; }
        public string Name { get; set; }
      //  Burası opsiyoneldir, istersen foreign key tanımlayabilirsin.
       // Eğer tanımlamazsan da default Convention gereği kendisi bir foreign key(DepartmanId) kolonu oluşturacaktır.
        public int DepartmanId { get; set; } //foreign key

        public Department Department { get; set; } //Navigation Property

    }
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

      //  Çoğul Navigation Property
        ICollection<Employeer> Employeers { get; set; }


    }
    #endregion


    #region Data Annotations:

   // Default Convention yönteminde Foreign kolonuna karşılık gelen property'i tanımladığımızda,
   // bu property ismi temel geleneksel entity tanımlama kurallarına uymuyorsa eğer Data Annotations ile müdahalede bulunabiliriz.
    public class Employeer
    {

        public int Id { get; set; }

        [ForeignKey(nameof(Department))]
        public int DId { get; set; }
        public string Name { get; set; }

        public Department Department { get; set; }

    }
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        ICollection<Employeer> Employeers { get; set; }

    }

    #endregion

    #region Fluet API
    public class Employeer
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public Department Department { get; set; }

    }
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        ICollection<Employeer> Employeers { get; set; }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employeer>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employeers);
    }



}

#endregion