namespace Atata.Reqnroll.NUnit.IntegrationTests;

public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder) =>
        builder.LogConsumers.AddNLogFile();
}
