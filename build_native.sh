#!/bin/bash
# Build the native sdformat_wrapper shared library.
# Prerequisites: cmake, a C++17 compiler, libsdformat (13-16) dev package.
#
# Usage:
#   ./build_native.sh            # build in Release mode
#   ./build_native.sh Debug      # build in Debug mode

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BUILD_TYPE="${1:-Release}"
BUILD_DIR="${SCRIPT_DIR}/sdformat_native~/build"

echo "==> Building sdformat_wrapper (${BUILD_TYPE}) ..."
mkdir -p "${BUILD_DIR}"
cd "${BUILD_DIR}"
cmake .. -DCMAKE_BUILD_TYPE="${BUILD_TYPE}"
cmake --build . --config "${BUILD_TYPE}" -j "$(nproc 2>/dev/null || echo 4)"

SHIM_LIB=$(find "${BUILD_DIR}" -name 'sdformat_wrapper.so' -o -name 'sdformat_wrapper.dylib' | head -n1)
if [[ -z "${SHIM_LIB}" ]]; then
    echo "ERROR: sdformat_wrapper library not found after build."
    exit 1
fi

echo "==> Built: ${SHIM_LIB}"

# Copy into the Plugins directory for Unity package
PLUGINS_DIR="${SCRIPT_DIR}/Plugins/x86_64"
mkdir -p "${PLUGINS_DIR}"
cp "${SHIM_LIB}" "${PLUGINS_DIR}/"

# Bundle non-system shared library dependencies alongside the shim.
# This avoids requiring ROS/Gazebo packages on the target machine.
# Only system libs (/lib, /usr/lib) are expected to be present at runtime.
echo "==> Bundling dependencies ..."
ldd "${SHIM_LIB}" | while read -r line; do
    # Extract the resolved path (third field), skip lines without " => "
    so_path=$(echo "$line" | awk '/=>/ { print $3 }')
    [[ -z "$so_path" || ! -f "$so_path" ]] && continue

    # Skip system libraries — keep only non-system ones
    case "$so_path" in
        /lib/*|/usr/lib/*|/lib64/*) continue ;;
    esac

    so_name=$(basename "$so_path")
    cp -v "$so_path" "${PLUGINS_DIR}/${so_name}"
done

# Recursively bundle transitive dependencies of the bundled libs
for bundled_lib in "${PLUGINS_DIR}"/*.so*; do
    [[ "$bundled_lib" == *.meta ]] && continue
    ldd "$bundled_lib" 2>/dev/null | while read -r line; do
        so_path=$(echo "$line" | awk '/=>/ { print $3 }')
        [[ -z "$so_path" || ! -f "$so_path" ]] && continue

        case "$so_path" in
            /lib/*|/usr/lib/*|/lib64/*) continue ;;
        esac

        so_name=$(basename "$so_path")
        if [[ ! -f "${PLUGINS_DIR}/${so_name}" ]]; then
            cp -v "$so_path" "${PLUGINS_DIR}/${so_name}"
        fi
    done
done

# Patch RUNPATH on all bundled .so files so they find each other via $ORIGIN
echo "==> Patching RUNPATH on bundled libraries ..."
if command -v patchelf &>/dev/null; then
    for lib in "${PLUGINS_DIR}"/*.so*; do
        [[ "$lib" == *.meta ]] && continue
        patchelf --set-rpath '$ORIGIN' "$lib"
        echo "  Patched: $(basename "$lib")"
    done
else
    echo "WARNING: patchelf not found. Install with: sudo apt install patchelf"
    echo "  Bundled libs may not resolve each other without system ROS install."
fi

echo "==> Plugins directory contents:"
ls -lh "${PLUGINS_DIR}"/*.so* 2>/dev/null

# Also copy into .NET project output for non-Unity usage
DOTNET_BUILD_DIR="${SCRIPT_DIR}/SdFormat.Net/bin"
mkdir -p "${DOTNET_BUILD_DIR}"
cp "${SHIM_LIB}" "${DOTNET_BUILD_DIR}/"
echo "==> Copied to ${DOTNET_BUILD_DIR}/"
echo "==> Done."
