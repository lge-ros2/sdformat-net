#!/bin/bash
# Build the native sdformat_shim shared library.
# Prerequisites: cmake, a C++17 compiler, libsdformat (13-16) dev package.
#
# Usage:
#   ./build_native.sh            # build in Release mode
#   ./build_native.sh Debug      # build in Debug mode

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BUILD_TYPE="${1:-Release}"
BUILD_DIR="${SCRIPT_DIR}/sdformat_native~/build"

echo "==> Building sdformat_shim (${BUILD_TYPE}) ..."
mkdir -p "${BUILD_DIR}"
cd "${BUILD_DIR}"
cmake .. -DCMAKE_BUILD_TYPE="${BUILD_TYPE}"
cmake --build . --config "${BUILD_TYPE}" -j "$(nproc 2>/dev/null || echo 4)"

SHIM_LIB=$(find "${BUILD_DIR}" -name 'sdformat_shim.so' -o -name 'sdformat_shim.dylib' | head -n1)
if [[ -z "${SHIM_LIB}" ]]; then
    echo "ERROR: sdformat_shim library not found after build."
    exit 1
fi

echo "==> Built: ${SHIM_LIB}"

# Copy into the .NET project output for convenience
DOTNET_BUILD_DIR="${SCRIPT_DIR}/SdFormat.Net/bin"
mkdir -p "${DOTNET_BUILD_DIR}"
cp "${SHIM_LIB}" "${DOTNET_BUILD_DIR}/"
echo "==> Copied to ${DOTNET_BUILD_DIR}/"
echo "==> Done."
