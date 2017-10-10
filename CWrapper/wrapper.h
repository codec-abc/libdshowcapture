#pragma once

extern "C"
{
	__declspec(dllexport) unsigned char startCapture(char* input);
}

extern "C"
{
	__declspec(dllexport) unsigned char getDevices(int* arraySize, char** arrayPtr);
}