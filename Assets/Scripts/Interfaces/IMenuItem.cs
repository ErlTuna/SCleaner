using UnityEngine.EventSystems;

// interface that gathers expected behavior from Menu Items
// not all menu items have these functionality but its meant to be an umbrella
interface IMenuItem : ISelectHandler, IDeselectHandler, ISubmitHandler { }