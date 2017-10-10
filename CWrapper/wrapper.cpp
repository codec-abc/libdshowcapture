#include <iostream>
#include <fstream>

#include "wrapper.h"
#include "../dshowcapture.hpp"

#include "proto.pb.h"
#include <google/protobuf/util/json_util.h>

#include <windows.h>

#include <codecvt>
#include <string>

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

unsigned char startCapture(char* startCaptureOptions)
{
	camera::StartCaptureArguments message;
	std::string messageAsString(startCaptureOptions);
	google::protobuf::util::JsonStringToMessage(messageAsString, &message);

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
			//std::cout << "resolution is " << config.cx << "x" << config.cy << std::endl;
			SampleCallback(config, data, size, startTime, stopTime);
		}
		(*frameCount)++;
	};

	videoConfig.callback = callback;
	videoConfig.useDefaultConfig = false;

	//videoConfig.cx = 640;
	//videoConfig.cy = 480;
	//videoConfig.frameInterval = 30000000;
	//videoConfig.format = DShow::VideoFormat::NV12;
	//videoConfig.internalFormat = DShow::VideoFormat::Any;

	//videoConfig.name = devices[0].name; // so direct access, much dangerous
	//videoConfig.path = devices[0].path;

	videoConfig.cx = message.width();
	videoConfig.cy = message.height();
	videoConfig.frameInterval = 10000000 / message.framerate();
	videoConfig.internalFormat = ProtobufCaptureToDshowCapture(message.encoding());
	videoConfig.format = DShow::VideoFormat::ARGB;
	videoConfig.name = utf8_to_wstring(message.cameraname());
	videoConfig.path = utf8_to_wstring(message.camerapath());

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

				//std::cout << 
				//	"minCX " << currentFormat->minCX << " " << 
				//	"maxCX " << currentFormat->maxCX << " " <<
				//	"minCY " << currentFormat->minCY << " " <<
				//	"maxCY " << currentFormat->maxCY << " " <<
				//	"minInterval " << currentFormat->minInterval << " " <<
				//	"maxInterval " << currentFormat->maxInterval << " " <<
				//	"granularityCX " << currentFormat->granularityCX << " " <<
				//	"granularityCY " << currentFormat->granularityCY << std::endl;
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