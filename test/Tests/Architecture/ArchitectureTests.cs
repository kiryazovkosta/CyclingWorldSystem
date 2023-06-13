namespace Tests.Architecture;

using NetArchTest.Rules;
using FluentAssertions;

public class ArchitectureTests
{
	private const string DomainNamespace = "Domain";
	private const string ApplicationNamespace = "Application";
	private const string InfrastructureNamespace = "Infrastructure";
	private const string PersistenceNamespace = "Persistence";
	private const string PresentationNamespace = "Presentation";
	private const string WebApiNamespace = "WebApi";

	[Fact]
	public void Domain_Should_NotHaveDependenciesOnOtherProjects()
	{
		// Arrange
		var assembly = Domain.AssemblyReference.Assembly;

		var otherProjects = new[]
		{
			ApplicationNamespace, 
			InfrastructureNamespace, 
			PersistenceNamespace, 
			PresentationNamespace, 
			WebApiNamespace
		};

		// Act 
		var testResult = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAll(otherProjects)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Application_Should_NotHaveDependenciesOnOtherProjects()
	{
		// Arrange
		var assembly = Application.AssemblyReference.Assembly;

		var otherProjects = new[]
		{
			InfrastructureNamespace,
			PersistenceNamespace,
			PresentationNamespace,
			WebApiNamespace
		};

		// Act 
		var testResult = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAll(otherProjects)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}


	[Fact]
	public void Infrastructure_Should_NotHaveDependenciesOnOtherProjects()
	{
		// Arrange
		var assembly = Infrastructure.AssemblyReference.Assembly;

		var otherProjects = new[]
		{
			PersistenceNamespace,
			PresentationNamespace,
			WebApiNamespace
		};

		// Act 
		var testResult = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAll(otherProjects)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Persistence_Should_NotHaveDependenciesOnOtherProjects()
	{
		// Arrange
		var assembly = Persistence.AssemblyReference.Assembly;

		var otherProjects = new[]
		{
			InfrastructureNamespace,
			PresentationNamespace,
			WebApiNamespace
		};

		// Act 
		var testResult = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAll(otherProjects)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Presentation_Should_NotHaveDependenciesOnOtherProjects()
	{
		// Arrange
		var assembly = Presentation.AssemblyReference.Assembly;

		var otherProjects = new[]
		{
			InfrastructureNamespace,
			PersistenceNamespace,
			WebApiNamespace
		};

		// Act 
		var testResult = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAll(otherProjects)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public  void Handlers_Should_HaveDependencyOnDomain()
	{
		// Arrange 
		var assembly = Application.AssemblyReference.Assembly;

		// Act
		var testResult = Types
			.InAssembly(assembly)
			.That()
			.HaveNameEndingWith("Handler")
			.Should()
			.HaveDependencyOn(DomainNamespace)
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Controllers_Should_HaveDependencyOnMediatR()
	{
		// Arrange 
		var assembly = Presentation.AssemblyReference.Assembly;

		// Act
		var testResult = Types
			.InAssembly(assembly)
			.That()
			.HaveNameEndingWith("Controller")
			.Should()
			.HaveDependencyOn("MediatR")
			.GetResult();

		// Assert
		testResult.IsSuccessful.Should().BeTrue();
	}
}
