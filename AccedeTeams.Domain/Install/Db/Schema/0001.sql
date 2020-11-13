create table app.PlayerLike (
	PlayerCustomEntityId int not null,
	UserId int not null,
	CreateDate datetime2(7) not null,

	constraint PK_PlayerLike primary key (PlayerCustomEntityId, UserId),
	constraint FK_PlayerLike_PlayerCustomEntity foreign key (PlayerCustomEntityId) references Cofoundry.CustomEntity (CustomEntityId) on delete cascade,
	constraint FK_PlayerLike_User foreign key (UserId) references Cofoundry.[User] (UserId) on delete cascade
)

create table app.PlayerLikeCount (
	PlayerCustomEntityId int not null,
	TotalLikes int not null,

	constraint PK_PlayerLikeCount primary key (PlayerCustomEntityId),
	constraint FK_PlayerLikeCount_PlayerCustomEntity foreign key (PlayerCustomEntityId) references Cofoundry.CustomEntity (CustomEntityId) on delete cascade
)