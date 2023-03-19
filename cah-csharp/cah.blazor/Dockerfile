#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cah.blazor/cah.blazor.csproj", "cah.blazor/"]
COPY ["cah.models/cah.models.csproj", "cah.models/"]
COPY ["cah.services/cah.services.csproj", "cah.services/"]
RUN dotnet restore "cah.blazor/cah.blazor.csproj"
COPY . .
WORKDIR "/src/cah.blazor"
RUN dotnet build "cah.blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cah.blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cah.blazor.dll"]