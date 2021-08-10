#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Stecpoint.ServiceB/Stecpoint.ServiceB.csproj", "src/Stecpoint.ServiceB/"]
COPY ["src/Stecpoint.Data/Stecpoint.Data.csproj", "src/Stecpoint.Data/"]
COPY ["src/Stecpoint.Core/Stecpoint.Core.csproj", "src/Stecpoint.Core/"]
RUN dotnet restore "src/Stecpoint.ServiceB/Stecpoint.ServiceB.csproj"
COPY . .
WORKDIR "/src/src/Stecpoint.ServiceB"
RUN dotnet build "Stecpoint.ServiceB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stecpoint.ServiceB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stecpoint.ServiceB.dll"]