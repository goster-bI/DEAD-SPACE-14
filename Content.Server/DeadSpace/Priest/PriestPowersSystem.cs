using Content.Server.Visible;
// using Content.Server.Popups;
// using Content.Shared.Alert;
using Content.Shared.Actions;
using Robust.Shared.Timing;
using Robust.Server.GameObjects;

namespace Content.Server.Abilities.Priest
{
    public sealed class PriestPowersSystem : EntitySystem
    {
        // [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
        // [Dependency] private readonly AlertsSystem _alertsSystem = default!;

        [Dependency] private readonly IGameTiming _timing = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<PriestPowersComponent, ComponentInit>(OnComponentInit);
            SubscribeLocalEvent<PriestPowersComponent, SeeGhostActionEvent>(OnSeeGhost);
        }

        private void OnComponentInit(EntityUid uid, PriestPowersComponent component, ComponentInit args)
        {
            _actionsSystem.AddAction(uid, component.SeeGhostAction, uid);
        }

        /// <summary>
        /// Увидеть призрака
        /// </summary>
        private void OnSeeGhost(EntityUid uid, PriestPowersComponent component, SeeGhostActionEvent args)
        {
            if (component.Enabled)
                return;

			if (EntityManager.TryGetComponent(uid, out EyeComponent? eye))
            {
                eye.VisibilityMask |= (uint) VisibilityFlags.Ghost;
				component.Enabled = true;
				component.StartTime = _timing.CurTime;
            }
            // Handle args so cooldown works
            args.Handled = true;
        }
		
		public override void Update(float frameTime)
		{
			base.Update(frameTime);
	
			foreach (var priest in EntityQuery<PriestPowersComponent>())
			{
				if ( _timing.CurTime - priest.StartTime >= priest.DelayEndTime && priest.Enabled)
				{
                    priest.Enabled = false;
					if (EntityManager.TryGetComponent(priest.Owner, out EyeComponent? eye))
					{
						eye.VisibilityMask = (uint) VisibilityFlags.Normal;
					}
				}
			}
		}
    }

    public sealed class SeeGhostActionEvent : InstantActionEvent {}
}