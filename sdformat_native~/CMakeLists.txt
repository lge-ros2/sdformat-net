cmake_minimum_required(VERSION 3.16)
project(sdformat_shim C CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)

find_package(sdformat16 QUIET)
if(NOT sdformat16_FOUND)
  find_package(sdformat15 QUIET)
endif()
if(NOT sdformat15_FOUND)
  find_package(sdformat14 QUIET)
endif()
if(NOT sdformat14_FOUND)
  find_package(sdformat13 QUIET)
endif()

if(sdformat16_FOUND)
  set(SDFORMAT_TARGET sdformat16::sdformat16)
  message(STATUS "Found sdformat16")
elseif(sdformat15_FOUND)
  set(SDFORMAT_TARGET sdformat15::sdformat15)
  message(STATUS "Found sdformat15")
elseif(sdformat14_FOUND)
  set(SDFORMAT_TARGET sdformat14::sdformat14)
  message(STATUS "Found sdformat14")
elseif(sdformat13_FOUND)
  set(SDFORMAT_TARGET sdformat13::sdformat13)
  message(STATUS "Found sdformat13")
else()
  message(FATAL_ERROR "Could not find any sdformat version (13-16). "
    "Install with: sudo apt install libsdformat14-dev")
endif()

add_library(sdformat_shim SHARED
  sdformat_shim.cpp
)

target_link_libraries(sdformat_shim PRIVATE ${SDFORMAT_TARGET})

# Set output name without 'lib' prefix on Linux for consistency
set_target_properties(sdformat_shim PROPERTIES
  PREFIX ""
  OUTPUT_NAME "sdformat_shim"
)

install(TARGETS sdformat_shim
  LIBRARY DESTINATION lib
  RUNTIME DESTINATION bin
)
