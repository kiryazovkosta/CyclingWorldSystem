using Domain.Entities;
using Domain.Identity;

namespace Tests.DomainTests.Entities;

public class UserChallengeTests
{
    [Fact]
    public void UserChallenge_UserId_SetAndGet()
    {
        // Arrange
        var userChallenge = new UserChallenge();
        var userId = Guid.NewGuid();

        // Act
        userChallenge.UserId = userId;

        // Assert
        Assert.Equal(userId, userChallenge.UserId);
    }

    [Fact]
    public void UserChallenge_User_SetAndGet()
    {
        // Arrange
        var userChallenge = new UserChallenge();
        var user = new User();

        // Act
        userChallenge.User = user;

        // Assert
        Assert.Same(user, userChallenge.User);
    }

    [Fact]
    public void UserChallenge_ChallengeId_SetAndGet()
    {
        // Arrange
        var userChallenge = new UserChallenge();
        var challengeId = Guid.NewGuid();

        // Act
        userChallenge.ChallengeId = challengeId;

        // Assert
        Assert.Equal(challengeId, userChallenge.ChallengeId);
    }

    [Fact]
    public void UserChallenge_Challenge_SetAndGet()
    {
        // Arrange
        var userChallenge = new UserChallenge();
        var challenge = new Challenge();

        // Act
        userChallenge.Challenge = challenge;

        // Assert
        Assert.Same(challenge, userChallenge.Challenge);
    }
}
