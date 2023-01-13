using UnityEngine;
using DiceDemo.UI;
using DiceDemo.Scenery;
using DiceDemo.Gameplay;

namespace DiceDemo.System
{
    public class GameProcessor : MonoBehaviour
    {
        [SerializeField] private UIHandler _userInterface;
        [SerializeField] private Environment _scenery;
        [SerializeField] private DiceHandler _gameplay;

        private AudioPlayer _audioPlayer;

        private bool _isWaitingDiceResult = false;

        void Start()
        {
            SetupScene();
        }

        private void SetupScene()
        {
            ThemeType initialThemeType = new();

            _scenery.SetTheme(initialThemeType);
            _gameplay.Init(ProcessDiceResult);
            _userInterface.Init(ProcessThemeChangeCommand, ProcessThrowDiceCommand, initialThemeType);

            _audioPlayer = GetComponent<AudioPlayer>();
        }

        private void ProcessThemeChangeCommand(ThemeType themeType)
        {
            if (_scenery.IsSetNewTheme(themeType))
            {
                _userInterface.ReactToThemeChange(themeType);
            }

            _audioPlayer.PlayButtonClickSound();
        }

        private void ProcessThrowDiceCommand()
        {
            if (_isWaitingDiceResult)
            {
                return;
            }

            _isWaitingDiceResult = true;
            _userInterface.ReactToDiceThrowing();
            _gameplay.InitiateDiceThrowing();
            _audioPlayer.PlayDiceShakingSound();
        }

        private void ProcessDiceResult(int[] diceResult)
        {
            _userInterface.ReactToDiceResult(diceResult);
            _isWaitingDiceResult = false;
        }
    }
}
