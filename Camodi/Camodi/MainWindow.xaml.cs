using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace Camodi
{
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Registro_tenis.mdb";
            MostrarDatos();
        }

        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Tenis";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtNped.Text = "";
            txtNombre.Text = "";
            cbMarca.SelectedIndex = 0;
            cbTenis.SelectedIndex = 0;
            cbNumero.SelectedIndex = 0;
            cbColor.SelectedIndex = 0;
            txtTelefono.Text = "";
            btnNuevo.Content = "Nuevo";
            txtNped.IsEnabled = true;
        }
        //Editamos alumnos existentes
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtNped.Text = row["Numero_Pedido"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                cbMarca.Text = row["Marca"].ToString();
                cbTenis.Text = row["Tenis"].ToString();
                cbNumero.Text = row["Numero"].ToString();
                cbColor.Text = row["Color"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtNped.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }

        //Eliminamos Alumnos
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Tenis where Numero_Pedido=" + row["Numero_Pedido"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Pedido Eliminado correctamenta.");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }
        //Salimos de la aplicación
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Agregamos nuevos alumnos
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtNped.Text != "")
            {
                if (txtNped.IsEnabled == true)
                {
                    if (cbMarca.Text != "Selecciona Marca")
                    {
                        if (cbMarca.IsEnabled == true)
                        {
                            if (cbNumero.Text != "Selecciona tipo de Tenis")
                            {
                                if (cbNumero.IsEnabled == true)
                                {
                                    if (cbColor.Text != "Numero")
                                    {
                                        if (cbColor.IsEnabled == true)
                                        {
                                            cmd.CommandText = "insert into Tenis(Numero_Pedido,Nombre,Marca,Tenis,Numero,Color,Telefono) " +
                            "Values(" + txtNped.Text + ",'" + txtNombre.Text + "','" + cbMarca.Text + "', '" + cbTenis.Text + "', '" + cbNumero.Text + "', '" + cbColor.Text + "', " + txtTelefono.Text + ")";
                                            cmd.ExecuteNonQuery();
                                            MostrarDatos();
                                            MessageBox.Show("Pedido agregado correctamente.");
                                            LimpiaTodo();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Selecciona el color.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Selecciona la Numero.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Selecciona tipo de Tenis.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selecciona la marca.");
                    }
                }
                else
                {
                    cmd.CommandText = "update Tenis set Nombre='" + txtNombre.Text + "',Marca='" + cbMarca.Text + "',Tenis='" + cbTenis.Text + "',Numero='"
                        + cbNumero.Text + "',Color='" + cbColor.Text + "', Telefono=" + txtTelefono.Text + " where Numero_Pedido=" + txtNped.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del pedido Actualizados.");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el numero de Pedido.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }
    }
}
