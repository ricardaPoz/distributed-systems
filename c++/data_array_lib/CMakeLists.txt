project(data_array_lib)

set(HEADER_FILES DataArray.h)
set(SOURCE_FILES DataArray.cpp)

add_library(data_array_lib ${SOURCE_FILES} ${HEADER_FILES})
include_directories(../pearson_lib)
target_link_libraries(data_array_lib pearson_lib)
