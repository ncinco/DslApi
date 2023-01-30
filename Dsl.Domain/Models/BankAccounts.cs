using Newtonsoft.Json;

namespace Dsl.Domain.Models
{
    public class BankAccounts
    {
        public Guid TokenisedCif { get; set; }

        public List<AccountDetail> Accounts { get; set; }
    }

    public class AccountDetail
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("product_number")]
        public string ProductNumber { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("account_hold_flag")]
        public string AccountHoldFlag { get; set; }

        [JsonProperty("account_branch")]
        public string AccountBranch { get; set; }

        [JsonProperty("account_open_datetime")]
        public DateTimeOffset AccountOpenDatetime { get; set; }

        [JsonProperty("account_closure_datetime")]
        public DateTimeOffset? AccountClosureDatetime { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("account_balance")]
        public decimal AccountBalance { get; set; }

        [JsonProperty("available_balance")]
        public decimal AvailableBalance { get; set; }

        public List<Relationship> Relationships { get; set; }
    }

    public class Relationship
    {
        [JsonProperty("tokenised_cif")]
        public string TokenisedCif { get; set; }

        [JsonProperty("link_type")]
        public string LinkType { get; set; }
    }
}