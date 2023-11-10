# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CleaningRobotApi/CleaningRobotApi.csproj", "./"]
RUN dotnet restore "CleaningRobotApi.csproj"
COPY CleaningRobotApi/ .
RUN dotnet build "CleaningRobotApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleaningRobotApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleaningRobotApi.dll"]
