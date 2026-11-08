using TMPro;
using UnityEngine;

public class Disclosure : MonoBehaviour
{
    public GameObject disclosureOverlay;
    public TextMeshProUGUI heading;
    public TextMeshProUGUI body;

    private void Start()
    {
        if (disclosureOverlay) disclosureOverlay.SetActive(false);
    }
    public void OpenDisclosure()
    {
        disclosureOverlay.SetActive(true);
        heading.text = "AI Disclosure";

        body.text = "This project used generative AI assistants during development:\r\n\r\n" +
            "- <b>Code:</b> AI wrote or assisted with most C# scripts (round logic, scoring, UI, events)\r\n" +
            "- <b>Design:</b> AI helped brainstorm rules, bonus system, and scoring formula\r\n\r\n" +
            "<b>Not used for:</b>\r\n- Art assets (none used — built with Unity UI primitives)\r\n" +
            "- Audio assets\r\n\r\n" +
            "All final design decisions, balance, Unity scene setup, playtesting, and submission were done by the developer.";
    }

    public void CloseDisclosure()
    {
        disclosureOverlay.SetActive(false);
    }
}
