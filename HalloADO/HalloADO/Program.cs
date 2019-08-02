using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
//using System.Data.SqlClient;

namespace HalloADO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo Datenbank");
            string conString = "Data Source=.;Initial Catalog=Northwind;Integrated Security=true;";
            string conStringNew = "Server=.;Database=Northwind;Trusted_Connection=yes;";

            using (var con = new SqlConnection(conString))
            {
                con.Open();
                Console.WriteLine("Yeah!");


                //var cmd = new SqlCommand();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT count(*) FROM Employees ";
                    //cmd.Connection = con;
                    Console.WriteLine($"{(int)cmd.ExecuteScalar()} Employees in DB");
                }


                Console.WriteLine("Suche:");
                var suche = Console.ReadLine();

                var liste = new List<Employee>();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM Employees WHERE LastName LIKE @search+'%'";
                    cmd.Parameters.AddWithValue("@search", suche);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new Employee();
                            emp.Id = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                            emp.LastName = (string)reader["LastName"];
                            emp.BirthDate = reader.GetDateTime(5);
                            emp.HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate"));
                            Console.WriteLine($"{emp.LastName} {emp.BirthDate:d} {emp.HireDate:d}");
                            liste.Add(emp);

                        }
                    }
                }

                using (var trans = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        //jünger version speichern
                        foreach (var item in liste)
                        {
                            using (var cmd = con.CreateCommand())
                            {
                                cmd.Transaction = trans;
                                cmd.CommandText = "UPDATE Employees SET birthdate = @bdate WHERE EmployeeID = @id";
                                cmd.Parameters.AddWithValue("@id", item.Id);
                                cmd.Parameters.AddWithValue("@bdate", item.BirthDate.AddYears(1));

                                var result = cmd.ExecuteNonQuery();
                                if (result == 0)
                                    Console.WriteLine("Da hat was nicht geklappt");
                                else if (result == 1)
                                    Console.WriteLine($"{item.LastName} wurde 1 Jahr jünger");
                                else
                                    Console.WriteLine($"PANIK! Vielleicht WHERE vergessen?");

                                //if (item.LastName.StartsWith("d", StringComparison.InvariantCultureIgnoreCase))
                                //    throw new ExecutionEngineException();

                            }
                        }
                        trans.Commit();
                        Console.WriteLine("Alle änderungen wurden übernommen");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Console.WriteLine("Alle änderungen wurden rückgängig gemacht");
                    }
                }
            }//con.Dispose(); ==> con.Close();

            Console.WriteLine("Ende");
            Console.ReadKey();
        }
    }

    class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }

    }
}
