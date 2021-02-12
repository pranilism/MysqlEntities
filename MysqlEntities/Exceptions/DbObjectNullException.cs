using System;
using System.Collections.Generic;
using System.Text;

namespace MysqlEntities.Exceptions
{
    public class DbObjectNullException : Exception
    {
        public DbObjectNullException() { }
        public DbObjectNullException(string exceptionString) : base(exceptionString) { }
    }
}
