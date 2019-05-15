USE [UglyExpressShop]
SELECT [Location].*, [Country].[Name] AS Country, [City].[Name] AS City, [Address].[Name] AS [Address] 
FROM [Location]
LEFT JOIN [Country] ON [Location].[CountryId] = [Country].[Id]
LEFT JOIN [City] ON [Location].[CityId] = [City].[Id]
LEFT JOIN [Address] ON [Location].[AddressId] = [Address].[Id]