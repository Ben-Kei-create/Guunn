using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // ← 追加

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject choicesPanel;
    public Button[] choiceButtons;
    public Sprite[] choiceImages;

    private List<DialogueData> dialogues;
    private int currentDialogueIndex = 0;
    private bool isShowingChoices = false;

    [System.Serializable]
    public class DialogueData
    {
        public string speaker;
        public string text;
        public ChoiceData[] choices;
        public int[] nextIndices;
    }


    [System.Serializable]
    public class ChoiceData
    {
        public string imageName;
        public string text;
    }

    [System.Serializable]
    public class DialogueArrayWrapper
    {
        public DialogueData[] dialogues;
    }

    void Start()
    {
        Debug.Log("TextManager Start()");

        // EventSystemの存在を確認
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            Debug.LogError("EventSystemが見つかりません。UI入力が機能しない可能性があります！");
        }

        LoadDialogueData();
        ShowNextDialogue();
    }

    void Update()
    {
        // マウスクリックがUI要素の上でなければ次のダイアログを表示
        if ((Input.GetKeyDown(KeyCode.Space) ||
            (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement()))
            && !isShowingChoices)
        {
            Debug.Log("Update: スペースキーまたはUIでないところでのマウスクリック");
            ShowNextDialogue();
        }
    }

    // UIの上でクリックしているかチェックするヘルパーメソッド
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void LoadDialogueData()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "chapter1_data.json");
        Debug.Log($"LoadDialogueData: JSON Path = {jsonPath}");

        if (!File.Exists(jsonPath))
        {
            Debug.LogError("LoadDialogueData: JSON ファイルが見つかりません: " + jsonPath);
            dialogues = new List<DialogueData>();
            return;
        }

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            dialogues = JsonUtility.FromJson<DialogueArrayWrapper>("{\"dialogues\":" + jsonText + "}").dialogues.ToList();
            Debug.Log($"LoadDialogueData: 正常にロードされました。ダイアログ数: {dialogues.Count}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("LoadDialogueData: JSON ファイルの読み込みエラー: " + ex.Message);
            dialogues = new List<DialogueData>();
        }
    }

    private void ShowNextDialogue()
    {
        Debug.Log($"ShowNextDialogue: currentDialogueIndex = {currentDialogueIndex}, dialogues.Count = {dialogues?.Count}");
        if (dialogues == null || dialogues.Count == 0 || currentDialogueIndex >= dialogues.Count)
        {
            Debug.Log("ShowNextDialogue: 会話終了、Chapter2Sceneへ遷移");
            dialogueText.text = "会話終了";
            choicesPanel.SetActive(false);
            SceneManager.LoadScene("Chapter2Scene"); // ← シーン遷移処理
            return;
        }

        DialogueData currentDialogue = dialogues[currentDialogueIndex];
        ShowDialogue(currentDialogue.text, currentDialogue.speaker);

        if (currentDialogue.choices != null && currentDialogue.choices.Length > 0)
        {
            Debug.Log("ShowNextDialogue: 選択肢を表示");
            ShowImageChoices(currentDialogue.choices);
            isShowingChoices = true;
        }
        else
        {
            Debug.Log("ShowNextDialogue: 次のダイアログへ");
            isShowingChoices = false;
            currentDialogueIndex++;
        }
    }

    public void ShowDialogue(string text, string speaker)
    {
        Debug.Log($"ShowDialogue: speaker = {speaker}, text = {text}");
        dialogueText.text = $"{speaker}\n{text}"; // ← 名前とセリフを表示
        choicesPanel.SetActive(false);
    }


    public void ShowImageChoices(ChoiceData[] choices)
    {
        Debug.Log($"ShowImageChoices: choices.Length = {choices?.Length}, choiceButtons.Length = {choiceButtons?.Length}");

        // 選択肢パネルを表示する前に、一度非アクティブにしてから再度アクティブにする
        // これによりUI要素のリフレッシュを促す
        choicesPanel.SetActive(false);
        choicesPanel.SetActive(true);

        // 全てのボタンを一度非表示にする
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                Debug.Log($"ShowImageChoices: ボタンインデックス = {i}, imageName = {choices[i].imageName}, text = {choices[i].text}");
                Button currentButton = choiceButtons[i];
                currentButton.gameObject.SetActive(true);

                // ボタンの子オブジェクトにある画像コンポーネントを取得
                Image buttonImage = currentButton.transform.Find("ButtonImage")?.GetComponent<Image>();
                Debug.Log($"ShowImageChoices: buttonImage (index {i}) = {buttonImage}");

                // 選択肢のテキストを設定（子オブジェクトのTextMeshProUGUIを使用）
                TextMeshProUGUI buttonText = currentButton.GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log($"ShowImageChoices: buttonText (index {i}) = {buttonText}");

                // 画像の設定
                if (buttonImage != null)
                {
                    Sprite targetSprite = choiceImages.FirstOrDefault(sprite =>
                        sprite != null && sprite.name == choices[i].imageName);
                    Debug.Log($"ShowImageChoices: targetSprite (index {i}) = {targetSprite?.name}");

                    if (targetSprite != null)
                    {
                        buttonImage.sprite = targetSprite;
                        buttonImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning($"ShowImageChoices: 画像が見つかりません: {choices[i].imageName}");
                        buttonImage.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogWarning($"ShowImageChoices: ボタン '{currentButton.name}' に 'ButtonImage' が見つかりません。");
                }

                // テキストの設定
                if (buttonText != null)
                {
                    buttonText.text = choices[i].text;
                    buttonText.gameObject.SetActive(!string.IsNullOrEmpty(choices[i].text));
                }
                else
                {
                    Debug.LogWarning($"ShowImageChoices: ボタン '{currentButton.name}' に TextMeshProUGUI が見つかりません。");
                }

                // クリックイベントの設定（匿名関数を使わない方法）
                int choiceIndex = i; // 現在のインデックスを保存
                currentButton.onClick.AddListener(delegate { HandleButtonClick(choiceIndex); });

                Debug.Log($"ShowImageChoices: OnClick listener added to button {i}");
                Debug.Log($"Button {i} interactable: {currentButton.interactable}");
                Debug.Log($"Button {i} raycast target: {currentButton.GetComponent<Graphic>()?.raycastTarget}");
            }
        }
    }

    // ボタンクリックを処理する専用メソッド
    public void HandleButtonClick(int choiceIndex)
    {
        Debug.Log($"HandleButtonClick: ボタン {choiceIndex} がクリックされました！");
        OnChoiceSelected(choiceIndex);
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"OnChoiceSelected: choiceIndex = {choiceIndex}");
        DialogueData currentDialogue = dialogues[currentDialogueIndex];

        // 次のインデックスが指定されている場合はそれを使用
        if (currentDialogue.nextIndices != null && choiceIndex < currentDialogue.nextIndices.Length)
        {
            Debug.Log($"OnChoiceSelected: 次のインデックスへ (nextIndices[{choiceIndex}] = {currentDialogue.nextIndices[choiceIndex]})");
            currentDialogueIndex = currentDialogue.nextIndices[choiceIndex];
        }
        else
        {
            Debug.Log("OnChoiceSelected: 次のダイアログへ (nextIndices が null または範囲外)");
            currentDialogueIndex++;
        }

        isShowingChoices = false;
        ShowNextDialogue();
    }
}