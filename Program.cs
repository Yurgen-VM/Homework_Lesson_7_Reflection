using System;
using System.Reflection;
using System.Text;

namespace Task_1
{
    /*
    
    Разработайте атрибут позволяющий методу ObjectToString сохранять поля классов с использованием произвольного имени.
    Метод StringToObject должен также уметь работать с этим атрибутом для записи значение в свойство по имени его атрибута.
    
    */

    internal class Program
    {

        public static TestClass MakeTypeTestClass()
        {
            Type testClass = typeof(TestClass);
            return Activator.CreateInstance(testClass) as TestClass;
        }

        public static TestClass MakeTypeTestClass(int i)
        {
            Type testClass = typeof(TestClass);
            return Activator.CreateInstance(testClass, new object[] { i }) as TestClass;
        }

        public static TestClass MakeTypeTestClass(int i, string s, decimal d, char[] c)
        {
            Type testClass = typeof(TestClass);
            return Activator.CreateInstance(testClass, new object[] { i, s, d, c }) as TestClass;
        }

        static string intName = "I";
        static string stringName = "S";
        static string decimalName = "D";
        static string charName = "C";

        static void PrintPropertisObject(TestClass obj)
        {
            Console.WriteLine($"{intName}: {obj.I}");
            Console.WriteLine($"{stringName}: {obj.S}");
            Console.WriteLine($"{decimalName}: {obj.D}");
            Console.Write($"{charName}: ");
            foreach (var item in obj.C)
            {
                Console.Write(item);
            }
            Console.WriteLine("\n");
        }

        static string ObjectToString(object obj)
        {
            Type type = obj.GetType();
            StringBuilder res = new StringBuilder();
            res.Append(type.AssemblyQualifiedName + ":");
            res.Append(type.Name + '|');
            var propertis = type.GetProperties();
            foreach (var prop in propertis)
            {
                var temp = prop.GetValue(obj);
                var atr = prop.GetCustomAttribute(typeof(CustomNameAttribute), false);               

                if (atr is CustomNameAttribute attribute)
                {
                    res.Append(attribute.name + ":");
                   
                }
                else
                {
                    res.Append(prop.Name + ":");
                }
                if (prop.PropertyType == typeof(char[]))
                {
                    res.Append(new string(temp as char[]) + "|");
                }
                else
                {
                    res.Append(temp);
                    res.Append('|');
                }
            }
            return res.ToString();
        }


        static object StringToObject(string s)
        {
            string[] strArray = s.Split('|'); // Конвертируем нашу строку в масив, разделяя элементы по |
            string[] element = strArray[0].Split(':'); // Разделяем первый элемент массива по :
            object? obj = Activator.CreateInstance(element[0].Split(",")[1], element[0].Split(",")[0])?.Unwrap();
                        
            if (strArray.Length > 1 && obj != null)
            {
                Type type = obj.GetType();
                for (int i = 1; i < strArray.Length - 1; i++)
                {
                    string[] nameAndValue = strArray[i].Split(':');
                    PropertyInfo[] propertis = type.GetProperties();

                    var atr = propertis[i - 1].GetCustomAttribute(typeof(CustomNameAttribute), false);
                    CustomNameAttribute? atribute = atr as CustomNameAttribute;
                    
                    if (atr is CustomNameAttribute && propertis[i - 1].PropertyType == typeof(int))
                    {
                        propertis[i - 1].SetValue(obj, int.Parse(nameAndValue[1]));
                        intName = atribute.name;
                    }
                    else if (propertis[i - 1].PropertyType == typeof(int))
                    {
                        propertis[i - 1].SetValue(obj, int.Parse(nameAndValue[1]));
                        intName = nameAndValue[0];
                    }
                    else if (atr is CustomNameAttribute && propertis[i - 1].PropertyType == typeof(string))
                    {
                        propertis[i - 1].SetValue(obj, nameAndValue[1]);
                        stringName = atribute.name;
                    }
                    else if (propertis[i - 1].PropertyType == typeof(string))
                    {
                        propertis[i - 1].SetValue(obj, nameAndValue[1]);
                        stringName = nameAndValue[0];
                    }
                    else if (atr is CustomNameAttribute && propertis[i - 1].PropertyType == typeof(decimal))
                    {
                        propertis[i - 1].SetValue(obj, decimal.Parse(nameAndValue[1]));
                        decimalName = atribute.name;
                    }
                    else if (propertis[i - 1].PropertyType == typeof(decimal))
                    {
                        propertis[i - 1].SetValue(obj, decimal.Parse(nameAndValue[1]));
                        decimalName = nameAndValue[0];
                    }
                    else if (atr is CustomNameAttribute && propertis[i - 1].PropertyType == typeof(char[]))
                    {
                        propertis[i - 1].SetValue(obj, nameAndValue[1].ToCharArray());
                        charName = atribute.name;
                    }
                    else if (propertis[i - 1].PropertyType == typeof(char[]))
                    {
                        propertis[i - 1].SetValue(obj, nameAndValue[1].ToCharArray());
                        charName = nameAndValue[0];
                    }
                }
            }
            return obj;
        }

        static void Main(string[] args)
        {
            char[] arrChars = { 'a', 'b', 'c' };
            var n3 = MakeTypeTestClass(2, "строка", 1234m, arrChars);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Исходный объект:");
            Console.ResetColor();
            PrintPropertisObject(n3);
            string res = ObjectToString(n3);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Строка полученная при помощи рефлексии:");
            Console.ResetColor();
            Console.WriteLine($"{res}\n");

            var obj = StringToObject(res) as TestClass;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Объект полученный из строки при помощи рефлексии c именем атрибута:");
            Console.ResetColor();
            if (obj != null)
                PrintPropertisObject(obj);

            Console.ReadLine();
        }
    }
}
