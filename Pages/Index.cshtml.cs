using Microsoft.AspNetCore.Mvc.RazorPages;
using ZXing;
using ZXing.Rendering;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace QRGenerator.Pages
{
    /// <summary>
    /// Модель страницы для генерации QR-кодов.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Base64-представление сгенерированного QR-кода.
        /// </summary>
        public string QRCodeBase64 { get; set; }

        /// <summary>
        /// Входной текст или ссылка для генерации QR-кода.
        /// </summary>
        [BindProperty]
        public string InputText { get; set; }

        /// <summary>
        /// Обработчик GET-запроса. Инициализирует страницу.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Обработчик POST-запроса для генерации QR-кода.
        /// </summary>
        /// <returns>Возвращает страницу с результатом.</returns>
        public IActionResult OnPostGenerate()
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                ModelState.AddModelError("InputText", "Пожалуйста, введите текст или ссылку.");
                return Page();
            }

            var writer = new BarcodeWriter<Bitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 400,
                    Width = 400
                },
                Renderer = new BitmapRenderer()
            };

            using (var bitmap = writer.Write(InputText))
            {
                using (var memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    QRCodeBase64 = $"data:image/png;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                }
            }

            return Page();
        }
    }
}