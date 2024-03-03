// See https://aka.ms/new-console-template for more information
using Repositories;

Console.WriteLine("Hello, World!");


using(UnitOfWork db =  new UnitOfWork())
{

  var res  =  db.UserGR.Get();



    Console.ReadKey();
}




