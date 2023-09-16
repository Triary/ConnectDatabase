using MySqlConnector;
using System.Data;


namespace ConnectDatabase
{
    internal class Program
    {

        static void display(MySqlDataReader dataReader)
        {
            foreach (var column in dataReader.GetColumnSchema())
            {
                Console.Write("\t");
                Console.Write(column.ColumnName);
                Console.Write("\t");
            }
            Console.WriteLine();


            while (dataReader.Read()) // построчно считываем данные
            {
                int count = dataReader.GetColumnSchema().Count;
                List<object> ints = new List<object>();
                for (int i = 0; i < count; i++)
                    ints.Add(dataReader.GetValue(i));

                foreach (object current in ints)
                {
                    Console.Write("\t");
                    Console.Write(current);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }

            dataReader.Close();
        }

        static void Main(string[] args)
        {

            var connectionStr_builder = new MySqlConnectionStringBuilder()
            {
                Database = "jyrs_databases",
                Server = "db4free.net",
                Port = 3306,
                UserID = "jyr_admin",
                Password = "JyrsDataBase0812",
            }; //создаем connection string через builder
            var connectString = connectionStr_builder.ConnectionString;         //создаем connection string через builder

            using (var mySqlConnection = new MySqlConnection(connectString)) //подключение к базе
            {
                mySqlConnection.Open();
                var command = mySqlConnection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                    FROM Workers";
                var result = command.ExecuteReader();

                display(result);

                Console.WriteLine("enter row id workers to display order");
                string id = Console.ReadLine();

                Console.WriteLine("enter id ");
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                    FROM Orders WHERE AcceptorId IN (SELECT Id FROM Workers WHERE Id =" + id + ")";
                var res2 = command.ExecuteReader();

                display(res2);


                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO Workers (ID, Name) VALUES (10, 'Kolobok')";
                var res3 = command.ExecuteReader();
                res3.Close();

                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                    FROM Workers";
                var res4 = command.ExecuteReader();

                display(res4);

            }
        }
    }
}