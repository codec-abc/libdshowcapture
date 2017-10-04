#pragma once

extern "C"
{
	__declspec(dllexport) int add(int a, int b);
}

extern "C"
{
	__declspec(dllexport) unsigned char enumDevices();
}

extern "C"
{
	__declspec(dllexport) unsigned char getDevices(int* arraySize, char** arrayPtr);
}