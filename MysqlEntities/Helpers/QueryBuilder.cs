using MysqlEntities.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

internal class QueryBuilder : IQueryBuilder
{
    //Valued Query Methods
    /// <summary>
    /// This method generates the Select query for specified type with values specified in inline query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">Object of type T</param>
    /// <returns>string</returns>
    [Obsolete("Use Raw Methods to generate queries.", true)]
    public string GetSelectQuery<T>(T obj)
    {
        if (obj != null)
        {
            string selectionProperties = GetPropertiesString(obj);
            string selectQuery = $"SELECT {selectionProperties} FROM {typeof(T).Name}";
            return selectQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the Insert query for specified type with values specified in inline query
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <returns>string</returns>
    [Obsolete("Use Raw Methods to generate queries.", true)]
    public string GetInsertQuery<T>(T obj)
    {
        if (obj != null)
        {
            string selectionProperties = GetPropertiesString(obj);
            string propertyValuesString = GetPropertiesValuesString(obj);
            string insertQuery = $"INSERT INTO {typeof(T).Name} ({selectionProperties}) VALUES ({propertyValuesString})";
            return insertQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the Delete query for specified type with values specified in inline query
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <param name="whereConditionColumn"></param>
    /// <param name="whereConditionValue"></param>
    /// <returns>string</returns>
    [Obsolete("Use Raw Methods to generate queries.", true)]
    public string GetDeleteQuery<T>(T obj, string whereConditionColumn, string whereConditionValue)
    {
        if (obj != null)
        {
            string selectQuery = $"DELETE FROM {typeof(T).Name} WHERE {whereConditionColumn} = {whereConditionValue}";
            return selectQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the Update query for specified type with values specified in inline query
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <param name="whereCondition"></param>
    /// <returns>string</returns>
    [Obsolete("Use Raw Methods to generate queries.", true)]
    public string GetUpdateQuery<T>(T obj, string whereCondition)
    {
        if (obj != null)
        {
            var allProps = GetProperties(obj);
            List<string> lstColumnValues = new List<string>();
            foreach (var prop in allProps)
            {
                lstColumnValues.Add($"{prop.Name}={GetTypedValues(prop, obj)}");
            }
            var columnValuesString = String.Join(",", lstColumnValues);
            string updateQuery = $"UPDATE {typeof(T).Name} SET {columnValuesString}";
            var qry = whereCondition.Length > 0 ? $" WHERE { whereCondition}" : whereCondition;
            return updateQuery + qry;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }


    //Raw Query Methods
    /// <summary>
    /// This method generates the Select query for specified type with sql placeholders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string GetSelectRawQuery<T>(T obj)
    {
        if (obj != null)
        {
            string selectionProperties = GetPropertiesString(obj);
            string selectQuery = $"SELECT {selectionProperties} FROM {typeof(T).Name}";
            return selectQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the Insert query for specified type with sql placeholders
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <returns>string</returns>
    public string GetInsertRawQuery<T>(T obj)
    {
        if (obj != null)
        {
            string selectionProperties = GetPropertiesString(obj);
            string propertiesPlaceholder = GetPropertiesPlaceholderString(obj);
            string insertQuery = $"INSERT INTO {typeof(T).Name} ({selectionProperties}) VALUES ({propertiesPlaceholder})";
            return insertQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the Update query for specified type with sql placeholders
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <param name="whereCondition"></param>
    /// <returns>string</returns>
    public string GetUpdateRawQuery<T>(T obj, string whereCondition)
    {
        if (obj != null)
        {
            var allProps = GetProperties(obj);
            List<string> lstColumnValues = new List<string>();
            foreach (var prop in allProps)
            {
                lstColumnValues.Add($"{prop.Name}=@{prop.Name}");
            }
            var columnValuesString = String.Join(",", lstColumnValues);
            string updateQuery = $"UPDATE {typeof(T).Name} SET {columnValuesString}";
            var qry = whereCondition.Length > 0 ? $" WHERE { whereCondition }" : whereCondition;
            return updateQuery + qry;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }

    /// <summary>
    /// This method generates the delete query for specified type with sql placeholders
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <param name="whereCondition"></param>
    /// <returns>string</returns>
    public string GetDeleteRawQuery<T>(T obj, string whereCondition)
    {
        if (obj != null)
        {
            string deleteQuery = $"DELETE FROM {typeof(T).Name} WHERE {whereCondition}";
            return deleteQuery;
        }
        else
        {
            throw new DbObjectNullException($"The object of type {obj.GetType()} should not be null");
        }
    }


    //Property Helpers
    private string GetPropertiesString<T>(T obj)
    {
        PropertyInfo[] allProperties = typeof(T).GetProperties();
        IEnumerable<string> lstProperties = from p in allProperties select p.Name;
        var propertiesString = String.Join(",", lstProperties);
        return propertiesString;
    }

    private string GetPropertiesValuesString<T>(T obj)
    {
        PropertyInfo[] allProperties = typeof(T).GetProperties();
        IEnumerable<object> lstValues = from p in allProperties select GetTypedValues(p, obj);
        var valuesString = String.Join(",", lstValues);
        return valuesString;
    }

    private PropertyInfo[] GetProperties<T>(T obj)
    {
        PropertyInfo[] allProperties = typeof(T).GetProperties();
        return allProperties;
    }

    /// <summary>
    /// This method is used to get the typed value of columns
    /// </summary>
    /// <param name="prop">PropertyInfo[] of Object of type T</param>
    /// <param name="obj">Object of type T</param>
    /// <returns>object</returns>
    private object GetTypedValues(PropertyInfo prop, object obj)
    {
        if (prop.PropertyType == typeof(string))
        {
            return $"'{prop.GetValue(obj)}'";
        }
        else
        {
            return Convert.ChangeType(prop.GetValue(obj), prop.PropertyType);
        }
    }

    /// <summary>
    /// This method is used to get the value of column
    /// </summary>
    /// <param name="prop">PropertyInfo[] of Object of type T</param>
    /// <param name="obj">Object of type T</param>
    /// <returns>object</returns>
    public object GetColumnValue(PropertyInfo prop, object obj)
    {
        if (prop.PropertyType == typeof(string))
        {
            return $"{prop.GetValue(obj)}";
        }
        else
        {
            if (prop.GetValue(obj) is null)
            {
                return null;
            }

            return Convert.ChangeType(prop.GetValue(obj), prop.PropertyType);
        }
    }


    //Placeholder Getter for Raw Queries
    private string GetPropertiesPlaceholderString<T>(T obj)
    {
        PropertyInfo[] allProperties = typeof(T).GetProperties();
        IEnumerable<string> lstProperties = from p in allProperties select "@" + p.Name;
        var propertiesString = String.Join(",", lstProperties);
        return propertiesString;
    }
}
