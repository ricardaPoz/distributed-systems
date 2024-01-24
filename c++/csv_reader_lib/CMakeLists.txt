project(csv_reader_lib)

set(HEADER_FILES csv_reader.h)
set(SOURCE_FILES csv_reader.cpp)

add_library(csv_reader_lib ${SOURCE_FILES} ${HEADER_FILES})