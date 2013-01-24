using Microsoft.Xna.Framework;
using CloudberryKingdom.Stats;

namespace CloudberryKingdom
{
    public class StartMenu_MW : StartMenu
    {
        bool CallingOptionsMenu;
        protected override void MenuGo_Options(MenuItem item)
        {
            Title.BackPanel.SetState(StartMenu_MW_Backpanel.State.Scene_Blur_Dark);
            Call(new StartMenu_MW_Options(Control, true), 0);
            CallingOptionsMenu = true;
        }

        protected override void Exit()
        {
            SelectSound = null;
            Title.BackPanel.SetState(StartMenu_MW_Backpanel.State.Scene_Blur_Dark);
            Call(new StartMenu_MW_Exit(Control), 0);
        }

        protected override void BringNextMenu()
        {
            base.BringNextMenu();

            Hide();
        }

        public TitleGameData_MW Title;
        public StartMenu_MW(TitleGameData_MW Title) : base()
        {
            this.Title = Title;
            CallingOptionsMenu = false;
        }

        public override void SlideIn(int Frames)
        {
            Title.BackPanel.SetState(StartMenu_MW_Backpanel.State.Scene_Title);
            base.SlideIn(0);
        }

        public override void SlideOut(PresetPos Preset, int Frames)
        {
            base.SlideOut(Preset, 0);
        }

        protected override void BringCampaign()
        {
            base.BringCampaign();

            Call(new StartMenu_MW_Campaign(Title));
        }

        protected override void BringArcade()
        {
            base.BringArcade();

            Call(new StartMenu_MW_Arcade(Title));
        }

        protected override void BringFreeplay()
        {
            base.BringFreeplay();

            SkipCallSound = true;
            Call(new StartMenu_MW_CustomLevel(Title));
        }

        protected override void SetItemProperties(MenuItem item)
        {
            base.SetItemProperties(item);

            item.MySelectedText.Shadow = item.MyText.Shadow = false;
        }

        public override void OnAdd()
        {
            base.OnAdd();
        }

        public override void OnReturnTo()
        {
            if (CallingOptionsMenu)
            {
                MyMenu.SelectItem(4);
                CallingOptionsMenu = false;
            }

            base.OnReturnTo();
        }

        public override bool MenuReturnToCaller(Menu menu)
        {
            if (NoBack) return false;

            return base.MenuReturnToCaller(menu);
        }

        public override void Init()
        {
 	        base.Init();

            CallDelay = ReturnToCallerDelay = 0;
            MyMenu.OnB = MenuReturnToCaller;

            BackBox = new QuadClass("Title_Strip");
            BackBox.Alpha = .9f;
            MyPile.Add(BackBox, "Back");

            //MyPile.FadeIn(.33f);

            SmallBlackBox();
        }

        protected override void MakeMenu()
        {
            MenuItem item;

            // Arcade
            item = new MenuItem(new EzText(Localization.Words.TheArcade, ItemFont));
            item.Name = "Arcade";
            item.Go = MenuGo_Arcade;
            AddItem(item);

            // Campaign
            item = new MenuItem(new EzText(Localization.Words.StoryMode, ItemFont));
            item.Name = "Campaign";
            AddItem(item);
            item.Go = MenuGo_Campaign;

            // Free Play
            item = new MenuItem(new EzText(Localization.Words.FreePlay, ItemFont));
            item.Name = "Freeplay";
            item.Go = MenuGo_Freeplay;
            AddItem(item);

            // Options
            item = new MenuItem(new EzText(Localization.Words.Options, ItemFont));
            item.Name = "Options";
            item.Go = MenuGo_Options;
            AddItem(item);

            // Exit
            item = new MenuItem(new EzText(Localization.Words.Back, ItemFont));
            item.Name = "Exit";
            item.Go = ItemReturnToCaller; //MenuGo_Exit;
            AddItem(item);

            EnsureFancy();

            this.CallToLeft = true;
        }

        QuadClass BackBox;

        void SmallBlackBox()
        {
            BackBox.TextureName = "White";
            BackBox.Quad.SetColor(ColorHelper.Gray(.1f));
            BackBox.Alpha = .73f;

            MenuItem _item;
            _item = MyMenu.FindItemByName("Arcade"); if (_item != null) { _item.SetPos = new Vector2(-2246.667f, 365.5279f); _item.MyText.Scale = 0.66f; _item.MySelectedText.Scale = 0.66f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Campaign"); if (_item != null) { _item.SetPos = new Vector2(-2247.832f, 160.3057f); _item.MyText.Scale = 0.66f; _item.MySelectedText.Scale = 0.66f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Freeplay"); if (_item != null) { _item.SetPos = new Vector2(-2161.775f, -26.47217f); _item.MyText.Scale = 0.66f; _item.MySelectedText.Scale = 0.66f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Options"); if (_item != null) { _item.SetPos = new Vector2(-2115.221f, -216.0278f); _item.MyText.Scale = 0.66f; _item.MySelectedText.Scale = 0.66f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Exit"); if (_item != null) { _item.SetPos = new Vector2(-2014.667f, -419.1389f); _item.MyText.Scale = 0.66f; _item.MySelectedText.Scale = 0.66f; _item.SelectIconOffset = new Vector2(0f, 0f); }

            MyMenu.Pos = new Vector2(1707.142f, -218.4129f);

            QuadClass _q;
            _q = MyPile.FindQuad("Back"); if (_q != null) { _q.Pos = new Vector2(-61.11133f, -336.1111f); _q.Size = new Vector2(524.4158f, 524.4158f); }

            MyPile.Pos = new Vector2(-27.77734f, -33.33337f);
        }
    }
}