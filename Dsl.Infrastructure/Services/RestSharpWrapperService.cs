using Dsl.Domain.Models;
using Newtonsoft.Json;
using System.Text;

namespace Dsl.Infrastructure.Services
{
    public interface IRestSharpWrapperService
    {
        Task<RestResponseResult<Transaction>> GetStargateAsync();

        Task<string> UploadFileAsync(FileUploadRequest fileUploadRequest);

        Task<BankAccounts> GetBankAccountsAsync(Guid tokenisedCif);
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

        public async Task<string> UploadFileAsync(FileUploadRequest fileUploadRequest)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Cassandra-Token", "AstraCS:AZjhNqTMgAilAZrCDBGejmtg:32372b11a3dd57f6e99092c8219939460084adfb5e1bd640309d14168a5bfd79");

            byte[] binaryImage;

            using (var stream = new MemoryStream())
            {
                fileUploadRequest.FileDetails.CopyTo(stream);
                binaryImage = stream.ToArray();
            }

            var fileUpload = new FileUpload
            {
                Id = Guid.NewGuid(),
                Image = binaryImage,
                FileName = fileUploadRequest.FileDetails.FileName
            };

            var payload = JsonConvert.SerializeObject(fileUpload);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://0b743e1f-1d8d-476d-a769-a3f511e7edd3-australiaeast.apps.astra.datastax.com/api/rest/v2/keyspaces/asb/transactions_blobs/", content);

            var jsonString = await response.Content.ReadAsStringAsync();

            return jsonString;
        }

        public async Task<BankAccounts> GetBankAccountsAsync(Guid tokenisedCif)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Cassandra-Token", "AstraCS:GzQkJAtvCOTQdhmCeCwmRRKF:2ee9dcf8290aeaa4adba26f91c3da103e295332fcf8fd9442aaf800c7ab1aad3");

            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page-size", "20");
            queryParameters.Add("where", "{\"tokenised_cif\":{\"$eq\":\"" + tokenisedCif.ToString() + "\"}}");
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

            var response = await client.GetAsync($"https://7414b8ee-3b1f-42d0-92c8-6affb805ae1f-australiaeast.apps.astra.datastax.com/api/rest/v2/keyspaces/accounts/bank_accounts_by_cif?{queryString}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var accountDetails = JsonConvert.DeserializeObject<RestResponseResult<AccountDetail>>(jsonString);

            foreach (var accountDetail in accountDetails.Data)
            {
                await GetRelationshipsAsync(accountDetail);
            }

            return new BankAccounts
            {
                TokenisedCif = tokenisedCif,
                Accounts = accountDetails.Data
            };
        }

        private async Task GetRelationshipsAsync(AccountDetail accountDetail)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Cassandra-Token", "AstraCS:GzQkJAtvCOTQdhmCeCwmRRKF:2ee9dcf8290aeaa4adba26f91c3da103e295332fcf8fd9442aaf800c7ab1aad3");

            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page-size", "20");
            queryParameters.Add("where", "{\"account_number\":{\"$eq\":\"" + accountDetail.AccountNumber + "\"}}");
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

            var response = await client.GetAsync($"https://7414b8ee-3b1f-42d0-92c8-6affb805ae1f-australiaeast.apps.astra.datastax.com/api/rest/v2/keyspaces/accounts/bank_account_relationship_by_account_number?{queryString}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var relatioships = JsonConvert.DeserializeObject<RestResponseResult<Relationship>>(jsonString);

            accountDetail.Relationships = relatioships.Data;
        }
    }
}