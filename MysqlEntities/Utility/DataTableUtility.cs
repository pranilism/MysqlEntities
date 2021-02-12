using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

internal class DataTableUtility
{
    /// <summary>
    /// Utility function to convert the Datatable in to the List
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="dt">Datatable Object to be converted into List<T></param>
    /// <returns></returns>
    protected internal static List<T> DataTableToList<T>(DataTable dt)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }

    private static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(obj, (dr[column.ColumnName] is null) ? "NULL" : dr[column.ColumnName], null);
                else
                    continue;
            }
        }
        return obj;
    }
}