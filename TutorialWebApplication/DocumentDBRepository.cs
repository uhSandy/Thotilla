using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TutorialWebApplication
{

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System.Collections.ObjectModel;

    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId1 = ConfigurationManager.AppSettings["collection1"];
        private static readonly string CollectionId2 = ConfigurationManager.AppSettings["collection2"];
        private static DocumentClient client;

        /*Cry detect DB configuration start*/
        public static async Task<Document> CryDetectAsync(T sound)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1), sound);
        }
        /*Cry detect DB configuration end*/

        /*Mobile response for baby cry start*/
        public static async Task<Document> CryMobileResAsync(Models.Sound sound)
        {
            // return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId2), sound);

            Document doc = client.CreateDocumentQuery<Document>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1))
                    .Where(f => f.Id == sound.Id).AsEnumerable().SingleOrDefault();

            //Update some properties on the found resource
            doc.SetPropertyValue("responseDone", sound.ResponseDone);
            doc.SetPropertyValue("response", sound.Response);
            doc.SetPropertyValue("dateTime", sound.DateTime);

            //Now persist these changes to the database by replacing the original resource
            Document updated = await client.ReplaceDocumentAsync(doc);

            return updated;
        }
        /*Mobile response for baby cry end*/

        /*Post change iot done response start*/
        public static async Task<Document> UpdateMobileResAsync(Models.Sound sound)
        {
            //Fetch the Document to be updated
            /* Document doc = client.CreateDocumentQuery<Document>(DatabaseId,CollectionId2)
                                        .Where(r => r.Id == "9")
                                        .AsEnumerable()
                                        .SingleOrDefault();*/

            Document doc = client.CreateDocumentQuery<Document>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1))
                    .Where(f => f.Id == sound.Id).AsEnumerable().SingleOrDefault();

            //Update some properties on the found resource
            doc.SetPropertyValue("status", sound.Status);

            //Now persist these changes to the database by replacing the original resource
            Document updated = await client.ReplaceDocumentAsync(doc);

            return updated;
        }
        /*Post change iot done response end*/

        public static async Task<T> GetResponseAsync(string id)
        {
           try
            {
                Document document =
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId1, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<T> GetSoundAsync(string id)
        {
            try
            {
               // ExecuteSimpleQuery(DatabaseId, CollectionId2);

                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId2, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static Models.Sound mobileNotifyBabyCry(string guId)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Models.Sound> soundQuery = client.CreateDocumentQuery<Models.Sound>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1), queryOptions)
                    .Where(f => f.GuId == guId);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (Models.Sound sound in soundQuery)
            {
             //   if (sound.Status) {
                    Console.WriteLine("\tRead {0}", sound);
                    return sound;
             //   }
            }

            // Now execute the same query via direct SQL
            /*  IQueryable<Family> familyQueryInSql = this.client.CreateDocumentQuery<Family>(
                      UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                      "SELECT * FROM Family WHERE Family.LastName = 'Andersen'",
                      queryOptions);

              Console.WriteLine("Running direct SQL query...");
              foreach (Family family in familyQueryInSql)
              {
                  Console.WriteLine("\tRead {0}", family);
              }

              Console.WriteLine("Press any key to continue ...");
              Console.ReadKey();*/

            return null;
        }

        public static Models.Sound mobileResponseForBabyCry(string guId)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Models.Sound> soundQuery = client.CreateDocumentQuery<Models.Sound>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1), queryOptions)
                    .Where(f => f.GuId == guId);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (Models.Sound sound in soundQuery)
            {
                if (sound.ResponseDone && sound.Response != null && !sound.Response.Equals(""))
                {
                    Console.WriteLine("\tRead {0}", sound);
                    return sound;
                }
            }

            return null;
        }

        public static async Task<Document> CreateItemAsync(T person)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1), person);
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            //CreateDatabaseIfNotExistsAsync().Wait();
            //CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId1));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection
                        {
                            Id = CollectionId1,
                           // PartitionKey = new PartitionKeyDefinition() { Paths = new Collection<string>() { "" } }
                        },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document =
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId1, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}