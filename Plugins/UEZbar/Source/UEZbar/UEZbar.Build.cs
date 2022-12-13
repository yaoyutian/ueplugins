// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.IO;

public class UEZbar : ModuleRules
{
    private string ModulePath
    {
        get { return ModuleDirectory; }
    }

    private string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModulePath, "../../ThirdParty/")); }
    }

    private string UProjectPath
    {
        get { return Directory.GetParent(ModulePath).Parent.FullName; }
    }

    private void CopyToBinaries(string Filepath, ReadOnlyTargetRules Target)
    {
        string binariesDir = Path.Combine(UProjectPath, "Binaries", Target.Platform.ToString());
        string filename = Path.GetFileName(Filepath);

        System.Console.WriteLine("Writing file " + Filepath + " to " + binariesDir);

        if (!Directory.Exists(binariesDir))
            Directory.CreateDirectory(binariesDir);

        if (!File.Exists(Path.Combine(binariesDir, filename)))
            File.Copy(Filepath, Path.Combine(binariesDir, filename), true);
    }

    public UEZbar(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            System.Console.WriteLine("... ThirdPartyPath -> " + ThirdPartyPath);

            PublicIncludePaths.AddRange(
            new string[] {
                Path.Combine(ThirdPartyPath,"zbar/include")
				// ... add public include paths required here ...
			}
            );
            //PublicSystemLibraryPaths.Add(Path.Combine(ThirdPartyPath, "zbar/x64/lib"));
            //PublicAdditionalLibraries.Add(Path.Combine(ThirdPartyPath, "zbar/x64/lib/libzbar64-0.lib"));
            
            //.lib filename
            PublicAdditionalLibraries.Add(Path.Combine(ThirdPartyPath, "zbar/x64/lib", "libzbar64-0.lib"));

            RuntimeDependencies.Add("$(BinaryOutputDir)/libiconv.dll", Path.Combine(ThirdPartyPath, "zbar/x64/bin/libiconv.dll"));
            RuntimeDependencies.Add("$(BinaryOutputDir)/libzbar64-0.dll", Path.Combine(ThirdPartyPath, "zbar/x64/bin/libzbar64-0.dll"));

            //RuntimeDependencies.Add(Path.Combine(ThirdPartyPath, "zbar/x64/bin/libiconv.dll"));
            //RuntimeDependencies.Add(Path.Combine(ThirdPartyPath, "zbar/x64/bin/libzbar64-0.dll"));

            //PublicDelayLoadDLLs.Add("libiconv.dll");
            //PublicDelayLoadDLLs.Add("libzbar64-0.dll");

            PrivateIncludePaths.AddRange(
                new string[] {
                    // ... add other private include paths required here ...
                }
                );


            PublicDependencyModuleNames.AddRange(
                new string[]
                {
                "Core",
                "CoreUObject",
                "Engine",
                "Projects"
                    // ... add other public dependencies that you statically link with here ...
                }
                );


            PrivateDependencyModuleNames.AddRange(
                new string[]
                {
                    // ... add private dependencies that you statically link with here ...	
                }
                );


            DynamicallyLoadedModuleNames.AddRange(
                new string[]
                {
                    // ... add any modules that your module loads dynamically here ...
                }
                );
        }
    }
}
