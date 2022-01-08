FROM alpine:3.14 AS base
RUN apk add --no-cache bash icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 AS api.build
WORKDIR /pokespeare
ADD pokespeare.api /pokespeare
RUN dotnet publish "pokespeare.api.csproj" --runtime alpine-x64 -c Release --self-contained true -o /pokespeare/publish

FROM node:alpine as web.builder 
ENV REACT_APP_BASE_URL=api/v1/
WORKDIR /pokespeare
ADD pokespeare.web /pokespeare
RUN npm install && npm run build

FROM base AS final
WORKDIR /app
COPY --from=api.build /pokespeare/publish /app
COPY --from=web.builder /pokespeare/build /app/wwwroot
ENTRYPOINT ["/app/pokespeare.api"]