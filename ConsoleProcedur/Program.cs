using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProcedur
{
    class Program
    {
        //sa mer SQL-in mianalu popoxakann e 
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";

        static void Main(string[] args)
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            
            Console.Write("Enter age: ");
            int  age = int.Parse(Console.ReadLine());


            AddUser(name, age);
            Console.WriteLine();
            GetUsers();

            Console.Read();
        }

        //avelacnum e user
        private static void AddUser(string name, int age)
        {
            //procedurdai anuny
            string sqlExpression = "sp_InsertUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                
                //cuyc enq talis, vor command-y nerkayacnum e proceduran
                command.CommandType = System.Data.CommandType.StoredProcedure;

                //parametr e name mutqagrelu hamar
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                //avelacnum enq parametry
                command.Parameters.Add(nameParam);

                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@age",
                    Value = age 
                };
                //avelacnum enq age parametry
                command.Parameters.Add(ageParam);

                var result = command.ExecuteScalar();
                // ete mez petq che veradarcnel id
                // var result = command.ExecuteNonQuery();

                Console.WriteLine("avelacvac obecti Id-n e: {0}", result);

            }

        }

        //durs e berum bolor user-nerin
        private static void GetUsers()
        {
            // procedurai anuny
            string sqlExpression = "sp_GetUsers";

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                // cuyc enq talis, vor command-y nerkayacnum e proceduran
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int age = reader.GetInt32(2);
                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }
                reader.Close();

            }
        }

    }
}
