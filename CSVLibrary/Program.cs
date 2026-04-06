using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 資料讀取的Model 跟資料寫入的Model 不會是同一個
            // DAO => Data Access Object=> 資料儲存物件
            // DTO => Data Transfer Object => 資料轉換物件

            // read header 問題
            // 10~20張 5MB 圖檔

            string filepath = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\Accounting\\WriteTextAsync.txt";
            //string writepath = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\Accounting\\WriteTextAsync-2.txt";

            //CSVHelper.CheckFile(filepath);



            List<Record> record_list = CSVHelper.Read<Record>(filepath);
            foreach (Record record in record_list)
            {
                Type type = record.GetType();
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    Console.WriteLine($"PropertyName: {prop.Name}, Value: {prop.GetValue(record)}");
                }
                CSVHelper.Write("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\Accounting\\WriteTextAsync-2.txt", record, true);
            }
            //CSVHelper.WriteList(writepath, record_list, true);


            // List<Student> list = CSVHelper.Read<Student>(filePath);
            // CSVHelper.Write<Student>(list);


        }
    }
}
