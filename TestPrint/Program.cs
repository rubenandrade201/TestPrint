using System;
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
            //ticket.HeaderImage = Image.FromFile(@"C:\Users\Ruben\Desktop\logo.png"); //esta propiedad no es obligatoria            
            ticket.HeaderImage = Image.FromFile(@"C:\Users\gusvo\Documents\repositories\TestPrint\TestPrint\descarga.jpg");
            ticket.AddHeaderLine("SUPERMERCADO EL DORADO");            
            ticket.AddHeaderLine("Afidro Agrupación 2");
            ticket.AddHeaderLine("CRA 90 BIS # 76 - 51 INT 30 LOCAL 102");
            ticket.AddHeaderLine("TEL: 478 01 98");
            ticket.AddHeaderLine("WhatsApp: 322 891 6997");

            //El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
            //de que al final de cada linea agrega una linea punteada "=========="            
            ticket.AddSubHeaderLine("Le atendió: Blanca");
            ticket.AddSubHeaderLine("Fecha: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

            //El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
            //del producto y el tercero es el precio
            ticket.AddItem("2", "gaseosa postobon manzana refresh 375 ml", "50.000", "100.000");
            ticket.AddItem("5", "quesadillo relleno arequipe y relleno bocadillo 70g", "300", "1.500");
            ticket.AddItem("3", "galleta quaker con avena relleno de cremoso con yogurt sabor a fresa 36g", "7.000", "21.000");

            //El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
            ticket.AddTotal("TOTAL", "$ 122.500");
            ticket.AddTotal("", ""); //Ponemos un total en blanco que sirve de espacio

            //El metodo AddFooterLine funciona igual que la cabecera
            ticket.AddFooterLine("GRACIAS POR SU VISITA");
            ticket.AddFooterLine("Haga sus pedidos a domicilio GRATIS!!");

            //Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
            //parametro de tipo string que debe de ser el nombre de la impresora.
            //ticket.PrintTicket("GP-80160N(Cut) Series");
            ticket.PrintTicket("ONE 500");

        }
    }
}

