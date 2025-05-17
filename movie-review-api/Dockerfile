# build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# restore
COPY ["src/MovieReviewApi/MovieReviewApi.csproj", "MovieReviewApi/"]
RUN dotnet restore "MovieReviewApi/MovieReviewApi.csproj"

#build
COPY ["src/MovieReviewApi/", "MovieReviewApi/"]
WORKDIR "/src/MovieReviewApi"
RUN dotnet build 'MovieReviewApi.csproj' -c Release -o /app/build

#publish stage
FROM build AS publish
RUN dotnet publish 'MovieReviewApi.csproj' -c Release -o /app/publish

#run stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=5238
EXPOSE 5238
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "MovieReviewApi.dll" ]
