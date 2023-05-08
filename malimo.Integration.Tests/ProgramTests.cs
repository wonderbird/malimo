namespace malimo.Integration.Tests;

public class ProgramTests
{
    [Fact]
    public void VerboseDryRun()
    {
        var process = new TestProcessWrapper.TestProcessWrapper("malimo", false);
        process.Start();
        process.WaitForProcessExit();
        process.ForceTermination();

        Assert.Matches("Would move '.*/noun-island-1479438.png'", process.RecordedOutput);
    }
}
