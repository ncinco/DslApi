using Cassandra;
using Cassandra.Mapping;
using Dsl.Domain.Models;
using Dsl.Infrastructure.Repositories;
using Dsl.Infrastructure.Services;
using Dsl.Infrastructure.Services.Performance;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dsl.Infrastructure
{
    public static class Startup
    {
        public static void Configure(IApplicationBuilder app)
        {
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            ConfigureDataStax(services);
            ConfigureRepositories(services);
            ConfigureInfraServices(services);
        }

        private static void ConfigureDataStax(IServiceCollection services)
        {
            var session = Cluster.Builder()
                .WithCloudSecureConnectionBundle(@"secure-connect-transactions.zip")
                .WithCredentials("AZjhNqTMgAilAZrCDBGejmtg", "ed2hryN3vtrxJScvLocdFNncCSWAfIUur6tDCc3dMtdETdSQzApAjbCaJPuUiPL0r2y12_J.0aIEKA.73FH8I2qUrqynMZki45n_8HZ3n56H1aOjzJeCUBGgwL3GB2LA")
                .Build()
                .Connect("asb");

            MappingConfiguration.Global.Define(
               new Map<AccountWithCif>()
                  .TableName("account_with_cif")
                  .Column(u => u.CifId, cm => cm.WithName("cifid"))
                  .Column(u => u.AccountNumber, cm => cm.WithName("accountnumber"))
                  .Column(u => u.AccountType, cm => cm.WithName("accounttype")));

            MappingConfiguration.Global.Define(
               new Map<Transaction>()
                  .TableName("transactions_by_account")
                  .Column(u => u.TransactionId, cm => cm.WithName("transactionid"))
                  .Column(u => u.AccountNumber, cm => cm.WithName("accountnumber"))
                  .Column(u => u.Amount, cm => cm.WithName("amount"))
                  .Column(u => u.Code, cm => cm.WithName("code"))
                  .Column(u => u.Description, cm => cm.WithName("description"))
                  .Column(u => u.Particular, cm => cm.WithName("particular"))
                  .Column(u => u.ProccessedDate, cm => cm.WithName("proccesseddate"))
                  .Column(u => u.Status, cm => cm.WithName("status"))
                  .Column(u => u.TransactionDate, cm => cm.WithName("transactiondate")));

            MappingConfiguration.Global.Define(
               new Map<FileUpload>()
                  .TableName("transactions_blobs")
                  .Column(u => u.Id, cm => cm.WithName("id"))
                  .Column(u => u.FileName, cm => cm.WithName("filename"))
                  .Column(u => u.Image, cm => cm.WithName("image")));

            var mapper = new Mapper(session);

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<ISession>(session);
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<ICassandraRepository, CassandraRepository>();
        }

        private static void ConfigureInfraServices(IServiceCollection services)
        {
            services.AddSingleton<IRestSharpWrapperService, RestSharpWrapperService>();
            services.AddSingleton<IStargatePerformanceService, StargatePerformanceService>();
            services.AddSingleton<ICSharpDriverPerformanceService, CSharpDriverPerformanceService>();
        }
    }
}