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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_Zoo_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            // Connection String
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_Zoo_Manager.Properties.Settings.zoodbConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoo();
            ShowAnimal();
            
        }

        private void ShowZoo()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    ListZoo.DisplayMemberPath = "location";
                    ListZoo.SelectedValuePath = "Id";
                    ListZoo.ItemsSource = zooTable.DefaultView;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowAnimal()
        {
            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    ListAnimal.DisplayMemberPath = "Name";
                    ListAnimal.SelectedValuePath = "Id";
                    ListAnimal.ItemsSource = animalTable.DefaultView;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowAssociatedAnimal()
        {
            try
            {
                string query = "select * from Animal join ZooAnimal on Animal.Id = ZooAnimal.animalid where ZooAnimal.zooid = @zooid";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@zooid", ListZoo.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    ListAssociatedAnimals.DisplayMemberPath = "Name";
                    ListAssociatedAnimals.SelectedValuePath = "Id";
                    ListAssociatedAnimals.ItemsSource = animalTable.DefaultView;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void ListZoo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimal();
            ShowSelectedZooInTextBox();
            
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where Id = @zooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@zooid", ListZoo.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowZoo();
            }
            

        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@Location", AddLocation.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowZoo();
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@zooid, @animalid)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@animalid", ListAnimal.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@zooid", ListZoo.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimal();
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where Id = @animalid";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@animalid", ListAnimal.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowAnimal();
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where Id = @animalid";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@animalid", ListAnimal.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowAnimal();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@Name", AddLocation.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowAnimal();
            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {
                string query = "select Location from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@zooid", ListZoo.SelectedValue);

                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    AddLocation.Text = zooTable.Rows[0]["Location"].ToString();

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                string query = "select Name from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@AnimalId", ListAnimal.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    AddLocation.Text = animalTable.Rows[0]["Name"].ToString();

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ListAnimal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo Set Location = @Location where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", AddLocation.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception exe)
            {
                MessageBox.Show(exe.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowZoo();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal Set Name = @Name where Id = @animalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@animalId", ListAnimal.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", AddLocation.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception exe)
            {
                MessageBox.Show(exe.ToString());
            }

            finally
            {
                sqlConnection.Close();
                ShowAnimal();
            }
        }
    }
}

