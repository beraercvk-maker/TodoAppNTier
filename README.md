# 🚀 TodoApp N-Tier Architecture

Bu proje, ASP.NET Core 9 kullanılarak geliştirilmiş, sürdürülebilir, ölçeklenebilir ve "Clean Code" prensiplerine tam uyumlu bir Çok Katmanlı Mimari (N-Tier Architecture) uygulamasıdır. Standart bir To-Do uygulamasının ötesinde, kurumsal projelerde kullanılan modern yazılım tasarım desenlerini (Design Patterns) barındırır.

## 🛠️ Kullanılan Teknolojiler ve Araçlar
* **Framework:** .NET 9.0 & ASP.NET Core MVC
* **ORM:** Entity Framework Core
* **Veritabanı:** SQL Server / SQLite
* **Validasyon:** FluentValidation
* **Nesne Dönüşümü (Mapping):** AutoMapper

## 🏗️ Katmanlar ve Sorumlulukları (N-Tier)

Proje, bağımlılıkların tek yönlü olduğu ve "Separation of Concerns" (Sorumlulukların Ayrılığı) ilkesinin katı bir şekilde uygulandığı 6 ana katmandan oluşmaktadır:

1. **Entities (`TodoAppNTier.Entities`):** Veritabanı tablolarının nesne (POCO) karşılıklarının tutulduğu çekirdek katmandır. Hiçbir dış bağımlılığı yoktur.
2. **DataAccess (`TodoAppNTier.DataAccess`):** Veritabanı iletişiminin kurulduğu katmandır. **Generic Repository** ve **Unit of Work (UoW)** desenleri burada uygulanmıştır.
3. **Dtos (`TodoAppNTier.Dtos`):** UI ile Business katmanı arasında güvenli veri taşımak için kullanılan Data Transfer Object katmanıdır.
4. **Common (`TodoAppNTier.Common`):** Projenin genelinde kullanılan ortak yapıları barındırır. Özel **Response Pattern** (IResponse, Response<T>) nesneleri bu katmanda yer alır.
5. **Business (`TodoAppNTier.Business`):** İş kurallarının işletildiği, AutoMapper dönüşümlerinin ve FluentValidation kurallarının uygulandığı katmandır.
6. **UI (`TodoAppNTier.UI`):** Kullanıcı arayüzü katmanıdır. İş mantığı barındırmaz ("Lean Controller" prensibi). Özel Controller Extension'ları sayesinde minimum kod ile maksimum işlevsellik sunar.

## ✨ Öne Çıkan Mimari Yaklaşımlar

* **Custom Response Pattern (Wrapper):** Servislerden UI katmanına doğrudan veri veya `null` dönmek yerine; işlemin durumunu (`Success`, `NotFound`, `ValidationError`), hata mesajlarını ve taşınan veriyi tek bir paket (`IResponse`) halinde sunan kurumsal bir yapı inşa edilmiştir.
* **Controller Extensions:** UI katmanında tekrar eden `if/else` doğrulama bloklarını ortadan kaldırmak için özel Extension metotlar (`ResponseView`, `ResponseRedirectToAction`) yazılmıştır. Bu sayede Controller'lar son derece sade ve okunabilir hale getirilmiştir.
* **Merkezi Hata Yönetimi:** FluentValidation ile yakalanan hatalar, Business katmanından kargo kutuları içinde UI'a taşınır ve otomatik olarak MVC'nin `ModelState` yapısına entegre edilir.
* **Custom 404 Pages:** Sistemde bulunamayan veriler veya hatalı yönlendirmeler için middleware seviyesinde özel hata sayfaları tasarlanmıştır.

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

1. Projeyi klonlayın:
   ```bash
   git clone [https://github.com/KULLANICI_ADIN/TodoAppNTier.git](https://github.com/beraercvk-maker/TodoAppNTier.git)

2.UI Projesi dizinine gidin: cd TodoAppNTier/TodoAppNTier.UI
3.Gerekli paketleri yükleyin: dotnet restore
4.projeyi çalışırın : dotnet run
