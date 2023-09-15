using System.Text.Json;

namespace GameStore.Api.Endpoints;

public static class HttpResponseExtensions
{
    public static void AddPaginationHeader(
        this HttpResponse resopnse,
        int totalCount,
        int pageSize)
    {
        var paginationHeader = new
        {
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        resopnse.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationHeader));
    }
}