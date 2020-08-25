using System.Collections.Generic;
using NUnit.Framework;
using Zentil.Core;

namespace Zentil.__tests__.Edit_Mode.Core {
  public class StateStore_test {
    [Test]
    public void ItWorksWithoutUnregisteredController() {
      var store = new StateStore();
      Assert.Throws<KeyNotFoundException>(() => {
        store.GetState<float>("NonExistingController");
      });
    }

    [Test]
    public void ItRegistersStateControllers() {
      var store = new StateStore();
      var controller = new CurrentHealthController();

      store.AddController("CurrentHealth", controller);

      var currentHealth = store.GetState<float>("CurrentHealth");

      Assert.AreEqual(currentHealth, controller.GetState());
    }

    [Test]
    public void AllIsWell() {
      var store = new StateStore();

      store.AddController("CurrentHealth", new CurrentHealthController());

      store.Dispatch(new StoreAction {
        Type = "UpdateCurrentHealth",
        Payload = 20f
      });

      Assert.AreEqual(store.GetState<float>("CurrentHealth"), 20f);
    }
  }

  public class CurrentHealthController : IStoreController {
    private float state = 0f;
    public dynamic GetState() => state;

    public void HandleAction(StoreAction action) {
      if (action.Type == "UpdateCurrentHealth") {
        this.state = (float) action.Payload;
      }
    }
  }
}