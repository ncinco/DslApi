using Dsl.Domain.Models;

namespace Dsl.Infrastructure.Services
{
    public interface IRestSharpWrapperService
    {
        Task<RestResponseResult<Transaction>> GetStargateAsync();
    }

    public class RestSharpWrapperService : IRestSharpWrapperService
    {
        public RestSharpWrapperService()
        {

        }

        public async Task<RestResponseResult<Transaction>> GetStargateAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Cassandra-Token", "AstraCS:AZjhNqTMgAilAZrCDBGejmtg:32372b11a3dd57f6e99092c8219939460084adfb5e1bd640309d14168a5bfd79");

            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page-size", "20");
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

            var response = await client.GetAsync($"https://0b743e1f-1d8d-476d-a769-a3f511e7edd3-australiaeast.apps.astra.datastax.com/api/rest/v2/keyspaces/asb/transactions_by_account/rows?{queryString}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var transactions = Newtonsoft.Json.JsonConvert.DeserializeObject<RestResponseResult<Transaction>>(jsonString);

            return transactions;
        }
    }
}