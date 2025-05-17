using Nest; // Elasticsearch için kullanılan resmi .NET istemcisi olan Nest kütüphanesini içe aktarıyoruz.
using MovieReviewApi.Interfaces; // Projede tanımlı olan IElasticsearchService arayüzünü kullanmak için içe aktarıyoruz.
using MovieReviewApi.Models; // Movie modelini kullanmak için içe aktarıyoruz.

public class ElasticsearchService : IElasticsearchService
{
    // Elasticsearch istemcisini temsil eden bir alan. Bu, Elasticsearch ile iletişim kurmak için kullanılır.
    private readonly IElasticClient _elasticClient;

    // Constructor (yapıcı metot), Elasticsearch istemcisini bağımlılık enjeksiyonu (Dependency Injection) ile alır.
    public ElasticsearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient; // Bağımlılık enjeksiyonu ile gelen istemciyi sınıfın alanına atıyoruz.
    }

    // Bir filmi Elasticsearch'e indekslemek için kullanılan metot.
    public async Task IndexMovieAsync(Movie movie)
    {
        // Elasticsearch'e bir belge (document) eklemek için IndexDocumentAsync metodu çağrılır.
        var response = await _elasticClient.IndexDocumentAsync(movie);

        // Eğer indeksleme işlemi başarısız olursa, bir hata fırlatılır.
        if (!response.IsValid)
        {
            // Hata mesajını içeren bir istisna (exception) oluşturulur.
            throw new Exception($"Failed to index movie: {response.OriginalException.Message}");
        }
    }

    // Elasticsearch'te belirli bir sorguya (query) göre film aramak için kullanılan metot.
    public async Task<IEnumerable<Movie>> SearchMoviesAsync(string query)
    {
        // Elasticsearch'te arama yapmak için SearchAsync metodu çağrılır.
        var response = await _elasticClient.SearchAsync<Movie>(s => s
            .Query(q => q // Arama sorgusunu tanımlıyoruz.
                .MultiMatch(m => m // MultiMatch, birden fazla alanda (field) arama yapmak için kullanılır.
                    .Fields(f => f // Arama yapılacak alanları belirtiyoruz.
                        .Field(p => p.Title) // Film başlığı (Title) alanında arama yap.
                        .Field(p => p.Description)) // Film açıklaması (Description) alanında arama yap.
                    .Query(query) // Kullanıcının arama sorgusunu (query) burada kullanıyoruz.
                )
            )
        );

        // Arama sonuçlarını döndürüyoruz. Bu, eşleşen belgelerin bir listesidir.
        return response.Documents;
    }
}