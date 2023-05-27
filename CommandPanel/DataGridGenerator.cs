using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CommandPanel
{
    public class DataGridGenerator
    {
        public static DataGrid GenerateDataGridFromTable(string connectionString, string query)
        {
            // Create a DataGrid control
            DataGrid dataGrid = new DataGrid();

            try
            {
                // Connect to the SQL database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Retrieve the table schema from SQL
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Create columns dynamically based on the table schema
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = column.ColumnName;
                        textColumn.Binding = new System.Windows.Data.Binding(column.ColumnName);
                        dataGrid.Columns.Add(textColumn);
                    }

                    // Set the data source for the DataGrid
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"An error occurred while connecting to the database: {ex.Message}");
            }

            return dataGrid;
        }
    }
}