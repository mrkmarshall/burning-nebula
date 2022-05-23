using WebApi.Program;
using WebApi.Library;
namespace WebApi.Program.Test;

[TestClass]
public class ProgramTest
{
    [TestMethod]
    public async Task TestProgram()
    {
        var fakeRepositoryList = new List<Repository>() { new Repository() { Name = "Name 1", Description = "Description 1" }};
        var processor = new TestRepositoryProcessor(fakeRepositoryList);
        
        using (var consoleCapture = new StringWriter())
        {
            Console.SetOut(consoleCapture);
            int repositoriesExpected = fakeRepositoryList.Count();
            int repositoriesFound = await Program.ProcessRepository(processor);

            string output = consoleCapture.ToString();

            Assert.IsTrue(repositoriesFound == repositoriesExpected);
            Assert.IsTrue(output.Contains("Name 1"));
            Assert.IsTrue(output.Contains("Description 1"));
        }
    }
}

internal class TestRepositoryProcessor : IRepositoryProcessor
{
    readonly List<Repository> testRepositoryList;

    public TestRepositoryProcessor(List<Repository> fakeResult)
    {
        testRepositoryList = fakeResult ??  new List<Repository>();
    }
    
    public async Task<List<Repository>> ProcessRepositories()
    {
        return await Task.Run<List<Repository>>(() =>
        {
            return testRepositoryList;
        });
    }
}