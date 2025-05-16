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
    /// ������ �������� ��� ��������� QR-�����.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Base64-������������� ���������������� QR-����.
        /// </summary>
        public string QRCodeBase64 { get; set; }

        /// <summary>
        /// ������� ����� ��� ������ ��� ��������� QR-����.
        /// </summary>
        [BindProperty]
        public string InputText { get; set; }

        /// <summary>
        /// ���������� GET-�������. �������������� ��������.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// ���������� POST-������� ��� ��������� QR-����.
        /// </summary>
        /// <returns>���������� �������� � �����������.</returns>
        public IActionResult OnPostGenerate()
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                ModelState.AddModelError("InputText", "����������, ������� ����� ��� ������.");
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