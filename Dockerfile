FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV EmailSenderOptions__Email="[YOUR MAILER EMAIL]"
ENV EmailSenderOptions__Password="[YOUR MAILER PASSWORD]"
ENV ConnectionStrings__DefaultConnectionString="Server=localhost;Port=3306;Database=MyAtelierDb;User ID=root;Password=kulik2004"
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Persistance/Persistance.csproj", "Persistance/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]
RUN dotnet restore Presentation/Presentation.csproj
COPY . .
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build -c $BUILD_CONFIGURATION -o Build

FROM build AS publish
ARG PUBLISH_CONFIGURATION=Release
RUN dotnet publish -c $PUBLISH_CONFIGURATION -o Publish

FROM base AS final
WORKDIR /app
COPY --from=publish src/Publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]