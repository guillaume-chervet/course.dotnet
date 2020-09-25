FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "Console.csproj" -c Release -r linux-x64 /p:PublishSingleFile=true  -o /publish

FROM mcr.microsoft.com/dotnet/core/runtime-deps:5.0 AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["/app/Console"]
