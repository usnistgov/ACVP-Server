FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY out/genval/ ./
ENTRYPOINT ["dotnet", "NIST.CVP.ACVTS.Generation.GenValApp.dll"]
CMD []




