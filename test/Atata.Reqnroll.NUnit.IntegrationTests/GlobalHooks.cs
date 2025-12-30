namespace Atata.Reqnroll.NUnit.IntegrationTests;

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
        // TODO: Add extra configuration for feature AtataContext.
    }

    private static void ConfigureScenarioAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext,
        ScenarioContext scenarioContext)
    {
        // TODO: Add extra configuration for scenario AtataContext.
    }
}
