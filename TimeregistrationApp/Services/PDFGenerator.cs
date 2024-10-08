﻿using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TimeregistrationApp.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
using Cell = iText.Layout.Element.Cell;

namespace TimeregistrationApp.Services
{
    public static class PDFGenerator
    {
        public static async Task GeneratePDF(List<TijdsRegistratie> tijdsRegistraties, string titel)
        {
            var fileName = titel + ".pdf";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                Paragraph title = new(titel);

                title.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
                title.SetFontSize(20);

                document.Add(title);

                var table = new Table(3, true);

                var headerDatum = new Cell().Add(new Paragraph("Date").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)));
                var headerUren = new Cell().Add(new Paragraph("Worked hours").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)));
                var headerNotitie = new Cell().Add(new Paragraph("Note").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)));

                table.AddHeaderCell(headerDatum);
                table.AddHeaderCell(headerUren);
                table.AddHeaderCell(headerNotitie);

                foreach (var tijdsRegistratie in tijdsRegistraties)
                {
                    table.AddCell(new Cell().Add(new Paragraph(tijdsRegistratie.StartTime.ToLongDateString())));
                    table.AddCell(new Cell().Add(new Paragraph(tijdsRegistratie.IsHoliday ? "Holiday" : tijdsRegistratie.StartTime.ToShortTimeString() + " - " + tijdsRegistratie.EndTime.ToShortTimeString())));
                    table.AddCell(new Cell().Add(new Paragraph(tijdsRegistratie.Note ?? "")));
                }

                document.Add(table);

                var totalHours = 0.0;
                foreach (var tijdsRegistratie in tijdsRegistraties)
                {
                    var duration = tijdsRegistratie.EndTime - tijdsRegistratie.StartTime;
                    totalHours += duration.TotalHours;
                }
                document.Add(new Paragraph("\n\n"));
                document.Add(new Paragraph($"Total hours: {totalHours}"));

                document.Close();
            }

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }
}