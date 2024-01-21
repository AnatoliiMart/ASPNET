using AutoMapper;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
   public class UserToConfirmService : IUserToConfirmService
   {
      public IUnitOfWork DataBase { get; protected set; }
      private readonly IPasswordService _passwordService;

      public UserToConfirmService(IUnitOfWork dataBase, IPasswordService passwordService)
      {
         DataBase = dataBase;
         _passwordService = passwordService;
      }

      public async Task CreateUserToConfirm(UserDTM user)
      {
         var usr = new UserToConfirm
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarPath = user.AvatarPath,
            Login = user.Login,
         };
         await _passwordService.CreatePassword(usr, user.Password);
         await DataBase.UsersToConfirm.Create(usr);
         await DataBase.Save();
      }

      public async Task DeleteUserToConfirm(int id)
      {
         await DataBase.UsersToConfirm.Delete(id);
         await DataBase.Save();
      }

      public async Task<IEnumerable<UserDTM>> GetUsersToConfirmList()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserToConfirm, UserDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<UserToConfirm>, IEnumerable<UserDTM>>(await DataBase.UsersToConfirm.GetAll());
      }

      public async Task<UserDTM> GetUserToConfirm(int id)
      {
         UserToConfirm? usr = await DataBase.UsersToConfirm.Get(id);
         return usr == null
                ? throw new ValidationException("This User does not found in our base of Users to confirm", "")
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
   }
}
