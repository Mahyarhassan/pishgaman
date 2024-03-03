using DataLayer.Repositories;
using EFCore.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IDisposable
    {
        private Testproject_dbContext db = new();

        private GenericRepository<TblUser>? _userGR;
       

        public GenericRepository<TblUser> UserGR
        {

            get
            {
                if (_userGR == null)
                {
                    _userGR = new GenericRepository<TblUser>(db);
                }

                return _userGR;
            }
        }

        private IUserRepository? _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(db);

                }

                return _userRepository;
            }
        }
        public void Dispose() => db.Dispose();

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
