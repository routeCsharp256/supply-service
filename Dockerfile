FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./src/Ozon.DotNetCourse.SupplyService/Ozon.DotNetCourse.SupplyService.csproj", "./Ozon.DotNetCourse.SupplyService/"]
COPY ["./src/Ozon.DotNetCourse.SupplyService.Migrator/Ozon.DotNetCourse.SupplyService.Migrator.csproj", "./Ozon.DotNetCourse.SupplyService.Migrator/"]
RUN dotnet restore "./Ozon.DotNetCourse.SupplyService/Ozon.DotNetCourse.SupplyService.csproj"

COPY "./src" .
WORKDIR "/src"

RUN dotnet build "Ozon.DotNetCourse.SupplyService/Ozon.DotNetCourse.SupplyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ozon.DotNetCourse.SupplyService/Ozon.DotNetCourse.SupplyService.csproj" -c Release -o /app/publish
COPY "entrypoint.sh" "/app/publish/."

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chmod +x entrypoint.sh
CMD /bin/bash entrypoint.sh