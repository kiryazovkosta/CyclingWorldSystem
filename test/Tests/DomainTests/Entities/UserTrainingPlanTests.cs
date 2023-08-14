using Domain.Entities;
using Domain.Identity;

namespace Tests.DomainTests.Entities;

public class UserTrainingPlanTests
{
    [Fact]
    public void UserTrainingPlan_UserId_SetAndGet()
    {
        // Arrange
        var userTrainingPlan = new UserTrainingPlan();
        var userId = Guid.NewGuid();

        // Act
        userTrainingPlan.UserId = userId;

        // Assert
        Assert.Equal(userId, userTrainingPlan.UserId);
    }

    [Fact]
    public void UserTrainingPlan_User_SetAndGet()
    {
        // Arrange
        var userTrainingPlan = new UserTrainingPlan();
        var user = new User();

        // Act
        userTrainingPlan.User = user;

        // Assert
        Assert.Same(user, userTrainingPlan.User);
    }

    [Fact]
    public void UserTrainingPlan_TrainingPlanId_SetAndGet()
    {
        // Arrange
        var userTrainingPlan = new UserTrainingPlan();
        var trainingPlanId = Guid.NewGuid();

        // Act
        userTrainingPlan.TrainingPlanId = trainingPlanId;

        // Assert
        Assert.Equal(trainingPlanId, userTrainingPlan.TrainingPlanId);
    }

    [Fact]
    public void UserTrainingPlan_TrainingPlan_SetAndGet()
    {
        // Arrange
        var userTrainingPlan = new UserTrainingPlan();
        var trainingPlan = new TrainingPlan();

        // Act
        userTrainingPlan.TrainingPlan = trainingPlan;

        // Assert
        Assert.Same(trainingPlan, userTrainingPlan.TrainingPlan);
    }

    [Fact]
    public void UserTrainingPlan_IsComplited_SetAndGet()
    {
        // Arrange
        var userTrainingPlan = new UserTrainingPlan();
        var isCompleted = true;

        // Act
        userTrainingPlan.IsComplited = isCompleted;

        // Assert
        Assert.Equal(isCompleted, userTrainingPlan.IsComplited);
    }
}
