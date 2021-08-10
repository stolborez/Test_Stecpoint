#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Stecpoint.ServiceA/Stecpoint.ServiceA.csproj", "src/Stecpoint.ServiceA/"]
COPY ["src/Stecpoint.Data/Stecpoint.Data.csproj", "src/Stecpoint.Data/"]
COPY ["src/Stecpoint.Core/Stecpoint.Core.csproj", "src/Stecpoint.Core/"]
RUN dotnet restore "src/Stecpoint.ServiceA/Stecpoint.ServiceA.csproj"
COPY . .
WORKDIR "/src/src/Stecpoint.ServiceA"
RUN dotnet build "Stecpoint.ServiceA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stecpoint.ServiceA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stecpoint.ServiceA.dll"]