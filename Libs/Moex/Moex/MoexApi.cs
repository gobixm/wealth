using Moex.Abstractions;
using Moex.Contracts;
using RestSharp;
using Throw;

namespace Moex;

public sealed class MoexApi : IMoexApi
{
    private readonly RestClient _client;

    public MoexApi()
    {
        _client = new RestClient("http://iss.moex.com");
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<IReadOnlyCollection<Security>> GetSecuritiesAsync(int start, int limit,
        CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("iss/securities.xml")
            .AddQueryParameter("securities.columns", "secid,name")
            .AddQueryParameter("iss.meta", "off")
            .AddQueryParameter("group_by_filter", "common_share")
            .AddQueryParameter("group_by", "type")
            .AddQueryParameter("start", start)
            .AddQueryParameter("limit", limit);

        var response = await _client.GetAsync<MoexResponse>(request, cancellationToken);
        response.ThrowIfNull();

        return response.Data.Rows.Rows
            .Select(x => new Security(x.GetAttributeValue("secid")!, x.GetAttributeValue("name")!))
            .ToList();
    }

    public async Task<IReadOnlyCollection<IndexSecurity>> GetIndexSecuritiesAsync(string index, int start, int limit,
        CancellationToken cancellationToken = default)
    {
        var request = new RestRequest($"iss/statistics/engines/stock/markets/index/analytics/{index}.xml")
            .AddQueryParameter("analytics.columns", "secids,weight")
            .AddQueryParameter("iss.meta", "off")
            .AddQueryParameter("start", start)
            .AddQueryParameter("limit", limit);

        var response = await _client.GetAsync<MoexResponse>(request, cancellationToken);
        response.ThrowIfNull();

        return response.Data.Rows.Rows
            .Select(x =>
                new IndexSecurity(x.GetAttributeValue("secids")!, double.Parse(x.GetAttributeValue("weight")!)))
            .ToList();
    }
}