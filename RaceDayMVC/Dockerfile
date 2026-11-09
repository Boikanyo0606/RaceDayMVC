FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDayMVC.csproj", "./"]
RUN dotnet restore "RaceDayMVC.csproj"
COPY . .
RUN dotnet build "RaceDayMVC.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "RaceDayMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RaceDayMVC.dll"]
