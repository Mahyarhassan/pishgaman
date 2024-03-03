using EFCore.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IUserRepository
    {
        bool IsExUser( string username , string password);
        bool IsExPhone( string phone);

        TblUser GetUserByPhone( string phone );


    }
}
