using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SimpleAuth.Api.Utilities.Interface;
using System;

namespace SimpleAuth.Api.Repository
{
    public class BaseRepository<T>
    {
        protected readonly string CollectionName;

        protected IMongoClient Client { get; set; }

        protected IMongoDatabase Database { get; set; }
        
        protected IMongoCollection<T> Collection => this.Database.GetCollection<T>(this.CollectionName);

        protected IConfigurationUtility ConfigurationUtility { get; set; }

        public BaseRepository(IConfigurationUtility configurationUtility, string collection)
        {
            this.ConfigurationUtility = configurationUtility;
            this.CollectionName = collection;

            var server = new MongoServerAddress(
                this.ConfigurationUtility.DatabaseConnectionHost,
                Int32.Parse(this.ConfigurationUtility.DatabaseConnectionPort));

            var settings = new MongoClientSettings()
            {
                Server = server,
                MaxConnectionIdleTime = TimeSpan.FromMilliseconds(45000)
            };

            MongoCredential credentials = null;
            if (string.IsNullOrWhiteSpace(this.ConfigurationUtility.DatabaseConnectionUsername) == false)
            {
                credentials = MongoCredential.CreateCredential(this.ConfigurationUtility.DatabaseConnectionName,
                    this.ConfigurationUtility.DatabaseConnectionUsername,
                    this.ConfigurationUtility.DatabaseConnectionPassword);
                settings.Credential = credentials;
            }

            this.Client = new MongoClient(settings);
            this.Database = this.Client.GetDatabase(this.ConfigurationUtility.DatabaseConnectionName);
        }

        public static void InitializeRepository()
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));

            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String)

            };

            ConventionRegistry.Register("Convention Global", conventionPack, t => true);
        }
    }
}