// Copyright (c) 2019 Nineva Studios

using System.IO;
using UnrealBuildTool;

#if UE_5_0_OR_LATER
using EpicGames.Core;
#else
using Tools.DotNETCommon;
#endif

public class MqttUtilities : ModuleRules
{
	public MqttUtilities(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicIncludePaths.AddRange(
			new string[] {
                Path.Combine (ModuleDirectory, "Public")
			    }
			);


		PrivateIncludePaths.AddRange(
			new string[] {
                Path.Combine (ModuleDirectory, "Private")
				// ... add other private include paths required here ...
			}
			);


		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				// ... add other public dependencies that you statically link with here ...
			}
			);


		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
                "Projects",
                // ... add private dependencies that you statically link with here ...
			}
			);


		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);



        // Additional routine for Windows
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            PrivateIncludePaths.Add(Path.Combine(ModuleDirectory, "Private/Windows"));

            LoadThirdPartyLibrary("mosquitto", Target);
            LoadThirdPartyLibrary("mosquittopp", Target);

            {
                string thirdpartyDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, "..", "ThirdParty"));
                string mqttLibDirectory = Path.Combine(thirdpartyDirectory, Target.Platform.ToString());
                string _cryptodllpath = Path.Combine(mqttLibDirectory, "libcrypto-3-x64.dll");
                string _ssldllpath = Path.Combine(mqttLibDirectory, "libssl-3-x64.dll");

                string pluginBinariesDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../..", "Binaries"));
                string _cryptobinpath = Path.Combine(pluginBinariesDir, Target.Platform.ToString(), "libcrypto-3-x64.dll");
                if (!File.Exists(_cryptobinpath))
                    File.Copy(_cryptodllpath, _cryptobinpath, true);
                string sslbinpath = Path.Combine(pluginBinariesDir, Target.Platform.ToString(), "libssl-3-x64.dll");
                if (!File.Exists(sslbinpath))
                    File.Copy(_ssldllpath, sslbinpath, true);

                PublicDelayLoadDLLs.Add("libcrypto-3-x64.dll");
                PublicDelayLoadDLLs.Add("libssl-3-x64.dll");

                RuntimeDependencies.Add(_cryptobinpath);
                RuntimeDependencies.Add(sslbinpath);
            }

        }

        // Additional routine for Android
        if (Target.Platform == UnrealTargetPlatform.Android)
        {
            PrivateIncludePaths.Add(Path.Combine(ModuleDirectory, "Private/Android"));

            PrivateDependencyModuleNames.AddRange(new string[] { "Launch" });
            string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);

            AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(PluginPath, "MqttUtilities_Android_UPL.xml"));
        }

        if (Target.Platform == UnrealTargetPlatform.Linux)
        {
            PrivateIncludePaths.Add(Path.Combine(ModuleDirectory, "Private/Linux"));

            LoadThirdPartyLibrary("libmosquitto", Target);
            LoadThirdPartyLibrary("libmosquittopp", Target);
        }
  	}

    public void LoadThirdPartyLibrary(string libname, ReadOnlyTargetRules Target)
    {
        string StaticLibExtension = ".lib";
        string DynamicLibExtension = string.Empty;

        if(Target.Platform == UnrealTargetPlatform.Win64)
        {
            DynamicLibExtension = ".dll";
        }
        if(Target.Platform == UnrealTargetPlatform.Linux)
        {
            DynamicLibExtension = ".so";
			StaticLibExtension = "_static.a";
        }

        string ThirdPartyPath = Path.Combine(ModuleDirectory, "../ThirdParty", Target.Platform.ToString());
        string LibrariesPath = Path.Combine(ThirdPartyPath, libname, "libraries");
        string IncludesPath = Path.Combine(ThirdPartyPath, libname, "includes");
        string BinariesPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../../Binaries", Target.Platform.ToString()));

        // Link static library (Windows only)

        if(Target.Platform == UnrealTargetPlatform.Win64 || Target.Platform == UnrealTargetPlatform.Linux)
        {
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, libname + StaticLibExtension));
        }

        // Copy dynamic library to Binaries folder

        if (!Directory.Exists(BinariesPath))
        {
            Directory.CreateDirectory(BinariesPath);
        }

        if (!File.Exists(Path.Combine(BinariesPath, libname + DynamicLibExtension)))
        {
            File.Copy(Path.Combine(LibrariesPath, libname + DynamicLibExtension), Path.Combine(BinariesPath, libname + DynamicLibExtension), true);
        }

        // Set up dynamic library
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            PublicDelayLoadDLLs.Add(libname + DynamicLibExtension);
        }
        if (Target.Platform == UnrealTargetPlatform.Linux)
        {
            PublicDelayLoadDLLs.Add(Path.Combine(BinariesPath, libname + DynamicLibExtension));
        }

        RuntimeDependencies.Add(Path.Combine(BinariesPath, libname + DynamicLibExtension));

        // Set up include path
        PublicIncludePaths.Add(IncludesPath);

        // Add definitions
        PublicDefinitions.Add(string.Format("WITH_" + libname.ToUpper() + "_BINDING={0}", 1));
    }
}
