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
    public class GetPlayerSummariesByMemberLikedQueryHandler
        : IAsyncQueryHandler<GetPlayerSummariesByMemberLikedQuery, ICollection<PlayerSummary>>
        , ILoggedInPermissionCheckHandler
    {
        private readonly AccedeTeamsDbContext _dbContext;
        private readonly ICustomEntityRepository _customEntityRepository;
        private readonly IImageAssetRepository _imageAssetRepository;

        public GetPlayerSummariesByMemberLikedQueryHandler(
            ICustomEntityRepository customEntityRepository,
            IImageAssetRepository imageAssetRepository,
            AccedeTeamsDbContext dbContext
            )
        {
            _customEntityRepository = customEntityRepository;
            _imageAssetRepository = imageAssetRepository;
            _dbContext = dbContext;
        }

        public async Task<ICollection<PlayerSummary>> ExecuteAsync(GetPlayerSummariesByMemberLikedQuery query, IExecutionContext executionContext)
        {
            var userCatIds = await _dbContext
                .PlayerLikes
                .AsNoTracking()
                .Where(c => c.UserId == query.UserId)
                .OrderByDescending(c => c.CreateDate)
                .Select(c => c.PlayerCustomEntityId)
                .ToListAsync();

            var customEntityQuery = new GetCustomEntityRenderSummariesByIdRangeQuery(userCatIds);
            var catCustomEntities = await _customEntityRepository.GetCustomEntityRenderSummariesByIdRangeAsync(customEntityQuery);

            // GetByIdRange queries return a dictionary to make lookups easier, so we 
            // have an extra step to do if we want to set the ordering
            var orderedCats = catCustomEntities
                .FilterAndOrderByKeys(userCatIds)
                .ToList();

            var allMainImages = await GetMainImages(orderedCats);
            var allLikeCounts = await GetLikeCounts(orderedCats);

            return MapPlayers(orderedCats, allMainImages, allLikeCounts);
        }

        private Task<IDictionary<int, ImageAssetRenderDetails>> GetMainImages(ICollection<CustomEntityRenderSummary> customEntities)
        {
            var imageAssetIds = customEntities
                .Select(i => (PlayerDataModel)i.Model)
                .Where(m => !EnumerableHelper.IsNullOrEmpty(m.ImageAssetIds))
                .Select(m => m.ImageAssetIds.First())
                .Distinct();

            return _imageAssetRepository.GetImageAssetRenderDetailsByIdRangeAsync(imageAssetIds);
        }

        private Task<Dictionary<int, int>> GetLikeCounts(ICollection<CustomEntityRenderSummary> customEntities)
        {
            var catIds = customEntities
                .Select(i => i.CustomEntityId)
                .Distinct()
                .ToList();

            return _dbContext
                .PlayerLikeCounts
                .AsNoTracking()
                .Where(c => catIds.Contains(c.PlayerCustomEntityId))
                .ToDictionaryAsync(c => c.PlayerCustomEntityId, c => c.TotalLikes);
        }

        private List<PlayerSummary> MapPlayers(
            ICollection<CustomEntityRenderSummary> customEntities,
            IDictionary<int, ImageAssetRenderDetails> images,
            IDictionary<int, int> allLikeCounts
            )
        {
            var players = new List<PlayerSummary>(customEntities.Count());

            foreach (var customEntity in customEntities)
            {
                var model = (PlayerDataModel)customEntity.Model;

                var player = new PlayerSummary();
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

            return players;
        }
    }
}
