#include <iostream>
#include <fstream>

#include "wrapper.h"
#include "../dshowcapture.hpp"

#include "proto.pb.h"
#include <google/protobuf/util/json_util.h>

#include <windows.h>

#include <codecvt>
#include <string>

class DeviceHolder
{
public:
	DeviceHolder(DShow::Device* devicePtr)
	{
		_devicePtr = devicePtr;
		_frameCount = 0;
	}
	~DeviceHolder()
	{
		if (_devicePtr != NULL)
		{
			_devicePtr->Stop();
			delete(_devicePtr);
		}
	}
private:
	DShow::Device* _devicePtr;
	int _frameCount;
	//std::vector<unsigned char*> _buffers;
};

// convert UTF-8 string to wstring
std::wstring utf8_to_wstring(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.from_bytes(str);
}

// convert wstring to UTF-8 string
std::string wstring_to_utf8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.to_bytes(str);
}

camera::CaptureEncoding DshowCaptureToProtobufCapture(const DShow::VideoInfo& info)
{
	switch (info.format)
	{
	case DShow::VideoFormat::ARGB:
		return camera::CaptureEncoding::ARGB;
	case DShow::VideoFormat::XRGB:
		return camera::CaptureEncoding::XRGB;
	case DShow::VideoFormat::I420:
		return camera::CaptureEncoding::I420;
	case DShow::VideoFormat::NV12:
		return camera::CaptureEncoding::NV12;
	case DShow::VideoFormat::YV12:
		return camera::CaptureEncoding::YV12;
	case DShow::VideoFormat::Y800:
		return camera::CaptureEncoding::Y800;
	case DShow::VideoFormat::YVYU:
		return camera::CaptureEncoding::YVYU;
	case DShow::VideoFormat::YUY2:
		return camera::CaptureEncoding::YUY2;
	case DShow::VideoFormat::UYVY:
		return camera::CaptureEncoding::UYVY;
	case DShow::VideoFormat::HDYC:
		return camera::CaptureEncoding::HDYC;
	case DShow::VideoFormat::MJPEG:
		return camera::CaptureEncoding::MJPEG;
	case DShow::VideoFormat::H264:
		return camera::CaptureEncoding::H264;
	case DShow::VideoFormat::Any:
		return camera::CaptureEncoding::Any;
	case DShow::VideoFormat::Unknown:
		return camera::CaptureEncoding::Unknown;
	default:
		return camera::CaptureEncoding::Unknown;
	}
}

DShow::VideoFormat ProtobufCaptureToDshowCapture(camera::CaptureEncoding format)
{
	switch (format)
	{
	case camera::CaptureEncoding::ARGB:
		return DShow::VideoFormat::ARGB;
	case camera::CaptureEncoding::XRGB:
		return DShow::VideoFormat::XRGB;
	case camera::CaptureEncoding::I420:
		return DShow::VideoFormat::I420;
	case camera::CaptureEncoding::NV12:
		return DShow::VideoFormat::NV12;
	case camera::CaptureEncoding::YV12:
		return DShow::VideoFormat::YV12;
	case camera::CaptureEncoding::Y800:
		return DShow::VideoFormat::Y800;
	case camera::CaptureEncoding::YVYU:
		return DShow::VideoFormat::YVYU;
	case camera::CaptureEncoding::YUY2:
		return DShow::VideoFormat::YUY2;
	case camera::CaptureEncoding::UYVY:
		return DShow::VideoFormat::UYVY;
	case camera::CaptureEncoding::HDYC:
		return DShow::VideoFormat::HDYC;
	case camera::CaptureEncoding::MJPEG:
		return DShow::VideoFormat::MJPEG;
	case camera::CaptureEncoding::H264:
		return DShow::VideoFormat::H264;
	case camera::CaptureEncoding::Any:
		return DShow::VideoFormat::Any;
	case camera::CaptureEncoding::Unknown:
		return DShow::VideoFormat::Unknown;
	default:
		return DShow::VideoFormat::Unknown;
	}
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

void AllocResultByteArray(const camera::StartCaptureResult& result, int* arraySize, char** arrayPtr)
{
	std::string output;
	auto status = google::protobuf::util::MessageToJsonString(result, &output);

	char* array = (char*)calloc(output.length() + 1, sizeof(char));
	output.copy(array, output.length());
	*arraySize = output.length();
	*arrayPtr = array;
}

void startCapture(char* startCaptureOptions, int* arraySize, char** arrayPtr)
{
	camera::StartCaptureArguments message;
	std::string messageAsString(startCaptureOptions);
	google::protobuf::util::JsonStringToMessage(messageAsString, &message);

	camera::StartCaptureResult result;

	result.set_canconnectfilters(false);
	result.set_canresetgraph(false);
	result.set_cansetaudioconfig(false);
	result.set_cansetvideoconfig(false);
	result.set_result(camera::StartResult::Error);


	DShow::Device* device = new DShow::Device();
	DeviceHolder* deviceHolder = new DeviceHolder(device);
	long addr = (long) (deviceHolder);
	result.set_devicepointer(addr);

	bool canResetGraph = device->ResetGraph();
	result.set_canresetgraph(canResetGraph);
	if (!canResetGraph)
	{
		AllocResultByteArray(result, arraySize, arrayPtr);
		return;
	}

	DShow::AudioConfig audioConfig;
	bool canSetAudioConfig = device->SetAudioConfig(nullptr);
	result.set_cansetaudioconfig(canSetAudioConfig);
	if (!canSetAudioConfig)
	{
		AllocResultByteArray(result, arraySize, arrayPtr);
		return;
	}

	DShow::VideoConfig videoConfig;
	int* frameCount = new int;
	*frameCount = 0;

	//auto callback = [frameCount]
	//(
	//	const DShow::VideoConfig &config,
	//	unsigned char *data, 
	//	size_t size,
	//	long long startTime, 
	//	long long stopTime
	//) 
	//{
	//	if (*frameCount == 10)
	//	{
	//		//std::cout << "resolution is " << config.cx << "x" << config.cy << std::endl;
	//		SampleCallback(config, data, size, startTime, stopTime);
	//	}
	//	(*frameCount)++;
	//};

	//videoConfig.callback = callback;
	videoConfig.useDefaultConfig = false;

	videoConfig.cx = message.width();
	videoConfig.cy = message.height();
	videoConfig.frameInterval = 10000000 / message.framerate();
	videoConfig.internalFormat = ProtobufCaptureToDshowCapture(message.encoding());
	videoConfig.format = DShow::VideoFormat::ARGB;
	videoConfig.name = utf8_to_wstring(message.cameraname());
	videoConfig.path = utf8_to_wstring(message.camerapath());

	bool canSetVideoConfig = device->SetVideoConfig(&videoConfig);
	result.set_cansetvideoconfig(canSetVideoConfig);
	if (!canSetVideoConfig)
	{
		AllocResultByteArray(result, arraySize, arrayPtr);
		return;
	}

	bool canConnectFilter = device->ConnectFilters();
	result.set_canconnectfilters(canConnectFilter);
	if (!canConnectFilter)
	{
		AllocResultByteArray(result, arraySize, arrayPtr);
		return;
	}

	auto startResult = device->Start();
	switch (startResult)
	{
	case DShow::Result::Success:
		result.set_result(camera::StartResult::Success);
		break;
	case DShow::Result::Error:
		result.set_result(camera::StartResult::Error);
		break;
	case DShow::Result::InUse:
		result.set_result(camera::StartResult::InUse);
		break;
	}
	AllocResultByteArray(result, arraySize, arrayPtr);

}

unsigned char getDevices(int* arraySize, char** arrayPtr)
{
	std::vector<DShow::VideoDevice> devices;
	bool canEnumDevices = DShow::Device::EnumVideoDevices(devices);

	if (!canEnumDevices)
	{
		return 0;
	}
	else
	{
		camera::CameraList cameraList;

		for (auto currentCamera = devices.begin(); currentCamera != devices.end(); currentCamera++)
		{
			auto camera = cameraList.add_cameras();

			camera->set_cameraname(wstring_to_utf8(currentCamera->name));
			camera->set_camerapath(wstring_to_utf8(currentCamera->path));

			auto formats = currentCamera->caps;
			for (auto currentFormat = formats.begin(); currentFormat != formats.end(); currentFormat++)
			{
				auto format = camera->add_formats();
				format->set_encoding(DshowCaptureToProtobufCapture(*currentFormat));
				format->set_width(currentFormat->maxCX);
				format->set_height(currentFormat->maxCY);
				format->set_framerate(10000000 / currentFormat->minInterval);
			}
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

void freeByteArray(unsigned char* byteArray)
{
	free(byteArray);
}

void shutDownAndFreeDevice(void* device)
{
	if (device != NULL)
	{
		DeviceHolder* deviceHolder = (DeviceHolder*)device;
		delete(deviceHolder);
	}
}