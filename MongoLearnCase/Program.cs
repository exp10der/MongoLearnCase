namespace MongoLearnCase
{
    using System;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;

    internal class Program
    {
        //λ C:\mongodb\bin\mongod.exe --dbpath C:\MongoData
        private static void Main()
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var client = new MongoClient("mongodb://192.168.1.40:27017");
            var database = client.GetDatabase("Task");
            var collection = database.GetCollection<BsonDocument>("todo");


            var document = new BsonDocument
            {
                {"MongoDB", "Learn"},
                {"LINQ", "GOOD"},
                {"C#", "GOOD"},
                {
                    "Library", new BsonDocument
                    {
                        {"AutoMapper", 100},
                        {"StructureMap", 200},
                        {"DelegateDecompiler", 50}
                    }
                }
            };


            var doc = collection.Find(new BsonDocument()).FirstOrDefault();

            doc.InvokeIfNull(async () => await collection.InsertOneAsync(document));

            var filter = collection.Find(new BsonDocument());
            Console.WriteLine(filter);
            doc = await filter.FirstOrDefaultAsync();

            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
            Console.WriteLine(doc.ToString());
            Console.WriteLine();
        }
    }

    public static class Ex
    {
        public static void InvokeIfNull<TInput>(this TInput input, Action func)
        {
            if (input != null) return;
            func();
        }
    }
}