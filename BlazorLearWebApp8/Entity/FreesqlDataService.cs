using BootstrapBlazor.Components;
using FreeSql;

namespace BlazorLearWebApp8.Entity
{
    public class FreesqlDataService<TModel> :DataServiceBase<TModel> where TModel:class, new()
    {
        private readonly IFreeSql _db = BaseEntity.Orm;

        public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            await _db.Delete<TModel>(models).ExecuteAffrowsAsync();
            return true;
        }

        public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
        {
            await _db.GetGuidRepository<TModel>().InsertOrUpdateAsync(model);
            return true;
        }

        public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
        {
            var Items = _db.Select<TModel>().WhereDynamicFilter(option.ToDynamicFilter())
                .OrderByPropertyNameIf(option.SortOrder != SortOrder.Unset, option.SortName, option.SortOrder == SortOrder.Asc)
                .Count(out var count)
                .Page(option.PageIndex, option.PageItems).ToList();

            var ret = new QueryData<TModel>()
            {
                TotalCount = (int)count,
                Items = Items,
                IsSorted = option.SortOrder != SortOrder.Unset,
                IsFiltered = option.Filters.Any(),
                IsAdvanceSearch = option.AdvanceSearches.Any(),
                IsSearch = option.Searches.Any() || option.CustomerSearches.Any()
            };

            return Task.FromResult(ret);
        }
    }
}
