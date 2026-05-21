# Atata.Reqnroll.NUnit

[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![X](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://x.com/AtataFramework)

**Atata.Reqnroll.NUnit** is a C#/.NET library that integrates [Atata](https://github.com/atata-framework/atata) with Reqnroll+NUnit.

*The package targets .NET 8.0 and .NET Framework 4.6.2.*

## Installation

Install the package via .NET CLI:

```bash
dotnet add package Atata.Reqnroll.NUnit
```

Or using Package Manager:

```powershell
Install-Package Atata.Reqnroll.NUnit
```

### Dependencies

- [Atata](https://www.nuget.org/packages/Atata)
- [Atata.NUnit](https://www.nuget.org/packages/Atata.NUnit)
- [Reqnroll.NUnit](https://www.nuget.org/packages/Reqnroll.NUnit)

## Usage

### Global fixture

For global setup across all test suites, create a class that inherits from `AtataGlobalFixture` (from Atata.NUnit library):

*The global fixture setup is the same as for Atata.NUnit.*

```cs
public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
        // Configure global properties for AtataContext
    }

    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
        // Configure base AtataContext configuration
    }

    protected override void ConfigureGlobalAtataContext(AtataContextBuilder builder)
    {
        // Configure global AtataContext
    }

    protected override void OnBeforeGlobalSetup()
    {
        // Custom logic before global setup
    }

    protected override void OnAfterGlobalSetup()
    {
        // Custom logic after global setup
    }

    protected override void OnBeforeGlobalTeardown()
    {
        // Custom logic before global teardown
    }

    protected override void OnAfterGlobalTeardown()
    {
        // Custom logic after global teardown
    }
}
```

All the methods are virtual and are not required to be overridden.
You can choose to override only those that are relevant to your setup.
Mostly you will only need to override `ConfigureAtataContextBaseConfiguration` and `ConfigureGlobalAtataContext`.

### Global hooks

Create a hooks class like below:

```cs
[Binding]
public sealed class GlobalHooks
{
    [BeforeFeature]
    public static void SetUpFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.SetUpFeature(featureContext, ConfigureFeatureAtataContext);

    [AfterFeature]
    public static void TearDownFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.TearDownFeature(featureContext);

    [BeforeScenario]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void SetUpScenario(FeatureContext featureContext, ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.SetUpScenario(featureContext, scenarioContext, ConfigureScenarioAtataContext);

    [AfterScenario]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void TearDownScenario(ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.TearDownScenario(scenarioContext);

    private static void ConfigureFeatureAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext)
    {
        // Add extra configuration for feature AtataContext
    }

    private static void ConfigureScenarioAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext,
        ScenarioContext scenarioContext)
    {
        // Add extra configuration for scenario AtataContext
    }
}
```

### Getting `AtataContext` instances

The current instance of `AtataContext` can be accessed via `ScenarioContext` or `FeatureContext`.

```cs
_scenarioContext.Get<AtataContext>();
```

## Examples

Check out example project with web UI tests: [Atata Samples / Using Reqnroll](https://github.com/atata-framework/atata-samples/tree/main/Reqnroll)

### Example of a steps class

```cs
[Binding]
public sealed class CalculationsSteps : Steps
{
    [Given(@"I am on the Calculations page")]
    public static void GivenIAmOnTheCalculationsPage() =>
        Go.To<CalculationsPage>();

    [When(@"I type (.*) and (.*) to the form")]
    public static void WhenITypeArgumentsToTheForm(int argument1, int argument2) =>
        Go.On<CalculationsPage>()
            .AdditionValue1.Set(argument1)
            .AdditionValue2.Set(argument2);

    [Then(@"I should see (.*) in result field")]
    public static void ThenIShouldSeeInResultField(int result) =>
        Go.On<CalculationsPage>()
            .AdditionResult.Should.Be(result);
}
```

### Example of a feature file

```gherkin
Feature: Calculations
	This feature file contains scenarios for Calculations functionality

Scenario Outline: Check calculation logic
	This scenario should check calculation logic

	Given I am on the Calculations page
	When I type <argument1> and <argument2> to the form
	Then I should see <result> in result field

Examples: 
        | argument1 | argument2 | result |
        | 10        | 20        | 30     |
        | 1         | 1         | 2      |
        | -1        | 1         | 0      |
        | -100      | 50        | -50    | 
```

### How to configure a single web session for feature

```cs
[Binding]
public sealed class GlobalHooks
{
    private const string UsesSingleWebSessionTag = "UsesSingleWebSession";

    [BeforeFeature]
    public static void SetUpFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.SetUpFeature(featureContext, ConfigureFeatureAtataContext);

    [AfterFeature]
    public static void TearDownFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.TearDownFeature(featureContext);

    [BeforeScenario]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void SetUpScenario(FeatureContext featureContext, ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.SetUpScenario(featureContext, scenarioContext, ConfigureScenarioAtataContext);

    [AfterScenario]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void TearDownScenario(ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.TearDownScenario(scenarioContext);

    private static void ConfigureFeatureAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext)
    {
        if (featureContext.FeatureInfo.Tags.Contains(UsesSingleWebSessionTag))
        {
            builder.Sessions.ConfigureWebDriver(x => x
                .UseStart(true)
                .UseAsShared());
        }

        // TODO: Add extra configuration for feature AtataContext, or remove the comment.
    }

    private static void ConfigureScenarioAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext,
        ScenarioContext scenarioContext)
    {
        if (scenarioContext.ScenarioInfo.CombinedTags.Contains(UsesSingleWebSessionTag))
        {
            builder.Sessions.ConfigureWebDriver(x => x
                .UseStart(false));
            builder.Sessions.Borrow<WebDriverSession>();
        }

        // TODO: Add extra configuration for scenario AtataContext, or remove the comment.
    }
}
```

Then you can apply `@UsesSingleWebSession` tag to features.

## Community

- Slack: [https://atata-framework.slack.com](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
- X: https://x.com/AtataFramework
- Stack Overflow: https://stackoverflow.com/questions/tagged/atata

## Feedback

Any feedback, issues and feature requests are welcome.

If you faced an issue please report it to [Atata.Reqnroll.NUnit Issues](https://github.com/atata-framework/atata-reqnroll-nunit/issues),
[ask a question on Stack Overflow](https://stackoverflow.com/questions/ask?tags=atata+csharp) using [atata](https://stackoverflow.com/questions/tagged/atata) tag
or use another [Atata Contact](https://atata.io/contact/) way.

## Contact author

Contact me if you need a help in test automation using Atata Framework, or if you are looking for a quality test automation implementation for your project.

- LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
- Email: yevgeniy.shunevych@gmail.com
- Consulting: https://atata.io/consulting/

## Contributing

Check out [Contributing Guidelines](CONTRIBUTING.md) for details.

## SemVer

Atata Framework tries to follow [Semantic Versioning 2.0](https://semver.org/) when possible.
Sometimes Selenium.WebDriver dependency package can contain breaking changes in minor version releases,
so those changes can break Atata as well.
But Atata manages its sources according to SemVer.
Thus backward compatibility is mostly followed and updates within the same major version
(e.g. from 2.1 to 2.2) should not require code changes.

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
