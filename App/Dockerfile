#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["plataforma-videos-api.csproj", "."]
RUN dotnet restore "./plataforma-videos-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "plataforma-videos-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "plataforma-videos-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "plataforma-videos-api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet plataforma-videos-api.dll