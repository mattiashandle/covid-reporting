#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"

var target = Argument("Target", "CI");
var SolutionFile = MakeAbsolute(File("Karolinska.sln"));
var srcFolder = SolutionFile.GetDirectory().Combine("src");
var outputFolder = SolutionFile.GetDirectory().Combine("outputs");
var testFolder = SolutionFile.GetDirectory().Combine("tests");
var testOutputFolder = outputFolder.Combine("tests");
var coverageOutputFile = testOutputFolder.CombineWithFilePath("coverage.dcvr");
var dotCoverFolder = MakeAbsolute(Context.Tools.Resolve("dotcover.exe").GetDirect‌​ory());
var hostProject = MakeAbsolute(File("./src/Karolinska.Web/Karolinska.Web.csproj"));

Setup(context =>
{
    CleanDirectory(outputFolder);
});

Task("CI")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Publish");

Task("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings {
        Sources = new []{
            "https://api.nuget.org/v3/index.json",
        }
    };

    DotNetCoreRestore(SolutionFile.FullPath, settings);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = "Debug"
    };

    DotNetCoreBuild(SolutionFile.FullPath, settings);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    Information($"Looking for test projects in {testFolder.FullPath}");

    var testProjects = GetFiles(testFolder, "*.csproj", SearchOption.AllDirectories);

    var dotCoverSettings = new DotCoverCoverSettings()
                                    .WithFilter("+:EMG.*")
                                    .WithFilter("-:Tests.*");

    foreach (var project in testProjects)
    {
        Information($"Testing {project.FullPath}");

        var testResultFile = testOutputFolder.CombineWithFilePath(project.GetFilenameWithoutExtension() + ".trx");
        var coverageResultFile = testOutputFolder.CombineWithFilePath(project.GetFilenameWithoutExtension() + ".dvcr");
        
        Verbose($"Saving test results on {testResultFile.FullPath}");

        var settings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            NoRestore = true,
            Logger = $"trx;LogFileName={testResultFile.FullPath}"
        };

        DotCoverCover(context => 
        {
                context.DotNetCoreTest(project.FullPath, settings);
        }, coverageResultFile, dotCoverSettings);

        if (BuildSystem.IsRunningOnTeamCity)
        {
            TeamCity.ImportData("mstest", testResultFile);
        }
    }

    var coverageFiles = GetFiles(testOutputFolder, "*.dvcr");
    DotCoverMerge(coverageFiles, coverageOutputFile);
    DeleteFiles(coverageFiles);

    if (BuildSystem.IsRunningOnTeamCity)
    {
        TeamCity.ImportDotCoverCoverage(coverageOutputFile, dotCoverFolder);
    }
});

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => 
{
    var settings = new DotNetCorePublishSettings
    {
        Configuration = "Release",
        OutputDirectory = outputFolder
    };

    DotNetCorePublish(hostProject.FullPath, settings);
});

bool IsBuildPersonal() => bool.TryParse(EnvironmentVariable("BUILD_IS_PERSONAL"), out bool res) && res;

RunTarget(target);

public static IEnumerable<FilePath> GetFiles(DirectoryPath directory, string pattern = "*.*", SearchOption option = SearchOption.TopDirectoryOnly)
{
    var files = System.IO.Directory.GetFiles(directory.FullPath, pattern, option);
    return files.Select(file => (FilePath)file);
}