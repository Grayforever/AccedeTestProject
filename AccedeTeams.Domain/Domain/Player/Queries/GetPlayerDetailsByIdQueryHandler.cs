using AccedeTeams.Data;
using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class GetPlayerDetailsByIdQueryHandler
        : IAsyncQueryHandler<GetPlayerDetailsByIdQuery, PlayerDetails>
        , IIgnorePermissionCheckHandler
    {
        private readonly ICustomEntityRepository _customEntityRepository;
        private readonly IImageAssetRepository _imageAssetRepository;
        private readonly IQueryExecutor _queryExecutor;
        private readonly AccedeTeamsDbContext _dbContext;

        public GetPlayerDetailsByIdQueryHandler(
            ICustomEntityRepository customEntityRepository,
            IImageAssetRepository imageAssetRepository,
            IQueryExecutor queryExecutor,
            AccedeTeamsDbContext dbContext
            )
        {
            _customEntityRepository = customEntityRepository;
            _imageAssetRepository = imageAssetRepository;
            _queryExecutor = queryExecutor;
            _dbContext = dbContext;
        }

        public async Task<PlayerDetails> ExecuteAsync(GetPlayerDetailsByIdQuery query, IExecutionContext executionContext)
        {
            var customEntityQuery = new GetCustomEntityRenderSummaryByIdQuery(query.PlayerId);
            var customEntity = await _customEntityRepository.GetCustomEntityRenderSummaryByIdAsync(customEntityQuery); ;
            if (customEntity == null) return null;

            return await MapPlayer(customEntity);
        }

        private async Task<PlayerDetails> MapPlayer(CustomEntityRenderSummary customEntity)
        {
            var model = customEntity.Model as PlayerDataModel;
            var player = new PlayerDetails();

            player.PlayerId = customEntity.CustomEntityId;
            player.Name = customEntity.Title;
            player.Description = model.Description;
            player.Features = await GetFeaturesAsync(model.FeatureIds);
            player.Images = await GetImagesAsync(model.ImageAssetIds);
            player.TotalLikes = await GetLikeCount(customEntity.CustomEntityId);

            return player;
        }

        private Task<int> GetLikeCount(int catId)
        {
            return _dbContext
                .PlayerLikeCounts
                .AsNoTracking()
                .Where(p => p.PlayerCustomEntityId == catId)
                .Select(p => p.TotalLikes)
                .FirstOrDefaultAsync();
        }

        private async Task<ICollection<Feature>> GetFeaturesAsync(ICollection<int> featureIds)
        {
            if (EnumerableHelper.IsNullOrEmpty(featureIds)) return Array.Empty<Feature>();
            var query = new GetFeaturesByIdRangeQuery(featureIds);

            var features = await _queryExecutor.ExecuteAsync(query);

            return features
                .Select(g => g.Value)
                .OrderBy(h => h.Title)
                .ToList();
        }

        private async Task<ICollection<ImageAssetRenderDetails>> GetImagesAsync(ICollection<int> imageAssetIds)
        {
            if (EnumerableHelper.IsNullOrEmpty(imageAssetIds)) return Array.Empty<ImageAssetRenderDetails>();

            var images = await _imageAssetRepository.GetImageAssetRenderDetailsByIdRangeAsync(imageAssetIds);

            return images
                .FilterAndOrderByKeys(imageAssetIds)
                .ToList();
        }
    }
}
