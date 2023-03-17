using Content.Shared.Actions.ActionTypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Server.Abilities.Priest
{
    /// <summary>
    /// Дает возможность Священнику использовать спец. способность: увидеть призрака
    /// </summary>
    [RegisterComponent]
    public sealed class PriestPowersComponent : Component
    {
        /// <summary>
        /// Whether this component is active or not.
        /// </summarY>
        [DataField("enabled")]
        public bool Enabled = true;
		
		/// <summary>
		/// Время начала использования способности
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		public TimeSpan StartTime;
	
		/// <summary>
		/// Как долго способность будет работать
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		public TimeSpan DelayEndTime = TimeSpan.FromSeconds(5);

        [DataField("SeeGhostAction")]
        public InstantAction SeeGhostAction = new()
        {
            UseDelay = TimeSpan.FromSeconds(60),
            Icon = new SpriteSpecifier.Texture(new ResourcePath("Effects/crayondecals.rsi/ghost.png")),
            DisplayName = "Поговорить с призраками",
            Description = "Позволяет увидеть призраков на короткий промежуток времени",
            Priority = -1,
            Event = new SeeGhostActionEvent(),
        };
    }
}