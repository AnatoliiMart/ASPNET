using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.DAL.Reposes
{
   public class EFUnitOfWork : IUnitOfWork
   {
      private readonly MyDbContext _context;

      private SongsRepository? _songsRepository;

      private GenresRepository? _genresRepository;

      private UsersRepository? _usersRepository;

      private UsersToConfirmRepository? _usersToConfirmRepository;

      public EFUnitOfWork(MyDbContext context) => _context = context;

      public IRepository<Song> Songs
      {
         get
         {
            _songsRepository ??= new SongsRepository(_context);
            return _songsRepository;
         }
      }

      public IRepository<Genre> Genres
      {
         get
         {
            _genresRepository ??= new GenresRepository(_context);
            return _genresRepository;
         }
      }

      public IRepository<User> Users
      {
         get
         {
            _usersRepository ??= new UsersRepository(_context);
            return _usersRepository;
         }
      }

      public IRepository<UserToConfirm> UsersToConfirm
      {
         get
         {
            _usersToConfirmRepository ??= new UsersToConfirmRepository(_context);
            return _usersToConfirmRepository;
         }
      }

      public async Task Save() => await _context.SaveChangesAsync();
   }
}
