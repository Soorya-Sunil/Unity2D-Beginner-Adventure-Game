using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
	// Public variables
    public static UIHandler instance { get; private set; }
	public float displayTime = 4.0f;

	// Private variables
	private float timerDisplay;
	private VisualElement m_Healthbar;
	private VisualElement m_NPCDialogue;


	// Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
	private void Awake()
	{
		instance = this;
	}


	// Start is called before the first frame update
	void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

		m_NPCDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
		m_NPCDialogue.style.display = DisplayStyle.None;
		timerDisplay = -1.0f;
    }


	// Update is called once per frame
	private void Update()
	{
		if (timerDisplay > 0) 
		{ 
			timerDisplay -= Time.deltaTime;

			if (timerDisplay < 0) 
			{
				m_NPCDialogue.style.display = DisplayStyle.None;
			}
		}
	}


	public void SetHealthValue(float percentage) 
    {
		m_Healthbar.style.width = Length.Percent(100 * percentage);
	}


	public void DisplayDialogue()
	{
		m_NPCDialogue.style.display = DisplayStyle.Flex;
		timerDisplay = displayTime;
	}
}
