using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO;
using PresentationProject.Controllers;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class UserControllerTest
    {
        private readonly Mock<ICreateUsersService> _mockCreateUserService;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _mockCreateUserService = new Mock<ICreateUsersService>();
            _userController = new UserController(_mockCreateUserService.Object);
        }
        [Fact]
        public async Task CreateAuthentication_ValidRequest_ReturnsOkResult()
        {
            // Mock Data (Create A Mock Data Folder and Transfer This, and Just Call It)
            var userAuth = new CreateUserWithAuthenticationDTO
            {
                UserName = "testuser",
                Password = "Test@123",
                Email = "testuser@gmail.com",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                ContactNumber = "1234567890",
                Address = "123 Test St"
            };
            var request = new CreateAuthenticationRequest
            {
                UserName = userAuth.UserName,
                Password = userAuth.Password,
                Email = userAuth.Email
            };
            var user = new CreateUserRequest
            {
                FirstName = userAuth.FirstName,
                MiddleName = userAuth.MiddleName,
                LastName = userAuth.LastName,
                ContactNumber = userAuth.ContactNumber,
                Address = userAuth.Address
            };
            var random = new Random();
            var expectedAuthId = random.Next(100);
            _mockCreateUserService.Setup(s => s.CreateUserAuthenticateAsync(request, user)).ReturnsAsync(expectedAuthId);
            _mockCreateUserService.Setup(s => s.CreateUser(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.CreateAuthentication(userAuth) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
