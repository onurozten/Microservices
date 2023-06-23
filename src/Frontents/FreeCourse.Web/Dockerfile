FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Frontents/FreeCourse.Web/*.csproj Frontents/FreeCourse.Web/
RUN dotnet restore Frontents/FreeCourse.Web/*.csproj
COPY . .
RUN dotnet publish Frontents/FreeCourse.Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Web.dll" ]
