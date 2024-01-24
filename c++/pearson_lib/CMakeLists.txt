project(pearson_lib)

set(HEADER_FILES pearson.h)
set(SOURCE_FILES pearson.cpp)

add_library(pearson_lib ${SOURCE_FILES} ${HEADER_FILES})