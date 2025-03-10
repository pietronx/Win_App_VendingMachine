using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;

namespace prueba
{
    public partial class HistoryForm : Form
    {
        private SQLiteConnection conexion;
        private bool Operacion; // Variable booleana para registrar si se compró o canceló

        public HistoryForm()
        {
            InitializeComponent();
            InicializarConexion();
            CargarHistorial();
        }

        private void HistoryForm_Load(object sender, EventArgs e) { }

        private void InicializarConexion()
        {
            string cadenaConexion = "Data Source=vending_machine.db;Version=3;";
            conexion = new SQLiteConnection(cadenaConexion);
            conexion.Open();

            string crearTabla = @"CREATE TABLE IF NOT EXISTS historial (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    fecha TEXT,
                                    hora TEXT,
                                    credito TEXT,
                                    operacion TEXT,
                                    bebida TEXT,
                                    precio TEXT
                                  );";
            using (SQLiteCommand cmd = new SQLiteCommand(crearTabla, conexion))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CargarHistorial()
        {
            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=vending_machine.db;Version=3;"))
            {
                conexion.Open();
                string consulta = "SELECT fecha AS 'Date', hora AS 'Time', credito AS 'Credit', operacion AS 'GO | Cancel', bebida AS 'Drink', precio AS 'Price' FROM historial ORDER BY id DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        History_Table.DataSource = dt;
                    }
                }
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=vending_machine.db;Version=3;"))
            {
                conexion.Open();
                string eliminarRegistros = "DELETE FROM historial"; // Borra todos los registros
                using (SQLiteCommand cmd = new SQLiteCommand(eliminarRegistros, conexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Limpiar la tabla en la interfaz
            History_Table.DataSource = null;
            History_Table.Rows.Clear();
            History_Table.Refresh();
        }

    }
}
