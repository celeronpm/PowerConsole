﻿using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleView: MonoBehaviour
	{
		private const int H_MARGIN = 2;
		private const int V_MARGIN = 2;

		public ConsoleDetailsPanel Details { get; private set; }
		public ConsoleInputPanel Input { get; private set; }
		public ConsoleMessagesPanel Messages { get; private set; }
		public ConsoleHintPanel Hints { get; private set; }

		public ConsoleStyles Styles { get; private set; }
		public ConsoleInstance Console { get; private set; }
		public ConsoleInput UserInput { get; private set; }

		public float Height = 400;

		public bool DisplayTitle = true;
		public bool DisplayHints = true;

		public TextAsset LocalizationCSV;

		[NonSerialized]
		private bool m_Initialized;

		public void Awake()
		{
			TryInitialize();
		}

		public void OnGUI()
		{
			//after recompiling, restart the console
			TryInitialize();

			//hs to be done in OnGUI
			Styles.TryInitialize(false);
			float offset = 0;

			float hDetails = GetHeight(Details, DisplayTitle);
			float hMessages = GetHeight(Messages);
			float hInput = GetHeight(Input);
			float hHints = GetHeight(Hints, DisplayHints);

			hMessages = hMessages-hDetails-hInput-hHints;

			Rect detailsRect = CreateRect(offset, hDetails);
			offset += hDetails;

			Rect messagesRect = CreateRect(offset, hMessages);
			offset += hMessages;

			Rect inputRect = CreateRect(offset, hInput);
			offset += hInput;

			Rect hintRect = CreateRect(offset, hHints);
			offset += hHints;

			if(DisplayTitle)
			{
				Details.Render(detailsRect);
			}
			Messages.Render(messagesRect);
			Input.Render(inputRect);
			if(DisplayHints)
			{
				Hints.Render(hintRect);
			}
		}

		private void TryInitialize()
		{
			if(!m_Initialized)
			{
				m_Initialized = true;

				Styles = new ConsoleStyles();
				Console = new ConsoleInstance(new LocalizationManager());
				UserInput = new ConsoleInput();
				
				Details = new ConsoleDetailsPanel(this);
				Input = new ConsoleInputPanel(this);
				Messages = new ConsoleMessagesPanel(this);
				Hints = new ConsoleHintPanel(this);
			}
		}

		private float GetHeight(AConsolePanel panel, bool display = true)
		{
			return (display? panel.PreferedHeight(Height) : 0);
		}

		private Rect CreateRect(float offset, float height)
		{
			return new Rect(0, offset, Screen.width, height).AddMargin(H_MARGIN, V_MARGIN);
		}
	}
}
