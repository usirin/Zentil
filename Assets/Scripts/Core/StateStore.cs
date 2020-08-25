using System.Collections.Generic;
using System.Linq;

namespace Zentil.Core {
  public interface IStoreController {
    dynamic GetState();
    void HandleAction(StoreAction action);
  }

  public class StoreAction {
    public object Payload;
    public string Type;
  }

  public class StateStore {
    private readonly Dictionary<string, IStoreController> controllers = new Dictionary<string, IStoreController>();

    public void AddController(string key, IStoreController controller) {
      this.controllers[key] = controller;
    }

    public void Dispatch(StoreAction action) {
      foreach (var entry in this.controllers) {
        entry.Value.HandleAction(action);
      }
    }

    public T GetState<T>(string key) {
      var controller = this.controllers[key];

      if (controller == null) {
        return default;
      }

      return (T) controller.GetState();
    }
  }
}