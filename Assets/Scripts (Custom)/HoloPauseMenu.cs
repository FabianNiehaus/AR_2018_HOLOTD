using Core.Game;
using HoloToolkit.Unity.InputModule;
using System;
using TowerDefense.Game;
using TowerDefense.UI.HUD;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GameUIState = TowerDefense.UI.HUD.GameUI.State;

namespace HoloTD.UI
{
	/// <summary>
	/// In-game pause menu
	/// </summary>
	public class HoloPauseMenu : MonoBehaviour
	{
		/// <summary>
		/// Enum to represent state of pause menu
		/// </summary>
		protected enum State
		{
			Open,
			RestartPressed,
			Closed
		}

		/// <summary>
		/// The CanvasGroup that holds the pause menu UI
		/// </summary>
		public Text titleText;
		
		public Text descriptionText;

        public GameObject menuContainer;

		/// <summary>
		/// The buttons present in the pause menu
		/// </summary>	
		public Button restartButton;

		public Image menuPanel;

		/// <summary>
		/// Color to change the top panel to highlight confirmation button
		/// </summary>
		public Color menuPanelDisabledColor = new Color(1, 1, 1, 1);

		/// <summary>
		/// State of pause menu
		/// </summary>
		protected State m_State;

		/// <summary>
		/// If the pause menu was opened/closed this frame
		/// </summary>
		bool m_MenuChangedThisFrame;

		/// <summary>
		/// Open the pause menu
		/// </summary>
		public void OpenPauseMenu()
		{
			SetMenuContainerActive(true);

			LevelItem level = GameManager.instance.GetLevelForCurrentScene();
			if (level == null)
			{
				return;
			}
			if (titleText != null)
			{
				titleText.text = level.name;
			}
			if (descriptionText != null)
			{
				descriptionText.text = level.description;
			}

			m_State = State.Open;
		}

		/// <summary>
		/// Fired when GameUI's State changes
		/// </summary>
		/// <param name="oldState">The State that GameUI is leaving</param>
		/// <param name="newState">The State that GameUI is entering</param>
		protected void OnGameUIStateChanged(GameUIState oldState, GameUIState newState)
		{
            m_MenuChangedThisFrame = true;
			if (newState == GameUIState.Paused)
			{
				OpenPauseMenu();
			}
			else
			{
				ClosePauseMenu();
			}
		}

		/// <summary>
		/// Close the pause menu
		/// </summary>
		public void ClosePauseMenu()
		{
			SetMenuContainerActive(false);

            restartButton.interactable = true;

			menuPanel.color = Color.white;

			m_State = State.Closed;
		}

		/// <summary>
		/// Hide the pause menu on awake
		/// </summary>
		protected void Awake()
        {
			m_State = State.Closed;
		}

		/// <summary>
		/// Subscribe to GameUI's stateChanged event
		/// </summary>
		protected void Start()
		{
            menuContainer = GameObject.FindGameObjectWithTag("PauseMenuContainer");
            SetMenuContainerActive(false);

            if (GameUI.instanceExists)
			{
				GameUI.instance.stateChanged += OnGameUIStateChanged;
			}
		}

		/// <summary>
		/// Unpause the game if the game is paused and the Escape key is pressed
		/// </summary>
		protected virtual void Update()
		{
			if (m_MenuChangedThisFrame)
			{
				m_MenuChangedThisFrame = false;
				return;
			}

			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && GameUI.instance.state == GameUIState.Paused)
			{
				Unpause();
			}
		}

		/// <summary>
		/// Show/Hide the pause menu canvas group
		/// </summary>
		protected void SetMenuContainerActive(bool enable)
        {
            try
            {
                menuContainer.gameObject.SetActive(enable);
            }
            catch (Exception e)
            {
                //Debug.LogError("Referenced instance ID: " + menuContainer.GetInstanceID().ToString());
            }
        }

		public void Pause()
		{
			if (GameUI.instanceExists)
			{
				GameUI.instance.Pause();
			}
		}

		public void Unpause()
		{
			if (GameUI.instanceExists)
			{
				GameUI.instance.Unpause();
			}
		}
    }
}