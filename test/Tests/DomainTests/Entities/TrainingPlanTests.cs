using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class TrainingPlanTests
{
    [Fact]
    public void TrainingPlan_Constructors_InitializePropertiesAndCollections()
    {
        // Arrange & Act
        var trainingPlan = new TrainingPlan();

        // Assert
        Assert.NotNull(trainingPlan.Workouts);
        Assert.IsType<HashSet<Workout>>(trainingPlan.Workouts);

        Assert.NotNull(trainingPlan.Users);
        Assert.IsType<HashSet<UserTrainingPlan>>(trainingPlan.Users);
    }

    [Fact]
    public void TrainingPlan_Title_SetAndGet()
    {
        // Arrange
        var trainingPlan = new TrainingPlan();
        var title = "Test Training Plan";

        // Act
        trainingPlan.Title = title;

        // Assert
        Assert.Equal(title, trainingPlan.Title);
    }

    [Fact]
    public void TrainingPlan_Workouts_SetAndGet()
    {
        // Arrange
        var trainingPlan = new TrainingPlan();
        var workout = new Workout();

        // Act
        trainingPlan.Workouts.Add(workout);

        // Assert
        Assert.Contains(workout, trainingPlan.Workouts);
    }

    [Fact]
    public void TrainingPlan_Users_SetAndGet()
    {
        // Arrange
        var trainingPlan = new TrainingPlan();
        var userTrainingPlan = new UserTrainingPlan();

        // Act
        trainingPlan.Users.Add(userTrainingPlan);

        // Assert
        Assert.Contains(userTrainingPlan, trainingPlan.Users);
    }
}
