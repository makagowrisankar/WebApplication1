#!/usr/bin/env bash
set -e

if [ ! -x "$HOME/.dotnet/dotnet" ]; then
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0
fi

exec "$HOME/.dotnet/dotnet" out/WebApplication1.dll