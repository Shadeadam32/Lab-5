using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Text;
using Lab5.Models;
namespace Lab5
{
    public class QueryBuilder : IDisposable
    {
        // db connection referenced by the 'connection' field
        private SqliteConnection connection;

        /// <summary>
        /// Constructor will set up our connection to a given SQLite database file and open it.
        /// </summary>
        /// <param name="databaseLocation">File path to a .db file</param>
        public QueryBuilder(string databaseLocation)
        {
            connection = new SqliteConnection("Data Source=" + databaseLocation);
            connection.Open();
        }

        /// <summary>
        /// By implementing IDisposable, we have the capability to 
        /// use a QueryBuilder object in a 'using' statement in our
        /// driver; when that using statement is complete, our Sqlite
        /// connection will be closed automatically
        /// </summary>
        public void Dispose()
        {
            connection.Dispose();
        }

        public List<T> ReadAll<T>() where T : IClassModel, new()
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name}";
            var reader = command.ExecuteReader();
            T data;
            var datas = new List<T>();
            while(reader.Read())
            {
                data = new T();
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(int))
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, Convert.ToInt32(reader.GetValue(i)));
                    else
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, reader.GetValue(i));              
                }
                datas.Add(data);
            }
            return datas;
        }
        public T Read<T>(int id) where T : IClassModel, new()
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT {typeof(T)} FROM {typeof(T).Name}";
            var reader = command.ExecuteReader();
            T data = new T();
            while (reader.Read())
            {
                data = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(int))
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, Convert.ToInt32(reader.GetValue(i)));
                    else
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, reader.GetValue(i));
                }
                return data;
            }
            return data;
        }

        public void Create<T>(T obj) where T : IClassModel
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            List<string> values = new List<string>();

            List<string> names = new List<string>();
            
            foreach(PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    values.Add("\"" + property.GetValue(obj) + "\"");

                }
                else
                    values.Add(property.GetValue(obj).ToString());
                names.Add(property.Name);


            }

            StringBuilder sbvalues = new StringBuilder();
            StringBuilder sbnames = new StringBuilder();
            for(int i =0; i< values.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    sbvalues.Append($"{values[i]}");
                    sbnames.Append($"{names[i]}");
                }
                else {
                    sbvalues.Append($"{values[i]},");
                    sbnames.Append($"{names[i]},");
                }
            }

            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {typeof(T).Name} ({sbnames}) VALUES ({sbvalues})";

            var insert = command.ExecuteNonQuery();
        }
        public void Update<T>(T obj) where T : IClassModel
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            List<string> values = new List<string>();

            List<string> names = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    values.Add("\"" + property.GetValue(obj) + "\"");

                }
                else
                    values.Add(property.GetValue(obj).ToString());
                names.Add(property.Name);


            }

            StringBuilder sbvalues = new StringBuilder();
            StringBuilder sbnames = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    sbvalues.Append($"[{values[i]}");
                    sbnames.Append($"[{values[i]}");
                }
                else
                {
                    sbvalues.Append($"[{values}]");
                    sbnames.Append($"[{values[i]}");
                }
            }

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {typeof(T).Name} SET ({sbnames}) = ({sbvalues}) WHERE Id = {obj.Id}";

            var insert = command.ExecuteNonQuery();
        }
            public void Delete<T> (T obj) where T : IClassModel
        {
            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE Id = {obj.Id}";

            var insert = command.ExecuteNonQuery();
        }
    }
}
