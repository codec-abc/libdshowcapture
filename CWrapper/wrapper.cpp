#include <iostream>
#include <fstream>

#include "wrapper.h"
#include "../dshowcapture.hpp"

#include "proto.pb.h"
#include <google/protobuf/util/json_util.h>

int add(int a, int b)
{
	return a + b;
}

void SampleCallback(const DShow::VideoConfig &config,
	unsigned char *data, size_t size,
	long long startTime, long long stopTime)
{
	int width = config.cx;
	int heigth = config.cy;
	std::ofstream myfile("image.rgba", std::ios_base::binary);
	myfile.write((char*) data, size);
	myfile.flush();
	myfile.close();
}


unsigned char enumDevices()
{
	std::vector<DShow::VideoDevice> devices;
	bool result1 = DShow::Device::EnumVideoDevices(devices);
	DShow::Device* device = new DShow::Device(); // so 2000, much memory leak
	bool result2 = device->ResetGraph();
	DShow::AudioConfig audioConfig;
	bool result3 = device->SetAudioConfig(nullptr);
	DShow::VideoConfig videoConfig;
	int* frameCount = new int;
	*frameCount = 0;

	auto callback = [frameCount]
	(
		const DShow::VideoConfig &config,
		unsigned char *data, 
		size_t size,
		long long startTime, 
		long long stopTime
	) 
	{
		if (*frameCount == 10)
		{
			SampleCallback(config, data, size, startTime, stopTime);
		}
		(*frameCount)++;
	};

	videoConfig.callback = callback;
	videoConfig.useDefaultConfig = false;
	videoConfig.cx = 640;
	videoConfig.cy = 480;
	videoConfig.format = DShow::VideoFormat::NV12;
	videoConfig.internalFormat = DShow::VideoFormat::Any;
	videoConfig.name = devices[0].name; // so direct access, much dangerous
	videoConfig.path = devices[0].path;
	bool result4 = device->SetVideoConfig(&videoConfig);
	bool result5 = device->ConnectFilters();
	auto result = device->Start();

	if (result == DShow::Result::Success)
	{
		return 1;
	}
	else if (result == DShow::Result::InUse)
	{
		return 2;
	}
	else if (result == DShow::Result::Error)
	{
		return 0;
	}
	else
	{
		return 3;
	}
}

unsigned char getDevices(int* arraySize, char** arrayPtr)
{
	std::vector<DShow::VideoDevice> devices;
	bool result1 = DShow::Device::EnumVideoDevices(devices);

	if (!result1)
	{
		return 0;
	}
	else
	{
		camera::CameraList cameraList;

		for (auto currentCamera = devices.begin(); currentCamera != devices.end(); currentCamera++)
		{
			auto camera = cameraList.add_cameras();
			camera->set_cameraname((const char*) currentCamera->name.c_str());
		}

		std::string output;
		auto status = google::protobuf::util::MessageToJsonString(cameraList, &output);

		char* array = (char*)calloc(output.length() + 1, sizeof(char));
		output.copy(array, output.length());
		*arraySize = output.length();
		*arrayPtr = array;
		return 1;
	}
}