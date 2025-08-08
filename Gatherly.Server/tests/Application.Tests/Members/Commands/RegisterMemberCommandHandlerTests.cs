using Application.UseCases.Members.Commands.Register;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.Tests.Members.Commands;

public class RegisterMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public RegisterMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        var firstName = "Yaser";
        var lastName = "Abu Nimreh";
        var email = "nimreh.yaser@gmail.com";

        var command = new RegisterMemberCommand(
            firstName,
            lastName,
            email);

        _memberRepositoryMock.Setup(
            _ => _.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new RegisterMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(MemberErrors.DuplicateEmail(email));
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        // Arrange
        var firstName = "Osama";
        var lastName = "Abu Nimreh";
        var email = "osama.nimreh@gmail.com";

        var command = new RegisterMemberCommand(
            firstName,
            lastName,
            email);

        _memberRepositoryMock.Setup(
            _ => _.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new RegisterMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUnique()
    {
        // Arrange
        var firstName = "Osama";
        var lastName = "Abu Nimreh";
        var email = "osama.nimreh@gmail.com";

        var command = new RegisterMemberCommand(
            firstName,
            lastName,
            email);

        _memberRepositoryMock.Setup(
            _ => _.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new RegisterMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _memberRepositoryMock.Verify(
            _ => _.Add(It.Is<Member>(member => member.Id == result.Value)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenEmailIsNotUnique()
    {
        // Arrange
        var firstName = "Yaser";
        var lastName = "Abu Nimreh";
        var email = "nimreh.yaser@gmail.com";

        var command = new RegisterMemberCommand(
            firstName,
            lastName,
            email);

        _memberRepositoryMock.Setup(
            _ => _.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new RegisterMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        await handler.Handle(command, default);

        // Assert
        _unitOfWorkMock.Verify(
            _ => _.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}