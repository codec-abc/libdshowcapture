#pragma once

extern "C"
{
	__declspec(dllexport) void startCapture(char* startCaptureOptions, int* arraySize, char** arrayPtr);
}

extern "C"
{
	__declspec(dllexport) unsigned char getDevices(int* arraySize, char** arrayPtr);
}

extern "C"
{
	__declspec(dllexport) void freeByteArray(unsigned char* byteArray);
}

extern "C"
{
	__declspec(dllexport) void shutDownAndFreeDevice(void* device);
}

extern "C"
{
	__declspec(dllexport) void tryGetBuffer
	(
		unsigned char** buffer, 
		void* device, 
		unsigned int* width, 
		unsigned int* height
	);
}

extern "C"
{
	__declspec(dllexport) void recycleUsedBuffer(unsigned char* buffer, void* device);
}