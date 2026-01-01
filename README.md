# DevOpsWizard - AI Destekli Web Uygulaması

Bu proje, **.NET 9.0 (Backend)** ve **React (Frontend)** teknolojileri kullanılarak geliştirilmiş, modern mimariye sahip, AI destekli tam kapsamlı bir web uygulamasıdır. Proje, katmanlı mimari (N-Layer Architecture) prensiplerine uygun olarak tasarlanmış ve Dockerize edilmiştir.

## 🚀 Proje Geliştirme Süreci ve Tamamlanan Özellikler

Aşağıdaki tablo, proje gereksinimlerini, kullanılan teknolojileri ve ilgili geliştirme dallarını (branch) özetlemektedir.

| # | Özellik (Feature) | Kullanılan Teknolojiler & Yöntemler | İlgili Branch | Açıklama |
|---|---|---|---|---|
| **1** | **Presentation Layer** | **React, React Router, Axios** | `presentation` | Kullanıcı arayüzü React ile geliştirildi. Sayfa yönlendirmeleri ve API iletişimi için Axios yapılandırıldı. |
| **2** | **Business Layer** | **.NET C# (Services & Managers)** | `business` | İş mantığı katmanı. SOLID prensiplerine uygun servisler ve DTO (Data Transfer Object) dönüşümleri burada yapıldı. |
| **3** | **Data Layer** | **EF Core, MSSQL, Code-First** | `data` | Veri erişim katmanı. Entity Framework Core kullanılarak Code-First yaklaşımıyla veritabanı tasarlandı. |
| **4** | **Web Service** | **ASP.NET Core Web API (RESTful)** | `web-service` | Frontend ile haberleşen RESTful API uç noktaları (Controllers) yazıldı. Swagger entegrasyonu yapıldı. |
| **5** | **RBAC Implementation** | **Role-Based Access Control** | `rbac` | Rol tabanlı yetkilendirme sistemi. Admin ve User rolleri ayrıştırıldı, Admin Dashboard korumaya alındı. |
| **6** | **Authorization** | **JWT (JSON Web Token)** | `auth` | Güvenli kimlik doğrulama. Kullanıcı girişlerinde Access Token üretimi sağlandı. |
| **7** | **Session / Cookie** | **HttpOnly Cookie, Secure Flag** | `session-cookie` | **Güvenlik İyileştirmesi:** XSS saldırılarına karşı Refresh Token'lar tarayıcıda `HttpOnly Cookie` olarak saklandı. |
| **8** | **Extension / Third Party Library Using** | **Markdig** | `extension-library` | AI tarafından üretilen Markdown içeriklerini işlemek, kod bloklarını (Fenced Code Blocks) ayrıştırmak ve programlama dillerini tespit etmek için **Markdig** kütüphanesi kullanıldı. |
| **9** | **Web Security** | **CORS, Rate Limiting** | `web-security` | API güvenliği için CORS politikaları ve Brute-Force saldırılarına karşı Rate Limiting (Hız Sınırlama) eklendi. |
| **10** | **Cloud Service (AI)** | **OpenRouter API (LLM Integration)** | `cloud-service` | Yapay zeka entegrasyonu. Kullanıcıların sorularını yanıtlayan AI Chatbot modülü eklendi. |

---

## 🛠️ Kullanılan Teknolojiler (Tech Stack)

### Backend
* **.NET 9.0 SDK**
* **Entity Framework Core** (ORM)
* **MSSQL Server 2022** (Database)
* **JWT & HttpOnly Cookies** (Auth)
* **FluentValidation & AutoMapper**

### Frontend
* **React.js** (Library)
* **React Router Dom** (Navigation)
* **Axios** (HTTP Client - with Interceptors)
* **React Toastify** (Notifications)

### DevOps & Araçlar
* **Docker & Docker Compose**
* **Git & GitHub** (Version Control)
* **Swagger / OpenAPI** (Documentation)

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel ortamda çalıştırmak için aşağıdaki adımları izleyin:

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/MustafaOgur/webapp.git
```
### 2. Gereksinimler
Bilgisayarınızda Docker Desktop uygulamasının yüklü ve çalışıyor olması gerekmektedir.

### 3. Ortam Değişkenlerini Ayarlama (.env)
Projenin kök dizininde (docker-compose.yml dosyasının olduğu yerde) .env adında bir dosya oluşturun ve içine OpenRouter API anahtarınızı ekleyin. Bu anahtar AI servisinin çalışması için gereklidir:
```bash
OPENROUTER_API_KEY=sk-or-v1-......(kendi_keyiniz_buraya)
```
### 4. Uygulamayı Başlatma
Bu komut, gerekli veritabanı, backend ve frontend servislerini derleyip ayağa kaldıracaktır:
```bash
docker-compose up --build
```
### 5. Erişim
Konteynerler hazır olduğunda (yaklaşık 1-2 dakika sürebilir) aşağıdaki adreslerden erişim sağlayabilirsiniz:

Web Arayüzü (Frontend): http://localhost:3000

API Dokümantasyonu (Swagger): http://localhost:5199/swagger

SQL Server: `localhost:1433`

### 6. Uygulamayı Durdurma
Uygulamayı kapatmak için terminalde CTRL + C yapabilir veya şu komutu kullanabilirsiniz:
```bash
docker-compose down
```
