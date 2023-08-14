

using Common.Enumerations;
using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class ChallengeTests
{
    [Fact]
    public void Challenge_Constructors_InitializePropertiesAndCollections()
    {
        // Arrange & Act
        var challenge = new Challenge();

        // Assert
        Assert.NotNull(challenge.Users);
        Assert.IsType<HashSet<UserChallenge>>(challenge.Users);
    }

    [Fact]
    public void Challenge_Title_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var title = "Test Challenge";

        // Act
        challenge.Title = title;

        // Assert
        Assert.Equal(title, challenge.Title);
    }

    [Fact]
    public void Challenge_Description_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var description = "Test challenge description.";

        // Act
        challenge.Description = description;

        // Assert
        Assert.Equal(description, challenge.Description);
    }

    [Fact]
    public void Challenge_BeginDateTime_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var beginDateTime = DateTime.Now;

        // Act
        challenge.BeginDateTime = beginDateTime;

        // Assert
        Assert.Equal(beginDateTime, challenge.BeginDateTime);
    }

    [Fact]
    public void Challenge_EndDateTime_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var endDateTime = DateTime.Now.AddHours(24);

        // Act
        challenge.EndDateTime = endDateTime;

        // Assert
        Assert.Equal(endDateTime, challenge.EndDateTime);
    }

    [Fact]
    public void Challenge_ChallengeType_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var challengeType = ChallengeType.Elemation;

        // Act
        challenge.ChallengeType = challengeType;

        // Assert
        Assert.Equal(challengeType, challenge.ChallengeType);
    }

    [Fact]
    public void Challenge_IsActive_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var isActive = true;

        // Act
        challenge.IsActive = isActive;

        // Assert
        Assert.Equal(isActive, challenge.IsActive);
    }

    [Fact]
    public void Challenge_Users_SetAndGet()
    {
        // Arrange
        var challenge = new Challenge();
        var userChallenge = new UserChallenge();

        // Act
        challenge.Users.Add(userChallenge);

        // Assert
        Assert.Contains(userChallenge, challenge.Users);
    }
}
