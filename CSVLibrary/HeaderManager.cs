using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    internal class HeaderManager
    {
        // 取得第一筆資料
        // 產生新的資料列表(把第一筆資料從資料源去除)
        // 判斷是否有 header
        // 沒有 header
        // header 有缺漏(內容錯誤)
        // h1,h2,h3,h4,h5
        // 1,,2,,4
        public Dictionary<string, int> headerPlace = new Dictionary<string, int>();

        public void GetFileHeaderPlace(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string[] headerArray = sr.ReadLine().Split(',');
                for (int i = 0; i < headerArray.Length; i++)
                {
                    headerPlace[headerArray[i]] = i;
                }
            }
        }

        public bool HeadersCheck<T>(string path)
        {

            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            string propHeader = string.Join("", props.Select(p => p.Name).ToList());

            using (StreamReader sr = new StreamReader(path))
            {
                if (sr.EndOfStream) return false;

                string[] headerArray = sr.ReadLine().Split(',');

                string pathHeader = string.Join("", headerArray);

                if (pathHeader != propHeader) return false;
            }
            return true;
        }

        public string ReProduceNewData(string path)
        {
            string dataReadToEnd;

            using (StreamReader sr = new StreamReader(path))
            {
                dataReadToEnd = sr.ReadToEnd().TrimEnd();
            }
            return string.IsNullOrWhiteSpace(dataReadToEnd) ? null : dataReadToEnd.TrimEnd();

            //return dataReadToEnd;
        }

        public List<string> RemoveHeader(string path)
        {
            List<string> dataList = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    dataList.Add(line);
                }
            }
            if (dataList.Count > 0)
                dataList.RemoveAt(0);
            return dataList;
        }

        public void AddHeader<T>(string path)
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            string propHeader = string.Join(",", props.Select(p => p.Name).ToList());

            string originData = ReProduceNewData(path);

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(propHeader);
                if (originData == null || originData == "") { return; }
                sw.WriteLine(originData);
            }
        }
    }
}
