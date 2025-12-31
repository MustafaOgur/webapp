# 1. Build Aşaması
FROM node:18-alpine as build
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
RUN npm run build

# 2. Sunucu Aşaması (Nginx ile servis edeceğiz)
FROM nginx:alpine
# React build dosyalarını Nginx'in html klasörüne atıyoruz
COPY --from=build /app/build /usr/share/nginx/html

# Nginx ayarı (React Router için gerekli)
# Eğer sayfa yenilendiğinde 404 hatası alırsan basit bir nginx.conf eklememiz gerekir.
# Şimdilik varsayılan yeterli olabilir ama React Router için şu komut iyidir:
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]