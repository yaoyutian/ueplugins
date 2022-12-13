// Fill out your copyright notice in the Description page of Project Settings.

#include "ZbarActor.h"
#include "UEZbar.h"
#include <Runtime/Engine/Public/ImageUtils.h>


// Sets default values
AZbarActor::AZbarActor()
{
	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	
	//PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AZbarActor::BeginPlay()
{
	Super::BeginPlay();

	m_imgscanner.set_config(zbar::ZBAR_NONE, zbar::ZBAR_CFG_ENABLE, 1);
}

// Called every frame
void AZbarActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}
void AZbarActor::EndPlay(const EEndPlayReason::Type EndPlayReason) {
	
	//m_imgscanner.~ImageScanner();
}

bool AZbarActor::ZbarScan(UTextureRenderTarget2D* rt, FString& result_type, FString& result_data) {
	if (m_imgscanner) {
		TArray64<uint8> RawData;

		EPixelFormat pixfmt= rt->GetFormat(); //rendertarget ��ʽformat ��Ҫ rgba8 4byte 

		FRenderTarget* RenderTarget = rt->GameThread_GetRenderTargetResource();
		FIntPoint size = RenderTarget->GetSizeXY();

		FImageUtils::GetRawData(rt, RawData);
		
		unsigned int _l = size.X * size.Y; //=>gray8 =0.21R+0.72G+0.07B;
		
		TArray64<uint8> GrayRaw;
		for(int i=0;i<RawData.Num();i+=4)
		{
			uint8 _g = 0.21 * RawData[i] + 0.72 * RawData[i + 1] + 0.07 * RawData[i + 2];
			GrayRaw.Add(_g);
		}

		zbar::Image image = zbar::Image(size.X, size.Y, "Y800", GrayRaw.GetData(), _l);

		int n = m_imgscanner.scan(image);
		if (n <= 0)
		{
			result_data = FString("scan failed...");
			return false;
		}
		//std::cout << "scan result:" << n << std::endl;
		//std::string rst_type, rst_data;
		// extract results
		for (zbar::Image::SymbolIterator symbol = image.symbol_begin(); symbol != image.symbol_end(); ++symbol) {
			// do something useful with results
			result_type = UTF8_TO_TCHAR((symbol->get_type_name()).c_str());
			//std::wstring = symbol->get_data();
			result_data =UTF8_TO_TCHAR (symbol->get_data().c_str());
			//std::cout << "decoded " << result_type<< " symbol \"" << result_data << '"' << std::endl;
		}
		// clean up
		image.set_data(NULL, 0);

	}

	return true;
}
