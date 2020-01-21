//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Authentication.Host.Enums;
//using Authentication.Host.Services;
//using Moq;
//using NSV.Security.JWT;
//using NSV.Security.Password;
//using Xunit;

//namespace Authentication.Tests.UserServiceTests
//{
//    public class WhenBlockUser
//    {
//        [Fact]
//        public async Task BlockUser_Sucess()
//        {
//            var id = 1;
//            var jwtService = new Mock<IJwtService>();
//            var passwordService = new Mock<IPasswordService>();

//            var userRepo = FakeRepositoryFactory.BlockFakeUserRepository();
//            var userService = new AdminService(userRepo, passwordService.Object, jwtService.Object);

//            var result = await userService.BlockUserAsync(id, CancellationToken.None);

//            Assert.IsType<AdminResult>>(result);
//            Assert.Equal(AdminResult.Ok, result.Value);
//        }

//        [Fact]
//        public async Task BlockUser_NotFound()
//        {
//            var id = 1;
//            var jwtService = new Mock<IJwtService>();
//            var passwordService = new Mock<IPasswordService>();

//            var a = passwordService.Object.Hash("21312321");

//            var userRepo = FakeRepositoryFactory.BlockFakeUserRepository_Exception();
//            var userService = new AdminService(userRepo, passwordService.Object, jwtService.Object);

//            var result = await userService.BlockUserAsync(id, CancellationToken.None);

//            Assert.IsType<Result<AdminResult>>(result);
//            Assert.Equal(AdminResult.UserNotFound, result.Value);
//        }
//    }
//}
