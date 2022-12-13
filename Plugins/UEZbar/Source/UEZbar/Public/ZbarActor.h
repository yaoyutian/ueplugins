// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "zbar.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Engine/TextureRenderTarget2D.h"
#include "ZbarActor.generated.h"

//using namespace zbar;
//class zbar::ImageScanner;

UCLASS()
class UEZBAR_API AZbarActor : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AZbarActor();

	UFUNCTION(BlueprintCallable,Category = "ZbarScan")
		bool ZbarScan(UTextureRenderTarget2D* rt, FString& result_type, FString& result_data);

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	virtual void EndPlay(const EEndPlayReason::Type EndPlayReason) override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

private:
	zbar::ImageScanner m_imgscanner;
	
};
