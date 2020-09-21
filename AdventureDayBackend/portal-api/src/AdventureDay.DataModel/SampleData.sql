DROP TABLE RunnerProperties
DROP TABLE TeamScores
DROP TABLE TeamLogEntries
DROP TABLE Members
DROP TABLE Teams


-- [TeamPassword] = 'Team' (hashed)
INSERT INTO [Teams]
    ([Name], [SubscriptionId], [TenantId], [TeamPassword])
VALUES
	( 'Team 01', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 02', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 03', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 04', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 05', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 06', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 07', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 08', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 09', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 10', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 11', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Team 12', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' )



INSERT INTO [TeamScores]
    ([TeamId], [Wins], [Loses], [Errors], [Income], [Costs])
VALUES
    ( 1, 100, 10, 5, 1000, 500 ),
    ( 2, 100, 10, 5, 1000, 500 ),
    ( 3, 100, 10, 5, 1000, 500 ),
    ( 4, 100, 10, 5, 1000, 500 ),
    ( 5, 100, 10, 5, 1000, 500 ),
    ( 6, 100, 10, 5, 1000, 500 ),
    ( 7, 100, 10, 5, 1000, 500 ),
    ( 8, 100, 10, 5, 1000, 500 ),
    ( 9, 100, 10, 5, 1000, 500 ),
    ( 10, 100, 10, 5, 1000, 500 ),
    ( 11, 100, 10, 5, 1000, 500 ),
	( 12, 100, 10, 5, 1000, 500 )


INSERT INTO [TeamLogEntries]
    ([TeamId], [Timestamp], [ResponeTimeMs], [Status], [Reason])
VALUES
	( 1, '2020-09-12T08:51:47.1234567', 10, 'SUCCESS', 'Smoorgh has won $20'),
	( 1, '2020-09-12T08:52:57.1234567', 10, 'SUCCESS', 'Human has won $20'),
	( 1, '2020-09-12T08:53:57.1234567', 10, 'FAILED', 'HTTP Status Code 500 received'),
	( 1, '2020-09-12T08:54:47.1234567', 10, 'FAILED', 'Timeout 5s happened'),
	( 1, '2020-09-12T08:55:47.1234567', 10, 'FAILED', 'Network Exception received'),
	( 1, '2020-09-12T08:56:47.1234567', 10, 'ATTACKED', 'Hacker was able to get access into the system'),
	( 1, '2020-09-12T08:57:47.1234567', 10, 'CANCELED','Smoorgh has canceled the game, because the stakes from humans are too low')


INSERT INTO [Members]
    ([TeamId], [Username], [Password])
VALUES
    ( 1, 'fromsample_1@asmw13.onmicrosoft.com', 'SuperSecret A' ),
    ( 1, 'fromsample_2@asmw13.onmicrosoft.com', 'SuperSecret B' )
