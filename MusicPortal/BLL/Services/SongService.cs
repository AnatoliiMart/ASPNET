using AutoMapper;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
    public class SongService : IModelService<SongDTM>
    {
        public IUnitOfWork DataBase { get; protected set; }

        public SongService(IUnitOfWork unit) => DataBase = unit;

        public async Task CreateItem(SongDTM song)
        {
            var sng = new Song
            {
                Id = song.Id,
                Name = song.Name,
                SongPath = song.SongPath,
                PreviewPath = song.PreviewPath,
                Rating = song.Rating,
                UserId = song.UserId,
                GenreId = song.GenreId
            };
            await DataBase.Songs.Create(sng);
            await DataBase.Save();
        }

        public async Task DeleteItem(int id)
        {
            await DataBase.Songs.Delete(id);
            await DataBase.Save();
        }

        public async Task<SongDTM> GetItem(int id)
        {
            var sng = await DataBase.Songs.Get(id);
            return sng == null
                   ? throw new ValidationException("This Song does not found in our base of Songs", "")
                   : new SongDTM
                   {
                       Id = sng.Id,
                       Name = sng.Name,
                       SongPath = sng.SongPath,
                       PreviewPath = sng.PreviewPath,
                       Rating = sng.Rating,
                       UserId = sng.UserId,
                       GenreId = sng.GenreId
                   };
        }

        public async Task<IEnumerable<SongDTM>> GetItemsList()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Song, SongDTM>()
                             .ForMember("GenreName", opt => opt.MapFrom(s => s.Genre!.Name ?? "NoGenre"))
                             .ForMember("UserLogin", opt => opt.MapFrom(s => s.User!.Login ?? "NoUserLogin")));
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Song>, IEnumerable<SongDTM>>(await DataBase.Songs.GetAll());
        }

        public async Task UpdateItem(SongDTM? song)
        {
            if (song == null)
                throw new ValidationException("", "Wrong data income!");
            var sng = await DataBase.Songs.Get(song.Id);
            sng!.Id = song.Id;
            sng.Name = song.Name;
            sng.SongPath = song.SongPath;
            sng.PreviewPath = song.PreviewPath;
            sng.Rating = song.Rating;
            sng.UserId = song.UserId;
            sng.GenreId = song.GenreId;
            DataBase.Songs.Update(sng);
            await DataBase.Save();
        }
    }
}
