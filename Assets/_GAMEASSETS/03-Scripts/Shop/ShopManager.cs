using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    public class ShopManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image submarinePeriscopeImage;
        [SerializeField] private Image submarineBodyImage;
        
        [SerializeField] private Image smallSubmarinePeriscopeImage;
        [SerializeField] private Image smallSubmarineBodyImage;
        [SerializeField] private GameObject periscopeLock;
        [SerializeField] private TMP_Text periscopePrice;
        [SerializeField] private GameObject bodyLock;
        [SerializeField] private TMP_Text bodyPrice;
        [SerializeField] private Button saveButton;
        [SerializeField] private TMP_Text saveButtonText;

        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private int activePeriscope;
        [SerializeField] private int activeBody;
        [SerializeField] private int periscopeAmount;
        [SerializeField] private int bodyAmount;

        [Header("BuyPanel")]
        [SerializeField] private GameObject buyPanel;
        [SerializeField] private Image buyPanelImage;
        [SerializeField] private TMP_Text buyPanelText;
        [SerializeField] private SubmarinePartType buyPanelType;
        [SerializeField] private Button buyPanelAcceptButton;
        
        [Header("Parts")]
        [ReadOnly] [SerializeField] private bool[] periscopesBought;
        [ReadOnly] [SerializeField] private bool[] bodiesBought;
        [ReadOnly] [SerializeField] private SubmarinePart[] periscopes;
        [ReadOnly] [SerializeField] private SubmarinePart[] bodies;

        private SaveManager saveManager;
        private PartManager partManager;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            partManager = GameObject.FindGameObjectWithTag(GameTags.PARTMANAGER).GetComponent<PartManager>();
            buyPanel.SetActive(false);
            periscopes = new SubmarinePart[200];
            bodies = new SubmarinePart[200];

            activeBody = saveManager.GetData().selectedBody;
            activePeriscope = saveManager.GetData().selectedPeriscope;
            
            GetBoughtItems();
            InitializeParts();
            UpdateSelectedParts();
        }

        private void InitializeParts()
        {
            bodyAmount = 0;
            foreach (SubmarinePart sp in partManager.GetPartsList(SubmarinePartType.PERISCOPE))
            {
                periscopes[sp.ID] = sp;
                if (sp.ID > periscopeAmount)
                {
                    periscopeAmount = sp.ID;
                }
            }

            bodyAmount = 0;
            foreach (SubmarinePart sp in partManager.GetPartsList(SubmarinePartType.BODY))
            {
                bodies[sp.ID] = sp;
                if (sp.ID > bodyAmount)
                {
                    bodyAmount = sp.ID;
                }
            }
        }

        private void GetBoughtItems()
        {
            periscopesBought = saveManager.GetBoughtPeriscopes();
            bodiesBought = saveManager.GetBoughtBodies();
            moneyText.text = saveManager.GetCoins().ToString();
        }

        private void UpdateSelectedParts()
        {
            submarinePeriscopeImage.sprite = periscopes[activePeriscope].SpriteImage;
            submarineBodyImage.sprite = bodies[activeBody].SpriteImage;
            smallSubmarinePeriscopeImage.sprite = periscopes[activePeriscope].SpriteImage;
            smallSubmarineBodyImage.sprite = bodies[activeBody].SpriteImage;

            descriptionText.text = periscopes[activePeriscope].Description + " " + bodies[activeBody].Description;
            moneyText.text = saveManager.GetCoins().ToString();

            bool isSelectable = true;
            if (!periscopesBought[activePeriscope] && periscopes[activePeriscope].Cost != 0)
            {
                isSelectable = false;
                submarinePeriscopeImage.color = new Color(1f,1f,1f,0.5f);
                periscopeLock.SetActive(true);
                periscopePrice.text = periscopes[activePeriscope].Cost + "G";
            }
            else
            {
                submarinePeriscopeImage.color = Color.white;
                periscopeLock.SetActive(false);
                periscopePrice.text = "";
            }
            
            if (!bodiesBought[activeBody] && bodies[activeBody].Cost != 0)
            {
                isSelectable = false;
                submarineBodyImage.color = new Color(1f,1f,1f,0.5f);
                bodyLock.SetActive(true);
                bodyPrice.text = bodies[activeBody].Cost + "G";
            }
            else
            {
                submarineBodyImage.color = Color.white;
                bodyLock.SetActive(false);
                bodyPrice.text = "";
            }

            if (saveManager.GetData().selectedBody == activeBody && saveManager.GetData().selectedPeriscope == activePeriscope)
            {
                isSelectable = false;
            }

            if (isSelectable)
            {
                saveButton.interactable = true;
                saveButtonText.text = "Save Submarine";
            }
            else
            {
                if (saveManager.GetData().selectedBody == activeBody && saveManager.GetData().selectedPeriscope == activePeriscope)
                {
                    saveButtonText.text = "Already selected!";
                }
                else
                {
                    saveButtonText.text = "Not bought parts!";
                }
                saveButton.interactable = false;
            }
        }

        private void OpenBuyPanel(SubmarinePartType buyTypeIn)
        {
            buyPanel.SetActive(true);
            buyPanelType = buyTypeIn;
            
            switch (buyPanelType)
            {
                case SubmarinePartType.BODY:
                    buyPanelImage.sprite = bodies[activeBody].SpriteImage;
                    buyPanelText.text = "Buy '" + bodies[activeBody].Description + "' for " + bodies[activeBody].Cost + "G ?";
                    buyPanelAcceptButton.interactable = saveManager.GetCoins() >= bodies[activeBody].Cost;
                    break;
                case SubmarinePartType.PERISCOPE:
                    buyPanelImage.sprite = periscopes[activePeriscope].SpriteImage;
                    buyPanelText.text = "Buy '" + periscopes[activePeriscope].Description + "' for " + periscopes[activePeriscope].Cost + "G ?";
                    buyPanelAcceptButton.interactable = saveManager.GetCoins() >= periscopes[activePeriscope].Cost;
                    break;
            }
        }

        private void BuyPart()
        {
            switch (buyPanelType)
            {
                case SubmarinePartType.BODY:
                    saveManager.BuyBody(activeBody, bodies[activeBody].Cost);
                    break;
                case SubmarinePartType.PERISCOPE:
                    saveManager.BuyPeriscope(activePeriscope, periscopes[activePeriscope].Cost);
                    break;
            }
            UpdateSelectedParts();
            buyPanel.SetActive(false);
        }

        public void BTN_NextPeriscope()
        {
            if (activePeriscope < periscopeAmount)
            {
                activePeriscope += 1;
            }
            else
            {
                activePeriscope = 0;
            }
            UpdateSelectedParts();
        }

        public void BTN_LastPeriscope()
        {
            if (activePeriscope > 0)
            {
                activePeriscope -= 1;
            }
            else
            {
                activePeriscope = periscopeAmount;
            }
            UpdateSelectedParts();
        }
        
        public void BTN_NextBody()
        {
            if (activeBody < bodyAmount)
            {
                activeBody += 1;
            }
            else
            {
                activeBody = 0;
            }
            UpdateSelectedParts();
        }

        public void BTN_LastBody()
        {
            if (activeBody > 0)
            {
                activeBody -= 1;
            }
            else
            {
                activeBody = bodyAmount;
            }
            UpdateSelectedParts();
        }

        public void BTN_BuyPeriscope()
        {
            OpenBuyPanel(SubmarinePartType.PERISCOPE);
        }

        public void BTN_BuyBody()
        {
            OpenBuyPanel(SubmarinePartType.BODY);
        }

        public void BTN_AcceptBuy()
        {
            BuyPart();
        }

        public void BTN_DenyBuy()
        {
            buyPanel.SetActive(false);
        }

        public void BTN_SaveSubmarine()
        {
            saveManager.SelectSubmarine(activePeriscope, activeBody);
            UpdateSelectedParts();
        }

        public void BTN_BuyCoins()
        {
            //WIP: Need to buy coins here
            Debug.Log("WIP!!! Buy Coins!");
            saveManager.AddCoins(10);
            UpdateSelectedParts();
        }

        public void BTN_BackToLevels()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }
        
        public void OnEscape(InputAction.CallbackContext ctx)
        {
            BTN_BackToLevels();
        }
    }
}