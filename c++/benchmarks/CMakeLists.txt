project(benchmarks)
enable_testing()

include(FetchContent)
set(CMAKE_CXX_STANDARD 14)
set(CMAKE_CXX_STANDARD_REQUIRED YES)
set(CMAKE_CXX_EXTENSIONS NO)

FetchContent_Declare(googletest
        GIT_REPOSITORY https://github.com/google/googletest.git
        GIT_TAG v1.10.x)

FetchContent_Declare(googlebenchmark
        GIT_REPOSITORY https://github.com/google/benchmark.git
        GIT_TAG main)

FetchContent_MakeAvailable(
        googletest
        googlebenchmark)

add_executable(benchmarks_run benchmark_reader.cpp)
target_link_libraries(benchmarks_run benchmark::benchmark)
target_link_libraries(benchmarks_run csv_reader_lib)
