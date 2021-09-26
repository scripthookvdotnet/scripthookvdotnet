//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTA
{
	public sealed class WeaponComponentCollection
	{
		#region Fields
		readonly Ped _owner;
		readonly Weapon _weapon;
		readonly Dictionary<WeaponComponentHash, WeaponComponent> _weaponComponents = new Dictionary<WeaponComponentHash, WeaponComponent>();
		readonly WeaponComponentHash[] _components;
		readonly static WeaponComponent _invalidComponent = new WeaponComponent(null, null, WeaponComponentHash.Invalid);
		#endregion

		internal WeaponComponentCollection(Ped owner, Weapon weapon)
		{
			_owner = owner;
			_weapon = weapon;
			_components = GetComponentsFromHash(weapon.Hash);
		}

		public WeaponComponent this[int index]
		{
			get
			{
				WeaponComponent component = null;
				if (index >= 0 && index < Count)
				{
					WeaponComponentHash componentHash = _components[index];

					if (!_weaponComponents.TryGetValue(componentHash, out component))
					{
						component = new WeaponComponent(_owner, _weapon, componentHash);
						_weaponComponents.Add(componentHash, component);
					}
					return component;
				}
				else
				{
					return _invalidComponent;
				}
			}
		}

		public WeaponComponent this[WeaponComponentHash componentHash]
		{
			get
			{
				if (_components.Contains(componentHash))
				{
					WeaponComponent component = null;
					if (!_weaponComponents.TryGetValue(componentHash, out component))
					{
						component = new WeaponComponent(_owner, _weapon, componentHash);
						_weaponComponents.Add(componentHash, component);
					}

					return component;
				}
				else
				{
					return _invalidComponent;
				}
			}
		}

		public int Count => _components.Length;

		public IEnumerator<WeaponComponent> GetEnumerator()
		{
			WeaponComponent[] AllComponents = new WeaponComponent[Count];
			for (int i = 0; i < Count; i++)
			{
				AllComponents[i] = this[_components[i]];
			}
			return (AllComponents as IEnumerable<WeaponComponent>).GetEnumerator();
		}

		public WeaponComponent GetClipComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Clip ||
					component.AttachmentPoint == WeaponAttachmentPoint.Clip2)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int ClipVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.Clip ||
					component.AttachmentPoint == WeaponAttachmentPoint.Clip2)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetScopeComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Scope ||
					component.AttachmentPoint == WeaponAttachmentPoint.Scope2)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int ScopeVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.Scope ||
					component.AttachmentPoint == WeaponAttachmentPoint.Scope2)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetBarrelComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Barrel)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int BarrelVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.Barrel)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetGunRootComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.GunRoot)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int GunRootVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.GunRoot)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetSuppressorComponent()
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Supp ||
					component.AttachmentPoint == WeaponAttachmentPoint.Supp2)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		public WeaponComponent GetFlashLightComponent()
		{
			foreach (var component in this)
			{
				// COMPONENT_AT_RAILCOVER_01 is the only component that attaches the flashlight position other than actual flashlights
				if (component.ComponentHash == WeaponComponentHash.AtRailCover01)
					continue;

				if (component.AttachmentPoint == WeaponAttachmentPoint.FlashLaser ||
					component.AttachmentPoint == WeaponAttachmentPoint.FlashLaser2)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		[Obsolete("WeaponComponentCollection.GetLuxuryFinishComponent is wrongly named and cannot necessarily get all of the components for gun_root (e.g. camo components), please use WeaponComponentCollection.GetGunRootComponent instead.")]
		public WeaponComponent GetLuxuryFinishComponent()
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.GunRoot)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		static WeaponComponentHash[] GetComponentsFromHash(WeaponHash hash)
		{
			return SHVDN.NativeMemory.GetAllCompatibleWeaponComponentHashes((uint)hash).Select(x => (WeaponComponentHash)x).ToArray();
		}
	}
}
