#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NOTS-WAPP-Demo-BrokenAccessControl/NOTS-WAPP-Demo-BrokenAccessControl.csproj", "NOTS-WAPP-Demo-BrokenAccessControl/"]
RUN dotnet restore "NOTS-WAPP-Demo-BrokenAccessControl/NOTS-WAPP-Demo-BrokenAccessControl.csproj"
COPY . .
WORKDIR "/src/NOTS-WAPP-Demo-BrokenAccessControl"
RUN dotnet build "NOTS-WAPP-Demo-BrokenAccessControl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NOTS-WAPP-Demo-BrokenAccessControl.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NOTS-WAPP-Demo-BrokenAccessControl.dll"]