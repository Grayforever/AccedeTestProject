using AccedeTeams.Data;
using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class SearchPlayerSummariesQueryHandler : IAsyncQueryHandler<SearchPlayerSummariesQuery, PagedQueryResult<PlayerSummary>>, IIgnorePermissionCheckHandler
    {
        private readonly ICustomEntityRepository _customEntityRepository;
        private readonly IImageAssetRepository _imageAssetRepository;
        private readonly AccedeTeamsDbContext _dbContext;

        public SearchPlayerSummariesQueryHandler(ICustomEntityRepository customEntityRepository, IImageAssetRepository imageAssetRepository, AccedeTeamsDbContext dbContext)
        {
            _customEntityRepository = customEntityRepository;
            _imageAssetRepository = imageAssetRepository;
            _dbContext = dbContext;
        }

        public async Task<PagedQueryResult<PlayerSummary>> ExecuteAsync(SearchPlayerSummariesQuery query, IExecutionContext executionContext)
        {
            var customEntityQuery = new SearchCustomEntityRenderSummariesQuery();
            customEntityQuery.CustomEntityDefinitionCode = PlayerCustomEntityDefinition.DefinitionCode;
            customEntityQuery.PageSize = query.PageSize = query.PageSize;
            customEntityQuery.PageNumber = query.PageNumber;
            customEntityQuery.PublishStatus = PublishStatusQuery.Published;
            customEntityQuery.SortBy = CustomEntityQuerySortType.PublishDate;

            var catCustomEntities = await _customEntityRepository.SearchCustomEntityRenderSummariesAsync(customEntityQuery);
            var allMainImages = await GetMainImages(catCustomEntities);
            var allLikeCounts = await GetLikeCounts(catCustomEntities);

            return MapPlayers(catCustomEntities, allMainImages, allLikeCounts);
        }

        private Task<IDictionary<int, ImageAssetRenderDetails>> GetMainImages(PagedQueryResult<CustomEntityRenderSummary> customEntityResult)
        {
            return _imageAssetRepository.GetImageAssetRenderDetailsByIdRangeAsync(customEntityResult
                .Items
                .Select(i => (PlayerDataModel)i.Model)
                .Where(m => !EnumerableHelper.IsNullOrEmpty(m.ImageAssetIds))
                .Select(m => m.ImageAssetIds.First())
                .Distinct());
        }

        private Task<Dictionary<int, int>> GetLikeCounts(PagedQueryResult<CustomEntityRenderSummary> customEntityResult)
        {
            return _dbContext
                .PlayerLikeCounts
                .AsNoTracking()
                .Where(c => customEntityResult
                .Items
                .Select(i => i.CustomEntityId)
                .Distinct()
                .ToList().Contains(c.PlayerCustomEntityId))
                .ToDictionaryAsync(c => c.PlayerCustomEntityId, c => c.TotalLikes);
        }

        private PagedQueryResult<PlayerSummary> MapPlayers(
            PagedQueryResult<CustomEntityRenderSummary> customEntityResult,
            IDictionary<int, ImageAssetRenderDetails> images,
            IDictionary<int, int> allLikeCounts
            )
        {
            var players = new List<PlayerSummary>(customEntityResult.Items.Count());
            foreach (var (customEntity, model, player) in from customEntity in customEntityResult.Items
                                                          let model = (PlayerDataModel)customEntity.Model
                                                          let player = new PlayerSummary()
                                                          select (customEntity, model, player))
            {
                player.PlayerId = customEntity.CustomEntityId;
                player.Name = customEntity.Title;
                player.Description = model.Description;
                player.TotalLikes = allLikeCounts.GetOrDefault(customEntity.CustomEntityId);
                if (!EnumerableHelper.IsNullOrEmpty(model.ImageAssetIds))
                {
                    player.MainImage = images.GetOrDefault(model.ImageAssetIds.FirstOrDefault());
                }

                players.Add(player);
            }

            return customEntityResult.ChangeType(players);
        }
    }
}
