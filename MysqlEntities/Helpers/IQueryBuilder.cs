using System.Reflection;

internal interface IQueryBuilder
{
    //Valued Query functions
    string GetSelectQuery<T>(T obj);
    string GetInsertQuery<T>(T obj);
    string GetUpdateQuery<T>(T obj, string whereCondition);
    string GetDeleteQuery<T>(T obj, string whereConditionColumn, string whereConditionValue);

    //Raw Query functions
    string GetSelectRawQuery<T>(T obj);
    string GetInsertRawQuery<T>(T obj);
    string GetUpdateRawQuery<T>(T obj, string whereCondition);
    string GetDeleteRawQuery<T>(T obj, string whereCondition);

    //Property Utils
    object GetColumnValue(PropertyInfo prop, object obj);
}
