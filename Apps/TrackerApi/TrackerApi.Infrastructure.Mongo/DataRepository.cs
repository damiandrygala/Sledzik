using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Infrastructure.Mongo
{
    public class DataRepository : IDataRepository
    {
        #region General // 10.09.2020

        public IMongoDatabase Database { get; set; }

        public DataRepository(IOptions<MongoConfiguration> mongoConfiguration)
        {
             MongoClient client = new MongoClient(mongoConfiguration.Value.Database);

             Database = client.GetDatabase(mongoConfiguration.Value.DatabaseName);

        }
        #endregion

        #region General Create // 16.04.2021
        public async Task Create<TDocument>(TDocument document) where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(document.GetType().Name);
            await collection.InsertOneAsync(document);
        }

        #endregion

        #region General Delete // 16.04.2021

        public async Task Delete<TDocument>(TDocument document) where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(document.GetType().Name);
            await collection.DeleteOneAsync(d => d.Id == document.Id);
        }

        #endregion

        #region General Update // 16.04.2021

        public async Task Update<TDocument>(TDocument document) where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(document.GetType().Name);
            await collection.ReplaceOneAsync(d => d.Id == document.Id, document);
        }

        #endregion

        #region General Get // 20.04.2021

        public async Task<TDocument> Get<TDocument>(string id) where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(typeof(TDocument).Name);
            var result = await collection.Find(d => d.Id == id).SingleOrDefaultAsync();
            return result;
        }

        public Task<IQueryable<TDocument>> GetAll<TDocument>() where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(typeof(TDocument).Name);
            return Task.FromResult((IQueryable<TDocument>)collection.AsQueryable());
        }

        public async Task<long> GetAllCount<TDocument>(Expression<Func<TDocument, bool>> expression) where TDocument : IEntity
        {
            IMongoCollection<TDocument> collection = Database.GetCollection<TDocument>(typeof(TDocument).Name);
            return await collection.CountDocumentsAsync(expression);
        }

        #endregion

        #region Kolekcja // 10.07.2022

        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            var dataDocumentName = typeof(T).Name;

            return Database.GetCollection<T>(dataDocumentName);
        }

        #endregion
    }

    public interface IDataRepository
    {
        /// ////////////////////////////////////////////////////////////
        //// General methods
        /// ////////////////////////////////////////////////////////////
        Task Create<TDocument>(TDocument document) where TDocument : IEntity;
        Task Delete<TDocument>(TDocument document) where TDocument : IEntity;
        Task Update<TDocument>(TDocument document) where TDocument : IEntity;
        Task<TDocument> Get<TDocument>(string id) where TDocument : IEntity;
        Task<IQueryable<TDocument>> GetAll<TDocument>() where TDocument : IEntity;
        IMongoCollection<T> GetCollection<T>() where T : class;

    }
}