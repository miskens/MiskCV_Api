#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1433

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MiskCv_Api/MiskCv_Api.csproj", "MiskCv_Api/"]
RUN dotnet restore "MiskCv_Api/MiskCv_Api.csproj"
COPY . .
WORKDIR "/src/MiskCv_Api"
RUN dotnet build "MiskCv_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiskCv_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiskCv_Api.dll"]