#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Karolinska.Web/Karolinska.Web.csproj", "Karolinska.Web/"]
RUN dotnet restore "Karolinska.Web/Karolinska.Web.csproj"
COPY . .
WORKDIR "/src/Karolinska.Web"
RUN dotnet build "Karolinska.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Karolinska.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Karolinska.Web.dll"]