#r "./packages/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.OctoTools
open Fake.Testing.XUnit2

let private splitOnSemiColon (paramValue: string) = paramValue.Split([|";"|], StringSplitOptions.RemoveEmptyEntries)

let getRequiredBuildParam = fun paramName ->
    if not (hasBuildParam paramName) then failwithf "Parameter '%s' must be specified to run current target" paramName
    getBuildParam paramName

let getBuildParamArray paramName = getBuildParam paramName |> splitOnSemiColon

let getRequiredBuildParamArray paramName = getRequiredBuildParam paramName |> splitOnSemiColon

let SetAssemblyVersion() =
    let version = getRequiredBuildParam "version"
    let semver = getRequiredBuildParam "semver"

    BulkReplaceAssemblyInfoVersions "./src" (fun f ->
                                              {f with
                                                  AssemblyVersion = version
                                                  AssemblyInformationalVersion = semver})

module Nuget =
    let private Publish = fun package ->
        let nugetApiKey = getRequiredBuildParam "nugetApiKey"

        let result = ExecProcess (fun info ->
            info.FileName <- ".nuget/nuget.exe"
            info.Arguments <- sprintf "push \"%s\" %s" package nugetApiKey) (TimeSpan.FromMinutes 5.0)

        if result <> 0 then failwithf "nuget.exe returned with a non-zero exit code"

    let private CreatePackage = fun projectName deployDirectory ->
        let csprojFile = "src/" + projectName + "/" + projectName + ".csproj"
        let version = getRequiredBuildParam "packageversion"
        let result = ExecProcess (fun info ->
            info.FileName <- ".nuget/nuget.exe"
            info.Arguments <- sprintf "pack \"%s\" -OutputDirectory \"%s\" -Version %s -IncludeReferencedProjects -Properties Configuration=Release -Properties Platform=AnyCPU" csprojFile deployDirectory version) (TimeSpan.FromMinutes 5.0)

        if result <> 0 then failwithf "nuget.exe returned with a non-zero exit code"

    let CreatePackages = fun projectNames deployDirectory ->
        projectNames |> Seq.iter(fun (projectName) ->
            CreatePackage projectName deployDirectory
        )

    let PublishPackages = fun sourceDir ->
        !! (sourceDir + "*.nupkg")
            |> Seq.iter Publish

module Build =
    type BuildParams =
        {
            Configuration : string
            BuildDirectory : string
            ProjectReferences : FileIncludes
            Version : string
        }

    let BuildDefaults() =
        { Configuration = "Release"
          BuildDirectory = null
          ProjectReferences = !!"./*.csproj"
          Version = (getRequiredBuildParam "version") }

    let CleanProject = fun projectName ->
        let binDirectory = "src/" + projectName + "/bin/"
        let objDirectory = "src/" + projectName + "/obj/"

        DeleteDirs[binDirectory;objDirectory]

    let Build = fun setParams ->
        let parameters = BuildDefaults() |> setParams

        let buildParameters defaults =
            [
                "Configuration", parameters.Configuration
                "Platform", "AnyCPU"
            ]

        MSBuildWithProjectProperties parameters.BuildDirectory "Build" buildParameters parameters.ProjectReferences |> ignore

module Test =
    type TestParams =
        {
            TestDirectory : string
            ToolPath : string
            ProjectReferences : FileIncludes
            TestAssemblyPattern : string
        }

    let TestDefaults() =
        { TestDirectory = null
          ToolPath = "./packages/xunit.runner.console/tools/xunit.console.exe"
          ProjectReferences = !!"*Tests/*.csproj"
          TestAssemblyPattern = "/*Tests.dll" }

    let RunTests = fun setParams->
        let parameters = TestDefaults() |> setParams

        Build.Build (fun p ->
            { p with
                BuildDirectory = parameters.TestDirectory
                ProjectReferences = parameters.ProjectReferences })

        let testAssemblies =  !! (parameters.TestDirectory + parameters.TestAssemblyPattern)

        if not (Seq.isEmpty testAssemblies) then
            testAssemblies |> xUnit2 (fun p ->
                {p with
                    ToolPath = parameters.ToolPath })
