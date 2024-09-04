using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using ResponsiveWorkManager.Common;
using ResponsiveWorkManager.ViewModels;
using ResponsiveWorkManager.DataObjects;

namespace ResponsiveWorkManager.PDF
{
	public static class CreatePDF
	{
		public static void CreatePDFDocument(object obj)
		{
			try
			{
				CommonHelper.WaitCursor();
				if (obj is LeaveDetailsViewModel)
				{
					List<LeaveDetails> leaveDetails = ((LeaveDetailsViewModel)obj).LeaveDetails;

					using (PdfWriter writer = new PdfWriter(ConfigurationManager.AppSettings["SavePath"] + @"\Leave-Details.pdf"))
					{
						string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Background\Noxum-Banner.png";
						PdfDocument pdf = new PdfDocument(writer);
						Document document = new Document(pdf, iText.Kernel.Geom.PageSize.LETTER);
						document.SetBackgroundColor(new DeviceRgb(253, 255, 222));
						//document.SetMargins(5, 5, 5, 5);

						PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
						PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
						Color colorBlue = new DeviceRgb(0, 255, 255);
						Color rowColour = new DeviceRgb(204, 255, 255);


						document.Add(new Image(ImageDataFactory.Create(path)));
						document.Add(new Paragraph());
						document.Add(new Paragraph("Leave Details").SetFont(bold).SetFontSize(15).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetUnderline());

						Table table = new Table(leaveDetails.Count);
						table.AddCell(new Paragraph("No.").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
						table.AddCell(new Paragraph("Type Of Leave").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
						table.AddCell(new Paragraph("Day Type").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
						table.AddCell(new Paragraph("Date").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
						table.AddCell(new Paragraph("Month").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
						table.AddCell(new Paragraph("Day").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
						table.AddCell(new Paragraph("Comment").SetFont(bold).SetBackgroundColor(colorBlue).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));

						int rowCount = 0;
						foreach (LeaveDetails details in leaveDetails)
						{

							table.StartNewRow();
							table.AddCell(new Paragraph((++rowCount).ToString()).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.TypeOfLeave).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.DayType).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.Date).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.Month).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.Day).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
							table.AddCell(new Paragraph(details.Comment == null ? string.Empty : details.Comment).SetFont(font)
									.SetBackgroundColor(rowColour)
									.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
						}

						document.Add(table);
						document.Close();
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				CommonHelper.NormalCursor();
			}
		}
	}
}
