using System;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using QRCoder;
using static QRCoder.QRCodeGenerator;

namespace LesserMigraDoc.Samples.QrCode
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: program [output filename]");
                return;
            }

            new Program().Run(args[0]);
        }


        private void Run(string outputFile)
        {
            // Create QRCode PNG image
            var qrcodeBytes = CreateQRcodeImage("Hello QRCode !");

            // Create Document
            var document = new Document();
            document.Info.Title = "タイトル - CreateOnTheFly";
            document.Info.Subject = "サブジェクト - CreateOnTheFly";
            document.Info.Author = "作者 - CreateOnTheFly";
            document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.RightMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(0.4);
            document.DefaultPageSetup.BottomMargin = Unit.FromCentimeter(0.1);

            // Create Section
            var section = document.AddSection();

            section.AddImage(ImageBytes2Uri(qrcodeBytes));

            SavePdf(document, outputFile);
        }

        private void SavePdf(Document document, string path)
        {
            var renderer = new PdfDocumentRenderer(true)
            {
                Document = document,
            };

            renderer.RenderDocument();

            using (var dest = File.Create(path))
            {
                renderer.Save(dest, true);
            }
        }

        private byte[] CreateQRcodeImage(string message)
        {
            using (var data = QRCodeGenerator.GenerateQrCode(message, ECCLevel.M, true, false, EciMode.Utf8))
            using (var qrcode = new PngByteQRCode(data))
            {
                return qrcode.GetGraphic(20);
            }
        }

        private String ImageBytes2Uri(byte[] byteArray)
        {
            return "base64:" + Convert.ToBase64String(byteArray);
        }
    }
}
