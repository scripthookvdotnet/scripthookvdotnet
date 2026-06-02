
using System;
using System.ComponentModel;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public sealed partial class TaskInvoker
    {
        [Obsolete("Use TaskInvoker.AimGunAtEntity for entity targets instead.")]
        public void AimAt(Entity target, int duration)
        {
            Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, _ped.Handle, target.Handle, duration, 0);
        }

        [Obsolete("Use TaskInvoker.AimGunAtPosition for coordinate targets instead.")]
        public void AimAt(Vector3 target, int duration)
        {
            Function.Call(Hash.TASK_AIM_GUN_AT_COORD, _ped.Handle, target.X, target.Y, target.Z, duration, 0, 0);
        }

        [Obsolete("Use TaskInvoker.CruiseWithVehicle(Vehicle, float, VehicleDrivingFlags) instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public void CruiseWithVehicle(Vehicle vehicle, float speed, DrivingStyle style = DrivingStyle.Normal)
        {
            Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, (int)style);
        }

        [Obsolete("Use DriveTo(Vehicle, Vector3, float, VehicleDrivingFlags, float) instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public void DriveTo(Vehicle vehicle, Vector3 target, float radius, float speed, DrivingStyle style = DrivingStyle.Normal)
        {
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, speed, (int)style, radius);
        }

        [Obsolete("Use TaskInvoker.Combat instead.")]
        public void FightAgainst(Ped target)
        {
            Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, 0, 16);
        }

        [Obsolete("Use TaskInvoker.CombatTimed instead.")]
        public void FightAgainst(Ped target, int duration)
        {
            Function.Call(Hash.TASK_COMBAT_PED_TIMED, _ped.Handle, target.Handle, duration, 0);
        }

        [Obsolete("Use TaskInvoker.CombatHatedTargetsAroundPed instead.")]
        public void FightAgainstHatedTargets(float radius)
        {
            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped.Handle, radius, 0);
        }

        [Obsolete("Use TaskInvoker.CombatHatedTargetsAroundPedTimed instead.")]
        public void FightAgainstHatedTargets(float radius, int duration)
        {
            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped.Handle, radius, duration, 0);
        }

        [Obsolete("TaskInvoker.GoTo with the position parameter may not obvious enough to suggest it uses navigation mesh. Use TaskInvoker.FollowNavMeshTo instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public void GoTo(Vector3 position, int timeout = -1)
        {
            Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1f, timeout, 0f, 0, 0f);
        }

        [Obsolete("TaskInvoker.StartScenario is obsolete, use TaskInvoker.StartScenarioInPlace instead.")]
        public void StartScenario(string name, float heading) => StartScenarioInPlace(name);

        [Obsolete("TaskInvoker.StartScenario is obsolete, use TaskInvoker.StartScenarioAtPosition instead.")]
        public void StartScenario(string name, Vector3 position, float heading) => StartScenarioAtPosition(name, position, heading, playIntroClip: false);

        [Obsolete("TaskInvoke.Wait is obsolete, use TaskInvoker.Pause instead.")]
        public void Wait(int duration) => Pause(duration);

        [Obsolete("the overload of TaskInvoker.WanderAround with no parameters is obsolete, use TaskInvoker.Wander instead.")]
        public void WanderAround() => Wander(0, false);

        [Obsolete("Use StopScriptedAnimationTask instead.")]
        public void ClearAnimation(string animSet, string animName)
        {
            Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, animSet, animName, -4f);
        }
    }
}
