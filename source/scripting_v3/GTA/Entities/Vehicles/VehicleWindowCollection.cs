using GTA.Native;
using System.Collections.Generic;

namespace GTA
{
	public sealed class VehicleWindowCollection
	{
		#region Fields

		Vehicle _owner;
		readonly Dictionary<VehicleWindowIndex, VehicleWindow> _vehicleWindows = new Dictionary<VehicleWindowIndex, VehicleWindow>();

		#endregion

		internal VehicleWindowCollection(Vehicle owner)
		{
			_owner = owner;
		}


		public VehicleWindow this[VehicleWindowIndex index]
		{
			get
			{
				VehicleWindow vehicleWindow = null;

				if (!_vehicleWindows.TryGetValue(index, out vehicleWindow))
				{
					vehicleWindow = new VehicleWindow(_owner, index);
					_vehicleWindows.Add(index, vehicleWindow);
				}

				return vehicleWindow;
			}
		}

		public bool AreAllWindowsIntact => Function.Call<bool>(Hash.ARE_ALL_VEHICLE_WINDOWS_INTACT, _owner.Handle);

		public void RollDownAllWindows()
		{
			Function.Call(Hash.ROLL_DOWN_WINDOWS, _owner.Handle);
		}
	}
}
