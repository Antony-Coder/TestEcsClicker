using UnityEngine;


namespace TestClickerEcs
{
    public class BusinessViewUpdateService
    {

        public void InitializeBusiness(in BusinessComponent business)
        {
            business.View.BusinessName.text = business.Settings.Text.Name;
            business.View.LevelLabel.text = business.Settings.Text.LevelLabel;
            business.View.ProfitLabel.text = business.Settings.Text.ProfitLabel;
            business.View.LevelUp.LevelUpLabel.text = business.Settings.Text.LevelUpLabel;

            if(business.View.Upgrades.Length != business.Settings.Upgrades.Length)
            {
                Debug.LogError("Erorr: business.View.Upgrades.Length != business.Settings.Upgrades.Length");
                return;
            }

            for(int i=0; i< business.View.Upgrades.Length; i++)
            {
                var view = business.View.Upgrades[i];
                var textConfig = business.Settings.Upgrades[i].TextUpgrade;
                var paramsConfig = business.Settings.Upgrades[i].ParamsUpgrade;

                view.NameUpgrade.text = textConfig.Name;
                view.Description.text = string.Format(textConfig.Description, paramsConfig.Procent);
            }
        }

        public void SetProgressBar(in BusinessComponent business, float value)
        {
            business.View.Bar.fillAmount = Mathf.Clamp01(value);
        }

        public void SetLevel(in BusinessComponent business, int value)
        {
            business.View.LevelValue.text = string.Format(business.Settings.Text.LevelValue, value);
        }

        public void SetProfit(in BusinessComponent business, int value)
        {
            business.View.ProfitValue.text = string.Format(business.Settings.Text.ProfitValue, value);
        }
        public void SetLevelUpPrice(in BusinessComponent business, int value)
        {
            business.View.LevelUp.Price.text = string.Format(business.Settings.Text.LevelUpValue, value);
        }

        public void SetLevelUpButtonInterctable(in BusinessComponent business, bool state)
        {
            business.View.LevelUp.Button.interactable = state;
        }

        public void SetUpgradePrice(in BusinessComponent business, int idUpgrade, int value)
        {
            business.View.Upgrades[idUpgrade].Price.text = string.Format(business.Settings.Upgrades[idUpgrade].TextUpgrade.Price, value);
        }

        public void SetUpgradePurchased(in BusinessComponent business, int idUpgrade)
        {
            business.View.Upgrades[idUpgrade].Price.text = business.Settings.Upgrades[idUpgrade].TextUpgrade.Purchased;
        }

        public void SetUpgradeButtonInterctable(in BusinessComponent business, int idUpgrade, bool state)
        {
            business.View.Upgrades[idUpgrade].Button.interactable = state;
        }

    }
}
