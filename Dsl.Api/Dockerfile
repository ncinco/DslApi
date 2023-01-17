#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dsl.Api/Dsl.Api.csproj", "Dsl.Api/"]
RUN dotnet restore "Dsl.Api/Dsl.Api.csproj"
COPY . .
WORKDIR "/src/Dsl.Api"
RUN dotnet build "Dsl.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dsl.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dsl.Api.dll"]