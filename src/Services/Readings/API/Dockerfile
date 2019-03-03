FROM microsoft/dotnet:2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2-sdk AS build
COPY . ./app
WORKDIR ./app/API
RUN dotnet restore
RUN dotnet build --no-restore -c Release -o /release

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /release

FROM build as unittest
WORKDIR /app/Readings.UnitTests

FROM base AS final
WORKDIR /app
COPY --from=publish /release .
ENTRYPOINT ["dotnet", "Readings.API.dll"]