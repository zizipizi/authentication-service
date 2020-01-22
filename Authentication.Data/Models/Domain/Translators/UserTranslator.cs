﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Data.Models.Entities;

namespace Authentication.Data.Models.Domain.Translators
{
    public static class UserTranslator
    {
        public static User ToDomain(this UserEntity entity)
        {
            if (entity == null)
                return null;

            return new User()
            {
                Id = entity.Id,
                Login = entity.Login,
                Password = entity.Password,
                IsActive = entity.IsActive
            };
        }

        public static UserEntity ToEntity(this User user)
        {
            if (user == null)
                return null;

            return new UserEntity()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Created = DateTime.Today,
                IsActive = true
            };
        }
    }
}
