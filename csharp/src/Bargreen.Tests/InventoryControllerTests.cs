using Bargreen.API.Controllers;
using Bargreen.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryControllerTests
    {
        [Fact]
        public async void InventoryController_Can_Return_Inventory_Balances()
        {
            var dependencyInjection = new InventoryService();
            var controller = new InventoryController(dependencyInjection);
            var result = await controller.GetInventoryBalances();
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Controller_Methods_Are_Async()
        {
            var methods = typeof(InventoryController)
                .GetMethods()
                .Where(m=>m.DeclaringType==typeof(InventoryController));

            Assert.All(methods, m =>
            {
                Type attType = typeof(AsyncStateMachineAttribute); 
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }
    }
}
