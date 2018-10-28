
var searchResults = searchClient.Client.Search<Submitter>(x => x
    .Index(clientIndexName)
    .Type(_elasticConfiguration.UserInfoTypeName)
    .From(query.PagingInfo.StartRow)
    .Size(query.PagingInfo.PageSize + 1)
    .Sort(s => s.Ascending(f => f.UserName.Suffix(KeywordExt)))
    .Query(
        q => q.Bool(
            b => b.MustNot(
                m => m.Term(t => t.Field(f => f.IsGlobalAdmin).Value(true)),
                m => m.Term(t => t.Field(f => f.IsClientAdmin).Value(true)),
                m => m.Term(t => t.Field(f => f.IsAllClientsAdmin).Value(true))
                )
                .Filter(f => f.Bool(
                    innerbool => innerbool.Should(
                        s => s.Match(m => m.Query(query.Search).Field(innerf => innerf.UserName).Operator(Operator.And)),
                        s => s.Match(m => m.Query(query.Search).Field(innerf => innerf.EmailAddress).Operator(Operator.And)))
                        )
                )
         )
     ));

public Result ComplicatedDal()
{
    ....
}

class EntityRepository
{
    public IEnumerable<Entity> Find(string search);
}


[Test]
public void Find_EmptySearch_ReturnsAll();

[Test]
public void Find_1WordSearch_ReturnsStartsWith();

[Test]
public void Find_2WordsSearch_ReturnsByNGram();

[Test]
public void Find_SearchNotExists_ReturnsEmpty();

[Test]
public void Find_ExistsButNoPermissions_ReturnsEmpty();

legacy.Legacy();
legacy1= legacy.Legacy + legacyLegacy;
legacy1.Legacy(le, ga, cy);

//legacy.CATabilityLevel = 42;

legacy42 = legacy
    .Where(l => l.IsLeagacy)
    .Select(l => l.Legacy);
legacyService.MakeLegacy(legacy42);

