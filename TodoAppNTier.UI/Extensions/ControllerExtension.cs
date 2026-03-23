using Microsoft.AspNetCore.Mvc;
using TodoAppNTier.Common.ResponseObjects;

namespace TodoAppNTier.UI.Extensions
{
    public static class ControllerExtension
    {
        // 1. GET İŞLEMLERİ İÇİN (Veri Gösterenler: Index, Update-Get)
        public static IActionResult ResponseView<T>(this Controller controller, IResponse<T> response)
        {
            if (response.ResponseType == ResponseType.NotFound)
            {
                controller.TempData["ErrorMessage"] = response.Message;
                return controller.NotFound();
            }

            // Başarılıysa veriyi (Data) View'a yolla
            return controller.View(response.Data);
        }

        // 2. POST İŞLEMLERİ İÇİN (Kaydedenler/Güncelleyenler: Create-Post, Update-Post, Remove)
        // DİKKAT: Burada IResponse<T> yerine IResponse var, çünkü kayıt işleminde Controller'a Data dönmüyoruz!
        public static IActionResult ResponseRedirectToAction(this Controller controller, IResponse response, string actionName)
        {
            if (response.ResponseType == ResponseType.NotFound)
            {
                controller.TempData["ErrorMessage"] = response.Message;
                return controller.NotFound();
            }

            // Hata çıkmadıysa istediğimiz sayfaya (Örn: Index) yönlendir.
            return controller.RedirectToAction(actionName);
        }

        // 3. POST İŞLEMLERİNDE HATA ÇIKARSA KULLANILACAK METOT (Validation İçin)
        // Hata çıktığında kullanıcının girdiği veriler (dto) silinmesin diye bunu kullanacağız.
        public static IActionResult ResponseViewError<T>(this Controller controller, IResponse response, T dto)
        {
            foreach (var error in response.ValidationErrors)
            {
                controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return controller.View(dto);
        }
    }
}