#pragma once

typedef __int32(__stdcall *Read)(void*, __int32);
typedef __int32(__stdcall *Write)(void*, __int32);
typedef __int64(__stdcall *Seek)(__int64, __int32);

typedef struct {

	Read Read;
	Write Write;
	Seek Seek;

} INativeStream;
