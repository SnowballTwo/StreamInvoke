#include "Unmanaged.h"

#if defined (UNMANAGED_EXPORTS)
#define API_EXPORT __declspec(dllexport)
#else
#define API_EXPORT __declspec(dllimport)
#endif

#if defined(__cplusplus)
extern "C" {
#endif

	API_EXPORT void Test(INativeStream* nativeStream)
	{
		char buffer[1024];

		auto read = nativeStream->Read(buffer, 1024);
		auto seek = nativeStream->Seek(-read / 2, 2);
		auto written = nativeStream->Write(buffer, read / 2 - 1);
	}

#if defined(__cplusplus)
}
#endif