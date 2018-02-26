#!/bin/bash

BASEDIR=$(dirname $0)
cd ${BASEDIR}
dotnet DKC3MapEditionHelper.dll $*