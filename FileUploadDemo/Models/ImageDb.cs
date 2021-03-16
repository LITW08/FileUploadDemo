using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadDemo.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime TimeUploaded { get; set; }
    }

    public class ImageDb
    {
        private readonly string _connectionString;

        public ImageDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetImages()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images";
            connection.Open();
            List<Image> images = new List<Image>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    FileName = (string)reader["FileName"],
                    TimeUploaded = (DateTime)reader["TimeUploaded"]
                });
            }

            return images;

        }

        public void AddImage(string fileName)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images (FileName, TimeUploaded) " +
                              "VALUES (@filename, GETDATE())";
            cmd.Parameters.AddWithValue("@filename", fileName);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
