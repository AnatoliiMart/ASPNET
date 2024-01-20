using AutoMapper;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
   public class GenreService : IGenreService
   {
      IUnitOfWork DataBase { get; set; }

      public GenreService(IUnitOfWork unit) => DataBase = unit;

      public async Task CreateGenre(GenreDTM genre)
      {
         var gnr = new Genre
         {
            Id = genre.Id,
            Name = genre.Name
         };
         await DataBase.Genres.Create(gnr);
         await DataBase.Save();
      }

      public async Task DeleteGenre(int id)
      {
         await DataBase.Genres.Delete(id);
         await DataBase.Save();
      }

      public async Task<GenreDTM> GetGenre(int id)
      {
         var gnr = await DataBase.Genres.Get(id);
         return gnr == null
                ? throw new ValidationException("This Genre does not found in our base of Genres", "")
                : new GenreDTM
                {
                   Id = gnr.Id,
                   Name = gnr.Name,
                };
      }

      public async Task<IEnumerable<GenreDTM>> GetGenresList()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTM>>(await DataBase.Genres.GetAll());
      }

      public async Task UpdateGenre(GenreDTM genre)
      {
         var gnr = new Genre
         {
            Id = genre.Id,
            Name = genre.Name
         };
         DataBase.Genres.Update(gnr);
         await DataBase.Save();
      }
   }
}
