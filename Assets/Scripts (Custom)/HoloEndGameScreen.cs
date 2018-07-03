
using Core.Game;
using Core.Health;
using HoloTD.all;
using HoloToolkit.Unity.InputModule;
using TowerDefense.Game;
using TowerDefense.Level;
using TowerDefense.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HoloTD.UI
{
	/// <summary>
	/// UI to display the game over screen
	/// </summary>
	public class HoloEndGameScreen : MonoBehaviour
	{
		/// <summary>
		/// AudioClip to play when victorious
		/// </summary>
		public AudioClip victorySound;

		/// <summary>
		/// AudioClip to play when failed
		/// </summary>
		public AudioClip defeatSound;

		/// <summary>
		/// AudioSource that plays the sound
		/// </summary>
		public AudioSource audioSource;

		/// <summary>
		/// The containing panel of the End Game UI
		/// </summary>
		public Canvas endGameCanvas;

		/// <summary>
		/// Reference to the Text object that displays the result message
		/// </summary>
		public Text endGameMessageText;

		/// <summary>
		/// Panel that shows final star rating
		/// </summary>
		public ScorePanel scorePanel;

		/// <summary>
		/// Text to be displayed on popup
		/// </summary>
		public string levelCompleteText = "Level COMPLETE!";
		
		public string levelFailedText = "Level FAILED!";

		/// <summary>
		/// Background image
		/// </summary>
		public Image background;

		/// <summary>
		/// Color to set background
		/// </summary>
		public Color winBackgroundColor;
		
		public Color loseBackgroundColor;

		/// <summary>
		/// Reference to the <see cref="LevelManager" />
		/// </summary>
		protected LevelManager m_LevelManager;

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// Reloads the active scene
		/// </summary>
		public void RestartLevel()
		{
			SafelyUnsubscribe();
            Scene activeScene = SceneManager.GetSceneAt(1);
            string activeSceneName = activeScene.name;

            GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<MainSceneLoader>().loadScene(activeSceneName);
        }

        /// <summary>
        /// Hide the panel if it is active at the start.
        /// Subscribe to the <see cref="LevelManager" /> completed/failed events.
        /// </summary>
        protected void Start()
		{
			LazyLoad();
            endGameCanvas.enabled = false;

			m_LevelManager.levelCompleted += Victory;
			m_LevelManager.levelFailed += Defeat;
		}

		/// <summary>
		/// Shows the end game screen
		/// </summary>
		protected void OpenEndGameScreen(string endResultText)
		{
			LevelItem level = GameManager.instance.GetLevelForCurrentScene();
			endGameCanvas.enabled = true;

			int score = CalculateFinalScore();
			scorePanel.SetStars(score);
			if (level != null) 
			{
				//endGameMessageText.text = string.Format (endResultText, level.name.ToUpper ());
				GameManager.instance.CompleteLevel (level.id, score);
			} 
			else 
			{
				// If the level is not in LevelList, we should just use the name of the scene. This will not store the level's score.
				//string levelName = SceneManager.GetActiveScene ().name;
				//endGameMessageText.text = string.Format (endResultText, levelName.ToUpper ());
			}

			if (!TowerDefense.UI.HUD.GameUI.instanceExists)
			{
				return;
			}
			if (TowerDefense.UI.HUD.GameUI.instance.state == TowerDefense.UI.HUD.GameUI.State.Building)
			{
                TowerDefense.UI.HUD.GameUI.instance.CancelGhostPlacement();
			}
            TowerDefense.UI.HUD.GameUI.instance.GameOver();
		}

		/// <summary>
		/// Occurs when the level is sucessfully completed
		/// </summary>
		protected void Victory()
		{
			OpenEndGameScreen(levelCompleteText);
			if ((victorySound != null) && (audioSource != null))
			{
				audioSource.PlayOneShot(victorySound);
			}
			background.color = winBackgroundColor;

			GameManager gm = GameManager.instance;
			LevelItem item = gm.GetLevelForCurrentScene();
			LevelList list = gm.levelList;
			int levelCount = list.Count;
			int index = -1;
			for (int i = 0; i < levelCount; i++)
			{
				if (item == list[i])
				{
					index = i;
					break;
				}
			}
		}

		/// <summary>
		/// Occurs when level is failed
		/// </summary>
		protected void Defeat()
		{
			OpenEndGameScreen(levelFailedText);
			if ((defeatSound != null) && (audioSource != null))
			{
				audioSource.PlayOneShot(defeatSound);
			}
			background.color = loseBackgroundColor;
		}

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// </summary>
		protected void OnDestroy()
		{
			SafelyUnsubscribe();
			if (TowerDefense.UI.HUD.GameUI.instanceExists)
			{
                TowerDefense.UI.HUD.GameUI.instance.Unpause();
			}
		}

		/// <summary>
		/// Ensure that <see cref="LevelManager" /> events are unsubscribed from when necessary
		/// </summary>
		protected void SafelyUnsubscribe()
		{
			LazyLoad();
			m_LevelManager.levelCompleted -= Victory;
			m_LevelManager.levelFailed -= Defeat;
		}

		/// <summary>
		/// Ensure <see cref="m_LevelManager" /> is not null
		/// </summary>
		protected void LazyLoad()
		{
			if ((m_LevelManager == null) && LevelManager.instanceExists)
			{
				m_LevelManager = LevelManager.instance;
			}
		}

		/// <summary>
		/// Add up the health of all the Home Bases and return a score
		/// </summary>
		/// <returns>Final score</returns>
		protected int CalculateFinalScore()
		{
			int homeBaseCount = m_LevelManager.numberOfHomeBases;
			PlayerHomeBase[] homeBases = m_LevelManager.playerHomeBases;

			float totalRemainingHealth = 0f;
			float totalBaseHealth = 0f;
			for (int i = 0; i < homeBaseCount; i++)
			{
				Damageable config = homeBases[i].configuration;
				totalRemainingHealth += config.currentHealth;
				totalBaseHealth += config.maxHealth;
			}
			int score = CalculateScore(totalRemainingHealth, totalBaseHealth);
			return score;
		}

		/// <summary>
		/// Take the final remaining health of all bases and rates them
		/// </summary>
		/// <param name="remainingHealth">the total remaining health of all home bases</param>
		/// <param name="maxHealth">the total maximum health of all home bases</param>
		/// <returns>0 to 3 depending on how much health is remaining</returns>
		protected int CalculateScore(float remainingHealth, float maxHealth)
		{
			float normalizedHealth = remainingHealth / maxHealth;
			if (Mathf.Approximately(normalizedHealth, 1f))
			{
				return 3;
			}
			if ((normalizedHealth <= 0.9f) && (normalizedHealth >= 0.5f))
			{
				return 2;
			}
			if ((normalizedHealth < 0.5f) && (normalizedHealth > 0f))
			{
				return 1;
			}
			return 0;
		}
	}
}