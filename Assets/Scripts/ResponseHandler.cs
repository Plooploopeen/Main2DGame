using System.Buffers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResponseHandler : MonoBehaviour
{
    private InputAction DPadAction;
    private InputAction acceptAction;

    [SerializeField] RectTransform responseBox;
    [SerializeField] RectTransform responseButtonTemplate;
    [SerializeField] RectTransform responseContainer;
    [SerializeField] GameObject selector;

    private DialogueUI dialogueUI;

    List<GameObject> tempResponseButtons = new List<GameObject>();

    private int selectedIndex = 0;
    private bool isSelectingResponse;
    private Response[] selectedResponses;

    private void Awake()
    {
        DPadAction = InputSystem.actions.FindAction("DPad");
        acceptAction = InputSystem.actions.FindAction("Focus");
    }

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
        selectedIndex = 0;
    }

    private void Update()
    {
        checkSelectorInput();
    }

    public void ShowResponses(Response[] responses)
    {
        selectedResponses = responses;

        float responseBoxHeight = 0;

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);

            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);    
        responseBox.gameObject.SetActive(true);

        selectedIndex = 0;
        isSelectingResponse = true;

        if (selector  != null)
        {
            selector.SetActive(true);
            updateSelectorPosition();
        }

    }

    private void OnPickedResponse(Response response)
    {
        isSelectingResponse = false;

        responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }

        tempResponseButtons.Clear();

        dialogueUI.ShowDialogue(response.DialogueObject);
    }

    void updateSelectorPosition()
    {
        if (selector != null && tempResponseButtons.Count > 0)
        {
            selector.transform.position = tempResponseButtons[selectedIndex].transform.position;
        }
    }

    void checkSelectorInput()
    {
        if (selector != null)
        {
            Vector2 DPadDirection = DPadAction.ReadValue<Vector2>();

            if (acceptAction.WasPressedThisFrame())
            {
                confirmSelection();
            }

            if (DPadAction.WasPressedThisFrame())
            {
                if (DPadDirection.y < 0)
                {
                    selectedIndex++;

                    if (selectedIndex >= tempResponseButtons.Count)
                    {
                        selectedIndex = 0;
                    }

                    updateSelectorPosition();
                }
                else if (DPadDirection.y > 0)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = tempResponseButtons.Count - 1;
                    }

                    updateSelectorPosition();
                }

                Debug.Log(selectedIndex);
            }
        }
    }
    
    void confirmSelection()
    {
        if (tempResponseButtons.Count == 0)
        {
            return;
        }

        Response selectedResponse = getResponseAtIndex(selectedIndex);

        OnPickedResponse(selectedResponse);
    }

    Response getResponseAtIndex(int index)
    {
        return selectedResponses[index];
    }
}
