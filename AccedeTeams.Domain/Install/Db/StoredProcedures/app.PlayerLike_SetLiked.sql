create procedure app.PlayerLike_SetLiked
	(
		@PlayerId int,
		@UserId int,
		@IsLiked bit,
		@CreateDate datetime2
	)
	as
begin

	if (@IsLiked = 1)
	begin
		merge app.PlayerLike as destination
		using (values (@PlayerId, @UserId, @CreateDate)) src (PlayerCustomEntityId, UserId, CreateDate)
        on destination.UserId = src.UserId AND destination.PlayerCustomEntityId = src.PlayerCustomEntityId
		when not matched then
			insert (PlayerCustomEntityId, UserId, CreateDate)
			values (src.PlayerCustomEntityId, src.UserId, src.CreateDate);
	end
	else
	begin
		delete from app.PlayerLike where PlayerCustomEntityId = @PlayerId and UserId = @UserId
	end

	merge app.PlayerLikeCount as destination
		using (
			select @PlayerId, Count(UserId) 
			from app.PlayerLike cl
			right outer join Cofoundry.CustomEntity c on cl.PlayerCustomEntityId = c.CustomEntityId
			where cl.PlayerCustomEntityId = @PlayerId or c.CustomEntityId = @PlayerId
			group by cl.PlayerCustomEntityId
		) src (PlayerCustomEntityId, TotalLikes)
        on destination.PlayerCustomEntityId = src.PlayerCustomEntityId
	when matched then 
		update set destination.TotalLikes = src.TotalLikes
    when not matched then
        insert (PlayerCustomEntityId, TotalLikes)
        values (src.PlayerCustomEntityId, src.TotalLikes);

end