FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY out/genval/ ./
ENTRYPOINT ["dotnet", "GenVal.dll"]

