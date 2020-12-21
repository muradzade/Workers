using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workers.Data.Entity;

namespace Workers.Data
{
    public class SeedData:ISeedData
    {
        private readonly ApplicationDbContext _db;
        public SeedData(ApplicationDbContext db)
        {
            _db = db;
        }

        public void EnsurePopulated()
        {
            if (!_db.Workers.Any() && !_db.Departments.Any())
            {
                var departments = new[]{
                    new Department(){DeptName= "Yazilim"},
                    new Department(){DeptName= "Bilgi Islem"},
                    new Department(){DeptName= "Insan Kaynaklari"}
                };
                _db.AddRange(departments);

                var managers = new[]{
                    new Worker(){Name= "Nejat",Surname="Yumusak",Phone="05555555",Department=departments[0]},
                    new Worker(){Name= "Umit",Surname="Kocabicak",Phone="88855566",Department=departments[1]},
                    new Worker(){Name= "Ibrahim",Surname="Emiroglu",Phone="11100022",Department=departments[2]},
                };

                _db.AddRange(managers);

                var workers = new[]{
                    new Worker(){Name= "Sinan",Surname="Ilyas",Phone="22222222",Department=departments[0],Manager=managers[0]},
                    new Worker(){Name= "Nevzat",Surname="Tasbasi",Phone="33322266",Department=departments[1],Manager=managers[1]},
                    new Worker(){Name= "Gokhan",Surname="Bilgin",Phone="11122335",Department=departments[2],Manager=managers[2]},
                };

                _db.AddRange(workers);

                _db.SaveChanges();
            }


        }
    }
}
