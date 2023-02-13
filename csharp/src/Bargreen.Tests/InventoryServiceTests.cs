using System;
using System.Collections.Generic;
using Xunit;
using Bargreen.Services;
using System.Linq;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            //TODO-CHALLENGE: Verify expected output of your recon algorithm. Note, this will probably take more than one test

            var inventorySystem = new List<InventoryBalance>() {
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 312,
                     WarehouseLocation = "WLA1"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 146,
                     WarehouseLocation = "WLA2"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ZZZ99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 47,
                     WarehouseLocation = "WLA3"
                },
                new InventoryBalance()
                {
                     ItemNumber = "zzz99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 91,
                     WarehouseLocation = "WLA4"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxccM",
                     PricePerItem = 245.25M,
                     QuantityOnHand = 32,
                     WarehouseLocation = "WLA5"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxddM",
                     PricePerItem = 747.47M,
                     QuantityOnHand = 15,
                     WarehouseLocation = "WLA6"
                }
            };

            var accountingSystem = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3435M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbr77",
                     TotalInventoryValue = 17.99M
                }
            };

            var expected = new List<InventoryReconciliationResult>()
            {
                  new InventoryReconciliationResult {
                    ItemNumber = "abc123",
                    TotalValueOnHandInInventory = 3435M,
                    TotalValueInAccountingBalance = 3435M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "zzz99",
                    TotalValueOnHandInInventory = 1930.62M,
                    TotalValueInAccountingBalance = 1930.62M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "xxccm",
                    TotalValueOnHandInInventory = 7848M,
                    TotalValueInAccountingBalance = 7602.75M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "xxddm",
                    TotalValueOnHandInInventory = 11212.05M,
                    TotalValueInAccountingBalance = null
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "fbr77",
                    TotalValueOnHandInInventory = null,
                    TotalValueInAccountingBalance = 17.99M
                  }
            };

            var expected2 = new List<InventoryReconciliationResult>()
            {
                  new InventoryReconciliationResult {
                    ItemNumber = "abc123",
                    TotalValueOnHandInInventory = 3435M,
                    TotalValueInAccountingBalance = 3435M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "zzz99",
                    TotalValueOnHandInInventory = 1930.62M,
                    TotalValueInAccountingBalance = 1930.62M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "xxccm",
                    TotalValueOnHandInInventory = 7848M,
                    TotalValueInAccountingBalance = 7602.75M
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "xxddm",
                    TotalValueOnHandInInventory = 11212.05M,
                    TotalValueInAccountingBalance = null
                  },
                  new InventoryReconciliationResult {
                    ItemNumber = "fbr77",
                    TotalValueOnHandInInventory = null,
                    TotalValueInAccountingBalance = 17.99M
                  }
            };

            var invService = new InventoryService();
            var actual = await invService.ReconcileInventoryToAccounting(inventorySystem, accountingSystem);



            Assert.Equal(expected, actual, new InventoryReconciliationResultComparer());
            
        }
    }

    public class InventoryReconciliationResultComparer : IEqualityComparer<InventoryReconciliationResult>
    {
        public bool Equals(InventoryReconciliationResult x, InventoryReconciliationResult y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.ItemNumber == y.ItemNumber &&
                   x.TotalValueOnHandInInventory == y.TotalValueOnHandInInventory &&
                   x.TotalValueInAccountingBalance == y.TotalValueInAccountingBalance;
        }

        public int GetHashCode(InventoryReconciliationResult obj)
        {
            return obj.ItemNumber.GetHashCode();
        }
    }

}
