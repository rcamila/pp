using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    class Connsql
    {
        public SqlConnection dbConnect(string database)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-F3N9MCR;Initial Catalog="+database+";Integrated Security=True");
            return conn;

        }
        public void dbDisconnect(string database)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-F3N9MCR;Initial Catalog=" + database + ";Integrated Security=True");
            conn.Close();
        }

       //private SqlConnection conn = new SqlConnection("Data Source=DESKTOP-F3N9MCR;Initial Catalog=PPDB;Integrated Security=True");
        public void InsertCliente(Cliente cliente)
        {
            try
            {
                conn.Open();

                string query = @"
                    INSERT INTO Clientes (Nombre, Apellido, FechaAlta, CP, Email)
                    VALUES (@Nombre, @Apellido, @FechaAlta, @CP, @Email);";
                
                SqlParameter Nombre = new SqlParameter("@Nombre", cliente.Nombre);
                SqlParameter Apellido = new SqlParameter("@Apellido", cliente.Apellido);
                SqlParameter FechaAlta = new SqlParameter("@FechaAlta", cliente.FechaAlta);
                //SqlParameter Pcia = new SqlParameter("@Pcia", cliente.Pcia);
                SqlParameter CP = new SqlParameter("@CP", cliente.CP);
                SqlParameter Email = new SqlParameter("@Email", cliente.Email);

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(Nombre);
                command.Parameters.Add(Apellido);
                command.Parameters.Add(FechaAlta);
                //command.Parameters.Add(Pcia);
                command.Parameters.Add(CP);
                command.Parameters.Add(Email);

                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateCliente(Cliente cliente, int id)
        {
            try
            {
                conn.Open();
                string query = @" UPDATE Clientes
                                  SET Nombre = @Nombre,
                                      Apellido = @Apellido,
                                      CP = @CP,
                                        Email = @Email
                                  WHERE ID = " + id.ToString() + ";";

                SqlParameter Nombre = new SqlParameter("@Nombre", cliente.Nombre);
                SqlParameter Apellido = new SqlParameter("@Apellido", cliente.Apellido);
                SqlParameter CP = new SqlParameter("@CP", cliente.CP);
                SqlParameter Email = new SqlParameter("@Email", cliente.Email);

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(Nombre);
                command.Parameters.Add(Apellido);
                command.Parameters.Add(CP);
                command.Parameters.Add(Email);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }
        }
        public int GetIdAuditoria ()
        {
            int idaud;
            try
            {
                conn.Open();
                string query = @" SELECT TOP 1  ID FROM Historico ORDER BY Fecha desc";

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                idaud = (int)(reader.GetValue(0));

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); } return idaud; 
        }
        public void DeleteCliente(int id)
        {
            try
            {
                conn.Open();
                string query = @"DELETE FROM Clientes 
                WHERE ID = " + id.ToString();

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(new SqlParameter("@ID", id));
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
        }
        public DataTable ListarClientes(string busq = null)
        {
            DataTable Table = new DataTable();

            try
            {
                conn.Open();
                string query = @" SELECT ID, cl.Nombre, cl.Apellido,
                                cl.FechaAlta AS Fecha_Alta,
                                l.LocalId AS CP, l.descLocal AS Localidad, p.DescPcia AS Provincia
                                FROM Clientes cl 
                                INNER JOIN Localidades l 
                                ON cl.CP = l.LocalId
                                INNER JOIN Provincias p
                                ON l.Pcia = p.PciaId";

                SqlCommand command = new SqlCommand();

                if (!string.IsNullOrEmpty(busq))
                {
                    query += @" WHERE Nombre LIKE @busq OR Apellido LIKE @busq OR 
                    descLocal LIKE @busq OR LocalId LIKE @busq";
                    command.Parameters.Add(new SqlParameter("@busq", $"%{busq}%"));
                }

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();
                Table.Load(reader);
                reader.Close();

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }

            return Table;
        }
        public bool SearchCP(int cp)
        {
            bool bEnc = false;
            try
            {
                conn.Open();
                string query = @" SELECT * from Localidades 
                                WHERE LocalId = " + cp;

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@LocalId", cp));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) {
                    bEnc = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return bEnc;
        }
        public bool BuscarCPClientes(int cp)
        {
            bool bEnc = false;
            try
            {
                conn.Open();
                string query = @" SELECT * from Clientes 
                                WHERE CP = " + cp;

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@CP", cp));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    bEnc = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return bEnc;
        }
        ////////////////////////////////////////////////////////////////////
        public void InsertLocalidad(Localidad localidad)
        {
            conn.Open();

            string query = @"
                    INSERT INTO Localidades (LocalId, descLocal, Pcia, Email)
                    VALUES (@LocalId, @descLocal, @Pcia, @Email);";

            SqlParameter LocalId = new SqlParameter("@LocalId", localidad.LocalId);
            SqlParameter descLocal = new SqlParameter("@descLocal", localidad.descLocal);
            SqlParameter Pcia = new SqlParameter("@Pcia", localidad.Pcia);
            SqlParameter Email = new SqlParameter("@Email", localidad.Email);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(LocalId);
            command.Parameters.Add(descLocal);
            command.Parameters.Add(Pcia);
            command.Parameters.Add(Email);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void UpdateLocalidad(Localidad localidad, int id)
        {
            conn.Open();
            string query = @" UPDATE Localidades
                                  SET descLocal = @descLocal,
                                        Email = @Email,
                                        Pcia = @Pcia
                                  WHERE LocalId = " + id.ToString() + ";";

            SqlParameter descLocal = new SqlParameter("@descLocal", localidad.descLocal);
            SqlParameter Email = new SqlParameter("@Email", localidad.Email);
            SqlParameter pcia = new SqlParameter("@Pcia", localidad.Pcia);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(descLocal);
            command.Parameters.Add(Email);
            command.Parameters.Add(pcia);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void DeleteLocalidad(int id)
        {
            conn.Open();
            string query = @"DELETE FROM localidades 
                WHERE localId = " + id.ToString();

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(new SqlParameter("@localId", id));
            command.ExecuteNonQuery();

            conn.Close();
        }
        public DataTable ListarLocalidades(string busq = null)
        {
            DataSet ds = new DataSet();
            //SqlDataAdapter dba;
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = @"SELECT LocalId AS CP, descLocal AS Localidad, 
                    DescPcia AS Provincia FROM Localidades INNER JOIN Provincias ON PciaId = Pcia";

            SqlCommand cmd = new SqlCommand();

            if (!string.IsNullOrEmpty(busq))
            {
                qry += " WHERE descLocal LIKE @busq OR LocalId LIKE @busq;";
                cmd.Parameters.Add(new SqlParameter("@busq", $"%{busq}%"));
            }

            cmd.CommandText = qry;
            cmd.Connection = conn;

            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        ////////////////////////////////////////////////////////////////////
        public void InsertCategory(Categoria categoria)
        {
            conn.Open();

            string query = @"
                    INSERT INTO Categorias (catId, desccat)
                    VALUES (@catId, @desccat);";

            SqlParameter catId = new SqlParameter("@catId", categoria.catId);
            SqlParameter desccat = new SqlParameter("@desccat", categoria.desccat);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(catId);
            command.Parameters.Add(desccat);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void UpdateCategory(Categoria categoria, int id)
        {
            conn.Open();
            string query = @" UPDATE Categorias
                                  SET desccat = @desccat,
                                   catId = @catId
                                  WHERE catId = " + id.ToString() + ";";

            SqlParameter desccat = new SqlParameter("@desccat", categoria.desccat);
            SqlParameter catId = new SqlParameter("@catId", categoria.catId);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(desccat);
            command.Parameters.Add(catId);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void DeleteCategory(int id)
        {
            conn.Open();
            string query = @"DELETE FROM Categorias 
                WHERE catId = " + id.ToString();

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(new SqlParameter("@catId", id));
            command.ExecuteNonQuery();

            conn.Close();
        }
        public DataTable CategoriesList()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter dba;
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = "SELECT catId AS Categoria, desccat AS Titulo FROM Categorias";

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = qry;
            cmd.Connection = conn;
            dba = new SqlDataAdapter(qry, conn);
            ds.Reset();
            dba.Fill(ds);
            dt = ds.Tables[0];
            conn.Close();

            return dt;
        }
        public bool SearchCat(int cat)
        {
            bool bEnc = false;
            try
            {
                conn.Open();
                string query = @" SELECT * from Categorias 
                                WHERE catId = " + cat;

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@catId", cat));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    bEnc = true;

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return bEnc;
        }
        public bool BuscarCatUsuario (int cat)
        {
            bool bEnc = false;
            try
            { //si es verdadero quiere decir q un usuario tiene la categoria q le paso como parametro
                conn.Open();
                string query = @" SELECT cat from Usuarios 
                                WHERE cat = " + cat;

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@cat", cat));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    bEnc = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return bEnc;
        }
        public int GetNroCat (string t)
        {
            int nro;

            try
            {
                conn.Open();
                string query = @"SELECT catId
                                FROM Categorias  
                                WHERE desccat = " + "'" + t + "'";

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                nro = (int)(reader.GetValue(0));

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return nro;
        }
        ////////////////////////////////////////////////////////////////////
        public void InsertUser(Usuario usuario)
        {
            conn.Open();

            string query = @"
                    INSERT INTO Usuarios (Nombre, Apellido, mail, pw, cat)
                    VALUES (@Nombre, @Apellido, @mail, @pw, @cat);";

            SqlParameter Nombre = new SqlParameter("@Nombre", usuario.Nombre);
            SqlParameter Apellido = new SqlParameter("@Apellido", usuario.Apellido);
            SqlParameter mail = new SqlParameter("@mail", usuario.mail);
            SqlParameter pw = new SqlParameter("@pw", usuario.pw);
            SqlParameter cat = new SqlParameter("@cat", usuario.cat);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(Nombre);
            command.Parameters.Add(Apellido);
            command.Parameters.Add(mail);
            command.Parameters.Add(pw);
            command.Parameters.Add(cat);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void UpdateUser(Usuario usuario, int id)
        {
            conn.Open();
            string query = @" UPDATE Usuarios
                                  SET Nombre = @Nombre,
                                      Apellido = @Apellido,
                                      mail = @mail,
                                      pw = @pw,
                                      cat = @cat
                                  WHERE ID = " + id.ToString() + ";";

            SqlParameter Nombre = new SqlParameter("@Nombre", usuario.Nombre);
            SqlParameter Apellido = new SqlParameter("@Apellido", usuario.Apellido);
            SqlParameter mail = new SqlParameter("@mail", usuario.mail);
            SqlParameter pw = new SqlParameter("@pw", usuario.pw);
            SqlParameter cat = new SqlParameter("@cat", usuario.cat);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(Nombre);
            command.Parameters.Add(Apellido);
            command.Parameters.Add(mail);
            command.Parameters.Add(pw);
            command.Parameters.Add(cat);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public void DeleteUser(int id)
        {
            conn.Open();
            string query = @"DELETE FROM Usuarios 
                WHERE ID = " + id.ToString();

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(new SqlParameter("@ID", id));
            command.ExecuteNonQuery();

            conn.Close();
        }
        public DataTable UsersList(string busq = null)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = "SELECT ID, Nombre, Apellido, mail AS Mail, pw AS Contraseña, cat AS Categoria, desccat AS " +
                "Titulo FROM Usuarios INNER JOIN Categorias ON cat = catId";

            SqlCommand cmd = new SqlCommand();

            if (!string.IsNullOrEmpty(busq))
            {
                qry += @" WHERE ID LIKE @busq OR Nombre LIKE @busq OR Apellido LIKE @busq OR mail LIKE @busq OR
                    cat LIKE @busq OR desccat LIKE @busq";
                cmd.Parameters.Add(new SqlParameter("@busq", $"%{busq}%"));
            }

            cmd.CommandText = qry;
            cmd.Connection = conn;

            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        public bool SearchMail(string mail, string db)
        {
            bool bEnc = false;
            try
            {
                dbConnect(db);
                string query = @" SELECT * FROM Usuarios 
                                WHERE mail = " + "'"+mail+"'";

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@mail", mail));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    bEnc = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }

            return bEnc;
        }
        public bool SearchPw(string mail)
        {
            bool bEnc = false;
            try
            {
                conn.Open();
                string query = @" SELECT pw from Usuarios 
                                WHERE mail = " + "'"+mail+"'";

                SqlCommand command = new SqlCommand();

                command.Parameters.Add(new SqlParameter("@mail", mail));

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    bEnc = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }
            return bEnc;
        }
        ////////////////////////////////////////////////////////////////////
        public DataTable PciasList()
        {
            DataTable Table = new DataTable();

            try
            {
                conn.Open();
                string query = @"SELECT PciaId AS NroProvincia, DescPcia AS Provincia FROM Provincias";

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();
                Table.Load(reader);
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }

            return Table;
        }
        public string devPcia(int nropcia, int cp)
        {
            string provincia = "";
            try
            {
                conn.Open();
                string query = @"SELECT descPcia
                                FROM Provincias  
                                INNER JOIN Localidades 
                                ON PciaId = " + nropcia;

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                provincia = reader.GetValue(0).ToString();

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return provincia;
        }
        public int nroPcia(int cp)
        {
            int npcia;
            try
            {
                conn.Open();
                string query = @"SELECT Pcia
                                FROM Localidades  
                                INNER JOIN Provincias 
                                ON LocalId = " + cp;

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                npcia = (int)(reader.GetValue(0));

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return npcia;
        }
        public string devLocal(int cp)
        {
            string local = "";
            try
            {
                conn.Open();
                string query = @"SELECT descLocal
                                FROM Localidades                                   
                                WHERE LocalId = " + cp;

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                local = reader.GetValue(0).ToString();

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return local;
        }
        ////////////////////////////////////////////////////////////////////
        public DataTable Historico(string busq = null)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = "SELECT ID, NombTabla AS Tabla, Accion, Fecha, Usuario, Terminal  FROM Historico";

            SqlCommand cmd = new SqlCommand();

            if (!string.IsNullOrEmpty(busq))
            {
                qry += @" WHERE NombTabla LIKE @busq OR ID LIKE @busq OR Accion LIKE @busq OR Fecha LIKE @busq OR
                    Usuario LIKE @busq OR Terminal LIKE @busq";
                cmd.Parameters.Add(new SqlParameter("@busq", $"%{busq}%"));
            }

            cmd.CommandText = qry;
            cmd.Connection = conn;

            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        public void UpdateHistorico(int id, string m)
        {
            conn.Open();
            string query = @" UPDATE Historico
                                  SET Usuario = @mail
                                  WHERE ID = " + id.ToString() + ";";

            SqlParameter Usuario = new SqlParameter("@mail", m);

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(Usuario);

            command.ExecuteNonQuery();

            conn.Close();
        }
        ////////////////////////////////////////////////////////////////////
        public int getNroPcia (string pcia)
        {
            int nro;

            try
            {
                conn.Open();
                string query = @"SELECT PciaId
                                FROM Provincias  
                                WHERE DescPcia = " + "'" +pcia+"'";

                SqlCommand command = new SqlCommand();

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                nro = (int)(reader.GetValue(0));

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return nro;
        }
        public List<string> getUser(string mail)
        {
            List<string> u = new List<string>();

            conn.Open();
            string qry;

            qry = @"SELECT ID, Nombre, Apellido, mail AS Mail, desccat AS Categoria
                    FROM Usuarios
                    INNER JOIN Categorias ON cat = catId
                    WHERE mail = " + "'" + mail + "'";

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = qry;
            cmd.Connection = conn;

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                u.Add(reader.GetValue(0).ToString()); //ID
                u.Add(reader.GetValue(1).ToString()); //Nombre
                u.Add(reader.GetValue(2).ToString()); //Ap
                u.Add(reader.GetValue(3).ToString()); //mail
                u.Add(reader.GetValue(4).ToString()); //cat
            }
            
            reader.Close();
     
            conn.Close();

            return u;
        }
        public DataTable dgvPaginationClientes(int skip, int nRows)
        {
            
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = @"SELECT ID, cl.Nombre, cl.Apellido,
                                cl.FechaAlta AS Fecha_Alta,
                                l.LocalId AS CP, l.descLocal AS Localidad, p.DescPcia AS Provincia
                                FROM Clientes cl  
                                INNER JOIN Localidades l  
                                ON cl.CP = l.LocalId 
                                INNER JOIN Provincias p 
                                ON l.Pcia = p.PciaId ORDER BY ID offset " + skip+" ROWS "+
                    "FETCH NEXT " + nRows + " ROWS ONLY;";

            SqlCommand cmd = new SqlCommand();

            //SqlParameter Skip = new SqlParameter("@skip", skip);
            //SqlParameter NRows = new SqlParameter("@skip", nRows);
            //cmd.Parameters.Add(Skip); cmd.Parameters.Add(NRows);

            cmd.CommandText = qry;
            cmd.Connection = conn;

            //cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        public DataTable dgvPaginationLocalidades(int skip, int nRows)
        {
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = @"SELECT LocalId AS CP, descLocal AS Localidad, 
                    DescPcia AS Provincia FROM Localidades INNER JOIN Provincias ON Pcia = PciaId
                     ORDER BY LocalId offset " + skip + " ROWS " +
                    "FETCH NEXT " + nRows + " ROWS ONLY;";

            SqlCommand cmd = new SqlCommand();

            //SqlParameter Skip = new SqlParameter("@skip", skip);
            //SqlParameter NRows = new SqlParameter("@skip", nRows);
            //cmd.Parameters.Add(Skip); cmd.Parameters.Add(NRows);

            cmd.CommandText = qry;
            cmd.Connection = conn;

            //cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        public DataTable dgvPaginationHistorico(int skip, int nRows)
        {
            DataTable dt = new DataTable();

            conn.Open();
            string qry;

            qry = @"SELECT * FROM Historico 
                     ORDER BY ID offset " + skip + " ROWS " +
                    "FETCH NEXT " + nRows + " ROWS ONLY;";

            SqlCommand cmd = new SqlCommand();

            //SqlParameter Skip = new SqlParameter("@skip", skip);
            //SqlParameter NRows = new SqlParameter("@skip", nRows);
            //cmd.Parameters.Add(Skip); cmd.Parameters.Add(NRows);

            cmd.CommandText = qry;
            cmd.Connection = conn;

            //cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();

            conn.Close();

            return dt;
        }
        public void backUp()
        {
            string database = conn.Database.ToString();
            string query = @"
                    BACKUP DATABASE [" + database + "] TO DISK = 'C:\\backup\\testDB.bak' WITH FORMAT;";
        
            conn.Open();
            
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

    }
}
