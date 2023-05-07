using SqlSugar;

namespace RoslynCat.SQL
{
    public class CodeSampleQuery
    {
        private readonly ISqlSugarClient _db;

        public CodeSampleQuery(ISqlSugarClient db) {
            _db = db;
        }
        public async Task<List<CodeSampleGroupAndTitle>> GetGroupAndTitleList() {
            return await _db.Queryable<CodeSample>()
                .Select(c => new CodeSampleGroupAndTitle { Id = c.Id,Group = c.Group,Title = c.Title })
                .ToListAsync();
        }
    }
}
