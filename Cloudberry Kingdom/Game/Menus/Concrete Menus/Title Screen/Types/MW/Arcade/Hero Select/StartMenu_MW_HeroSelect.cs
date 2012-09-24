using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CloudberryKingdom.Stats;

namespace CloudberryKingdom
{
    public class HeroItem : MenuItem
    {
        public BobPhsx Hero;
        public bool Locked;

        public HeroItem(BobPhsx Hero)
            : base(new EzText(Hero.Name, Tools.Font_Grobold42_2))
        {
            this.Hero = Hero;

            Locked = false;
        }
    }

    public class StartMenu_MW_HeroSelect : ArcadeBaseMenu
    {
        public static BobPhsx ChosenHero = BobPhsxNormal.Instance;

        public TitleGameData_MW Title;
        public ArcadeMenu Arcade;

        public StartMenu_MW_HeroSelect(TitleGameData_MW Title, ArcadeMenu Arcade, ArcadeItem MyArcadeItem)
            : base()
        {
            this.Title = Title;
            this.Arcade = Arcade;
            this.MyArcadeItem = MyArcadeItem;
        }

        public override void Release()
        {
            base.Release();

            if (MyHeroDoll != null) MyHeroDoll.Release();

            Title = null;
            Arcade = null;
        }

        void OnSelect()
        {
            var item = MyMenu.CurItem as HeroItem;
            if (null == item) return;

            MyHeroDoll.MakeHeroDoll(item.Hero);
        }

        public override void SlideIn(int Frames)
        {
            Title.BackPanel.SetState(StartMenu_MW_Backpanel.State.Scene_Kobbler_Blur);
            base.SlideIn(0);

            if (MyHeroDoll != null) MyHeroDoll.SlideIn(0);
        }

        public override void SlideOut(PresetPos Preset, int Frames)
        {
            base.SlideOut(Preset, 0);
            
            if (MyHeroDoll != null) MyHeroDoll.SlideOut(Preset, 0);
        }

        protected override void SetItemProperties(MenuItem item)
        {
            base.SetItemProperties(item);

            item.MySelectedText.Shadow = item.MyText.Shadow = false;

            StartMenu.SetItemProperties_Green(item, true);

            item.MyText.OutlineColor.W *= .4f;
            item.MySelectedText.OutlineColor.W *= .7f;
        }

        public override void OnAdd()
        {
            base.OnAdd();

            MyHeroDoll = new HeroDoll(Control);
            MyGame.AddGameObject(MyHeroDoll);
        }

        HeroDoll MyHeroDoll;
        public override void Init()
        {
 	        base.Init();

            MyPile = new DrawPile();

            CallDelay = ReturnToCallerDelay = 0;

            // Heroes
            BobPhsx SmallJetpackWheelie = BobPhsx.MakeCustom(Hero_BaseType.Wheel, Hero_Shape.Classic, Hero_MoveMod.Jetpack);
            SmallJetpackWheelie.Name = "Flaming Vomit";

            BobPhsx Uber = BobPhsx.MakeCustom(Hero_BaseType.Classic, Hero_Shape.Big, Hero_MoveMod.Classic);
            Uber.Name = "Uber Man";
            Uber.Gravity *= .935f;
            Uber.MaxSpeed *= 1.65f;
            Uber.XAccel *= 1.65f;

            var list = new BobPhsx[] { BobPhsxNormal.Instance,
                                       Uber,
                                       BobPhsxInvert.Instance,
                                       BobPhsxBraid.Instance,
                                       BobPhsxDouble.Instance,
                                       BobPhsxJetman.Instance,
                                       //BobPhsxMeat.Instance,
                                       BobPhsxBouncy.Instance,
                                       BobPhsxBox.Instance,
                                       //BobPhsxRocketbox.Instance,
                                       BobPhsxScale.Instance,
                                       BobPhsxSmall.Instance,
                                       BobPhsxSpaceship.Instance,
                                       BobPhsxWheel.Instance,
                                       SmallJetpackWheelie };
                                        
            // Menu
            MiniMenu mini = new MiniMenu();
            MyMenu = mini;

            mini.WrapSelect = false;
            mini.Shift = new Vector2(0, -135);
            mini.ItemsToShow = 6;
            FontScale *= .75f;
            foreach (var phsx in list)
            {
                var item = new HeroItem(phsx);
                item.AdditionalOnSelect = OnSelect;
                AddItem(item);
                item.Go = Go;
            }
            
            MyMenu.OnB = MenuReturnToCaller;
            EnsureFancy();

            /// <summary>
            /// Left Side
            /// </summary>
            #region
            // Black box, left side
            var BackBoxLeft = new QuadClass("Arcade_BoxLeft");
            BackBoxLeft.Alpha = 1f;
            MyPile.Add(BackBoxLeft, "BoxLeft");
            #endregion

            /// <summary>
            /// Right Side
            /// </summary>
            #region
            // Black box, right side
            var BackBox = new QuadClass("Arcade_Box");
            BackBox.Alpha = 1f;
            MyPile.Add(BackBox, "BoxRight");

            // Score, level
            var ScoreHeader = new EzText("High Score", Tools.Font_Grobold42_2);
            StartMenu.SetText_Green(ScoreHeader, true);
            MyPile.Add(ScoreHeader, "ScoreHeader");

            Score = new EzText("284,566", Tools.Font_Grobold42_2);
            MyPile.Add(Score, "Score");

            var LevelHeader = new EzText("Best Level", Tools.Font_Grobold42_2);
            StartMenu.SetText_Green(LevelHeader, true);
            MyPile.Add(LevelHeader, "LevelHeader");

            Level = new EzText("63", Tools.Font_Grobold42_2);
            MyPile.Add(Level, "Level");

            // Options
            string Space = "{s34,0}";
            EzText StartText = new EzText(ButtonString.Go(80) + Space + "{c122,209,39,255} Start", ItemFont, true, true);
            MyPile.Add(StartText, "Go");

            EzText LeaderText = new EzText(ButtonString.X(80) + Space + "{c150,189,244,255} Leaderboard", ItemFont, true, true);
            MyPile.Add(LeaderText, "Leaderboard");

            #endregion

            /// <summary>
            /// Back
            /// </summary>
            MyPile.Add(new QuadClass(ButtonTexture.Back), "Back");
            MyPile.Add(new QuadClass("BackArrow2", "BackArrow"));

            MyPile.FadeIn(.33f);

            SetPos();
        }

        EzText Score, Level;

        void SetPos()
        {
            MyMenu.Pos = new Vector2(-1340.222f, 104.4444f);

            EzText _t;
            _t = MyPile.FindEzText("ScoreHeader"); if (_t != null) { _t.Pos = new Vector2(-22.22266f, 636.1111f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("Score"); if (_t != null) { _t.Pos = new Vector2(402.7764f, 352.7778f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("LevelHeader"); if (_t != null) { _t.Pos = new Vector2(-2.779297f, 105.5556f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("Level"); if (_t != null) { _t.Pos = new Vector2(927.7764f, -136.1111f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("Go"); if (_t != null) { _t.Pos = new Vector2(513.8887f, -472.2224f); _t.Scale = 0.7423338f; }
            _t = MyPile.FindEzText("Leaderboard"); if (_t != null) { _t.Pos = new Vector2(825f, -655.5554f); _t.Scale = 0.7660002f; }

            QuadClass _q;
            _q = MyPile.FindQuad("BoxLeft"); if (_q != null) { _q.Pos = new Vector2(-972.2227f, -127.7778f); _q.Size = new Vector2(616.5467f, 1004.329f); }
            _q = MyPile.FindQuad("BoxRight"); if (_q != null) { _q.Pos = new Vector2(666.6641f, -88.88879f); _q.Size = new Vector2(776.5515f, 846.666f); }
            _q = MyPile.FindQuad("Back"); if (_q != null) { _q.Pos = new Vector2(-1269.443f, -1011.111f); _q.Size = new Vector2(64.49973f, 64.49973f); }
            _q = MyPile.FindQuad("BackArrow"); if (_q != null) { _q.Pos = new Vector2(-1416.666f, -1016.667f); _q.Size = new Vector2(71.89921f, 61.83332f); }

            MyPile.Pos = new Vector2(83.33417f, 130.9524f);
        }

        protected virtual void Go(MenuItem item)
        {
            var hitem = item as HeroItem;
            if (null == hitem || hitem.Locked) return;

            StartMenu_MW_HeroSelect.ChosenHero = hitem.Hero;

            StartLevelMenu levelmenu = new StartLevelMenu(MyArcadeItem.MyChallenge.HighLevel.Top);

            levelmenu.MyMenu.SelectItem(Challenge_HeroRush.PreviousMenuIndex);
            levelmenu.StartFunc = StartFunc;
            levelmenu.ReturnFunc = null;

            Call(levelmenu);
            Hide();
        }
    }
}