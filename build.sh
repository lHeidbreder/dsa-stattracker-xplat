#!/bin/bash

#PREP
set -e
if [ ! -d dist/ ]; then mkdir dist; fi

exportsvg () {
    echo Creating PNGs from SVGs
    inkscape --export-type=png $(find $(dirname $0) -name *.svg)
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

    find $(dirname $0)/dist/ -name *.pdb -delete
}

android () {
    echo Android
    dotnet publish \
        -c Release \
        -o dist/android \
        dsa_stattracker_xplat.Android/
    
    find $(dirname $0)/dist/android/ -type f -not -iname *signed.apk -delete
}

wasm () {
    echo WASM
    dotnet publish \
        -c Release \
        -p:Configuration=Release \
        -o dist/wasm \
        dsa_stattracker_xplat.Browser/
}

package () {
    pushd $(dirname $0)/dist

    RELEASES=$(ls -d */)
    for r in $RELEASES; do
        zip -r "${r%/}.zip" . -i "${r}*"
    done

    popd
}

#PERFORM
exportsvg

desktop
android
wasm

package

echo Successfully performed!
