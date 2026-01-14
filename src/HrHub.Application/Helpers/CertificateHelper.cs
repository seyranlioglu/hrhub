using DinkToPdf;
using DinkToPdf.Contracts;
using FluentFTP;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using System.Text;

namespace HrHub.Application.Helpers
{
    public static class CertificateHelper
    {
        private static readonly SynchronizedConverter _pdfConverter = new SynchronizedConverter(new PdfTools());

        public static string PrepareHtmlContent(string htmlContent, CertificateDataModel data)
        {
            if (string.IsNullOrEmpty(htmlContent)) return "<h1>HATA: Sertifika sablonu bos.</h1>";

            // Tüm parametreleri replace ediyoruz
            return htmlContent
                .Replace("{{StudentName}}", data.StudentName)
                .Replace("{{TrainingName}}", data.TrainingName)
                .Replace("{{Score}}", data.Score.ToString("F0"))
                .Replace("{{Date}}", data.CompletionDate)
                .Replace("{{VerificationCode}}", data.VerificationCode)
                // Yeni Eklenen Metin Alanları
                .Replace("{{Title}}", data.Title)
                .Replace("{{Subtitle}}", data.Subtitle)
                .Replace("{{BodyText}}", data.BodyText) // BodyText içinde de isim geçebilir, onu data.BodyText oluşturulurken çözeceğiz
                .Replace("{{ScoreLabel}}", data.ScoreLabel)
                .Replace("{{DateLabel}}", data.DateLabel)
                .Replace("{{SignerName}}", data.SignerName)
                .Replace("{{SignerTitle}}", data.SignerTitle);
        }

        public static byte[] GeneratePdfBytes(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };
            return _pdfConverter.Convert(doc);
        }

        public static Response<string> SaveCertificate(byte[] fileBytes, string fileName, CertificateSettings settings)
        {
            // ... (Save mantığı aynı kalıyor, burayı tekrar yazarak kalabalık etmiyorum) ...
            try
            {
                // FTP Kayıt
                if (!string.IsNullOrEmpty(settings.StorageType) && settings.StorageType.ToUpper() == "FTP")
                {
                    using (var ftp = new FtpClient(settings.FtpHost, settings.FtpUser, settings.FtpPass, settings.FtpPort))
                    {
                        ftp.Connect();
                        string remotePath = $"/certificates/{fileName}";
                        if (!ftp.DirectoryExists("/certificates")) ftp.CreateDirectory("/certificates");

                        var status = ftp.UploadBytes(fileBytes, remotePath, FtpRemoteExists.Overwrite);
                        if (status == FtpStatus.Success)
                            return Response<string>.Success($"ftp://{settings.FtpHost}{remotePath}", new ResponseHeader { ResCode = HrStatusCodes.Status200OK });

                        return Response<string>.Fail<string>($"FTP Hatasi: {status}", HrStatusCodes.Status500InternalServerError);
                    }
                }

                // Local Kayıt
                string savePath = settings.LocalPath ?? "wwwroot/certificates";
                string folder = Path.Combine(Directory.GetCurrentDirectory(), savePath);

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fullPath = Path.Combine(folder, fileName);
                File.WriteAllBytes(fullPath, fileBytes);

                string publicUrl = $"/{savePath.Replace("wwwroot/", "").Replace("wwwroot\\", "").Trim('/', '\\')}/{fileName}";

                return Response<string>.Success(publicUrl, new ResponseHeader { ResCode = HrStatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Response<string>.Fail<string>($"Dosya kaydedilemedi: {ex.Message}", HrStatusCodes.Status500InternalServerError);
            }
        }
    }

    // Genişletilmiş Model
    public class CertificateDataModel
    {
        public string StudentName { get; set; }
        public string TrainingName { get; set; }
        public string CompletionDate { get; set; }
        public double Score { get; set; }
        public string VerificationCode { get; set; }

        // Parametrik Metinler
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string BodyText { get; set; }
        public string ScoreLabel { get; set; }
        public string DateLabel { get; set; }
        public string SignerName { get; set; }
        public string SignerTitle { get; set; }
    }
}