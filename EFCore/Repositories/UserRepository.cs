using EFCore.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        Testproject_dbContext db;


        public UserRepository(Testproject_dbContext context)
        {
            db = context;
        }
        public bool IsExUser(string username, string password)
        {            
          return  db.TblUsers.Where(x=>x.Username == username && x.Password == password).Any();
        }
        public bool IsExPhone(string phone)
        {
           return db.TblUsers.Where(x => x.Phone == phone).Any();
        }

        public TblUser GetUserByPhone(string phone)
        {
            return db.TblUsers.First(x=>x.Phone == phone);
        }
    }
}
