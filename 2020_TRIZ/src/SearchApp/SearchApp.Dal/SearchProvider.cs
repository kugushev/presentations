using System;
using System.Linq;
using Nest;
using SearchApp.Domain;

namespace SearchApp.Dal
{
    public class SearchProvider
    {
        public void ExecuteSearch(SearchRequestEntity searchRequest)
        {
            var client = new ElasticClient();

            new TermQueryEntity()
            {
                Boost = default,
                IsStrict = default,
                IsVerbatim = default,
                IsWritable = default,
                Name = default,
            };
            
            client.Search<FinancialData>(s => s.Query(
                    q => q.Bool(b => b
                        .Should(
                            searchRequest.Queries
                                .Select<TermQueryEntity, Func<QueryContainerDescriptor<FinancialData>, QueryContainer>>
                                (sq =>
                                    bs => bs.Term(
                                        p => new TermQuery
                                        {
                                            Field = sq.Field, 
                                            Value = sq.Value,
                                            Boost = sq.Boost,
                                            IsStrict = sq.IsStrict,
                                            IsVerbatim = sq.IsVerbatim,
                                            Name = sq.Name,
                                        }))
                                .ToArray()
                        )
                    )
                )
            );
        }
    }
}