using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQLAIRefactor
{
    public partial class Form2 : Form
    {
        public string DataTypesString { get; private set; }  
        public string SQLServerName { get; private set; }  
        public string DatabaseName { get; private set; }  

        public Form2()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;

            textBox4.UseSystemPasswordChar = true;
            this.AcceptButton = this.button1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }
        
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string serverName = textBox1.Text.Trim();
            string databaseName = textBox2.Text.Trim();
            SqlConnection ConnectionToSQL;

            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(databaseName))
            {
                MessageBox.Show("Insert SQL Server name or Database Name");
                return;
            }

            //Build Connection string
            string connectionString;

            if (radioButton1.Checked)
            {
                connectionString = $"Server={serverName};Database={databaseName};Integrated Security=true;";
            }
            else if (radioButton2.Checked)
            {
                string username = textBox3.Text.Trim();
                string password = textBox4.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Insert username and password for SQL Authentication");
                    return;
                }
                connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};";
            }
            else
            {
                MessageBox.Show("Select authentication method");
                return;
            }

            //Connect to the SQL Server database to retrieve metadata table columns types
            ConnectionToSQL = new SqlConnection(connectionString);
            try
            {
                ConnectionToSQL.Open();
                button1.ForeColor = Color.Green;
                button1.Text = "Connected";
                GetDataTypesInfo(ConnectionToSQL);   
                SQLServerName = serverName;
                DatabaseName = databaseName;
                ConnectionToSQL.Close();
                this.DialogResult = DialogResult.OK;   
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
            }
        }


        private void GetDataTypesInfo(SqlConnection ConnectionToSQL)  //---Get all columns data types in JSON format
        {
            try
            {
                string query = @"DECLARE @jsonResult NVARCHAR(MAX);
                                    SET @jsonResult = (
                                            SELECT
                                                (
                                                    SELECT
                                                        SCHEMA_NAME(t.schema_id) AS [Schema],
                                                        t.name AS [Table],
                                                        c.name AS [Column],
                                                        CASE 
                                                            WHEN ty.name IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') THEN 
                                                                ty.name + 
                                                                CASE 
                                                                    WHEN c.max_length = -1 THEN '(MAX)'
                                                                    ELSE '(' + CAST(c.max_length / 
                                                                        CASE 
                                                                            WHEN ty.name IN ('nchar', 'nvarchar') THEN 2 
                                                                            ELSE 1 
                                                                        END AS VARCHAR) + ')'
                                                                END
                                                            WHEN ty.name IN ('decimal', 'numeric') THEN 
                                                                ty.name + '(' + CAST(c.precision AS VARCHAR) + ',' + CAST(c.scale AS VARCHAR) + ')'
                                                            ELSE ty.name
                                                        END AS DataType
                                                    FROM 
                                                        sys.tables t
                                                        INNER JOIN sys.columns c ON t.object_id = c.object_id
                                                        INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
                                                    ORDER BY 
                                                        t.name, c.column_id
                                                    FOR JSON PATH
                                                )
                                        )
                                        SELECT @jsonResult";

                using (SqlCommand command = new SqlCommand(query, ConnectionToSQL))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        DataTypesString = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during query execution: " + ex.Message);
            }
        }
    }
}
