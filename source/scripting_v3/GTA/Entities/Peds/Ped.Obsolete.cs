using System;
using System.ComponentModel;
using GTA.Native;

namespace GTA
{
    public sealed partial class Ped : Entity
    {
        /// <summary>
        /// Spawn an identical clone of this <see cref="Ped"/>.
        /// </summary>
        /// <param name="heading">
        /// This was meant to be the direction the clone should be facing, but has no effect.
        /// </param>
        [Obsolete("`Ped.Clone(float)` is obsolete because the float parameter does not make any sense. " +
                  "Use `Ped.Clone(bool)` instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Ped Clone(float heading = 0.0f)
        {
            const bool RegisterAsNetworkObject = true;
            const bool ScriptHostObject = true;

            // Do not return null even if the native function returns 0, this overload always returns a new `Ped`
            // instance in v3.6.0 and earlier.
            return new Ped(Function.Call<int>(Hash.CLONE_PED, Handle, RegisterAsNetworkObject, ScriptHostObject,
                false));
        }

        [Obsolete("Use Ped.GiveHelmet(bool, HelmetPropFlags, int) instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public void GiveHelmet(bool canBeRemovedByPed, Helmet helmetType, int textureIndex)
        {
            Function.Call(Hash.GIVE_PED_HELMET, Handle, !canBeRemovedByPed, (int)helmetType, textureIndex);
        }

        /// <summary>
        /// Sets how high up on this <see cref="Ped"/>s body water should be visible.
        /// </summary>
        /// <value>
        /// The height offset ranges from -2f to 1.99f inclusive, -2f being no water visible, 1.99f being fully covered in water.
        /// </value>
        /// <remarks>
        /// Although zero sets the height offset of the water line to zero in meters on water height members of <c>CPed</c>,
        /// This property will clear the wet/soaked effect if the value is set to the zero for the compatibility of scripts built against v3.6.0.
        /// </remarks>
        [Obsolete("Ped.WetnessHeight is obsolete because it does not indicate that it clears the wetness effect from the ped if the value is exactly zero," +
            "while the value can take any values in the range of -2f to 1.99f inclusive. Please use Ped.Wet or Ped.ClearWetnessEffect instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public float WetnessHeight
        {
            set
            {
                if (value == 0.0f)
                {
                    Function.Call(Hash.CLEAR_PED_WETNESS, Handle);
                }
                else
                {
                    Function.Call(Hash.SET_PED_WETNESS_HEIGHT, Handle, value);
                }
            }
        }

        /// <inheritdoc cref="GetConfigFlag(PedConfigFlagToggles)"/>
        [Obsolete("Use GetConfigFlag(PedConfigFlagToggles) instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public bool GetConfigFlag(int flagID) => GetConfigFlag((PedConfigFlagToggles)flagID);
        /// <inheritdoc cref="GetConfigFlag(PedConfigFlagToggles)"/>
        [Obsolete("Use SetConfigFlag(PedConfigFlagToggles, bool) instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void SetConfigFlag(int flagID, bool value) => SetConfigFlag((PedConfigFlagToggles)flagID, value);

        /// <summary>
        /// Do not use this method and use <see cref="Ped.SetResetFlag(PedResetFlagToggles, bool)"/> or <see cref="Ped.GetResetFlag(PedResetFlagToggles)"/> instead,
        /// because <c>SET_PED_RESET_FLAG</c> uses different flag IDs from the IDs <see cref="GetConfigFlag(int)"/> and <see cref="SetConfigFlag(int, bool)"/> use.
        /// </summary>
        [Obsolete("Ped.ResetConfigFlag is obsolete since SET_PED_RESET_FLAG uses different flag IDs from the IDs GET_PED_CONFIG_FLAG and SET_PED_CONFIG_FLAG use " +
            "and the said overload always set the flag (2nd argument of SET_PED_RESET_FLAG) to true. Use Ped.SetResetFlag or Ped.GetResetFlag instead", true)]
        public void ResetConfigFlag(int flagID)
        {
            Function.Call(Hash.SET_PED_RESET_FLAG, Handle, flagID, true);
        }

        /// <summary>
        /// Sets whether this <see cref="Ped"/> keeps their tasks when they are marked as no longer needed by <see cref="Entity.MarkAsNoLongerNeeded"/>.
        /// Despite the property name, this property does not determine whether permanent events can interrupt the <see cref="Ped"/>'s tasks (e.g. seeing hated peds or getting shot at).
        /// </summary>
        /// <inheritdoc cref="KeepTaskWhenMarkedAsNoLongerNeeded"/>
        [Obsolete("Ped.AlwaysKeepTask is obsolete because it does not indicate that it only affects when the ped is marked as no longer needed. " +
                  "Use Ped.KeepTaskWhenMarkedAsNoLongerNeeded instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AlwaysKeepTask
        {
            set => KeepTaskWhenMarkedAsNoLongerNeeded = value;
        }

        [Obsolete("Use IsEnteringVehicle instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsGettingIntoVehicle => IsEnteringVehicle;

        /// <summary>
        /// Sets the value that indicates whether this <see cref="Ped"/> can be knocked off a <see cref="Vehicle"/> (not limited to a bike despite the property name).
        /// </summary>
        [Obsolete("Use Ped.KnockOffVehicleType instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanBeKnockedOffBike
        {
            set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, !value);
        }

        /// <summary>
        /// <para>
        /// Sets the drive tasks driving style.
        /// </para>
        /// <para>
        /// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Despite the interface, this actually changes the driving flags field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
        /// </remarks>
        [Obsolete("Use Ped.VehicleDrivingFlags instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public DrivingStyle DrivingStyle
        {
            set => Function.Call(Hash.SET_DRIVE_TASK_DRIVING_STYLE, Handle, (int)value);
        }

        /// <summary>
        /// Sets the movement clip/animation set this <see cref="Ped"/> should use or <see langword="null"/>
        /// to reset to the default value defined in <c>peds.meta</c> under <c>&lt;MovementClipSet&gt;</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When the value is set to <see langword="null"/>, <see cref="TaskInvoker.ClearAll"/> will be called
        /// right after resetting the movement clipset.
        /// </para>
        /// <para>
        /// Despite what the doc for this property said in between v3.0.0 in v3.6.0, the loading state of some animation
        /// dictionaries has nothing to do with this property. Specifying a string only registered as a clip/animation
        /// dictionary will result in the setter failure.
        /// </para>
        /// </remarks>
        [Obsolete("Use Ped.SetMovementClipSet or Ped.ResetMovementClipSet instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public string MovementAnimationSet
        {
            set
            {
                if (value == null)
                {
                    Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Handle, 0.25f);
                    Task.ClearAll();
                }
                else
                {
                    Function.Call(Hash.REQUEST_CLIP_SET, value);
                    int startTime = Environment.TickCount;

                    while (!Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, value))
                    {
                        Script.Yield();

                        if (Environment.TickCount - startTime >= 1000)
                        {
                            return;
                        }
                    }

                    Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Handle, value, 0.25f);
                }
            }
        }
    }
}
