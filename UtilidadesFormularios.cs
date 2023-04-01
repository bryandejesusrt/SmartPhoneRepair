using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartPhone7
{
    public static class UtilidadesFormularios
    {
        public static Form CrearFormularioDeFondo(Form formulario)
        {
            Form formularioDeFondo = new Form();
            formularioDeFondo.FormBorderStyle = FormBorderStyle.None;
            formularioDeFondo.Opacity = .50d;
            formularioDeFondo.BackColor = Color.Black;
            formularioDeFondo.Size = formulario.Size;
            formularioDeFondo.ShowInTaskbar = false;
            formularioDeFondo.Show(formulario);
            formularioDeFondo.ShowIcon = false;
            formularioDeFondo.ShowInTaskbar = false;

            formularioDeFondo.ClientSize = formulario.ClientSize;
            formularioDeFondo.Location = formulario.PointToScreen(Point.Empty);

            return formularioDeFondo;
        }

        public static void CentrarFormularioModal(Form formularioModal, Form formularioPadre)
        {
            int centerX = formularioPadre.Location.X + (formularioPadre.Width - formularioModal.Width) / 2;
            int centerY = formularioPadre.Location.Y + (formularioPadre.Height - formularioModal.Height) / 2;
            formularioModal.Location = new Point(centerX, centerY);

        }
        public static void ExportToPDF(DataGridView dgv, string title)
        {
            // Configurar la página y el documento
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(pdfDoc, new FileStream("DataGridView.pdf", FileMode.Create));

            // Abrir el documento
            pdfDoc.Open();

            // Agregar la imagen del logo
            // Obtener el administrador de recursos para la carpeta SmartPhone7
            ResourceManager resourceManager = new ResourceManager("SmartPhone7.Resources", Assembly.GetExecutingAssembly());

            // Obtener la imagen con el nombre "iphone_x_60px" y crear la instancia de la imagen

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Properties.Resources.iphone_x_60px, System.Drawing.Imaging.ImageFormat.Png);
            logo.ScaleAbsolute(60f, 60f);
            logo.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(logo);

            // Agregar el nombre del sistema
            Paragraph systemName = new Paragraph("SmartPhone7", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY));
            systemName.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(systemName);

            // Agregar una línea en blanco
            pdfDoc.Add(Chunk.NEWLINE);
            pdfDoc.Add(Chunk.NEWLINE);

            // Crear el título y agregarlo al documento
            Paragraph para = new Paragraph(title, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 18, iTextSharp.text.Font.BOLD));
            para.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(para);

            // Agregar una línea en blanco después del título
            pdfDoc.Add(Chunk.NEWLINE);

            // Crear la tabla y agregar las filas y columnas
            PdfPTable table = new PdfPTable(dgv.Columns.Count);

            // Agregar las celdas de encabezado para cada columna
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                table.AddCell(cell);
            }

            // Agregar las filas de datos a la tabla
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Phrase(cell.Value?.ToString() ?? "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12)));
                }
            }

            // Agregar la tabla al documento
            pdfDoc.Add(table);

            // Cerrar el documento
            pdfDoc.Close();

            // Abrir el archivo PDF recién creado
            Process.Start("DataGridView.pdf");
        }
    }
}
