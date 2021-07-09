using DGVPrinterHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private Connsql connsql;
        
        public Form1()
        {
            InitializeComponent();
            connsql = new Connsql();
        }
       
        private DataSet ds = new DataSet();
        string btnop = "editclient";
        string search = "";
        private void Form1_Load(object sender, EventArgs e)
        {                       
            btnedit.Visible = false; btneliminarcte.Visible = false; btnconfirm.Visible = false;  lblwpw.Visible = false;
            btnnuevalocal.Visible = false; btnaddclient.Visible = false; lblwmail.Visible = false;
            btnusers1.Visible = false; btncat.Visible = false; btnaduser.Visible = false;
            btnregauditoria.Visible = false; btnconfirm.Enabled = false;
            btnaddcat.Visible = false; btnimpresoras.Visible = false; 

            gpbdatoscliente.Visible = false; gpblocalidad.Visible = false; psearch.Visible = false;
            gpbdatosusuario.Visible = false; gpbcat.Visible = false; panelprinters.Visible = false;

            paneldgv.Visible = false; paneldgv.Location = new Point(19, 66);
            panel1.Visible = false;
            cmbprinters.Visible = false; // this.Size = (362, 400); 
            cmbprin();
        }
        private void cmbPcias()
        {
            cmbp.DataSource = connsql.PciasList();
            cmbp.DisplayMember = "Provincia";
            cmbp.ValueMember = "NroProvincia";
        }
        private void btnclients_Click(object sender, EventArgs e)
        {
            paneldgv.Visible = true; btnaduser.Visible = false;

            gpbdatoscliente.Visible = true; psearch.Visible = true; gpblocalidad.Visible = false;
            gpbdatoscliente.Location = new Point(838, 72);

            btnedit.Visible = true; btneliminarcte.Visible = true; btnconfirm.Visible = true;
            btnnuevalocal.Visible = false;
            btnconfirm.Location = new Point(833, 330);

            btnaddclient.Visible = true; btnaddclient.Location = new Point (656, 17);

            dataGridView1.DataSource = connsql.ListarClientes();

            search = "searchclient";
        }
        private void btnaddclient_Click(object sender, EventArgs e)
        {
            ClearForm();

            gpbdatoscliente.Location = new Point (838, 101);
            gpbdatoscliente.Visible = true; gpblocalidad.Visible = false;
            btnconfirm.Location = new Point(833, 370);
            txtname.Enabled = true; txtapellido.Enabled = true; txtpcia.Enabled = false;
            txtlocal.Enabled = false; txtcp.Enabled = true; btnconfirm.Enabled = true;

            btnop = "addclient"; 
        }
        private void btnsearch_Click(object sender, EventArgs e)
        {
            if (search == "searchclient")
            {
                dataGridView1.DataSource = connsql.ListarClientes(txtsearch.Text);
            }
            else
            {
                if (search == "searchlocal")
                {
                    dataGridView1.DataSource = connsql.ListarLocalidades(txtsearch.Text);
                }
                else
                {
                    if (ActiveUser().ToArray()[4] == "AUDITOR")
                    {
                        dataGridView1.DataSource = connsql.Historico(txtsearch.Text);
                    }
                    else
                    {
                        if (ActiveUser().ToArray()[4] == "SUPERVISOR")
                        {
                            dataGridView1.DataSource = connsql.UsersList(txtsearch.Text);
                        }
                    }
                }
            }
            txtsearch.Text = string.Empty;
        }
        private void btneliminarcte_Click(object sender, EventArgs e)
        {
            btnconfirm.Enabled = true;
            if (gpbdatoscliente.Visible)
            {
                btnop = "deleteclient";
                DateTime dt;

                dt = DateTime.Parse(dataGridView1.CurrentRow.Cells["Fecha_Alta"].Value.ToString());
                txtfechaalta.Text = dt.ToString("dd/MM/yyyy");
                cmbp.Text = dataGridView1.CurrentRow.Cells["Provincia"].Value.ToString(); cmbp.Enabled = false;
                txtcp.Text = dataGridView1.CurrentRow.Cells["CP"].Value.ToString(); txtcp.Enabled = false;
                txtlocal.Text = dataGridView1.CurrentRow.Cells["Localidad"].Value.ToString(); txtlocal.Enabled = false;
                lblIDClient.Text = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                txtname.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString(); txtname.Enabled = false;
                txtapellido.Text = dataGridView1.CurrentRow.Cells["Apellido"].Value.ToString(); txtapellido.Enabled = false;
            }
            else
            {
                if (gpblocalidad.Visible)
                {
                    btnop = "deletelocal";
                    txtcodlocal.Text = dataGridView1.CurrentRow.Cells["CP"].Value.ToString(); txtcodlocal.Enabled = false;
                    txtdesclocal.Text = dataGridView1.CurrentRow.Cells["Localidad"].Value.ToString(); txtdesclocal.Enabled = false;
                }
                else
                {
                    if (gpbcat.Visible)
                    {
                        btnop = "deletecat";
                        txtcatid.Text = dataGridView1.CurrentRow.Cells["Categoria"].Value.ToString(); txtcatid.Enabled = false;
                        txtcatdesc.Text = dataGridView1.CurrentRow.Cells["Titulo"].Value.ToString(); txtcatdesc.Enabled = false;
                    }
                    else
                    {
                        if (gpbdatosusuario.Visible)
                        {
                            btnop = "deleteuser";
                            lblUserId.Text = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                            txtusername.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString(); txtusername.Enabled = false;
                            txtusersurname.Text = dataGridView1.CurrentRow.Cells["Apellido"].Value.ToString(); txtusersurname.Enabled = false;
                            txtmail.Text = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); txtmail.Enabled = false;
                            txtuserpw.Text = dataGridView1.CurrentRow.Cells["Contraseña"].Value.ToString(); txtuserpw.Enabled = false;
                            txtidcat.Text = dataGridView1.CurrentRow.Cells["Categoria"].Value.ToString(); txtidcat.Enabled = false;
                            cmbcat.Text = dataGridView1.CurrentRow.Cells["Titulo"].Value.ToString(); cmbcat.Enabled = false;
                        }
                    }
                }
            }
        }
        private void btnedit_Click(object sender, EventArgs e)
        {
            btnconfirm.Enabled = true;
            if (gpbdatoscliente.Visible)
            {
                DateTime dt; btnop = "editclient";

                dt = DateTime.Parse(dataGridView1.CurrentRow.Cells["Fecha_Alta"].Value.ToString());
                txtname.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString(); txtname.Enabled = true;
                txtapellido.Text = dataGridView1.CurrentRow.Cells["Apellido"].Value.ToString(); txtapellido.Enabled = true;
                txtfechaalta.Text = dt.ToString("dd/MM/yyyy");
                txtpcia.Text = dataGridView1.CurrentRow.Cells["Provincia"].Value.ToString(); txtpcia.Enabled = false;
                txtcp.Text = dataGridView1.CurrentRow.Cells["CP"].Value.ToString(); txtcp.Enabled = true;
                txtlocal.Text = dataGridView1.CurrentRow.Cells["Localidad"].Value.ToString(); txtlocal.Enabled = false;
                lblIDClient.Text = (dataGridView1.CurrentRow.Cells["ID"].Value.ToString()); 
                
            }
            else
            {
                if (gpblocalidad.Visible)
                {
                    btnop = "editlocal";
                    txtcodlocal.Text = dataGridView1.CurrentRow.Cells["CP"].Value.ToString();
                    txtdesclocal.Text = dataGridView1.CurrentRow.Cells["Localidad"].Value.ToString(); txtdesclocal.Enabled = true;
                    txtcodlocal.Enabled = false;
                    cmbPcias();
                }
                else
                {
                    if (gpbcat.Visible)
                    {
                        btnop = "editcat";
                        txtcatid.Text = dataGridView1.CurrentRow.Cells["Categoria"].Value.ToString(); txtidcat.Enabled = true;
                        txtcatdesc.Text = dataGridView1.CurrentRow.Cells["Titulo"].Value.ToString(); txtcatdesc.Enabled = true;
                    }
                    else
                    {
                        if (gpbdatosusuario.Visible)
                        {
                            btnop = "edituser";
                            lblUserId.Text = dataGridView1.CurrentRow.Cells["ID"].Value.ToString(); 
                            txtusername.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString(); txtusername.Enabled = true;
                            txtusersurname.Text = dataGridView1.CurrentRow.Cells["Apellido"].Value.ToString(); txtusersurname.Enabled = true;
                            txtmail.Text = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); txtmail.Enabled = true;
                            txtuserpw.Text = dataGridView1.CurrentRow.Cells["Contraseña"].Value.ToString(); txtuserpw.Enabled = true;
                            txtidcat.Text = dataGridView1.CurrentRow.Cells["Categoria"].Value.ToString(); txtidcat.Enabled = false;
                            cmbcat.Text = dataGridView1.CurrentRow.Cells["Titulo"].Value.ToString();  cmbcat.Enabled = true;
                        }
                    }
                }
            }
        }
        private void ClearForm()
        {
            txtname.Text = string.Empty; txtlocal.Text = string.Empty;
            txtapellido.Text = string.Empty;
            txtpcia.Text = string.Empty; txtcodlocal.Text = string.Empty; lblUserId.Text = string.Empty;
            txtdesclocal.Text = string.Empty; txtcp.Text = string.Empty; lblIDClient.Text = string.Empty;

            txtcatid.Text = string.Empty; txtcatdesc.Text = string.Empty;

            txtusername.Text = string.Empty; txtusersurname.Text = string.Empty;
            txtmail.Text = string.Empty; txtuserpw.Text = string.Empty; txtidcat.Text = string.Empty;
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string c; string d;
            //MessageBox.Show(dataGridView2.Columns["Local_Clave"].HeaderText);
            if (dataGridView1.Columns["CP"].HeaderText == "CP")
            {
                c = dataGridView1.CurrentRow.Cells["CP"].Value.ToString();
                d = dataGridView1.CurrentRow.Cells["Localidad"].Value.ToString();
                txtcp.Text = c; txtlocal.Text = d;
            }
        }
        private void btnnuevalocal_Click(object sender, EventArgs e)
        {          
            cmbPcias();
            btnop = "addlocal"; btnconfirm.Enabled = true;
            txtcodlocal.Enabled = true; txtdesclocal.Enabled = true; 
        }
        private void btnlocalidades_Click(object sender, EventArgs e)
        {
            btnaddclient.Visible = false; btnnuevalocal.Visible = true;
            btnnuevalocal.Location = new Point(656, 17);
            btnconfirm.Location = new Point (833, 291);

            btneliminarcte.Visible = true; btnedit.Visible = true; btnconfirm.Visible = true;
            btnaduser.Visible = false;

            psearch.Visible = true; gpblocalidad.Visible = true; gpbdatoscliente.Visible = false;
            gpblocalidad.Location = new Point (838, 128);

            paneldgv.Visible = true;
            dataGridView1.DataSource = connsql.ListarLocalidades();

            cmbPcias();

            search = "searchlocal";
        }
        private void btnconfirm_Click(object sender, EventArgs e)
        {
            if (btnop == "addclient")
            {
                int cod = Convert.ToInt32(txtcp.Text);
                if (connsql.SearchCP(cod))
                {
                    Cliente cliente = new Cliente();

                    cliente.Nombre = txtname.Text;
                    cliente.Apellido = txtapellido.Text;
                    cliente.FechaAlta = DateTime.Parse(txtfechaalta.Text);
                    //MessageBox.Show((cod).ToString());
                    cliente.CP = cod;
                    //MessageBox.Show(form2.ActiveUser().ToArray()[3]);
                    cliente.Email = ActiveUser().ToArray()[3];

                    connsql.InsertCliente(cliente);
                    dataGridView1.DataSource = connsql.ListarClientes();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Ingrese un codigo postal valido");
                    txtcp.Text = string.Empty;
                }
            }
            else
            {
                if (btnop == "deleteclient")
                {
                    connsql.DeleteCliente(Convert.ToInt32(lblIDClient.Text));
                    connsql.UpdateHistorico(connsql.GetIdAuditoria(), ActiveUser().ToArray()[3]);
                    dataGridView1.DataSource = connsql.ListarClientes();
                    txtname.Enabled = true; txtapellido.Enabled = true;
                    txtpcia.Enabled = true;
                    ClearForm();
                }
                else
                {
                    if (btnop == "editclient")
                    {
                        int cod = Convert.ToInt32(txtcp.Text);

                        if (connsql.SearchCP(cod))
                        {
                            Cliente cliente = new Cliente();

                            cliente.Nombre = txtname.Text;
                            cliente.Apellido = txtapellido.Text;
                            cliente.FechaAlta = DateTime.Parse(txtfechaalta.Text);
                            //MessageBox.Show((cod).ToString());
                            cliente.CP = cod;
                            cliente.Email = ActiveUser().ToArray()[3];

                            connsql.UpdateCliente(cliente, (Convert.ToInt32(lblIDClient.Text)));
                            dataGridView1.DataSource = connsql.ListarClientes();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Ingrese un codigo postal valido");
                            dataGridView1.Visible = true;
                            dataGridView1.DataSource = connsql.ListarLocalidades();
                            txtcp.Text = string.Empty;
                        }
                    }
                    else
                    {
                        if (btnop == "addlocal")
                        {
                            int cod = (Convert.ToInt32(txtcodlocal.Text));
                            //MessageBox.Show(cod.ToString());
                            if (!(connsql.SearchCP(cod)))
                            {
                                Localidad localidad = new Localidad();

                                localidad.LocalId = Convert.ToInt32(txtcodlocal.Text);
                                localidad.descLocal = txtdesclocal.Text;
                                localidad.Email = ActiveUser().ToArray()[3];
                                //MessageBox.Show(ActiveUser().ToArray()[3]);
                                string pcia = cmbp.Text;
                                connsql.getNroPcia(pcia);

                                localidad.Pcia = connsql.getNroPcia(pcia);

                                connsql.InsertLocalidad(localidad);
                                dataGridView1.DataSource = connsql.ListarLocalidades();
                                ClearForm();
                            }
                            else
                            {
                                MessageBox.Show("El Codigo Postal ingresado ya existe");
                                txtcodlocal.Text = string.Empty;
                            }
                        }
                        else
                        {
                            if (btnop == "deletelocal")
                            {
                                int cp = Convert.ToInt32(txtcodlocal.Text);
                                if (!(connsql.BuscarCPClientes(cp)))
                                {
                                    connsql.DeleteLocalidad(Convert.ToInt32(txtcodlocal.Text));
                                    connsql.UpdateHistorico(connsql.GetIdAuditoria(), ActiveUser().ToArray()[3]);
                                    dataGridView1.DataSource = connsql.ListarLocalidades();
                                    ClearForm();
                                }
                                else
                                {
                                    MessageBox.Show("No se puede eliminar el Codigo Postal seleccionado ya que esta siendo usado para un cliente");
                                    ClearForm();
                                }
                            }
                            else
                            {
                                if (btnop == "editlocal")
                                {
                                    Localidad localidad = new Localidad();

                                    localidad.LocalId = Convert.ToInt32(txtcodlocal.Text);
                                    localidad.descLocal = txtdesclocal.Text;
                                    localidad.Email = ActiveUser().ToArray()[3];

                                    string pcia = cmbp.Text; 
                                    //MessageBox.Show(pcia);
                                    int n = connsql.getNroPcia(pcia); //MessageBox.Show(n.ToString());
                                   
                                    localidad.Pcia = n;                                                                      
                                    connsql.UpdateLocalidad(localidad, (Convert.ToInt32(txtcodlocal.Text)));
                                    dataGridView1.DataSource = connsql.ListarLocalidades();
                                    ClearForm();
                                }
                                else
                                {
                                    if (btnop == "addcat")
                                    { //si encuentra la cat quiere decir q existe, por lo tanto no debe insertarlo
                                        if (!(connsql.SearchCat(Convert.ToInt32(txtcatid.Text))))
                                        {
                                            Categoria categoria = new Categoria();

                                            categoria.catId = Convert.ToInt32(txtcatid.Text);
                                            categoria.desccat = txtcatdesc.Text;

                                            connsql.InsertCategory(categoria);
                                            dataGridView1.DataSource = connsql.CategoriesList();
                                            ClearForm();
                                        }
                                        else
                                        {
                                            MessageBox.Show("El ID de Categoria ya existe. Por favor, ingrese uno distinto");
                                        }
                                    }
                                    else
                                    {
                                        if (btnop == "editcat")
                                        {
                                            int c = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Categoria"].Value.ToString());
                                            if (!(connsql.BuscarCatUsuario(Convert.ToInt32(c))))
                                            {                                                
                                                Categoria categoria = new Categoria();                                                
                                                categoria.catId = Convert.ToInt32(txtcatid.Text);
                                                categoria.desccat = txtcatdesc.Text;

                                                connsql.UpdateCategory(categoria, c);
                                                dataGridView1.DataSource = connsql.CategoriesList();
                                                ClearForm();
                                            }
                                            else
                                            {
                                                MessageBox.Show("No se puede editar" +
                                                    " la categoria ya que esta asociada con un usuario");
                                                ClearForm();
                                            }
                                        }
                                        else
                                        {
                                            if (btnop == "deletecat")
                                            {
                                                if (!(connsql.BuscarCatUsuario(Convert.ToInt32(txtcatid.Text))))
                                                {
                                                    connsql.DeleteCategory(Convert.ToInt32(txtcatid.Text));
                                                    dataGridView1.DataSource = connsql.CategoriesList();
                                                    ClearForm();
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No se puede eliminar" +
                                                        "la categoria ya que esta asociada con un usuario");
                                                    ClearForm();
                                                }
                                            }
                                            else
                                            {
                                                if (btnop == "adduser")
                                                {
                                                    string m = txtmail.Text;
                                                    if (!(connsql.SearchMail(m)))
                                                    {
                                                        Usuario usuario = new Usuario();

                                                        usuario.Nombre = txtusername.Text;
                                                        usuario.Apellido = txtusersurname.Text;
                                                        usuario.mail = txtmail.Text;
                                                        usuario.pw = txtuserpw.Text;
                                                        usuario.cat = Convert.ToInt32(txtidcat.Text);

                                                        connsql.InsertUser(usuario);
                                                        dataGridView1.DataSource = connsql.UsersList();
                                                        ClearForm();
                                                    }
                                                    else
                                                    {
                                                        
                                                    }
                                                }
                                                else
                                                {
                                                    if (btnop == "edituser")
                                                    {
                                                        string m = txtmail.Text;
                                                        if (!(connsql.SearchMail(m)))
                                                        {
                                                            Usuario usuario = new Usuario();

                                                            usuario.Nombre = txtusername.Text;
                                                            usuario.Apellido = txtusersurname.Text;
                                                            usuario.cat = Convert.ToInt32(txtidcat.Text);

                                                            usuario.mail = txtmail.Text;
                                                            usuario.pw = txtuserpw.Text;

                                                            connsql.UpdateUser(usuario, Convert.ToInt32(lblUserId.Text));
                                                            dataGridView1.DataSource = connsql.UsersList();
                                                            ClearForm();
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("El mail esta siendo usado por otro usuario, ingrese" +
                                                            " otro", "Error",
                                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            txtmail.Text = string.Empty;
                                                        }                                                          
                                                    }
                                                    else
                                                    {
                                                        if (btnop == "deleteuser")
                                                        {
                                                            connsql.DeleteUser(Convert.ToInt32(lblUserId.Text));
                                                            dataGridView1.DataSource = connsql.UsersList();
                                                            ClearForm();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void txtcodlocal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtcp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                paneldgv.Visible = true;
                dataGridView1.DataSource = connsql.ListarLocalidades();
            }
        }
        private void txtcp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            txtcp.AcceptsReturn = true;
        }
        private void txtcp_TextChanged(object sender, EventArgs e)
        {
            if (!(txtcp.Text == ""))
            {
                int cod = (Convert.ToInt32(txtcp.Text));
                if (txtcp.TextLength == 4)
                {
                    txtlocal.Text = connsql.devLocal(cod);
                    txtpcia.Text = (connsql.devPcia(connsql.nroPcia(cod), cod));
                }
            }
        }
        private void btnusers_Click(object sender, EventArgs e)
        {
            if (btnusers1.Visible)
            {
                btnusers1.Visible = false; btncat.Visible = false;
            }
            else
            {
                btnusers1.Visible = true; btncat.Visible = true;
            }                   
        }
        private void btnusers1_Click(object sender, EventArgs e)
        {
            btnusers1.Visible = false; btncat.Visible = false; btnaddclient.Visible = false; btnnuevalocal.Visible = false;
            btnedit.Visible = true; btnaddclient.Visible = true; btneliminarcte.Visible = true;
            btnconfirm.Visible = true; btnconfirm.Location = new Point(833, 333);
            btnaduser.Visible = true; btnaduser.Location = new Point(656, 17); label19.Visible = false;
            button1.Visible = false; button3.Visible = false;

            gpbdatosusuario.Visible = true; gpbdatosusuario.Location = new Point(833,115);
            gpbcat.Visible = false; psearch.Visible = true;

            dataGridView1.DataSource = connsql.UsersList();
            paneldgv.Visible = true;
        }
        private void btncat_Click(object sender, EventArgs e)
        {           
            btnusers1.Visible = false; btncat.Visible = false; 
            btneliminarcte.Visible = true; btnedit.Visible = true;
            btnaduser.Visible = false;
            btnaddcat.Visible = true; btnaddcat.Location = new Point(656, 17);
            label19.Visible = false;
            button1.Visible = false; button3.Visible = false;

            gpbcat.Visible = true; gpbdatosusuario.Visible = false;
            psearch.Visible = false;
            gpbcat.Location = new Point(847, 135);

            btnconfirm.Visible = true; btnconfirm.Location = new Point(832, 267);

            paneldgv.Visible = true;
            dataGridView1.DataSource = connsql.CategoriesList();
        }
        private void txtmail_Leave(object sender, EventArgs e)
        {
            if (!(txtmail.Text.Contains('@')))
            {
                txtmail.ForeColor = Color.DarkRed;
                lblwmail.Visible = true;
            }
            else
            {
                if (connsql.SearchMail(txtmail.Text))
                {
                    txtmail.ForeColor = Color.DarkRed; lblwmail.Visible = true;
                }
                else
                {
                    lblwmail.Visible = false ;
                }
            }
        }
        private void txtmail_Enter(object sender, EventArgs e)
        {
            txtmail.ForeColor = Color.Black;
        }
        private void btnregauditoria_Click(object sender, EventArgs e)
        {
            paneldgv.Visible = true; btnregauditoria.Visible = false;
            dataGridView1.DataSource = connsql.Historico(); 
            psearch.Visible = true; button1.Visible = true; button3.Visible = true; label19.Visible = true;
        }
        private void btnimprimir_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "";
            printer.SubTitle = "";
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.PrintSettings.PrinterName = cmbprinters.Text;
            printer.PrintNoDisplay(dataGridView1);
        }
        private void btnauditorias_Click(object sender, EventArgs e)
        {
            if (btnregauditoria.Visible == true)
            {
                btnregauditoria.Visible = false;
            }
            else
            {
                btnregauditoria.Visible = true;
            }
        }
        private void Createpdf(DataGridView dgw)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable table = new PdfPTable(dgw.Columns.Count);
            table.DefaultCell.Padding = 5;
            table.WidthPercentage = 100;

            iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            foreach (DataGridViewColumn col in dgw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, text));

                table.AddCell(cell);
            }
            foreach (DataGridViewRow row in dgw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = ".pdf";
            save.Filter = "PDF (*.pdf)|*.pdf";
            
            if (save.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();

                    pdfdoc.Add(table);
                    pdfdoc.Close();
                    stream.Close();
                }
            }            
        }
        private void btnarchivo_Click(object sender, EventArgs e)
        {
            Createpdf(dataGridView1);
        }
        private void btnsalir_Click(object sender, EventArgs e)
        {
            //form2.ShowDialog(); form2.Activate();
            //this.Hide();
            panel1.Visible = false; paneldgv.Visible = false; panelprinters.Visible = false;
            gpbcat.Visible = false; gpbdatoscliente.Visible = false; gpbdatosusuario.Visible = false; 
            gpblocalidad.Visible = false;          
            btnconfirm.Visible = false;
            label2.Visible = true; label1.Visible = true;
            txtuseracc.Visible = true; txtpwacc.Visible = true; btnacc.Visible = true;
            label2.Location = new Point (63, 124); label1.Location = new Point (63, 150);
            txtuseracc.Location = new Point (130, 121); txtpwacc.Location = new Point (130, 147);
            txtuseracc.Text = string.Empty; txtpwacc.Text = string.Empty;
        }
        private void cmbprin()
        {
            foreach (string name in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                listBox1.Items.Add(name.ToString());             
            }
        }
        private void cmbcategories()
        {
            cmbcat.DataSource = connsql.CategoriesList();
            cmbcat.DisplayMember = "Titulo";
            cmbcat.ValueMember = "Categoria";
        }
        private void cmbcat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtidcat.Text = (cmbcat.SelectedIndex +1).ToString();
        }
        private void btnaduser_Click(object sender, EventArgs e)
        {
            btnconfirm.Enabled = true; btnop = "adduser"; txtusername.Enabled = true;
            txtusersurname.Enabled = true; txtuserpw.Enabled = true; txtmail.Enabled = true;
            cmbcat.Enabled = true;
        }
        private void btnaddcat_Click(object sender, EventArgs e)
        {
            btnconfirm.Enabled = true; btnop = "addcat"; txtidcat.Enabled = true; txtcatdesc.Enabled = true;
        }
        private void btnconfig_Click(object sender, EventArgs e)
        {
            if (btnimpresoras.Visible == true)
            {
                btnimpresoras.Visible = false;
            }
            else
            {
                btnimpresoras.Visible = true;
            }
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem);
            listBox1.Items.Remove(listBox1.SelectedItem);
            listBox1.Refresh();
            cmbprinters.Items.Clear();
            foreach (string name in listBox2.Items)
            {
                cmbprinters.Items.Add(name);
            }
        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox2.SelectedItem);
            listBox2.Items.Remove(listBox2.SelectedItem);
            listBox2.Refresh();
            cmbprinters.Items.Clear();
            foreach (string name in listBox2.Items)
            {
                cmbprinters.Items.Add(name);
            }

        }
        private void btnimpresoras_Click(object sender, EventArgs e)
        {
            panelprinters.Visible = true;
            panelprinters.Location = new Point(41, 58); btnimpresoras.Visible = false;
        }
        private void btnver_Click(object sender, EventArgs e)
        {
           
        }
        private void btnacc_Click(object sender, EventArgs e)
        {
            if ((connsql.SearchMail(txtuseracc.Text)) && (connsql.SearchPw(txtuseracc.Text)))
            {
                panel1.Visible = true;
                label1.Visible = false; label2.Visible = false;
                txtuseracc.Visible = false; txtpwacc.Visible = false; btnacc.Visible = false;
                
                string[] u = ActiveUser().ToArray();               

                lblID.Text = "ID #" + u[0]; lblnombap.Text = u[1] + " " + u[2];
                lblcat.Text = u[4];

                if (lblcat.Text == "AUDITOR")
                {
                    btnclients.Enabled = false; btnauditorias.Enabled = true;
                    btnlocalidades.Enabled = false;                   
                    btnconfig.Enabled = false;
                    btnedit.Visible = false; btneliminarcte.Visible = false; btnconfirm.Visible = false;
                    btnnuevalocal.Visible = false; btnaddclient.Visible = false;
                    btnusers1.Visible = false; btncat.Visible = false; btnaduser.Visible = false;
                    btnregauditoria.Visible = false; btnconfirm.Enabled = false;
                    btnaddcat.Visible = false; btnimpresoras.Visible = false; btnusers.Enabled = false;

                    gpbdatoscliente.Visible = false; gpblocalidad.Visible = false; psearch.Visible = false;
                    gpbdatosusuario.Visible = false; gpbcat.Visible = false; panelprinters.Visible = false;
                  
                    paneldgv.Visible = false; paneldgv.Location = new Point(19, 66); 
                    panel1.Visible = true; btnregauditoria.Location = new Point (385, 35);
                    cmbprinters.Visible = true; 
                }
                else
                {
                    if (lblcat.Text == "SUPERVISOR")
                    {
                        btnedit.Visible = false; btneliminarcte.Visible = false; btnconfirm.Visible = false;
                        btnnuevalocal.Visible = false; btnaddclient.Visible = false;
                        btnusers1.Visible = false; btncat.Visible = false; btnaduser.Visible = false;
                        btnregauditoria.Visible = false; btnconfirm.Enabled = false;
                        btnaddcat.Visible = false; btnimpresoras.Visible = false;

                        gpbdatoscliente.Visible = false; gpblocalidad.Visible = false; psearch.Visible = false;
                        gpbdatosusuario.Visible = false; gpbcat.Visible = false; panelprinters.Visible = false;

                        paneldgv.Visible = false; paneldgv.Location = new Point(19, 66);
                        panel1.Visible = true; btnusers1.Location = new Point(281, 35);
                        btncat.Location = new Point (281, 60);
                        cmbprinters.Visible = false;

                        btnclients.Enabled = false;
                        btnlocalidades.Enabled = false; btnusers.Enabled = true;
                        btnauditorias.Enabled = false; btnconfig.Enabled = false;
                        cmbcategories();
                    }
                    else
                    {
                        if (lblcat.Text == "USUARIO")
                        {
                            cmbprin(); cmbprinters.Visible = true;
                            btnauditorias.Enabled = false; btnconfig.Enabled = false;
                            btnusers.Enabled = false; btnclients.Enabled = true; btnlocalidades.Enabled = true;

                            txtfechaalta.Text = DateTime.Today.ToString("dd/MM/yyyy");
                            txtfechaalta.Enabled = false;
                            txtlocal.Enabled = false; txtpcia.Enabled = false;

                            txtlocal.MaxLength = (30);
                            txtpcia.MaxLength = (30); txtcp.MaxLength = (4); txtcodlocal.MaxLength = (4);
                            txtdesclocal.MaxLength = (30);
                        }
                        else
                        {
                            if (lblcat.Text == "ADMINISTRADOR")
                            {
                                btnedit.Visible = false; btneliminarcte.Visible = false; btnconfirm.Visible = false;
                                btnnuevalocal.Visible = false; btnaddclient.Visible = false;
                                btnusers1.Visible = false; btncat.Visible = false; btnaduser.Visible = false;
                                btnregauditoria.Visible = false; btnconfirm.Enabled = false;
                                btnaddcat.Visible = false; btnimpresoras.Visible = false;

                                gpbdatoscliente.Visible = false; gpblocalidad.Visible = false; psearch.Visible = false;
                                gpbdatosusuario.Visible = false; gpbcat.Visible = false; panelprinters.Visible = false;

                                paneldgv.Visible = false; paneldgv.Location = new Point(19, 66);
                                panel1.Visible = true; panelprinters.Location = new Point(490, 35);
                                cmbprinters.Visible = false;

                              
                                btnclients.Enabled = false; btnlocalidades.Enabled = false;
                                btnusers.Enabled = false; btnauditorias.Enabled = false;
                                btnconfig.Enabled = true; panelprinters.Visible = false;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Usuario y/o contraseña incorrectos");
            }               
        }
        public List<string> ActiveUser()
        {
            string m = txtuseracc.Text;
            List<string> user = new List<string>();

            user = connsql.getUser(m);
            return user;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (btnnuevalocal.Visible)
            {
               dt = connsql.ListarLocalidades();
                if (dt.Rows.Count > 10)
                {
                    int nropag; int ultpag; int nroSkip; int totalRegs; string x;

                    label19.Text = ((Convert.ToInt32(label19.Text) + 1)).ToString();

                    totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                    double y = (totalRegs / 10);
                    x = Math.Truncate(y).ToString(); ;

                    ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));
                    if (nropag == 1)
                    {
                        nroSkip = 10;
                        dataGridView1.DataSource = connsql.dgvPaginationLocalidades(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                    }
                    else
                    {
                        nroSkip = (nropag * 10);
                        dataGridView1.DataSource = connsql.dgvPaginationLocalidades(nroSkip - 10, ultpag);
                    }
                }
            }
            else
            {
                if (btnaddclient.Visible)
                {
                    dt = connsql.ListarClientes();
                    if (dt.Rows.Count > 10)
                    {
                        int nropag; int ultpag; int nroSkip; int totalRegs; string x;

                        label19.Text = ((Convert.ToInt32(label19.Text) + 1)).ToString();

                        totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                        double y = (totalRegs / 10);
                        x = Math.Truncate(y).ToString(); ;

                        ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));
                        if (nropag == 1)
                        {
                            nroSkip = 10;
                            dataGridView1.DataSource = connsql.dgvPaginationClientes(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                        }
                        else
                        {
                            nroSkip = (nropag * 10);
                            dataGridView1.DataSource = connsql.dgvPaginationClientes(nroSkip - 10, ultpag);
                        }
                    }
                }
                else
                {
                    if (lblcat.Text == "AUDITOR")
                    {
                        dt = connsql.Historico();
                        if (dt.Rows.Count > 10)
                        {
                            int nropag; int ultpag; int nroSkip; int totalRegs; string x;

                            label19.Text = ((Convert.ToInt32(label19.Text) + 1)).ToString();

                            totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                            double y = (totalRegs / 10);
                            x = Math.Truncate(y).ToString(); ;

                            ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));
                            if (nropag == 1)
                            {
                                nroSkip = 10;
                                dataGridView1.DataSource = connsql.dgvPaginationHistorico(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                            }
                            else
                            {
                                nroSkip = (nropag * 10);
                                dataGridView1.DataSource = connsql.dgvPaginationHistorico(nroSkip - 10, ultpag);
                            }
                        }
                    }
                }
            }                 
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int nropag; int ultpag; int nroSkip; int totalRegs; string x;
            DataTable dt = new DataTable();
            if (btnnuevalocal.Visible)
            {
                dt = connsql.ListarLocalidades();
                if (dt.Rows.Count > 10)
                {                    
                    label19.Text = ((Convert.ToInt32(label19.Text) - 1)).ToString();

                    totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                    double y = (totalRegs / 10);
                    x = Math.Truncate(y).ToString(); 
                    ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));

                    if (nropag == 1)
                    {
                        nroSkip = 10;
                        dataGridView1.DataSource = connsql.dgvPaginationLocalidades(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                    }
                    else
                    {
                        nroSkip = (nropag * 10);
                        dataGridView1.DataSource = connsql.dgvPaginationLocalidades(nroSkip - 10, ultpag);
                    }
                }
            }
            else
            {
                if (btnaddclient.Visible)
                {
                    dt = connsql.ListarClientes();
                    if (dt.Rows.Count > 10)
                    {                       
                        label19.Text = ((Convert.ToInt32(label19.Text) - 1)).ToString();

                        totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                        double y = (totalRegs / 10);
                        x = Math.Truncate(y).ToString(); 

                        ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));
                        if (nropag == 1)
                        {
                            nroSkip = 10;
                            dataGridView1.DataSource = connsql.dgvPaginationClientes(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                        }
                        else
                        {
                            nroSkip = (nropag * 10);
                            dataGridView1.DataSource = connsql.dgvPaginationClientes(nroSkip - 10, ultpag);
                        }
                    }
                }
                else
                {
                    if (lblcat.Text == "AUDITOR")
                    {
                        dt = connsql.Historico();
                        if (dt.Rows.Count > 10)
                        {
                            label19.Text = ((Convert.ToInt32(label19.Text) - 1)).ToString();

                            totalRegs = dt.Rows.Count; nropag = (Convert.ToInt32(label19.Text));
                            double y = (totalRegs / 10);
                            x = Math.Truncate(y).ToString(); ;

                            ultpag = (totalRegs - (10 * (Convert.ToInt32(x))));
                            if (nropag == 1)
                            {
                                nroSkip = 10;
                                dataGridView1.DataSource = connsql.dgvPaginationHistorico(nroSkip * (Convert.ToInt32(label19.Text) - 1), 10);
                            }
                            else
                            {
                                nroSkip = (nropag * 10);
                                dataGridView1.DataSource = connsql.dgvPaginationHistorico(nroSkip - 10, ultpag);
                            }
                        }
                    }
                }
            }                               
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void txtuserpw_Leave(object sender, EventArgs e)
        {
            if (txtuserpw.Text == "")
            {
                lblwpw.Visible = true;
            }
            else
            {
                lblwpw.Visible = false;
            }
        }
        private void btnbackup_Click(object sender, EventArgs e)
        {
            connsql.backUp();
        }
    }
}





