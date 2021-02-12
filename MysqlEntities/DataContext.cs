using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MysqlEntities
{
    public class DataContext : IDataContext
    {
        //Declarations
        private string ConnectionString { get; set; }
        private IQueryBuilder _queryBuilder { get; set; } = new QueryBuilder();

        //Default Constructor
        public DataContext() { }

        //Parameterized Constructor
        public DataContext(string connectionString) { ConnectionString = connectionString; }

        // Utility to Set the connection string
        public void SetConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public async Task<long> AddAsync<T>(T obj)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    var query = _queryBuilder.GetInsertRawQuery(obj);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            cmd.Parameters.AddWithValue(prop.Name, _queryBuilder.GetColumnValue(prop, obj));
                        }

                        int result = await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw ex;
            }
        }

        public async Task<long> UpdateAsync<T>(T obj, string q = "", params object[] p)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    var queryString = String.Format(q, p);
                    var query = _queryBuilder.GetUpdateRawQuery(obj, queryString);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            cmd.Parameters.AddWithValue(prop.Name, _queryBuilder.GetColumnValue(prop, obj));
                        }

                        int result = await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw ex;
            }
        }
      
        public async Task<List<T>> SelectAsync<T>()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    var tInstance = (T)Activator.CreateInstance<T>();
                    var query = _queryBuilder.GetSelectRawQuery(tInstance);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        await da.FillAsync(dt);
                        List<T> lstResult = DataTableUtility.DataTableToList<T>(dt);
                        return lstResult;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw ex;
            }
        }

        public async Task<long> DeleteAsync<T>(string q = "", params object[] p)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    var queryString = String.Format(q, p);
                    var tInstance = (T)Activator.CreateInstance<T>();
                    var query = _queryBuilder.GetDeleteRawQuery(tInstance, queryString);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        int result = await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw ex;
            }
        }
    }
}
