var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./bin") + Directory(configuration);
var publishDir = Directory("./publish") + Directory(configuration);

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/MitsubaArchivizer.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      MSBuild("./src/MitsubaArchivizer.GUI/MitsubaArchivizer.GUI.csproj", settings => {
        settings
            .SetConfiguration(configuration)
            .WithProperty("OutputPath", MakeAbsolute(buildDir).FullPath);
      });
    }
    else
    {
      XBuild("./src/MitsubaArchivizer.GUI/MitsubaArchivizer.GUI.csproj", settings => {
        settings
            .SetConfiguration(configuration)
            .WithProperty("OutputPath", MakeAbsolute(buildDir).FullPath);
      });
    }
    
    DotNetCorePublish("./src/MitsubaArchivizer.CLI/MitsubaArchivizer.CLI.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.2",
        Configuration = configuration,
        OutputDirectory = buildDir
    });
});

Task("Zip")
    .IsDependentOn("Build")
    .Does(() =>
{
	CreateDirectory(publishDir);
    Zip(buildDir, publishDir + File("MitsubaArchivizer.zip"));
});

Task("Default")
    .IsDependentOn("Zip");

RunTarget(target);