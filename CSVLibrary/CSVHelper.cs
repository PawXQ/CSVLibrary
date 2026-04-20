using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    public class CSVHelper
    {
        public static List<T> Read<T>(string path) where T : class, new()
        {
            List<T> _list = new List<T>();
            Type type = typeof(T);

            HeaderManager headerManager = new HeaderManager();
            // Object Relaction Mapping => ORM 物件關聯關係對應
            headerManager.GetFileHeaderPlace(path);

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine(); // exclude headers

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    string[] datas = line.Split(',');
                    T t = new T();

                    for (int j = 0; j < datas.Length; j++)
                    {
                        PropertyInfo[] props = type.GetProperties();

                        props[j].SetValue(t, datas[headerManager.headerPlace[props[j].Name]]);
                    }

                    _list.Add(t);
                }
            }

            return _list;
        }


        public static List<T> Read<T>(string path, int start, int quantity) where T : class, new()
        {
            List<T> _list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            HeaderManager headerManager = new HeaderManager();
            headerManager.GetFileHeaderPlace(path);

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine(); // exclude headers

                int lineCount = 0;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    lineCount++;

                    if (lineCount >= start && lineCount < start + quantity)
                    {
                        string[] datas = line.Split(',');
                        T t = new T();

                        for (int j = 0; j < datas.Length; j++)
                        {
                            props[j].SetValue(t, datas[headerManager.headerPlace[props[j].Name]]);
                        }

                        _list.Add(t);
                    }

                    if (lineCount >= start + quantity) break;
                }
            }

            return _list;
        }

        public static void Write<T>(string path, T t, bool hasAddHeader = false)
        {
            List<T> values = new List<T>() { t };

            WriteList(path, values, hasAddHeader);
        }

        public static void WriteList<T>(string path, List<T> t, bool hasAddHeader = false) // TODO:埋個伏筆
        {
            CheckFile(path);

            HeaderManager headerManager = new HeaderManager();
            bool headerCheck;

            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            headerCheck = headerManager.HeadersCheck<T>(path);

            if (!headerCheck && hasAddHeader) { headerManager.AddHeader<T>(path); }

            using (StreamWriter outputFile = new StreamWriter(path, true))
            {
                foreach (T _t in t)
                {
                    List<string> string_props_value = new List<string>();
                    foreach (PropertyInfo prop in props) { string_props_value.Add(prop.GetValue(_t).ToString()); }
                    string text = string.Join(",", string_props_value);

                    outputFile.WriteLine(text);
                }
            }
        }

        public static void CheckFile(string path)
        {
            string[] pathArray = path.Split('\\');
            string directory = string.Join("\\", pathArray.Take(pathArray.Length - 1).ToArray());

            string[] fileArray = pathArray[pathArray.Length - 1].Split('.');
            string extension = fileArray[fileArray.Length - 1];

            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }


            if (extension != "csv")
            {
                throw new FileNotFoundException(pathArray[pathArray.Length - 1]);
            }

            if (!File.Exists(path)) { File.Create(path).Close(); }
        }
    }
}
