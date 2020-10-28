DROP TABLE RunnerProperties
DROP TABLE TeamScores
DROP TABLE TeamLogEntries
DROP TABLE Members
DROP TABLE Teams


-- [TeamPassword] = 'Team' (hashed)
INSERT INTO [Teams]
    ([Name], [SubscriptionId], [TenantId], [TeamPassword])
VALUES
	( 'Gefährliches Halbwissen', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Power Rangers', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Teddybären', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Do.Fail.Learn.Repeat.', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Fortuna Azure', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Club der Architekten', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Azure Dummies', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Newbies', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Only Cloud!', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Added Value', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'We Scale', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' ),
	( 'Azure Monkeys', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','egeI+nzHhEmXV9uTAfXGfnxjL69/bKAE6RsPJoWGHAA=' )



INSERT INTO [TeamScores]
    ([TeamId], [Wins], [Loses], [Errors], [Income], [Costs])
VALUES
    ( 1,  945, 10, 5, 1012,    501 ),
    ( 2,  872, 110, 15, 1011,  502 ),
    ( 3,  793, 150, 25, 1010,  503 ),
    ( 4,  623, 210, 35, 1009,  504 ),
    ( 5,  563, 300, 45, 1008,  505 ),
    ( 6,  519, 311, 55, 1007,  506 ),
    ( 7,  434, 412, 65, 1006,  507 ),
    ( 8,  498, 413, 75, 1005,  508 ),
    ( 9,  345, 514, 85, 1004,  509 ),
    ( 10, 294, 615, 95, 1003,  510 ),
    ( 11, 230, 616, 105, 1002, 511 ),
	( 12, 101, 717, 106, 1001, 512 )


DELETE FROM [TeamLogEntries]

INSERT INTO [TeamLogEntries]
    ([TeamId], [Timestamp], [ResponeTimeMs], [Status], [Reason])
VALUES
	( 1, '2020-09-12T08:51:47.1234567', 10, 'SUCCESS', 'Smoorgh has won $20'),
	( 1, '2020-09-12T08:52:57.1234567', 10, 'SUCCESS', 'Human has won $20'),
	( 1, '2020-09-12T08:53:57.1234567', 10, 'FAILED', 'HTTP Status Code 500 received'),
	( 1, '2020-09-12T08:54:47.1234567', 10, 'FAILED', 'Timeout 5s happened'),
	( 1, '2020-09-12T08:55:47.1234567', 10, 'FAILED', 'Network Exception received'),
	( 1, '2020-09-12T08:56:47.1234567', 10, 'ATTACKED', 'Hacker was able to get access into the system'),
	( 1, '2020-09-12T08:57:47.1234567', 10, 'CANCELED','Smoorgh has canceled the game, because the stakes from humans are too low'),
	( 1, '2020-10-02T13:36:00.1234567', 10, 'SUCCESS', 'Smoorgh has won $20'),
	( 1, '2020-10-02T13:34:48.1234567', 10, 'SUCCESS', 'Human has won $20'),
	( 1, '2020-10-02T13:35:49.1234567', 10, 'FAILED', 'HTTP Status Code 500 received'),
	( 1, '2020-10-02T13:36:50.1234567', 10, 'FAILED', 'Timeout 5s happened'),
	( 1, '2020-10-02T13:37:51.1234567', 10, 'FAILED', 'Network Exception received'),
	( 1, '2020-10-02T13:38:52.1234567', 10, 'ATTACKED', 'Hacker was able to get access into the system'),
	( 1, '2020-10-02T13:39:53.1234567', 10, 'CANCELED','Smoorgh has canceled the game, because the stakes from humans are too low')


INSERT INTO [Members]
    ([TeamId], [Username], [Password])
VALUES
    ( 1, 'fromsample_1@asmw13.onmicrosoft.com', 'SuperSecret A' ),
    ( 1, 'fromsample_2@asmw13.onmicrosoft.com', 'SuperSecret B' )
