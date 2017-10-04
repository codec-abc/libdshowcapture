pushd %~dp0
..\packages\google-protobuf-cpp.3.3.0.2\installed\x64-windows-static\tools\protoc.exe .\proto.proto --cpp_out=.\output\cpp
popd