using AutoMapper;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
   public class UserService : IModelService<UserDTM>
   {
      public IUnitOfWork DataBase { get; protected set; }
      private readonly IPasswordService _passwordService;

      public UserService(IUnitOfWork dataBase, IPasswordService passwordService)
      {
         DataBase = dataBase;
         _passwordService = passwordService;
      }

      public async Task CreateItem(UserDTM user)
      {
         var usr = new User
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarPath = user.AvatarPath,
            Login = user.Login,
         };

         await _passwordService.CreatePassword(usr, user.Password);
         await DataBase.Users.Create(usr);
         await DataBase.Save();
      }

      public async Task DeleteItem(int id)
      {
         await DataBase.Users.Delete(id);
         await DataBase.Save();
      }


      public async Task<IEnumerable<UserDTM>> GetItemsList()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<User>, IEnumerable<UserDTM>>(await DataBase.Users.GetAll());
      }

      public async Task<UserDTM> GetItem(int id)
      {
         User? usr = await DataBase.Users.Get(id);
         return usr == null
                ? throw new ValidationException("This User does not found in our base of Users", "")
                : new UserDTM
                {
                   Id = usr.Id,
                   FirstName = usr.FirstName,
                   LastName = usr.LastName,
                   AvatarPath = usr.AvatarPath,
                   Login = usr.Login,
                   Password = usr.Password,
                   Salt = usr.Salt,
                };
      }

      public async Task UpdateItem(UserDTM user)
      {
         var usr = new User
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            AvatarPath = user.AvatarPath,
            Password = user.Password,
            Salt = user.Salt
         };
         DataBase.Users.Update(usr);
         await DataBase.Save();
      }
   }
}
