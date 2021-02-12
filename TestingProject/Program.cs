using MysqlEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingProject
{
    class Program
    {
        public class m_tbluser
        {
            public long userId { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
            public DateTime addedon { get; set; }
        }

        public static string connection_string = "server=localhost;database=userdb;user=root;password=mysql";

        static void Main(string[] args)
        {
            IDataContext context = new DataContext(connection_string);

            //To Insert into table
            long res = Task.Run(() => context.AddAsync(new m_tbluser
            {
                userId = 0,
                userName = "Pranil Testing",
                password = "psttesting",
                addedon = DateTime.Now
            })).Result;
            if (res > 0) Console.WriteLine("Data Saved Successfully.\n");

            //To Get from table
            List<m_tbluser> lstUsers = Task.Run(() => context.SelectAsync<m_tbluser>()).Result;
            PrintList(lstUsers);

            //To Update the tuple from table
            var lastObject = lstUsers[lstUsers.Count - 1];
            lastObject.userName = "dummyGod";
            lastObject.password = "DummyGod1234";
            Task.Run(() => context.UpdateAsync(lastObject, "userId={0}", lastObject.userId));

            //To Delete the tuple from table
            var firstObject = lstUsers[0];
            Task.Run(() => context.DeleteAsync<m_tbluser>("userId={0}", firstObject.userId));

            List<m_tbluser> updatedUserList = Task.Run(() => context.SelectAsync<m_tbluser>()).Result;
            PrintList(updatedUserList);

            Console.ReadLine();
        }

        public static void PrintList(List<m_tbluser> lstUsers)
        {
            Console.WriteLine("userId\tuserName\tpassword");
            Console.WriteLine("_______________________________________________________________________\n");

            foreach (var item in lstUsers)
            {
                Console.WriteLine($"{item.userId}\t{item.userName}\t{item.password}");
                Console.WriteLine("-------------------------------------------------------------------");
            }
        }
    }
}
