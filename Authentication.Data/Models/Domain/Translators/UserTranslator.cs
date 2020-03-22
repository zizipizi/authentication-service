using System;
using System.Linq;
using Authentication.Data.Models.Entities;

namespace Authentication.Data.Models.Domain.Translators
{
    public static class UserTranslator
    {
        public static User ToUserModel(this UserEntity entity)
        {
            if (entity == null)
                return null;

            return new User()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Login = entity.Login,
                Password = entity.Password,
                IsActive = entity.IsActive,
                Role = entity.Roles.Select(p => p.RoleEn.Role).ToList()
            };
        }

        public static UserInfo ToUserInfo(this UserEntity entity)
        {
            if (entity == null)
                return null;

            return new UserInfo
            {
                Id = entity.Id,
                Login = entity.Login
            };
        }

        public static UserInfo ToUserInfo(this User user)
        {
            if (user == null)
                return null;

            return new UserInfo
            {
                Id = user.Id,
                Login = user.Login
            };
        }

        public static UserEntity ToEntity(this User user)
        {
            if (user == null)
                return null;

            return new UserEntity()
            {
                Login = user.Login,
                UserName = user.UserName,
                Password = user.Password,
                Created = DateTime.Today,
                IsActive = true
            };
        }
    }
}
