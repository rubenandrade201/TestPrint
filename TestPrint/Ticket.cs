﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPrint
{
    public class Ticket
    {
        ArrayList headerLines = new ArrayList();
        ArrayList subHeaderLines = new ArrayList();
        ArrayList items = new ArrayList();
        ArrayList totales = new ArrayList();
        ArrayList footerLines = new ArrayList();
        private Image headerImage = null;

        int count = 0;

        int maxChar = 46;
        int maxCharDescription = 26;

        int imageHeight = 0;

        float leftImageMargin = 26;
        float leftMargin = 0;
        float topMargin = 0;

        string fontName = "Arial";
        int fontSize = 9;

        Font printFont = null;
        SolidBrush myBrush = new SolidBrush(Color.Black);

        Graphics gfx = null;

        string line = null;

        public Ticket()
        {

        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }

        public void AddItem(string cantidad, string item, string singlePrice,  string cost)
        {
            OrderItem newItem = new OrderItem('?');
            items.Add(newItem.GenerateItem(cantidad, item, singlePrice, cost));
        }

        public void AddTotal(string name, string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public void AddFooterLine(string line)
        {
            footerLines.Add(line);
        }

        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string AlignCenterText(int lenght)
        {
            int dmax = maxChar / 2;
            int dlenght = lenght / 2;
            string espacios = "";
            int spaces = dmax - dlenght;
            for (int x = 0; x< spaces; x++)
            {
                espacios += " ";
            }
            return espacios;
        }

        private string AlignCenterHeaderText(int lenght)
        {
            decimal spaces = maxChar - lenght;
            string espacios = "";
            spaces = Math.Ceiling((spaces/2)) ;
            for (int x = 0; x < spaces; x++)
            {
                espacios += " ";
            }
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        public void PrintTicket(string impresora)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;

            DrawImage();
            DrawHeader();
            DrawSubHeader();
            DrawItems();
            DrawTotales();
            DrawFooter();

            if (headerImage != null)
            {
                HeaderImage.Dispose();
                headerImage.Dispose();
            }
        }

        private float YPosition()
        {
            return topMargin + (count * printFont.GetHeight(gfx) + imageHeight);
        }

        private void DrawImage()
        {
            if (headerImage != null)
            {
                try
                {
                    gfx.DrawImage(headerImage, new Point((int)leftImageMargin, (int)YPosition()));
                    double height = ((double)headerImage.Height / 58) * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
        }

        private void DrawHeader()
        {
            float lineheight14 = printFont.GetHeight(gfx);
            StringFormat formatCenter = new StringFormat(StringFormatFlags.NoClip);
            formatCenter.Alignment = StringAlignment.Center;
            float Offset = 0;
            SizeF layoutSize = new SizeF(70 - Offset * 2, lineheight14);
            RectangleF layout = new RectangleF(new PointF(0, YPosition()), layoutSize);
            foreach (string header in headerLines)
            {

                line = header;
                gfx.DrawString(line, printFont, myBrush, layout, formatCenter);
                count++;
                if (count == 0)
                {
                    Offset += 5 + printFont.GetHeight(gfx);
                }
                else
                {
                    Offset += count * printFont.GetHeight(gfx);
                }
                layout = new RectangleF(new PointF(0, YPosition()), layoutSize);
            }
            DrawEspacio();
        }

        private void DrawSubHeader()
        {
            foreach (string subHeader in subHeaderLines)
            {
                if (subHeader.Length > maxChar)
                {
                    int currentChar = 0;
                    int subHeaderLenght = subHeader.Length;

                    while (subHeaderLenght > maxChar)
                    {
                        line = subHeader;
                        gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        subHeaderLenght -= maxChar;
                    }
                    line = subHeader;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = subHeader;

                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;

                    line = DottedLine();

                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            DrawEspacio();
        }

        private void DrawItems()
        {
            OrderItem ordIt = new OrderItem('?');
            gfx.DrawString("QTY DESCRIPCION                      UND       VALOR  ", printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
            DrawEspacio();

            foreach (string item in items)
            {
                line = ordIt.GetItemCantidad(item);

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                line = ordIt.GetItemPrice(item);
                line = AlignRightText(line.Length) + "             " + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                line = ordIt.GetItemCost(item);
                line = AlignRightText(line.Length) + "                           " + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                string name = ordIt.GetItemName(item);

                leftMargin = 0;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemName(item);
                        var subString = line.Substring(currentChar, maxCharDescription);
                        gfx.DrawString("   " + subString, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }

                    line = ordIt.GetItemName(item);
                    var subString2 = line.Substring(currentChar, line.Length - currentChar);
                    gfx.DrawString("   " + subString2, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    gfx.DrawString("   " + ordIt.GetItemName(item), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
            }

            leftMargin = 0;
            DrawEspacio();
            line = DottedLine();

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
            DrawEspacio();
        }

        private void DrawTotales()
        {
            OrderTotal ordTot = new OrderTotal('?');

            foreach (string total in totales)
            {
                line = ordTot.GetTotalCantidad(total);
                line = AlignRightText(line.Length) + "                         " + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                leftMargin = 0;

                line = "                                  " + ordTot.GetTotalName(total);
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                count++;
            }
            leftMargin = 0;
            DrawEspacio();
            DrawEspacio();
        }

        private void DrawFooter()
        {
            float lineheight14 = printFont.GetHeight(gfx);
            StringFormat formatCenter = new StringFormat(StringFormatFlags.NoClip);
            formatCenter.Alignment = StringAlignment.Center;
            float Offset = 0;
            SizeF layoutSize = new SizeF(70 - Offset * 2, lineheight14);
            RectangleF layout = new RectangleF(new PointF(0, 4 + YPosition()), layoutSize);

            foreach (string footer in footerLines)
            {
                line = footer;
                gfx.DrawString(line, printFont, myBrush, layout, formatCenter);
                if(count == 0)
                {
                    Offset += 4 + printFont.GetHeight(gfx);
                }
                else
                {
                    Offset += 4 + printFont.GetHeight(gfx) + YPosition();
                }
                layout = new RectangleF(new PointF(0, 4 + Offset), layoutSize);

                count++;

            }
            leftMargin = 0;
            DrawEspacio();
        }

        private void DrawEspacio()
        {
            line = "";

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
        }
    }

    public class OrderItem
    {
        char[] delimitador = new char[] { '?' };

        public OrderItem(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetItemCantidad(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemName(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemPrice(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[2];
        }

        public string GetItemCost(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[3];
        }

        public string GenerateItem(string cantidad, string itemName, string singlePrince, string cost)
        {
            return cantidad + delimitador[0] + itemName + delimitador[0] + singlePrince + delimitador[0] + cost;
        }
    }

    public class OrderTotal
    {
        char[] delimitador = new char[] { '?' };

        public OrderTotal(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetTotalName(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetTotalCantidad(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[1];
        }

        public string GenerateTotal(string totalName, string price)
        {
            return totalName + delimitador[0] + price;
        }
    }
}