namespace SoulsLike
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager _playerManager;

        protected override void Awake() {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Start() {
            base.Start();

            // WHY CALCULATE THESE HERE?
            // WHEN WE MAKE A CHARACTER CREATION MENU, AND SET THE STATS DEPENDING ON THE CLASS, THIS WILL BE CALCULATED THERE
            // UNTIL THEN HOWEVER, STATS ARE NEVER CALCULATED, SO WE DO IT HERE ON START, IF A SAVE FILE EXISTS THEY WILL BE OVERWRITTEN WHEN LOADING INTO A SCENE
            CalculateHealthBasedOnVitalityLevel(_playerManager.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(_playerManager.playerNetworkManager.endurance.Value);
        }
    }
}