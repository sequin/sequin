#r "./packages/FAKE/tools/FakeLib.dll"
#load "buildHelpers.fsx"

open Fake
open BuildHelpers

let projectNames = getRequiredBuildParamArray "projects"

let artifacts = "./artifacts/"
let testDirectory = artifacts + "tests/"
let deployDirectory = artifacts + "deploy/"

Target "Clean" (fun _ ->
    projectNames |> Seq.iter (fun(projectName) ->
        Build.CleanProject projectName
    )

    CleanDirs[testDirectory; deployDirectory]
)

Target "Restore" (fun _ ->
    "./src/Sequin.sln"
        |> RestoreMSSolutionPackages (fun p ->
            { p with
                OutputPath = "./src/packages" })
)

Target "SetVersion" (fun _ ->
    SetAssemblyVersion()
)

Target "Build" (fun _ ->
    projectNames |> Seq.iter (fun(projectName) ->
        Build.Build (fun p ->
            {p with
                ProjectReferences = !! ("src/" + projectName + "/*.csproj") }))
)

Target "RunTests" (fun _ ->
    Test.RunTests (fun p ->
        {p with
            TestDirectory = testDirectory
            ProjectReferences = !! "src/*Integration/*.csproj"
            TestAssemblyPattern = "/*Integration.dll" })
)

Target "CreatePackages" (fun _ ->
    Nuget.CreatePackages projectNames deployDirectory
)

Target "Publish" (fun _ ->
    Nuget.PublishPackages deployDirectory
)

"Clean"
    ==> "Restore"
    ==> "SetVersion"
    ==> "Build"
    ==> "RunTests"
    ==> "CreatePackages"
    ==> "Publish"

RunTargetOrDefault "RunTests"
