using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Versio1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }
        int i = 0;
        int j = 0;
        private void enviar_Click(object sender, EventArgs e)
        {
            if (i == 1)
            {
                if (ganador.Checked)
                {
                    // Quiere la longitud del nombre
                    string mensaje = "1/";
                    // Enviamos al servidor el nombre
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show(mensaje);
                }
                if (Puntos.Checked)
                {
                    if ((string.IsNullOrEmpty(Nombre.Text)) && (string.IsNullOrEmpty(Data.Text)))
                    {
                        MessageBox.Show("Introduzca su nombre y data porfavor.");
                    }
                    else
                    {
                        // Quiere la longitud del nombre
                        string mensaje = "2/" + Nombre.Text + Data.Text;
                        // Enviamos al servidor el nombre
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show(mensaje);
                    }
                }
                if (nganador.Checked)
                {
                    if (string.IsNullOrEmpty(Nombre.Text))
                    {
                        MessageBox.Show("Introduzca su nombre porfavor.");
                    }
                    else
                    {
                        // Quiere la longitud del nombre
                        string mensaje = "3/" + Nombre.Text;
                        // Enviamos al servidor el nombre
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show(mensaje);
                    }
                }
            }
            else
                MessageBox.Show("No has iniciado sesión aún.");
        }

        private void salir_Click(object sender, EventArgs e)
        {
            if (j == 1)
            {
                //Enviamos mensajje de desconexión
                string mensaje = "0/";
                byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg2);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                Close();
            }
            else
                Close();
        }

        private void Registarse_Click(object sender, EventArgs e)
        {
            //IPEndPoint con el ip y el puerto del servidor al que queremos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9700);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Intentamos conectar el socket
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");
            }

            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
            if ((string.IsNullOrEmpty(usuario.Text)) && (string.IsNullOrEmpty(contraseña.Text)))
            {
                MessageBox.Show("Introduzca su usuario y contraseña porfavor.");
            }
            else
            {
                // Quiere la longitud del nombre
                string mensaje = "4/" + usuario.Text + "/" + contraseña.Text;
                // Enviamos al servidor el nombre
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                int i = Convert.ToInt32(mensaje);
                if (i == 1)
                {
                    MessageBox.Show("El usuario ya existe.");
                }
            }
        }

        private void Logearse_Click(object sender, EventArgs e)
        {
            //IPEndPoint con el ip y el puerto del servidor al que queremos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9110);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Intentamos conectar el socket
                server.Connect(ipep);
                if ((string.IsNullOrEmpty(usuario.Text)) && (string.IsNullOrEmpty(contraseña.Text)))
                {
                    MessageBox.Show("Introduzca su usuario y contraseña porfavor.");
                }
                else
                {
                    // Quiere la longitud del nombre
                    string mensaje = "5/" + usuario.Text + "/" + contraseña.Text;
                    // Enviamos al servidor el nombre
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    if (contraseña.Text == mensaje)
                    {

                        this.BackColor = Color.Green;
                        MessageBox.Show("Bienvindo usuario.");
                        i = 1;
                        j = 1;
                    }
                    else
                    {
                        //Enviamos mensaje de desconexión
                        string mensaj = "0/";
                        byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaj);
                        server.Send(msg3);

                        // Nos desconectamos
                        server.Shutdown(SocketShutdown.Both);
                        server.Close();
                        MessageBox.Show("El usuario o la contraseña son incorrectas.");
                    }
                }
            }

            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
           
        }
    }
}
