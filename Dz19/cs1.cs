using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Dz19
{
    [Serializable]
    class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }
    internal class cs1
    {
        public static void task_1()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Fraction[] fractions;

            Console.WriteLine("1. Введення масиву дробів з клавіатури");
            fractions = ReadFractionsFromConsole();

            Console.WriteLine("2. Серіалізація масиву дробів");
            byte[] serializedData = SerializeFractions(fractions);

            Console.WriteLine("3. Збереження серіалізованого масиву у файл");
            SaveSerializedDataToFile(serializedData, "fractions.dat");

            Console.WriteLine("4. Завантаження серіалізованого масиву з файлу");
            byte[] loadedData = LoadSerializedDataFromFile("fractions.dat");
            Fraction[] loadedFractions = DeserializeFractions(loadedData);

            Console.WriteLine("Дроби після завантаження з файлу:");
            PrintFractions(loadedFractions);

            Console.WriteLine();
        }

        static Fraction[] ReadFractionsFromConsole()
        {
            Console.Write("Введіть кількість дробів: ");
            int count = int.Parse(Console.ReadLine());

            Fraction[] fractions = new Fraction[count];

            for (int i = 0; i < count; i++)
            {
                Console.Write($"Введіть чисельник дробу #{i + 1}: ");
                int numerator = int.Parse(Console.ReadLine());

                Console.Write($"Введіть знаменник дробу #{i + 1}: ");
                int denominator = int.Parse(Console.ReadLine());

                fractions[i] = new Fraction(numerator, denominator);
            }

            return fractions;
        }

        static byte[] SerializeFractions(Fraction[] fractions)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, fractions);
                return memoryStream.ToArray();
            }
        }

        static void SaveSerializedDataToFile(byte[] data, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(data, 0, data.Length);
                Console.WriteLine($"Дані збережено у файлі: {filePath}");
            }
        }

        static byte[] LoadSerializedDataFromFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                return data;
            }
        }

        static Fraction[] DeserializeFractions(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (Fraction[])binaryFormatter.Deserialize(memoryStream);
            }
        }

        static void PrintFractions(Fraction[] fractions)
        {
            for (int i = 0; i < fractions.Length; i++)
            {
                Console.WriteLine($"Дріб #{i + 1}: {fractions[i]}");
            }
        }
    }   
}
