FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 as build
WORKDIR /app

EXPOSE 80

#RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Order/FreeCourse.Services.Order.Application/*.csproj Services/Order/FreeCourse.Services.Order.Application/
COPY Services/Order/FreeCourse.Services.Order.Domain/*.csproj Services/Order/FreeCourse.Services.Order.Domain/
COPY Services/Order/FreeCourse.Services.Order.Domain.Core/*.csproj Services/Order/FreeCourse.Services.Order.Domain.Core/
COPY Services/Order/FreeCourse.Services.Order.Infrastucture/*.csproj Services/Order/FreeCourse.Services.Order.Infrastucture/
COPY Services/Order/FreeCourse.Services.Order.API/*.csproj Services/Order/FreeCourse.Services.Order.API/




RUN dotnet restore Services/Order/FreeCourse.Services.Order.API/*.csproj
COPY . .
RUN dotnet publish Services/Order/FreeCourse.Services.Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 as runtime
WORKDIR /app

RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Services.Order.API.dll" ]
