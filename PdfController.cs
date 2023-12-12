using iText.Forms;
using iText.Kernel.Pdf;

namespace PeopleCalcBot
{
    public class PdfController
    {
        public PdfController() { }

        private async Task<string?> FilterPdfFieldsAsync(string? meetingType, string? week)
        {
            if (meetingType == null || week == null)
            {
                return null;
            }

            string fieldNameSuffix = "";

            switch (meetingType)
            {
                case "LAM - Life and Ministry":
                    fieldNameSuffix = "LAM";
                    break;
                case "WKD - Weekend Meeting":
                    fieldNameSuffix = "WKD";
                    break;
                default:
                    return null;
            }

            switch (week)
            {
                case "First":
                    return $"FirstWeek{fieldNameSuffix}";
                case "Second":
                    return $"SecondWeek{fieldNameSuffix}";
                case "Third":
                    return $"ThirdWeek{fieldNameSuffix}";
                case "Fourth":
                    return $"FourthWeek{fieldNameSuffix}";
                case "Fifth":
                    return $"FifthWeek{fieldNameSuffix}";
                default:
                    return null; 
            }
        }

        public async Task<byte[]> FillPdfFieldsAsync(string templatePath, string? meetingType, string? week, string amount, BotDBContext.Congregations? congregation, BotDBContext.Months? month)
        {
            byte[] filledPdf;
            if (meetingType == null || week == null || congregation == null || month == null)
            {
                throw new ArgumentNullException("One or more required parameters are null");
            }

            using (FileStream inputFileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream outputMemoryStream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(inputFileStream);
                    PdfWriter writer = new PdfWriter(outputMemoryStream);
                    PdfDocument pdf = new PdfDocument(reader, writer);

                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
                    var field = await FilterPdfFieldsAsync(meetingType, week);
                    form.GetField(field
                ).SetValue(amount);
                    form.GetField("CongregationName").SetValue(congregation?.Name);
                    form.GetField("Month").SetValue(month?.Name);
                  
                   

                    pdf.Close();
                    filledPdf = outputMemoryStream.ToArray();


                }
            }
            return filledPdf;
        }
        public async Task<byte[]> ModifyPDFAsync(byte[]? documentBytes, string? meetingType, string? week, string amount, BotDBContext.Congregations? congregation, BotDBContext.Months? month)
        {
            byte[] filledPdf;

            using (MemoryStream inputMemoryStream = new MemoryStream(documentBytes))
            {
                using (MemoryStream outputMemoryStream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(inputMemoryStream);
                    PdfWriter writer = new PdfWriter(outputMemoryStream);
                    PdfDocument pdf = new PdfDocument(reader, writer);

                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

                    form.GetField(
                        await FilterPdfFieldsAsync(meetingType, week))?.SetValue(amount);
                    form.GetField("CongregationName").SetValue(congregation?.Name);
                    form.GetField("Month").SetValue(month?.Name);

                    pdf.Close();
                    filledPdf = outputMemoryStream.ToArray();
                }
            }
            return filledPdf;

        }
    }
}
