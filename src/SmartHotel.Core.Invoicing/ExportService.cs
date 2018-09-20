using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SmartHotel.Core.Invoicing
{
    public class ExportService
    {
        public void ExportInvoice(Invoice invoice, string path)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + ".txt";
           
            var finalPath = Path.Combine(path, "Invoices");

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            finalPath = Path.Combine(finalPath, fileName);

            var info =
                "###########################################\n" +
                "\n" +
                $"{invoice.HotelName}\n" +
                $"Invoice: {invoice.InvoiceNumber}\n" +
                "\n" +
                "###########################################\n" +
                "\n" +
                $"{invoice.Name}\n" +
                $"{string.Concat(invoice.Items.Select(i => i + "\n").ToArray())}" +
                "\n" +
                $"Total: {invoice.Total.ToString("C")}\n" +
                "\n" +
                "###########################################\n";

            File.WriteAllText(finalPath, info);
        }
    }
}
