FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY avtoservis/avtoservis.csproj avtoservis/
RUN dotnet restore avtoservis/avtoservis.csproj

COPY avtoservis/ avtoservis/
RUN dotnet publish avtoservis/avtoservis.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "avtoservis.dll"]
