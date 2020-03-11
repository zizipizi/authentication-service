using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NSV.Security.Password;

namespace Authentication.Tests
{
    public static class FakePasswordServiceFactory
    {
        public static IPasswordService FakeHashPassword(PasswordHashResult.HashResult passwordHashResult)
        {
            var passwordService = new Mock<IPasswordService>();

            passwordService.Setup(c => c.Hash(It.IsAny<string>()))
                .Returns(new PasswordHashResult(passwordHashResult, "HashedPassword"));

            return passwordService.Object;
        }

        public static IPasswordService FakeValidate(PasswordValidateResult.ValidateResult passwordValidateResult)
        {
            var passwordService = new Mock<IPasswordService>();

            passwordService.Setup(c => c.Validate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PasswordValidateResult(passwordValidateResult));

            return passwordService.Object;
        }
    }
}
