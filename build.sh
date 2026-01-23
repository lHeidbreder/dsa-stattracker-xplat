#!/bin/bash

#PREP
set -e
if [ ! -d dist/ ]; then mkdir dist; fi

exportsvg () {
    echo Creating PNGs from SVGs
    find $(dirname $0) -name '*.svg' -exec inkscape --export-type=png {} \;
}

desktop () {
    DESKTOP_PLATFORMS="linux-x64 win-x64"

    for p in $DESKTOP_PLATFORMS
    do
        echo Desktop: $p
        dotnet publish \
            -c Release \
            -r $p \
            -o dist/$p \
            dsa_stattracker_xplat.Desktop/
    done
}

android () {
    echo Android
    dotnet publish \
        -c Release \
        -o dist/android \
        dsa_stattracker_xplat.Android/
}

wasm () {
    echo WASM
    dotnet publish \
        -c Release \
        -p:Configuration=Release \
        -o dist/wasm \
        dsa_stattracker_xplat.Browser/
}

#PERFORM
exportsvg

desktop
android
wasm

echo Successfully performed!
