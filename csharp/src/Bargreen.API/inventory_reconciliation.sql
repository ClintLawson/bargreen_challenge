--Create a table to hold inventory balances:
declare @inventory table (
    ItemNumber varchar(50) not null,
    WarehouseLocation varchar(50) not null,
    QuantityOnHand int not null,
    PricePerItem decimal(10,2) not null -- **WARNING: had to alter this data field to maintain precision to 2 decimal places
)

--Create a table to hold accounting balances: 
declare @accounting table (
    ItemNumber varchar(50) not null,
    TotalInventoryValue decimal(10,2) not null
)

--Mock up some inventory balances
INSERT INTO @inventory VALUES ('ABC123', 'WLA1', 312, 7.5)
INSERT INTO @inventory VALUES ('ABC123', 'WLA2', 146, 7.5)
INSERT INTO @inventory VALUES ('ZZZ99', 'WLA3', 47, 13.99)
INSERT INTO @inventory VALUES ('zzz99', 'WLA4', 91, 13.99)
INSERT INTO @inventory VALUES ('xxccM', 'WLA5', 32, 245.25)
INSERT INTO @inventory VALUES ('xxddM', 'WLA6', 15, 747.47)

--Mock up some accounting balances
INSERT INTO @accounting VALUES ('ABC123', 3435)
INSERT INTO @accounting VALUES ('ZZZ99', 1930.62)
INSERT INTO @accounting VALUES ('xxccM', 7602.75)
INSERT INTO @accounting VALUES ('fbr77', 17.99)

--TODO-CHALLENGE: Write a query to reconcile matches/differences between the inventory and accounting tables

SELECT 
    coalesce(acc.ItemNumber, inv.ItemNumber) as ItemNumber,
    SUM(inv.TotalValueOnHandInInventory) as TotalValueOnHandInInventory,
    SUM(acc.TotalInventoryValue) as TotalValueInAccountingBalance
FROM (
		SELECT 
			ItemNumber,
			sum(TotalInventoryValue) as TotalInventoryValue
		FROM @accounting
		GROUP BY ItemNumber
	) as acc 
	full join (
		SELECT 
			ItemNumber,
			sum(CONVERT(decimal(10,0), QuantityOnHand) * PricePerItem) as TotalValueOnHandInInventory
		FROM @inventory
		GROUP BY ItemNumber
	) as inv on inv.ItemNumber = acc.ItemNumber
GROUP BY coalesce(acc.ItemNumber, inv.ItemNumber), TotalInventoryValue;
