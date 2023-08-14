using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class WorkoutTests
{
    [Fact]
    public void Workout_Title_SetAndGet()
    {
        // Arrange
        var workout = new Workout();
        var title = "Test Workout";

        // Act
        workout.Title = title;

        // Assert
        Assert.Equal(title, workout.Title);
    }

    [Fact]
    public void Workout_TrainingPlanId_SetAndGet()
    {
        // Arrange
        var workout = new Workout();
        var trainingPlanId = Guid.NewGuid();

        // Act
        workout.TrainingPlanId = trainingPlanId;

        // Assert
        Assert.Equal(trainingPlanId, workout.TrainingPlanId);
    }

    [Fact]
    public void Workout_TrainingPlan_SetAndGet()
    {
        // Arrange
        var workout = new Workout();
        var trainingPlan = new TrainingPlan();

        // Act
        workout.TrainingPlan = trainingPlan;

        // Assert
        Assert.Same(trainingPlan, workout.TrainingPlan);
    }
}
