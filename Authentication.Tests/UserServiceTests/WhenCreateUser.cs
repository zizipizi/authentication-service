//using Authentication.Data.Exceptions;
//using Authentication.Data.Models.Domain;
//using Authentication.Host.Models;
//using Authentication.Host.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Authentication.Host.Enums;
//using Moq;
//using NSV.Security.JWT;
//using NSV.Security.Password;
//using Xunit;

//namespace Authentication.Tests.UserServiceTests
//{
//    public class WhenCreateUser
//    {
//        [Fact]
//        public async Task CreateUser_Sucess()
//        {
//            var passService = new Mock<IPasswordService>();
//            var jwtService = new Mock<IJwtService>();

//            var userRepo = FakeRepositoryFactory.CreateFakeUserRepository();
//            var userService = new AdminService(userRepo, passService.Object, jwtService.Object);
                
//            var userCreateModel = new UserCreateModel();

//            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);

//            Assert.IsType<AdminResult>(result);
//            Assert.Equal(AdminResult.Ok, result.Value);
//        }

//        [Fact]
//        public async Task CreateUser_Exist()
//        {
//            var ex = new EntityNotFoundException("User not found");

//            var passService = new Mock<IPasswordService>();
//            var jwtService = new Mock<IJwtService>();

//            var userRepo = FakeRepositoryFactory.CreateFakeUserRepository_Exception();

//            var userService = new AdminService(userRepo, passService.Object, jwtService.Object);

//            var userCreateModel = new UserCreateModel();

//            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);


//            Assert.IsType<AdminResult>(result);
//            Assert.Equal(AdminResult.UserExist, result.Value);
//        }
//    }
//}
