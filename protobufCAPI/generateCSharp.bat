pushd %~dp0
.\bin\protoc.exe proto.proto --csharp_out=.\output\cs
popd