﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Drawing.Printing;
using System.Reflection;
using System.IO;

namespace TestPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            Ticket ticket = new Ticket();
            //PARA RUTA RELATIVA
            string strURL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ticket.HeaderImage = Image.FromFile(@"C:\Users\gusvo\Documents\repositories\TestPrint\TestPrint\descarga.jpg"); //esta propiedad no es obligatoria

            ticket.AddHeaderLine("SUPERMERCADO EL DORADO");
            ticket.AddHeaderLine("NIT. 860.076.918-1");
            ticket.AddHeaderLine("CRA 90 BIS # 76 - 51 AG2 LOCAL");
            ticket.AddHeaderLine("TELEFONO: 478 01 98");
            ticket.AddHeaderLine("REGIMEN ");

            //El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
            //de que al final de cada linea agrega una linea punteada "=========="
            ticket.AddSubHeaderLine("Caja # 1 - Factura de Venta No. # 0001");
            ticket.AddSubHeaderLine("Le atendió: Blanca");
            ticket.AddSubHeaderLine("Fecha: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

            //El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
            //del producto y el tercero es el precio
            ticket.AddItem("1", "Leche Alqueria", "60.000");
            ticket.AddItem("2", "Huevos", "600");

            //El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
            //ticket.AddTotal("SUBTOTAL", "2600");
            //ticket.AddTotal("IVA", "0");
            ticket.AddTotal("TOTAL", "120.000");
            ticket.AddTotal("", ""); //Ponemos un total en blanco que sirve de espacio
            //ticket.AddTotal("EFECTIVO", "5.000");
            //ticket.AddTotal("CAMBIO", "2.400");
            ticket.AddTotal("", "");// Ponemos un total en blanco que sirve de espacio
            //ticket.AddTotal("USTED AHORRO", "0.00");

            //El metodo AddFooterLine funciona igual que la cabecera
            ticket.AddFooterLine("Gracias, Thanks, Danke, Grazie,");
            ticket.AddFooterLine("Merci, Obrigado!");
            ticket.AddFooterLine("GRACIAS POR TU VISITA");

            //Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
            //parametro de tipo string que debe de ser el nombre de la impresora.
            ticket.PrintTicket("ONE 500");
            //string s = "Esto es una prueba linea 1";          
            //s += "\n";
            //s += "Linea 2";


            //PrintDocument p = new PrintDocument();
            //p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            //{
            //    e1.Graphics.DrawString(s, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            //};
        //    try
        //    {
        //        //p.Print();
        //        PrintDocument pd = new PrintDocument();
        //        //pd.DefaultPageSettings.PrinterSettings.PrinterName = "Printer Name";
        //        //pd.DefaultPageSettings.Landscape = true; //or false!
        //        pd.PrintPage += (sender, args2) =>
        //        {
        //            Image i = Image.FromFile(@"C:\Users\gusvo\Documents\repositories\TestPrint\TestPrint\descarga.jpg");
        //            Rectangle m = args2.MarginBounds;

        //            if ((double)i.Width / (double)i.Height > (double)m.Width / (double)m.Height) // image is wider
        //            {
        //                m.Height = (int)((double)i.Height / (double)i.Width * (double)m.Width);
        //            }
        //            else
        //            {
        //                m.Width = (int)((double)i.Width / (double)i.Height * (double)m.Height);
        //            }
        //            args2.Graphics.DrawImage(i, m);

        //            SolidBrush Brush = new SolidBrush(Color.Black);

        //            //gets the text from the textbox
        //            string printText = "Alejandro Sierra";
        //            StringFormat sf = new StringFormat();
        //            sf.LineAlignment = StringAlignment.Center;
        //            sf.Alignment = StringAlignment.Center;
        //            printText += Environment.NewLine + System.DateTime.Today.ToShortDateString() + Environment.NewLine + Environment.NewLine
        //            + "-------------------------------------------------------------------------------------------------"
        //            + Environment.NewLine;

        //            //Makes the file to print and sets the look of it
        //            args2.Graphics.DrawString(printText, new Font("Arial", 9), Brush, 10, 50);

        //        };

        //        pd.Print();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception Occured While Printing", ex);
        //    }
        }
        }
}

/*}
 
     using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;
using Microsoft.VisualBasic;

namespace Reciept_Print_Test
{
    public partial class Form1 : Form
    {

        

        private void btnPrintReciept_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            printDialog.Document = printDocument; //add the document to the dialog box...        

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            DialogResult result = printDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
            printDocument.Print();

            } 
        }

        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            int total = 0;
            float cash = float.Parse(txtCash.Text.Substring(1,5));
            float change = 0.00f;
           
            //this prints the reciept

            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 12); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString(" Wangoland Coffee Shop", new Font("Courier New", 18), new SolidBrush(Color.Black), startX, startY);
            string top = "Item Name".PadRight(30) + "Price";
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            float totalprice = 0.00f;

            foreach (string item in listBox1.Items)
            {
                //create the string to print on the reciept
                string productDescription = item;
                string productTotal = item.Substring(item.Length - 6, 6);
                float productPrice = float.Parse(item.Substring(item.Length - 5, 5));

                //MessageBox.Show(item.Substring(item.Length - 5, 5) + "PROD TOTAL: " + productTotal);
                

                totalprice += productPrice;

                if (productDescription.Contains("  -"))
                {
                    string productLine = productDescription.Substring(0,24);

                    graphic.DrawString(productLine, new Font("Courier New", 12, FontStyle.Italic), new SolidBrush(Color.Red), startX, startY + offset);

                    offset = offset + (int)fontHeight + 5; //make the spacing consistent
                }
                else
                {
                    string productLine = productDescription;

                    graphic.DrawString(productLine, font, new SolidBrush(Color.Black), startX, startY + offset);

                    offset = offset + (int)fontHeight + 5; //make the spacing consistent
                }

            }

            change = (cash - totalprice);

            //when we have drawn all of the items add the total

            offset = offset + 20; //make some room so that the total stands out.

            graphic.DrawString("Total to pay ".PadRight(30) + String.Format("{0:c}", totalprice), new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("CASH ".PadRight(30) + String.Format("{0:c}", cash), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("CHANGE ".PadRight(30) + String.Format("{0:c}", change), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("     Thank-you for your custom,", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("       please come back soon!", font, new SolidBrush(Color.Black), startX, startY + offset);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}﻿
     
     */
