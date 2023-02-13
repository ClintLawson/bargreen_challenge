using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        // null values indicate that not only is that system reporting 0 value but it is reporting that the item may not even exist in the given system
        // NOTE: this change would need approval due to the unknown nature of the client consuming this api.
        public decimal? TotalValueOnHandInInventory { get; set; }
        public decimal? TotalValueInAccountingBalance { get; set; }
    }

    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
    }

    public class AccountingBalance
    {
        public string ItemNumber { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public interface IInventoryService
    {
        Task<IEnumerable<InventoryBalance>> GetInventoryBalances();
        Task<IEnumerable<AccountingBalance>> GetAccountingBalances();
        Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);
    }


    public class InventoryService : IInventoryService
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return new List<InventoryBalance>()
            {
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
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return new List<AccountingBalance>()
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
        }

        public async Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences

            Dictionary<string, InventoryReconciliationResult> results = new Dictionary<string, InventoryReconciliationResult>();

            foreach(var item in inventoryBalances)
            {
                var id = item.ItemNumber.ToLower();

                // dump all records from inventory into reconcilliation collection
                if (results.TryGetValue(id, out var result))
                {
                    // in case it exists in multiple locations and already exists
                    result.TotalValueOnHandInInventory = result.TotalValueOnHandInInventory + (item.PricePerItem * item.QuantityOnHand);
                }
                else
                {
                    var record = new InventoryReconciliationResult()
                    {
                        ItemNumber = id,
                        TotalValueOnHandInInventory = item.PricePerItem * item.QuantityOnHand
                    };
                    results.Add(id, record);
                }
            }

            foreach(var item in accountingBalances)
            {
                var id = item.ItemNumber.ToLower();

                // look for existing record from inventory system or create a new one
                if (results.TryGetValue(id, out var result))
                {
                    result.TotalValueInAccountingBalance = item.TotalInventoryValue;
                }
                else
                {
                    var record = new InventoryReconciliationResult()
                    {
                        ItemNumber = id,
                        TotalValueInAccountingBalance = item.TotalInventoryValue
                    };
                    results.Add(id, record);
                }
            }

            return new List<InventoryReconciliationResult>(results.Values);
        }
    }
}