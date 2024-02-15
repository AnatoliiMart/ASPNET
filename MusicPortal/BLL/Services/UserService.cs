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

        public UserService(IUnitOfWork dataBase, IPasswordService passwordService, IUserToConfirmService userToConfirmService)
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
                IsAdmin = user.IsAdmin,
            };

            if (!(await DataBase.UsersToConfirm.GetAll()).Any(usr => usr.Login == user.Login))
                await _passwordService.CreatePassword(usr, user.Password);
            else
            {
                UserToConfirm? data = await DataBase.UsersToConfirm.Get(user.Id);
                if (data != null)
                {
                    usr.Id = 0;
                    usr.Password = data.Password;
                    usr.Salt = data.Salt;
                }
                else
                    throw new ValidationException("User do not exists", "");
            }

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
                       IsAdmin = usr.IsAdmin,
                   };
        }

        public async Task UpdateItem(UserDTM? user)
        {
            if (user == null)
                throw new ValidationException("", "Wrong data income!");
            var usr = await DataBase.Users.Get(user.Id);
            usr!.Id = user.Id;
            usr.FirstName = user.FirstName;
            usr.LastName = user.LastName;
            usr.Login = user.Login;
            usr.AvatarPath = user.AvatarPath;
            usr.IsAdmin = user.IsAdmin;
            DataBase.Users.Update(usr);
            await DataBase.Save();
        }
    }
}
