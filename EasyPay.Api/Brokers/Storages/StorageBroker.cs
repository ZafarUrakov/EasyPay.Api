//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        public IQueryable<T> SelectAll<T>(T @object) where T : class
        {
            var broker = new StorageBroker(configuration);

            return broker.Set<T>();
        }

        public async ValueTask<T> SelectAsync<T>(params object[] objectsId) where T : class
        {
            var broker = new StorageBroker(configuration);

            return await broker.FindAsync<T>(objectsId);
        }

        public async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new StorageBroker(configuration);
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new StorageBroker(configuration);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration
                .GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
