using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Dz19
{
    [Serializable]
    public class Article
    {
        public string Title { get; set; }
        public int CharacterCount { get; set; }
        public string Preview { get; set; }

        public Article(string title, int characterCount, string preview)
        {
            Title = title;
            CharacterCount = characterCount;
            Preview = preview;
        }
    }

    [Serializable]
    public class Magazine
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public List<Article> Articles { get; set; }

        public Magazine(string title, string publisher, DateTime publicationDate, int pageCount, List<Article> articles)
        {
            Title = title;
            Publisher = publisher;
            PublicationDate = publicationDate;
            PageCount = pageCount;
            Articles = articles;
        }

        public override string ToString()
        {
            string magazineInfo = $"Назва журналу: {Title}\n" +
                                  $"Видавництво: {Publisher}\n" +
                                  $"Дата видання: {PublicationDate.ToShortDateString()}\n" +
                                  $"Кількість сторінок: {PageCount}\n\n" +
                                  "Статті:\n";

            foreach (Article article in Articles)
            {
                magazineInfo += $"Назва статті: {article.Title}\n" +
                                $"Кількість символів: {article.CharacterCount}\n" +
                                $"Анонс статті: {article.Preview}\n\n";
            }

            return magazineInfo;
        }
    }
    internal class cs2
    {
        public static void task_2()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Magazine[] magazines = CreateMagazineArray();

            Console.WriteLine("Введіть номер журналу, щоб вивести інформацію:");
            int magazineIndex = GetValidMagazineIndex(magazines);

            PrintMagazineInfo(magazines[magazineIndex]);

            byte[] serializedData = SerializeMagazine(magazines[magazineIndex]);
            string filePath = "magazine.bin";
            SaveSerializedDataToFile(serializedData, filePath);

            byte[] loadedData = LoadSerializedDataFromFile(filePath);
            Magazine deserializedMagazine = DeserializeMagazine(loadedData);

            Console.WriteLine("Після десеріалізації з файлу:");
            PrintMagazineInfo(deserializedMagazine);


        }
        static Magazine[] CreateMagazineArray()
        {
            Console.Write("Введіть кількість журналів: ");
            int magazineCount;
            while (!int.TryParse(Console.ReadLine(), out magazineCount) || magazineCount <= 0)
            {
                Console.WriteLine("Некоректне значення. Введіть ціле додатнє число:");
            }

            Magazine[] magazines = new Magazine[magazineCount];

            for (int i = 0; i < magazineCount; i++)
            {
                Console.WriteLine($"Журнал #{i + 1}");
                magazines[i] = CreateMagazine();
                Console.WriteLine();
            }

            return magazines;
        }

        static Magazine CreateMagazine()
        {
            Console.Write("Введіть назву журналу: ");
            string title = Console.ReadLine();

            Console.Write("Введіть назву видавництва: ");
            string publisher = Console.ReadLine();

            Console.Write("Введіть дату видання (у форматі дд.мм.рррр): ");
            DateTime publicationDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out publicationDate))
            {
                Console.WriteLine("Некоректний формат. Введіть дату у форматі дд.мм.рррр:");
            }

            Console.Write("Введіть кількість сторінок: ");
            int pageCount;
            while (!int.TryParse(Console.ReadLine(), out pageCount) || pageCount <= 0)
            {
                Console.WriteLine("Некоректне значення. Введіть ціле додатнє число:");
            }

            List<Article> articles = CreateArticleList();

            return new Magazine(title, publisher, publicationDate, pageCount, articles);
        }

        static List<Article> CreateArticleList()
        {
            Console.Write("Введіть кількість статей: ");
            int articleCount;
            while (!int.TryParse(Console.ReadLine(), out articleCount) || articleCount <= 0)
            {
                Console.WriteLine("Некоректне значення. Введіть ціле додатнє число:");
            }

            List<Article> articles = new List<Article>();

            for (int i = 0; i < articleCount; i++)
            {
                Console.WriteLine($"Стаття #{i + 1}");
                articles.Add(CreateArticle());
                Console.WriteLine();
            }

            return articles;
        }

        static Article CreateArticle()
        {
            Console.Write("Введіть назву статті: ");
            string title = Console.ReadLine();

            Console.Write("Введіть кількість символів: ");
            int characterCount;
            while (!int.TryParse(Console.ReadLine(), out characterCount) || characterCount <= 0)
            {
                Console.WriteLine("Некоректне значення. Введіть ціле додатнє число:");
            }

            Console.Write("Введіть анонс статті: ");
            string preview = Console.ReadLine();

            return new Article(title, characterCount, preview);
        }

        static void PrintMagazineInfo(Magazine magazine)
        {
            Console.WriteLine(magazine.ToString());
        }

        static byte[] SerializeMagazine(Magazine magazine)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, magazine);
                return stream.ToArray();
            }
        }

        static void SaveSerializedDataToFile(byte[] data, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(data, 0, data.Length);
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

        static Magazine DeserializeMagazine(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                return (Magazine)formatter.Deserialize(stream);
            }
        }

        static int GetValidMagazineIndex(Magazine[] magazines)
        {
            int magazineIndex;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out magazineIndex) && magazineIndex >= 1 && magazineIndex <= magazines.Length)
                {
                    return magazineIndex - 1;
                }
                else
                {
                    Console.WriteLine("Некоректний номер журналу. Введіть номер журналу зі списку:");
                }
            }
        }
    }
}
