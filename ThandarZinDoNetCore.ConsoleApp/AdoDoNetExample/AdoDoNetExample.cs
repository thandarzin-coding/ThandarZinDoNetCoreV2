using Microsoft.Data.SqlClient;
using System.Data;

namespace ThandarZinDoNetCore.ConsoleApp.AdoDoNetExample
{
	public class AdoDoNetExample
	{
		public void Run()
		{

			//Read();

			Delete(7);
		
	

		}

		public void Read()
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
			{
				DataSource = ".",
				InitialCatalog = "TestDb",
				UserID = "sa",
				Password = "sa@123",
				TrustServerCertificate = true
			};
			SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			Console.WriteLine("Connection opening...");
			connection.Open();
			Console.WriteLine("Connection opened.");

			string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog]";
			SqlCommand sqlCommand = new SqlCommand(query, connection);
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			DataTable dataTable = new DataTable();
			sqlDataAdapter.Fill(dataTable);
			Console.WriteLine("Connection closing...");
			connection.Close();
			Console.WriteLine("Connection closed...");
			foreach (DataRow item in dataTable.Rows)
			{
				Console.WriteLine("Id => " + item["Blog_Id"]);
				Console.WriteLine("Blog_Author => " + item["Blog_Author"]);
				Console.WriteLine("Blog_Content => " + item["Blog_Content"]);
				Console.WriteLine("Blog_Title => " + item["Blog_Title"]);
			}

		}

		public void Edit(int Id)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
			{
				DataSource = ".",
				InitialCatalog = "TestDb",
				UserID = "sa",
				Password = "sa@123",
				TrustServerCertificate = true
			};
			SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			Console.WriteLine("Connection opening...");
			connection.Open();
			Console.WriteLine("Connection opened.");

			string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Blog_Id", Id);
			DataTable dataTable = new DataTable();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
			sqlDataAdapter.Fill(dataTable);

			connection.Close();

			if (dataTable is null)
			{
				Console.WriteLine("Data Not Found");
			}

			DataRow dr = dataTable.Rows[0];
			Console.WriteLine("Id => " + dr["Blog_Id"]);
			Console.WriteLine("Blog_Title => " + dr["Blog_Title"]);
			Console.WriteLine("Blog_Author => " + dr["Blog_Author"]);
			Console.WriteLine("Blog_Content => " + dr["Blog_Content"]);

		}

		public void Create(string Title, string Author, string Content)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
			{
				DataSource = ".",
				InitialCatalog = "TestDb",
				UserID = "sa",
				Password = "sa@123",
				TrustServerCertificate = true
			};
			SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			Console.WriteLine("Connection opening...");
			connection.Open();
			Console.WriteLine("Connection opened.");

			string query = @"INSERT INTO [dbo].[Tbl_Blog]
							   ([Blog_Title]
							   ,[Blog_Author]
							   ,[Blog_Content])
						 VALUES
							   (@Blog_Title
							   ,@Blog_Author
							   ,@Blog_Content)";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Blog_Title", Title);
			command.Parameters.AddWithValue("@Blog_Content", Content);
			command.Parameters.AddWithValue("@Blog_Author", Author);
			var result = command.ExecuteNonQuery();
			connection.Close();
			string message = result > 0 ? " Saving Successfully " : "Saving Faild";
			Console.WriteLine(message);

		}

		public void Delete(int Id)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
			{
				DataSource = ".",
				InitialCatalog = "TestDb",
				UserID = "sa",
				Password = "sa@123",
				TrustServerCertificate = true
			};
			SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			
			connection.Open();
			string query = @"DELETE FROM [dbo].[Tbl_Blog]
						   WHERE Blog_Id = @Blog_Id";

			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Blog_Id", Id);
			var result = command.ExecuteNonQuery();
			connection.Close();
			string message = result > 0 ? "Deleted successfully" : "Delete Faild";
			Console.WriteLine(message);


		}

		private void Update(int id, string title, string author, string content)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
			{
				DataSource = ".",
				InitialCatalog = "TestDb",
				UserID = "sa",
				Password = "sa@123",
				TrustServerCertificate = true
			};
			SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			Console.WriteLine("Connection opening...");
			connection.Open();
			Console.WriteLine("Connection opened.");

			string query = @"UPDATE [dbo].[Tbl_Blog]
							   SET [Blog_Title] = @Blog_Title
								  ,[Blog_Author] = @Blog_Author
								  ,[Blog_Content] = @Blog_Content
							 WHERE Blog_Id = @Blog_Id)";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Blog_Id", id);
			command.Parameters.AddWithValue("@Blog_Title", title);
			command.Parameters.AddWithValue("@Blog_Content", author);
			command.Parameters.AddWithValue("@Blog_Author", content);
			var result = command.ExecuteNonQuery();
			connection.Close();
			string message = result > 0 ? " Updated successfully" : "Updateed Faild";
			Console.WriteLine(message);


		}


	}
}
