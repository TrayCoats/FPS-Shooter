//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class shopUI : MonoBehaviour
//{


//    [SerializeField] int skillPoints;

//    public TMP_Text unlockBtnText, upgradeBtnText, levelText, upgradeNameText;
//    public Text speedText, damageText, skillPointsText;
//    public Button unlockBtn, upgradeBtn;

//    private int currentIndex = 0;

//    private void Start()
//    {
//        skillPointsText.text = "" + skillPoints;

//        unlockBtn.onClick.AddListener(() => UnlockSelectButton());
//        upgradeBtn.onClick.AddListener(() => UpgradeButton());

//        UnlockButtonStatus();
//        UpgradeButtonStatus();
//    }

//    private void SetUpgradeInfo()
//    {
//        upgradeNameText.text = shopData.shopItem[currentIndex].upgradeName;
//        int currentLevel = shopData.ShopItem[currentIndex].upgradeLevel;
//        levelText.text = "Level:" + (currentLevel + 1);
//        speedText.text = "Speed:" + shopData.ShopItem[currentIndex].carLevelsData[currentLevel].speed;
//        damageText.text = "Damage:" + shopData.ShopItem[currentIndex].carLevelsData[currentLevel].acceleration;
//    }



//}


///// <summary>
///// Method called on Next button click
///// </summary>
//private void NextButton()
//{
//    //check if currentIndex is less than the total shope items we have - 1
//    if (currentIndex < shopData.shopItems.Length - 1)
//    {
//        carList[currentIndex].SetActive(false);                     //deactivate old model
//        currentIndex++;                                             //increase count by 1
//        carList[currentIndex].SetActive(true);                      //activate the new model
//        SetCarInfo();                                               //set car information

//        //check if current index is equal to total items - 1
//        if (currentIndex == shopData.shopItems.Length - 1)
//        {
//            nextBtn.interactable = false;                           //then set nextBtn interactable false
//        }

//        if (!previousButton.interactable)                           //if previousButton is not interactable
//        {
//            previousButton.interactable = true;                     //then set it interactable
//        }

//        UnlockButtonStatus();
//        UpgradeButtonStatus();
//    }
//}

///// <summary>
///// Method called on Previous button click
///// </summary>
//private void PreviousButton()
//{
//    if (currentIndex > 0)                           //we check is currentIndex i more than 0
//    {
//        carList[currentIndex].SetActive(false);     //deactivate old model
//        currentIndex--;                             //reduce count by 1
//        carList[currentIndex].SetActive(true);      //activate the new model
//        SetCarInfo();                               //set car information

//        if (currentIndex == 0)                      //if currentIndex is 0
//        {
//            previousButton.interactable = false;    //set previousButton interactable to false
//        }

//        if (!nextBtn.interactable)                  //if nextBtn interactable is false
//        {
//            nextBtn.interactable = true;            //set nextBtn interactable to true
//        }
//        UnlockButtonStatus();
//        UpgradeButtonStatus();
//    }
//}

///// <summary>
///// Method called on Unlock button click
///// </summary>
//private void UnlockSelectButton()
//{
//    bool yesSelected = false;   //local bool
//    if (shopData.shopItems[currentIndex].isUnlocked)    //if shop item at currentIndex is already unlocked
//    {
//        yesSelected = true;                             //set yesSelected to true
//    }
//    else if (!shopData.shopItems[currentIndex].isUnlocked)  //if shop item at currentIndex is not unlocked
//    {
//        //check if we have enough coins to unlock it
//        if (totalCoins >= shopData.shopItems[currentIndex].unlockCost)
//        {
//            //if yes then reduce the cost coins from our total coins
//            totalCoins -= shopData.shopItems[currentIndex].unlockCost;
//            totalCoinsText.text = "" + totalCoins;          //set the coins text
//            yesSelected = true;                             //set yesSelected to true
//            shopData.shopItems[currentIndex].isUnlocked = true; //mark the shop item unlocked
//            UpgradeButtonStatus();
//        }
//    }

//    if (yesSelected)
//    {
//        unlockBtnText.text = "Selected";                    //set the unlockBtnText text to Selected
//        selectedIndex = currentIndex;                       //set the selectedIndex to currentIndex
//        PlayerPrefs.SetInt("SelectedItem", selectedIndex);  //save the selectedIndex
//        unlockBtn.interactable = false;                     //set unlockBtn interactable to false
//    }

//}

///// <summary>
///// Method called on Upgrade button click
///// </summary>
//private void UpgradeButton()//upgrade button is interactable only if we have any level left to upgrade
//{
//    //get the next level index
//    int nextLevelIndex = shopData.shopItems[currentIndex].unlockedLevel + 1;
//    //we check if we have enough coins
//    if (totalCoins >= shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].unlockCost)
//    {
//        totalCoins -= shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].unlockCost;
//        totalCoinsText.text = "" + totalCoins;          //set the coins text
//                                                        //if yes we increate the unlockedLevel by 1
//        shopData.shopItems[currentIndex].unlockedLevel++;

//        //we check if are not at max level
//        if (shopData.shopItems[currentIndex].unlockedLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
//        {
//            upgradeBtnText.text = "Upgrade Cost " +
//                shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex + 1].unlockCost;
//        }
//        else    //we check if we are at max level
//        {
//            upgradeBtn.interactable = false;            //set upgradeBtn interactable to false
//            upgradeBtnText.text = "Max Lvl Reached";    //set the btn text
//        }

//        SetCarInfo();
//    }
//}

///// <summary>
///// This method is called when we are changing the current item in the shop
///// This method set the interactablity and text of unlock btn
///// </summary>
//private void UnlockButtonStatus()
//{
//    //if current item is unlocked
//    if (shopData.shopItems[currentIndex].isUnlocked)
//    {
//        //if selectedIndex is not equal to currentIndex set unlockBtn interactable false else make it true
//        unlockBtn.interactable = selectedIndex != currentIndex ? true : false;
//        //set the text
//        unlockBtnText.text = selectedIndex == currentIndex ? "Selected" : "Select";
//    }
//    else if (!shopData.shopItems[currentIndex].isUnlocked) //if current item is not unlocked
//    {
//        unlockBtn.interactable = true;  //set the unlockbtn interactable
//        unlockBtnText.text = shopData.shopItems[currentIndex].unlockCost + ""; //set the text as cost of item
//    }
//}

///// <summary>
///// Method to set Upgrade button interactable and text sttus
///// </summary>
//private void UpgradeButtonStatus()
//{
//    //if current item is unlocked
//    if (shopData.shopItems[currentIndex].isUnlocked)
//    {
//        //if unlockLevel of current item is less than its max level
//        if (shopData.shopItems[currentIndex].unlockedLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
//        {
//            upgradeBtn.interactable = true;                     //make upgradeBtn interactable true
//            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedLevel + 1;
//            //set the next level as value of upgrade button text
//            upgradeBtnText.text = "Upgrade Cost:" +
//                shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].unlockCost;
//        }
//        else   //if unlockLevel of current item is equal to max level
//        {
//            upgradeBtn.interactable = false;                    //make upgradeBtn interactable false
//            upgradeBtnText.text = "Max Lvl Reached";
//        }
//    }
//    else if (!shopData.shopItems[currentIndex].isUnlocked)  //if current item is not unlocked
//    {
//        upgradeBtn.interactable = false;                        //make upgradeBtn interactable false
//        upgradeBtnText.text = "Locked";
//    }
//}
//    }
//}
