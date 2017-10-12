#include <iostream>
#include <fstream>

#include "wrapper.h"
#include "../dshowcapture.hpp"

#include "proto.pb.h"
#include <google/protobuf/util/json_util.h>

#include <ppl.h>
#include <windows.h>
#include <intrin.h>

#include <codecvt>
#include <string>

int clamp(int val, int min, int max)
{
	if (val >= max)
	{
		return max - 1;
	}
	else if (val <= min)
	{
		return min;
	}
	return val;
}

enum FlippingMode
{
	None,
	Vertically,
	Horizontally,
	Both
};

FlippingMode ProtobufFlippingModeToFlippingMode(cameraReaderWindows::Flip flippingMode)
{
	switch (flippingMode)
	{
	case cameraReaderWindows::FlipNone:
		return FlippingMode::None;
	case cameraReaderWindows::FlipVertically:
		return FlippingMode::Vertically;
	case cameraReaderWindows::FlipHorizontally:
		return FlippingMode::Horizontally;
	case cameraReaderWindows::FlipBoth:
		return FlippingMode::Both;
	default:
		return None;
	}
}

void convertToYUV
(
	unsigned char* inputBuff, 
	unsigned char* outputBuff, 
	int width, 
	int height,
	FlippingMode flippingMode
)
{
	__m128 divideBy255 = _mm_set_ps1(0.0039215f); //0. 0039215 = 1.0 / 255.0
	__m128 multiplyBy255 = _mm_set_ps1(255.0f);
	__m128 zero = _mm_set_ps1(0.0f);
	__m128 one = _mm_set_ps1(1.0f);

	__m128 offset = _mm_setr_ps(
		0.0f,
		0.5f,
		0.5f,
		0
	);

	__m128 yFactors = _mm_setr_ps(
		0.299f,
		0.587f,
		0.114f,
		0.0f
	);

	__m128 uFactors = _mm_setr_ps(
		-0.14713f,
		-0.28886f,
		0.436f,
		0.0f
	);

	__m128 vFactors = _mm_setr_ps(
		0.615f,
		-0.51498f,
		-0.10001f,
		0.0f
	);

	concurrency::parallel_for(
	(int)0, width, [&](int i)
	//for (int i = 0; i < width; i++)
	{
		for (int j = 0; j < height; j++)
		{
			int index = (i * height + j) * 4;

			// create a word containing the rgb(a) values as 32 bits integers.
			__m128i rgbaValues = _mm_setr_epi8
			(
				inputBuff[index + 0], 0, 0, 0,
				inputBuff[index + 1], 0, 0, 0,
				inputBuff[index + 2], 0, 0, 0,
				0, 0, 0, 0
			);

			// convert the values to float
			__m128 rgbaAsFloat = _mm_cvtepi32_ps(rgbaValues);

			// divide by 255
			__m128 normalizedRgba = _mm_mul_ps(rgbaAsFloat, divideBy255);

			// apply the conversion factors
			__m128 yFloat_ = _mm_dp_ps(normalizedRgba, yFactors, 0b1111'1111);
			__m128 uFloat_ = _mm_dp_ps(normalizedRgba, uFactors, 0b1111'1111);
			__m128 vFloat_ = _mm_dp_ps(normalizedRgba, vFactors, 0b1111'1111);

			//create a word with yuv values
			__m128 yuvFloatInRange_0_1 =
				_mm_setr_ps
				(
					yFloat_.m128_f32[0],
					uFloat_.m128_f32[0],
					vFloat_.m128_f32[0],
					0
				);

			// add offset so U and V are in [0;1] instead of [-0.5; 0.5]
			yuvFloatInRange_0_1 = _mm_add_ps(yuvFloatInRange_0_1, offset);

			// Then clamp all values in [0;1]
			// minimum is 0
			yuvFloatInRange_0_1 = _mm_max_ps(yuvFloatInRange_0_1, zero);

			// max is 1
			yuvFloatInRange_0_1 = _mm_min_ps(yuvFloatInRange_0_1, one);

			// multiply by 255 to convert back to byte
			__m128 yuvFloat = _mm_mul_ps(yuvFloatInRange_0_1, multiplyBy255);

			// convert it back to integer
			__m128i yuvInteger = _mm_cvtps_epi32(yuvFloat);

			auto imageSize = width * height;

			if (flippingMode == FlippingMode::None)
			{
				int pixelIndex = i * height + j;

				outputBuff[0 * imageSize + pixelIndex] = yuvInteger.m128i_u32[0];
				outputBuff[1 * imageSize + pixelIndex] = yuvInteger.m128i_u32[1];
				outputBuff[2 * imageSize + pixelIndex] = yuvInteger.m128i_u32[2];
			}
			else if (flippingMode == FlippingMode::Vertically)
			{
				int oldPixelIndex = i * height + j;
				int jPrime = oldPixelIndex / width;
				int iPrime = oldPixelIndex - jPrime * width;
				jPrime = height - 1 - jPrime;
				int pixelIndex = jPrime * width + iPrime;

				outputBuff[0 * imageSize + pixelIndex] = yuvInteger.m128i_u32[0];
				outputBuff[1 * imageSize + pixelIndex] = yuvInteger.m128i_u32[1];
				outputBuff[2 * imageSize + pixelIndex] = yuvInteger.m128i_u32[2];
			}
			else if (flippingMode == FlippingMode::Horizontally)
			{
				int oldPixelIndex = i * height + j;
				int jPrime = oldPixelIndex / width;
				int iPrime = oldPixelIndex - jPrime * width;
				int pixelIndex = jPrime * width + iPrime;
				int delta = 2 * (ceil(width / 2.0) - iPrime);
				pixelIndex += delta;

				outputBuff[0 * imageSize + pixelIndex] = yuvInteger.m128i_u32[0];
				outputBuff[1 * imageSize + pixelIndex] = yuvInteger.m128i_u32[1];
				outputBuff[2 * imageSize + pixelIndex] = yuvInteger.m128i_u32[2];
			}
			else if (flippingMode == FlippingMode::Both)
			{
				int pixelIndex = imageSize - 1 - (i * height + j);

				outputBuff[0 * imageSize + pixelIndex] = yuvInteger.m128i_u32[0];
				outputBuff[1 * imageSize + pixelIndex] = yuvInteger.m128i_u32[1];
				outputBuff[2 * imageSize + pixelIndex] = yuvInteger.m128i_u32[2];
			}
		}
	});
}

class DeviceHolder
{
public:

	DeviceHolder(DShow::Device* devicePtr, cameraReaderWindows::Flip flippingmode)
	{
		_devicePtr = devicePtr;
		_frameCount = 0;
		_imageSize = 0;
		_width = 0;
		_height = 0;
		_flippingMode = ProtobufFlippingModeToFlippingMode(flippingmode);
	}

	~DeviceHolder()
	{
		if (_devicePtr != NULL)
		{
			_devicePtr->Stop();
			_callbackMutex.lock();
			_mutex.lock();
			for (auto buffIt = _imagesBuffers.begin();  buffIt != _imagesBuffers.end(); buffIt++)
			{
				auto buff = *buffIt;
				free(buff);
			}
			_callbackMutex.unlock();
			_mutex.unlock();
			delete(_devicePtr);
		}
	}
	
	void HandleFrame
	(
		const DShow::VideoConfig &config,
		unsigned char *data,
		size_t size,
		long long startTime,
		long long stopTime
	)
	{
		_callbackMutex.lock();
		_frameCount++;
		if (_imagesBuffers.size() == 0)
		{
			_imageSize = size;
			_width = config.cx;
			_height = config.cy;

			// allocate some buffers
			for (int i = 0; i < 3; i++)
			{
				auto buffer = (unsigned char*)malloc(size);
				_imagesBuffers.push_back(buffer);
				_freeBuffers.push_back(buffer);
			}
		}

		if (size == _imageSize || _imageSize == (config.cx * config.cy * 4))
		{
			_mutex.lock();
			if (_freeBuffers.size() > 0)
			{
				unsigned char* buff = _freeBuffers.back();
				_freeBuffers.pop_back();
				convertToYUV(data, buff, config.cx, config.cy, _flippingMode);
				_readyBuffers.push_back(buff);
			}
			else
			{
				unsigned char* buff = _readyBuffers[0];
				_readyBuffers.erase(_readyBuffers.begin());
				convertToYUV(data, buff, config.cx, config.cy, _flippingMode);
				_readyBuffers.push_back(buff);
			}
			_mutex.unlock();
		}
		else
		{
			std::cout << "ERROR : Image dimension or format is not valid" << std::endl;
		}
		_callbackMutex.unlock();
	}

	unsigned char* tryGetReadyBuffer()
	{
		_mutex.lock();
		if (_readyBuffers.size() == 0)
		{
			_mutex.unlock();
			return NULL;
		}
		else
		{
			// get the last buffer
			auto buff = _readyBuffers.back();
			_readyBuffers.pop_back();
			// remove older buffers since there are not needed anymore
			for (auto it = _readyBuffers.begin(); it != _readyBuffers.end(); it++)
			{
				_freeBuffers.push_back(*it);
			}
			_readyBuffers.clear();
			_mutex.unlock();
			return buff;
		}
	}

	void recycleUsedBuffer(unsigned char* buffer)
	{
		if (buffer != nullptr)
		{
			_mutex.lock();
			_freeBuffers.push_back(buffer);
			_mutex.unlock();
		}
	}

	unsigned int getWidth()
	{
		return _width;
	}

	unsigned int getHeight()
	{
		return _height;
	}

private:
	DShow::Device* _devicePtr;
	int _frameCount;
	std::vector<unsigned char*> _imagesBuffers;
	std::vector<unsigned char*> _freeBuffers;
	std::vector<unsigned char*> _readyBuffers;
	size_t _imageSize;
	unsigned int _width;
	unsigned int _height;
	std::mutex _mutex;
	std::mutex _callbackMutex;
	FlippingMode _flippingMode;
};

std::wstring utf8_to_wstring(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.from_bytes(str);
}

std::string wstring_to_utf8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.to_bytes(str);
}

cameraReaderWindows::CaptureEncoding DshowCaptureToProtobufCapture(const DShow::VideoInfo& info)
{
	switch (info.format)
	{
	case DShow::VideoFormat::ARGB:
		return cameraReaderWindows::CaptureEncoding::ARGB;
	case DShow::VideoFormat::XRGB:
		return cameraReaderWindows::CaptureEncoding::XRGB;
	case DShow::VideoFormat::I420:
		return cameraReaderWindows::CaptureEncoding::I420;
	case DShow::VideoFormat::NV12:
		return cameraReaderWindows::CaptureEncoding::NV12;
	case DShow::VideoFormat::YV12:
		return cameraReaderWindows::CaptureEncoding::YV12;
	case DShow::VideoFormat::Y800:
		return cameraReaderWindows::CaptureEncoding::Y800;
	case DShow::VideoFormat::YVYU:
		return cameraReaderWindows::CaptureEncoding::YVYU;
	case DShow::VideoFormat::YUY2:
		return cameraReaderWindows::CaptureEncoding::YUY2;
	case DShow::VideoFormat::UYVY:
		return cameraReaderWindows::CaptureEncoding::UYVY;
	case DShow::VideoFormat::HDYC:
		return cameraReaderWindows::CaptureEncoding::HDYC;
	case DShow::VideoFormat::MJPEG:
		return cameraReaderWindows::CaptureEncoding::MJPEG;
	case DShow::VideoFormat::H264:
		return cameraReaderWindows::CaptureEncoding::H264;
	case DShow::VideoFormat::Any:
		return cameraReaderWindows::CaptureEncoding::Any;
	case DShow::VideoFormat::Unknown:
		return cameraReaderWindows::CaptureEncoding::Unknown;
	default:
		return cameraReaderWindows::CaptureEncoding::Unknown;
	}
}

DShow::VideoFormat ProtobufCaptureToDshowCapture(cameraReaderWindows::CaptureEncoding format)
{
	switch (format)
	{
	case cameraReaderWindows::CaptureEncoding::ARGB:
		return DShow::VideoFormat::ARGB;
	case cameraReaderWindows::CaptureEncoding::XRGB:
		return DShow::VideoFormat::XRGB;
	case cameraReaderWindows::CaptureEncoding::I420:
		return DShow::VideoFormat::I420;
	case cameraReaderWindows::CaptureEncoding::NV12:
		return DShow::VideoFormat::NV12;
	case cameraReaderWindows::CaptureEncoding::YV12:
		return DShow::VideoFormat::YV12;
	case cameraReaderWindows::CaptureEncoding::Y800:
		return DShow::VideoFormat::Y800;
	case cameraReaderWindows::CaptureEncoding::YVYU:
		return DShow::VideoFormat::YVYU;
	case cameraReaderWindows::CaptureEncoding::YUY2:
		return DShow::VideoFormat::YUY2;
	case cameraReaderWindows::CaptureEncoding::UYVY:
		return DShow::VideoFormat::UYVY;
	case cameraReaderWindows::CaptureEncoding::HDYC:
		return DShow::VideoFormat::HDYC;
	case cameraReaderWindows::CaptureEncoding::MJPEG:
		return DShow::VideoFormat::MJPEG;
	case cameraReaderWindows::CaptureEncoding::H264:
		return DShow::VideoFormat::H264;
	case cameraReaderWindows::CaptureEncoding::Any:
		return DShow::VideoFormat::Any;
	case cameraReaderWindows::CaptureEncoding::Unknown:
		return DShow::VideoFormat::Unknown;
	default:
		return DShow::VideoFormat::Unknown;
	}
}

void AllocResultByteArray
(
	const cameraReaderWindows::StartCaptureResult& result, 
	int* arraySize, 
	char** arrayPtr
)
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
	cameraReaderWindows::StartCaptureArguments message;
	std::string messageAsString(startCaptureOptions);
	google::protobuf::util::JsonStringToMessage(messageAsString, &message);

	cameraReaderWindows::StartCaptureResult result;

	result.set_canconnectfilters(false);
	result.set_canresetgraph(false);
	result.set_cansetaudioconfig(false);
	result.set_cansetvideoconfig(false);
	result.set_result(cameraReaderWindows::StartResult::Error);

	DShow::Device* device = new DShow::Device();
	DeviceHolder* deviceHolder = new DeviceHolder(device, message.flippingmode());
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

	auto callback = [deviceHolder]
	(
		const DShow::VideoConfig &config,
		unsigned char *data, 
		size_t size,
		long long startTime, 
		long long stopTime
	) 
	{
		deviceHolder->HandleFrame(config, data, size, startTime, stopTime);
	};

	videoConfig.callback = callback;
	videoConfig.useDefaultConfig = false;

	videoConfig.cx = message.width();
	videoConfig.cy = message.height();
	videoConfig.frameInterval = message.frameinterval();
	videoConfig.internalFormat = ProtobufCaptureToDshowCapture(message.encoding());
	videoConfig.format = DShow::VideoFormat::ARGB;
	std::wstring cameraName = utf8_to_wstring(message.cameraname());;
	videoConfig.name = cameraName;
	std::wstring cameraPath = utf8_to_wstring(message.camerapath());
	videoConfig.path = cameraPath;

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
		result.set_result(cameraReaderWindows::StartResult::Success);
		break;
	case DShow::Result::Error:
		result.set_result(cameraReaderWindows::StartResult::Error);
		break;
	case DShow::Result::InUse:
		result.set_result(cameraReaderWindows::StartResult::InUse);
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
		cameraReaderWindows::CameraList cameraList;

		for (auto currentCamera = devices.begin(); currentCamera != devices.end(); currentCamera++)
		{
			auto camera = cameraList.add_cameras();

			std::string cameraName = wstring_to_utf8(currentCamera->name);
			camera->set_cameraname(cameraName);

			std::string cameraPath = wstring_to_utf8(currentCamera->path);
			camera->set_camerapath(cameraPath);

			auto formats = currentCamera->caps;
			for (auto currentFormat = formats.begin(); currentFormat != formats.end(); currentFormat++)
			{
				auto format = camera->add_formats();
				format->set_encoding(DshowCaptureToProtobufCapture(*currentFormat));
				format->set_width(currentFormat->maxCX);
				format->set_height(currentFormat->maxCY);
				format->set_frameinterval(currentFormat->minInterval);
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

void tryGetBuffer
(
	unsigned char** buffer, 
	void* device, 
	unsigned int* width, 
	unsigned int* height
)
{
	DeviceHolder* deviceHolder = (DeviceHolder*)device;
	*width = deviceHolder->getWidth();
	*height = deviceHolder->getHeight();
	*buffer = deviceHolder->tryGetReadyBuffer();
}

void recycleUsedBuffer(unsigned char* buffer, void* device)
{
	DeviceHolder* deviceHolder = (DeviceHolder*)device;
	deviceHolder->recycleUsedBuffer(buffer);
}
