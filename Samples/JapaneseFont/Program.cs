using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharpCore.Fonts;
using PdfSharpCore.Utils;

namespace LesserMigraDocCore.Samples.CreateOnTheFly
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

        static Program()
        {
            RegisterEncodingProvider();
            PrepareFontResolver();
        }

        private static void RegisterEncodingProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private static void PrepareFontResolver()
        {
            GlobalFontSettings.FontResolver = new CustomFontResolver();
        }

        private readonly MigraDoc.DocumentObjectModel.Font font1 = new MigraDoc.DocumentObjectModel.Font
        {
            Name = "Gen Shin Gothic",
            Size = 15,
            Color = Colors.DarkOrange,
        };
        private readonly MigraDoc.DocumentObjectModel.Font font2 = new MigraDoc.DocumentObjectModel.Font
        {
            Name = "TakaoMincho",
            Size = 20,
        };


        private void Run(string outputFile)
        {
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
            section.PageSetup.HeaderDistance = Unit.FromCentimeter(1);
            section.PageSetup.FooterDistance = Unit.FromCentimeter(1);
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.TopMargin = Unit.FromCentimeter(4.5);

            section.Headers.Primary
                .AddParagraph()
                .AddFormattedText("こんにちは ヘッダー！", font1);

            section.Add(CreateTable1());

            section.Footers.Primary
                .AddParagraph()
                .AddFormattedText("こんにちは フッター!", font1);

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

        private Table CreateTable1()
        {
            var table = new Table
            {
                TopPadding = 0,
                BottomPadding = 1,
            };
            table.Borders.Visible = true;
            table.AddColumn(Unit.FromCentimeter(10.5))
                .Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromPoint(60));
            table.AddColumn();

            var row1 = table.AddRow();
            row1.Borders.Visible = true;
            row1.Cells[0]
                .AddParagraph()
                .AddFormattedText("こんにちは 表セル1!", font2);

            var row2 = table.AddRow();
            row2.HeightRule = RowHeightRule.Auto;
            row2.Cells[1]
                .AddParagraph()
                .AddFormattedText("こんにちは 表セル2!", font2);

            var row3 = table.AddRow();
            row3.Cells[2]
                .AddParagraph()
                .AddFormattedText("こんにちは 表セル3!", font2);

            return table;
        }
    }
}
