using Autofac.Extras.Moq;
using Moq;
using Shouldly;
using Stackoverflow.Application;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Domain.Repositories;

namespace Stackoverflow.Clone.Test
{
    public class PostManagementServiceTests
    {
        private AutoMock _mock;
        private Mock<IPostRepository> _postRepositoryMock;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private PostManagementService _postManagementService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        { 
            _postRepositoryMock = _mock.Mock<IPostRepository>();
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _postManagementService = _mock.Create<PostManagementService>();
        }

        [TearDown]
        public void TearDown()
        {
            _postRepositoryMock?.Reset();
            _unitOfWorkMock?.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task CreatePostAsync_CreatesNewPost()
        {
            // Arrange
            const string title = "what is C#";
            const string body = "A beginner guide to learn C#";
            Guid userId = new Guid("01337065-B6F7-42A6-8CE1-3A73DD0AF8E3");
            
            var post = new Post
            {
                Title = title, 
                Body = body,
                UserId = userId
            };

            _unitOfWorkMock.SetupGet(x => x.PostRepository).Returns(_postRepositoryMock.Object).Verifiable();

            _postRepositoryMock.Setup(x => x.AddAsync(It.Is<Post>(y => y.Title == title
                 && y.Body == body && y.UserId == userId))).Returns(Task.CompletedTask).Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _postManagementService.CreatePostAsync(title, body, userId);

            //// Assert
            //_unitOfWorkMock.VerifyAll();
            //_postRepositoryMock.VerifyAll();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _unitOfWorkMock.VerifyAll(),
                () => _postRepositoryMock.VerifyAll()
            );
        }

    }
}