cmake_minimum_required(VERSION 3.11)
project(c)

set(CMAKE_CXX_STANDARD 23)
set(SOURCE_FILES main.cpp)

add_executable(c_run ${SOURCE_FILES})

add_subdirectory(csv_reader_lib)
include_directories(csv_reader_lib)
target_link_libraries(c_run csv_reader_lib)
#add_subdirectory(benchmarks)
