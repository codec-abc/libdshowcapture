// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: proto.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Camera {

  /// <summary>Holder for reflection information generated from proto.proto</summary>
  public static partial class ProtoReflection {

    #region Descriptor
    /// <summary>File descriptor for proto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cgtwcm90by5wcm90bxIGY2FtZXJhInMKDUNhcHR1cmVGb3JtYXQSDQoFd2lk",
            "dGgYASABKA0SDgoGaGVpZ2h0GAIgASgNEhgKEGZyYW1laW50ZXJ2YWxfdXMY",
            "AyABKAQSKQoIZW5jb2RpbmcYBCABKA4yFy5jYW1lcmEuQ2FwdHVyZUVuY29k",
            "aW5nIlgKBkNhbWVyYRISCgpjYW1lcmFOYW1lGAUgASgJEhIKCmNhbWVyYVBh",
            "dGgYBiABKAkSJgoHZm9ybWF0cxgHIAMoCzIVLmNhbWVyYS5DYXB0dXJlRm9y",
            "bWF0Ii0KCkNhbWVyYUxpc3QSHwoHY2FtZXJhcxgHIAMoCzIOLmNhbWVyYS5D",
            "YW1lcmEiowEKFVN0YXJ0Q2FwdHVyZUFyZ3VtZW50cxISCgpjYW1lcmFOYW1l",
            "GAggASgJEhIKCmNhbWVyYVBhdGgYCSABKAkSDQoFd2lkdGgYCiABKA0SDgoG",
            "aGVpZ2h0GAsgASgNEhgKEGZyYW1laW50ZXJ2YWxfdXMYDCABKAQSKQoIZW5j",
            "b2RpbmcYDSABKA4yFy5jYW1lcmEuQ2FwdHVyZUVuY29kaW5nIrgBChJTdGFy",
            "dENhcHR1cmVSZXN1bHQSFQoNY2FuUmVzZXRHcmFwaBgOIAEoCBIZChFjYW5T",
            "ZXRBdWRpb0NvbmZpZxgPIAEoCBIZChFjYW5TZXRWaWRlb0NvbmZpZxgQIAEo",
            "CBIZChFjYW5Db25uZWN0RmlsdGVycxgRIAEoCBIjCgZyZXN1bHQYEiABKA4y",
            "Ey5jYW1lcmEuU3RhcnRSZXN1bHQSFQoNZGV2aWNlUG9pbnRlchgTIAEoBCqg",
            "AQoPQ2FwdHVyZUVuY29kaW5nEgcKA0FueRAAEgsKB1Vua25vd24QARIICgRB",
            "UkdCEAISCAoEWFJHQhADEggKBEk0MjAQBBIICgROVjEyEAUSCAoEWVYxMhAG",
            "EggKBFk4MDAQBxIICgRZVllVEAgSCAoEWVVZMhAJEggKBFVZVlkQChIICgRI",
            "RFlDEAsSCQoFTUpQRUcQDBIICgRIMjY0EA0qMAoLU3RhcnRSZXN1bHQSCwoH",
            "U3VjY2VzcxAAEgkKBUluVXNlEAESCQoFRXJyb3IQAmIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Camera.CaptureEncoding), typeof(global::Camera.StartResult), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Camera.CaptureFormat), global::Camera.CaptureFormat.Parser, new[]{ "Width", "Height", "FrameintervalUs", "Encoding" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Camera.Camera), global::Camera.Camera.Parser, new[]{ "CameraName", "CameraPath", "Formats" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Camera.CameraList), global::Camera.CameraList.Parser, new[]{ "Cameras" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Camera.StartCaptureArguments), global::Camera.StartCaptureArguments.Parser, new[]{ "CameraName", "CameraPath", "Width", "Height", "FrameintervalUs", "Encoding" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Camera.StartCaptureResult), global::Camera.StartCaptureResult.Parser, new[]{ "CanResetGraph", "CanSetAudioConfig", "CanSetVideoConfig", "CanConnectFilters", "Result", "DevicePointer" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum CaptureEncoding {
    [pbr::OriginalName("Any")] Any = 0,
    [pbr::OriginalName("Unknown")] Unknown = 1,
    [pbr::OriginalName("ARGB")] Argb = 2,
    [pbr::OriginalName("XRGB")] Xrgb = 3,
    [pbr::OriginalName("I420")] I420 = 4,
    [pbr::OriginalName("NV12")] Nv12 = 5,
    [pbr::OriginalName("YV12")] Yv12 = 6,
    [pbr::OriginalName("Y800")] Y800 = 7,
    [pbr::OriginalName("YVYU")] Yvyu = 8,
    [pbr::OriginalName("YUY2")] Yuy2 = 9,
    [pbr::OriginalName("UYVY")] Uyvy = 10,
    [pbr::OriginalName("HDYC")] Hdyc = 11,
    [pbr::OriginalName("MJPEG")] Mjpeg = 12,
    [pbr::OriginalName("H264")] H264 = 13,
  }

  public enum StartResult {
    [pbr::OriginalName("Success")] Success = 0,
    [pbr::OriginalName("InUse")] InUse = 1,
    [pbr::OriginalName("Error")] Error = 2,
  }

  #endregion

  #region Messages
  public sealed partial class CaptureFormat : pb::IMessage<CaptureFormat> {
    private static readonly pb::MessageParser<CaptureFormat> _parser = new pb::MessageParser<CaptureFormat>(() => new CaptureFormat());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CaptureFormat> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Camera.ProtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CaptureFormat() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CaptureFormat(CaptureFormat other) : this() {
      width_ = other.width_;
      height_ = other.height_;
      frameintervalUs_ = other.frameintervalUs_;
      encoding_ = other.encoding_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CaptureFormat Clone() {
      return new CaptureFormat(this);
    }

    /// <summary>Field number for the "width" field.</summary>
    public const int WidthFieldNumber = 1;
    private uint width_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Width {
      get { return width_; }
      set {
        width_ = value;
      }
    }

    /// <summary>Field number for the "height" field.</summary>
    public const int HeightFieldNumber = 2;
    private uint height_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Height {
      get { return height_; }
      set {
        height_ = value;
      }
    }

    /// <summary>Field number for the "frameinterval_us" field.</summary>
    public const int FrameintervalUsFieldNumber = 3;
    private ulong frameintervalUs_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong FrameintervalUs {
      get { return frameintervalUs_; }
      set {
        frameintervalUs_ = value;
      }
    }

    /// <summary>Field number for the "encoding" field.</summary>
    public const int EncodingFieldNumber = 4;
    private global::Camera.CaptureEncoding encoding_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Camera.CaptureEncoding Encoding {
      get { return encoding_; }
      set {
        encoding_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CaptureFormat);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CaptureFormat other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Width != other.Width) return false;
      if (Height != other.Height) return false;
      if (FrameintervalUs != other.FrameintervalUs) return false;
      if (Encoding != other.Encoding) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Width != 0) hash ^= Width.GetHashCode();
      if (Height != 0) hash ^= Height.GetHashCode();
      if (FrameintervalUs != 0UL) hash ^= FrameintervalUs.GetHashCode();
      if (Encoding != 0) hash ^= Encoding.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Width != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(Width);
      }
      if (Height != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Height);
      }
      if (FrameintervalUs != 0UL) {
        output.WriteRawTag(24);
        output.WriteUInt64(FrameintervalUs);
      }
      if (Encoding != 0) {
        output.WriteRawTag(32);
        output.WriteEnum((int) Encoding);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Width != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Width);
      }
      if (Height != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Height);
      }
      if (FrameintervalUs != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(FrameintervalUs);
      }
      if (Encoding != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Encoding);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CaptureFormat other) {
      if (other == null) {
        return;
      }
      if (other.Width != 0) {
        Width = other.Width;
      }
      if (other.Height != 0) {
        Height = other.Height;
      }
      if (other.FrameintervalUs != 0UL) {
        FrameintervalUs = other.FrameintervalUs;
      }
      if (other.Encoding != 0) {
        Encoding = other.Encoding;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Width = input.ReadUInt32();
            break;
          }
          case 16: {
            Height = input.ReadUInt32();
            break;
          }
          case 24: {
            FrameintervalUs = input.ReadUInt64();
            break;
          }
          case 32: {
            encoding_ = (global::Camera.CaptureEncoding) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Camera : pb::IMessage<Camera> {
    private static readonly pb::MessageParser<Camera> _parser = new pb::MessageParser<Camera>(() => new Camera());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Camera> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Camera.ProtoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Camera() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Camera(Camera other) : this() {
      cameraName_ = other.cameraName_;
      cameraPath_ = other.cameraPath_;
      formats_ = other.formats_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Camera Clone() {
      return new Camera(this);
    }

    /// <summary>Field number for the "cameraName" field.</summary>
    public const int CameraNameFieldNumber = 5;
    private string cameraName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CameraName {
      get { return cameraName_; }
      set {
        cameraName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "cameraPath" field.</summary>
    public const int CameraPathFieldNumber = 6;
    private string cameraPath_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CameraPath {
      get { return cameraPath_; }
      set {
        cameraPath_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "formats" field.</summary>
    public const int FormatsFieldNumber = 7;
    private static readonly pb::FieldCodec<global::Camera.CaptureFormat> _repeated_formats_codec
        = pb::FieldCodec.ForMessage(58, global::Camera.CaptureFormat.Parser);
    private readonly pbc::RepeatedField<global::Camera.CaptureFormat> formats_ = new pbc::RepeatedField<global::Camera.CaptureFormat>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Camera.CaptureFormat> Formats {
      get { return formats_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Camera);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Camera other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CameraName != other.CameraName) return false;
      if (CameraPath != other.CameraPath) return false;
      if(!formats_.Equals(other.formats_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (CameraName.Length != 0) hash ^= CameraName.GetHashCode();
      if (CameraPath.Length != 0) hash ^= CameraPath.GetHashCode();
      hash ^= formats_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (CameraName.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(CameraName);
      }
      if (CameraPath.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(CameraPath);
      }
      formats_.WriteTo(output, _repeated_formats_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (CameraName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CameraName);
      }
      if (CameraPath.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CameraPath);
      }
      size += formats_.CalculateSize(_repeated_formats_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Camera other) {
      if (other == null) {
        return;
      }
      if (other.CameraName.Length != 0) {
        CameraName = other.CameraName;
      }
      if (other.CameraPath.Length != 0) {
        CameraPath = other.CameraPath;
      }
      formats_.Add(other.formats_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 42: {
            CameraName = input.ReadString();
            break;
          }
          case 50: {
            CameraPath = input.ReadString();
            break;
          }
          case 58: {
            formats_.AddEntriesFrom(input, _repeated_formats_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class CameraList : pb::IMessage<CameraList> {
    private static readonly pb::MessageParser<CameraList> _parser = new pb::MessageParser<CameraList>(() => new CameraList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CameraList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Camera.ProtoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CameraList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CameraList(CameraList other) : this() {
      cameras_ = other.cameras_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CameraList Clone() {
      return new CameraList(this);
    }

    /// <summary>Field number for the "cameras" field.</summary>
    public const int CamerasFieldNumber = 7;
    private static readonly pb::FieldCodec<global::Camera.Camera> _repeated_cameras_codec
        = pb::FieldCodec.ForMessage(58, global::Camera.Camera.Parser);
    private readonly pbc::RepeatedField<global::Camera.Camera> cameras_ = new pbc::RepeatedField<global::Camera.Camera>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Camera.Camera> Cameras {
      get { return cameras_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CameraList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CameraList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!cameras_.Equals(other.cameras_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= cameras_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      cameras_.WriteTo(output, _repeated_cameras_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += cameras_.CalculateSize(_repeated_cameras_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CameraList other) {
      if (other == null) {
        return;
      }
      cameras_.Add(other.cameras_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 58: {
            cameras_.AddEntriesFrom(input, _repeated_cameras_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class StartCaptureArguments : pb::IMessage<StartCaptureArguments> {
    private static readonly pb::MessageParser<StartCaptureArguments> _parser = new pb::MessageParser<StartCaptureArguments>(() => new StartCaptureArguments());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<StartCaptureArguments> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Camera.ProtoReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureArguments() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureArguments(StartCaptureArguments other) : this() {
      cameraName_ = other.cameraName_;
      cameraPath_ = other.cameraPath_;
      width_ = other.width_;
      height_ = other.height_;
      frameintervalUs_ = other.frameintervalUs_;
      encoding_ = other.encoding_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureArguments Clone() {
      return new StartCaptureArguments(this);
    }

    /// <summary>Field number for the "cameraName" field.</summary>
    public const int CameraNameFieldNumber = 8;
    private string cameraName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CameraName {
      get { return cameraName_; }
      set {
        cameraName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "cameraPath" field.</summary>
    public const int CameraPathFieldNumber = 9;
    private string cameraPath_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CameraPath {
      get { return cameraPath_; }
      set {
        cameraPath_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "width" field.</summary>
    public const int WidthFieldNumber = 10;
    private uint width_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Width {
      get { return width_; }
      set {
        width_ = value;
      }
    }

    /// <summary>Field number for the "height" field.</summary>
    public const int HeightFieldNumber = 11;
    private uint height_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Height {
      get { return height_; }
      set {
        height_ = value;
      }
    }

    /// <summary>Field number for the "frameinterval_us" field.</summary>
    public const int FrameintervalUsFieldNumber = 12;
    private ulong frameintervalUs_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong FrameintervalUs {
      get { return frameintervalUs_; }
      set {
        frameintervalUs_ = value;
      }
    }

    /// <summary>Field number for the "encoding" field.</summary>
    public const int EncodingFieldNumber = 13;
    private global::Camera.CaptureEncoding encoding_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Camera.CaptureEncoding Encoding {
      get { return encoding_; }
      set {
        encoding_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as StartCaptureArguments);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(StartCaptureArguments other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CameraName != other.CameraName) return false;
      if (CameraPath != other.CameraPath) return false;
      if (Width != other.Width) return false;
      if (Height != other.Height) return false;
      if (FrameintervalUs != other.FrameintervalUs) return false;
      if (Encoding != other.Encoding) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (CameraName.Length != 0) hash ^= CameraName.GetHashCode();
      if (CameraPath.Length != 0) hash ^= CameraPath.GetHashCode();
      if (Width != 0) hash ^= Width.GetHashCode();
      if (Height != 0) hash ^= Height.GetHashCode();
      if (FrameintervalUs != 0UL) hash ^= FrameintervalUs.GetHashCode();
      if (Encoding != 0) hash ^= Encoding.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (CameraName.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(CameraName);
      }
      if (CameraPath.Length != 0) {
        output.WriteRawTag(74);
        output.WriteString(CameraPath);
      }
      if (Width != 0) {
        output.WriteRawTag(80);
        output.WriteUInt32(Width);
      }
      if (Height != 0) {
        output.WriteRawTag(88);
        output.WriteUInt32(Height);
      }
      if (FrameintervalUs != 0UL) {
        output.WriteRawTag(96);
        output.WriteUInt64(FrameintervalUs);
      }
      if (Encoding != 0) {
        output.WriteRawTag(104);
        output.WriteEnum((int) Encoding);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (CameraName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CameraName);
      }
      if (CameraPath.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CameraPath);
      }
      if (Width != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Width);
      }
      if (Height != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Height);
      }
      if (FrameintervalUs != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(FrameintervalUs);
      }
      if (Encoding != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Encoding);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(StartCaptureArguments other) {
      if (other == null) {
        return;
      }
      if (other.CameraName.Length != 0) {
        CameraName = other.CameraName;
      }
      if (other.CameraPath.Length != 0) {
        CameraPath = other.CameraPath;
      }
      if (other.Width != 0) {
        Width = other.Width;
      }
      if (other.Height != 0) {
        Height = other.Height;
      }
      if (other.FrameintervalUs != 0UL) {
        FrameintervalUs = other.FrameintervalUs;
      }
      if (other.Encoding != 0) {
        Encoding = other.Encoding;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 66: {
            CameraName = input.ReadString();
            break;
          }
          case 74: {
            CameraPath = input.ReadString();
            break;
          }
          case 80: {
            Width = input.ReadUInt32();
            break;
          }
          case 88: {
            Height = input.ReadUInt32();
            break;
          }
          case 96: {
            FrameintervalUs = input.ReadUInt64();
            break;
          }
          case 104: {
            encoding_ = (global::Camera.CaptureEncoding) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  public sealed partial class StartCaptureResult : pb::IMessage<StartCaptureResult> {
    private static readonly pb::MessageParser<StartCaptureResult> _parser = new pb::MessageParser<StartCaptureResult>(() => new StartCaptureResult());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<StartCaptureResult> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Camera.ProtoReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureResult() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureResult(StartCaptureResult other) : this() {
      canResetGraph_ = other.canResetGraph_;
      canSetAudioConfig_ = other.canSetAudioConfig_;
      canSetVideoConfig_ = other.canSetVideoConfig_;
      canConnectFilters_ = other.canConnectFilters_;
      result_ = other.result_;
      devicePointer_ = other.devicePointer_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StartCaptureResult Clone() {
      return new StartCaptureResult(this);
    }

    /// <summary>Field number for the "canResetGraph" field.</summary>
    public const int CanResetGraphFieldNumber = 14;
    private bool canResetGraph_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool CanResetGraph {
      get { return canResetGraph_; }
      set {
        canResetGraph_ = value;
      }
    }

    /// <summary>Field number for the "canSetAudioConfig" field.</summary>
    public const int CanSetAudioConfigFieldNumber = 15;
    private bool canSetAudioConfig_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool CanSetAudioConfig {
      get { return canSetAudioConfig_; }
      set {
        canSetAudioConfig_ = value;
      }
    }

    /// <summary>Field number for the "canSetVideoConfig" field.</summary>
    public const int CanSetVideoConfigFieldNumber = 16;
    private bool canSetVideoConfig_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool CanSetVideoConfig {
      get { return canSetVideoConfig_; }
      set {
        canSetVideoConfig_ = value;
      }
    }

    /// <summary>Field number for the "canConnectFilters" field.</summary>
    public const int CanConnectFiltersFieldNumber = 17;
    private bool canConnectFilters_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool CanConnectFilters {
      get { return canConnectFilters_; }
      set {
        canConnectFilters_ = value;
      }
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 18;
    private global::Camera.StartResult result_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Camera.StartResult Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    /// <summary>Field number for the "devicePointer" field.</summary>
    public const int DevicePointerFieldNumber = 19;
    private ulong devicePointer_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong DevicePointer {
      get { return devicePointer_; }
      set {
        devicePointer_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as StartCaptureResult);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(StartCaptureResult other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CanResetGraph != other.CanResetGraph) return false;
      if (CanSetAudioConfig != other.CanSetAudioConfig) return false;
      if (CanSetVideoConfig != other.CanSetVideoConfig) return false;
      if (CanConnectFilters != other.CanConnectFilters) return false;
      if (Result != other.Result) return false;
      if (DevicePointer != other.DevicePointer) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (CanResetGraph != false) hash ^= CanResetGraph.GetHashCode();
      if (CanSetAudioConfig != false) hash ^= CanSetAudioConfig.GetHashCode();
      if (CanSetVideoConfig != false) hash ^= CanSetVideoConfig.GetHashCode();
      if (CanConnectFilters != false) hash ^= CanConnectFilters.GetHashCode();
      if (Result != 0) hash ^= Result.GetHashCode();
      if (DevicePointer != 0UL) hash ^= DevicePointer.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (CanResetGraph != false) {
        output.WriteRawTag(112);
        output.WriteBool(CanResetGraph);
      }
      if (CanSetAudioConfig != false) {
        output.WriteRawTag(120);
        output.WriteBool(CanSetAudioConfig);
      }
      if (CanSetVideoConfig != false) {
        output.WriteRawTag(128, 1);
        output.WriteBool(CanSetVideoConfig);
      }
      if (CanConnectFilters != false) {
        output.WriteRawTag(136, 1);
        output.WriteBool(CanConnectFilters);
      }
      if (Result != 0) {
        output.WriteRawTag(144, 1);
        output.WriteEnum((int) Result);
      }
      if (DevicePointer != 0UL) {
        output.WriteRawTag(152, 1);
        output.WriteUInt64(DevicePointer);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (CanResetGraph != false) {
        size += 1 + 1;
      }
      if (CanSetAudioConfig != false) {
        size += 1 + 1;
      }
      if (CanSetVideoConfig != false) {
        size += 2 + 1;
      }
      if (CanConnectFilters != false) {
        size += 2 + 1;
      }
      if (Result != 0) {
        size += 2 + pb::CodedOutputStream.ComputeEnumSize((int) Result);
      }
      if (DevicePointer != 0UL) {
        size += 2 + pb::CodedOutputStream.ComputeUInt64Size(DevicePointer);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(StartCaptureResult other) {
      if (other == null) {
        return;
      }
      if (other.CanResetGraph != false) {
        CanResetGraph = other.CanResetGraph;
      }
      if (other.CanSetAudioConfig != false) {
        CanSetAudioConfig = other.CanSetAudioConfig;
      }
      if (other.CanSetVideoConfig != false) {
        CanSetVideoConfig = other.CanSetVideoConfig;
      }
      if (other.CanConnectFilters != false) {
        CanConnectFilters = other.CanConnectFilters;
      }
      if (other.Result != 0) {
        Result = other.Result;
      }
      if (other.DevicePointer != 0UL) {
        DevicePointer = other.DevicePointer;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 112: {
            CanResetGraph = input.ReadBool();
            break;
          }
          case 120: {
            CanSetAudioConfig = input.ReadBool();
            break;
          }
          case 128: {
            CanSetVideoConfig = input.ReadBool();
            break;
          }
          case 136: {
            CanConnectFilters = input.ReadBool();
            break;
          }
          case 144: {
            result_ = (global::Camera.StartResult) input.ReadEnum();
            break;
          }
          case 152: {
            DevicePointer = input.ReadUInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code