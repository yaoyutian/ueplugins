// Copyright Epic Games, Inc. All Rights Reserved.

#include "UEZbar.h"
#include "Core.h"
#include "Modules/ModuleManager.h"
#include "Interfaces/IPluginManager.h"

#define LOCTEXT_NAMESPACE "FUEZbarModule"

void FUEZbarModule::StartupModule()
{
	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module

	// Get the base directory of this plugin
	FString BaseDir = IPluginManager::Get().FindPlugin("UEZbar")->GetBaseDir();

	// Add on the relative location of the third party dll and load it
	FString LibiconvPath, LibraryPath;
#if PLATFORM_WINDOWS
	LibiconvPath = FPaths::Combine(*BaseDir, TEXT("ThirdParty/zbar/x64/bin/libiconv.dll"));
	LibraryPath = FPaths::Combine(*BaseDir, TEXT("ThirdParty/zbar/x64/bin/libzbar64-0.dll"));
#elif PLATFORM_MAC
	LibraryPath = FPaths::Combine(*BaseDir, TEXT("Source/ThirdParty/UEZbarLibrary/Mac/Release/libExampleLibrary.dylib"));
#elif PLATFORM_LINUX
	LibraryPath = FPaths::Combine(*BaseDir, TEXT("Binaries/ThirdParty/UEZbarLibrary/Linux/x86_64-unknown-linux-gnu/libExampleLibrary.so"));
#endif 

	// PLATFORM_WINDOWS

	LibiconvHandle = !LibiconvPath.IsEmpty() ? FPlatformProcess::GetDllHandle(*LibiconvPath) : nullptr;
	ZbarLibraryHandle = !LibraryPath.IsEmpty() ? FPlatformProcess::GetDllHandle(*LibraryPath) : nullptr;
	if (ZbarLibraryHandle)
	{
		// Call the test function in the third party library that opens a message box
		//FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT("Zbar dll load scuess", "success to load example third party library"));
	}
	else
	{
		FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT("ThirdPartyLibraryError", "Failed to load example third party library"));
	}


}

void FUEZbarModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.

	// Free the dll handle
	if (ZbarLibraryHandle) {
		FPlatformProcess::FreeDllHandle(ZbarLibraryHandle);
		ZbarLibraryHandle = nullptr;
	}
	if (LibiconvHandle) {
		FPlatformProcess::FreeDllHandle(LibiconvHandle);
		LibiconvHandle = nullptr;
	}
}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FUEZbarModule, UEZbar)
