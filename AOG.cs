using System;
using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace AfraidOfWoman
{
    public class AfraidOfWoman : Script
    {
        private readonly float INTERACTIONRANGE = 5.5f;
        private readonly Gender afraidOfGender = Gender.Female;
        public AfraidOfWoman() 
        {
            Tick += onTick;
            Interval = 1;
        }

        private void onTick(object sender, EventArgs e)
        {
            Ped player = Function.Call<Ped>(Hash.PLAYER_PED_ID);
            if (Game.IsControlJustPressed(0, Control.Talk) && !player.IsInVehicle() && !UI.IsHudComponentActive(HudComponent.WeaponWheel))
            {
                List<Ped> interactablePeds = World.GetNearbyPeds(player, INTERACTIONRANGE).Where(p => CanInteractWithPed(player, p)).ToList();

                if (interactablePeds.Any())
                {
                    // Find the closest interactable ped
                    Ped closestPed = interactablePeds.OrderBy(p => player.Position.DistanceToSquared(p.Position)).First();
                    if(closestPed.Gender == afraidOfGender) 
                    {
                        player.Kill();
                    }
                }
            }
        }
        private bool CanInteractWithPed(Ped source, Ped target)
        {
            if (!target.IsAlive) return false;
            Vector3 sourcePosition = source.Position;
            Vector3 targetPosition = target.Position;

            RaycastResult raycastResult = World.Raycast(sourcePosition, targetPosition, IntersectOptions.Everything);
            return raycastResult.DitHitEntity && raycastResult.HitEntity == target;
        }
    }
}