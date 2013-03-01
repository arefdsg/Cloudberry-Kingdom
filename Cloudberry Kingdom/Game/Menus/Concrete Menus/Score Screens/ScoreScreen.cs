using Microsoft.Xna.Framework;

using CoreEngine;
using CloudberryKingdom.Levels;
using CloudberryKingdom.Stats;
using CloudberryKingdom.InGameObjects;

namespace CloudberryKingdom
{
    public class ScoreScreen : CkBaseMenu
    {
        bool _Add_Watch, _Add_Save;
        protected virtual void MakeMenu()
        {
            if (AsMenu)
            {
                MyMenu = new Menu(false);

                MyMenu.Control = -1;

                MyMenu.OnB = null;

                _Add_Watch = MyGame.MyLevel.ReplayAvailable;
                _Add_Save = MyGame.MyLevel.MyLevelSeed != null && MyGame.MyLevel.MyLevelSeed.Saveable;

                MenuItem item, go;

				if (InCampaign)
				{
					go = item = new MenuItem(new EzText(Localization.Words.Continue, ItemFont));
					item.Go = MenuGo_Continue;
				}
				else
				{
					go = item = new MenuItem(new EzText(Localization.Words.KeepSettings, ItemFont));
					item.Go = MenuGo_NewLevel;
				}
                item.Name = "Continue";
                AddItem(item);
                item.MyText.Scale =
                item.MySelectedText.Scale *= 1.3f;
                item.Shift(new Vector2(-86f, 65));
                item.SelectedPos.X += 6;

                if (_Add_Watch)
                {
                    item = new MenuItem(new EzText(Localization.Words.WatchReplay, ItemFont));
                    item.Name = "Replay";
                    item.Go = MenuGo_WatchReplay;
                    AddItem(item);
                }

                if (_Add_Save)
                {
                    item = new MenuItem(new EzText(Localization.Words.SaveSeed, ItemFont));
                    item.Name = "Save";
                    item.Go = MenuGo_Save;
					item.Selectable = CloudberryKingdomGame.CanSave();
                    AddItem(item);
                }

				MenuItem back;
				if (InCampaign)
				{
					//back = MakeBackButton(Localization.Words.Exit, true);
					back = new MenuItem(new EzText(Localization.WordString(Localization.Words.Exit), ItemFont));
					AddItem(back);
					back.Go = MenuGo_ExitCampaign;
				}
				else
				{
					//back = MakeBackButton(Localization.Words.BackToFreeplay, true);
					back = new MenuItem(new EzText(Localization.WordString(Localization.Words.BackToFreeplay), ItemFont));
					AddItem(back);
					back.Go = MenuGo_ExitFreeplay;
				}

				MyMenu.OnB = null;
                //MyMenu.OnB = Cast.ToMenu(go.Go);
				//MyMenu.OnB = Cast.ToMenu(back.Go);

                EnsureFancy();
                MyMenu.FancyPos.RelVal = new Vector2(869.0476f, -241.6667f);
            }
            else
            {
                QuadClass ContinueButton = new QuadClass(ButtonTexture.Go, 90, false);
                ContinueButton.Name = "GoButton";
                MyPile.Add(ContinueButton);
                ContinueButton.Pos = new Vector2(180f, -477.7778f) + ShiftAll;

                EzText ContinueText = new EzText(Localization.Words.Continue, ItemFont);
                ContinueText.Name = "Continue";
                SetHeaderProperties(ContinueText);
                ContinueText.MyFloatColor = Menu.DefaultMenuInfo.SelectedNextColor;
                MyPile.Add(ContinueText);
                ContinueText.Pos = new Vector2(180f, -477.7778f) + ShiftAll;

                if (MyGame.MyLevel.ReplayAvailable)
                {
                    QuadClass XButton = new QuadClass(ButtonTexture.X, 90, false);
                    XButton.Name = "XButton";
                    MyPile.Add(XButton);
                    XButton.Pos = new Vector2(180f, -325.3333f) + ShiftAll;

                    EzText ReplayText = new EzText(Localization.Words.WatchReplay, ItemFont);
                    SetHeaderProperties(ReplayText);
                    ReplayText.MyFloatColor = Menu.DefaultMenuInfo.SelectedBackColor;
                    ReplayText.MyFloatColor = new Color(184, 231, 231).ToVector4();
                    MyPile.Add(ReplayText);
                    ReplayText.Pos = new Vector2(180f, -325.3333f) + ShiftAll;
                }
            }
        }

        EzSound ScoreSound, BonusSound;

        public int DelayPhsx = 5;

        public ScoreScreen(bool CallBaseConstructor) : base(CallBaseConstructor) { }

		bool InCampaign;
        public ScoreScreen(StatGroup group, GameData game, bool InCampaign) : base(false)
        {
			this.InCampaign = InCampaign;

            MyGame = game;
            MyStatGroup = group;
            FontScale = .6f;

            Constructor();
        }

        protected override void SetItemProperties(MenuItem item)
        {
            base.SetItemProperties(item);

			//StartMenu.SetItemProperties_Red(item);	
            //CkColorHelper.GreenItem(item);
        }

        protected override void SetHeaderProperties(EzText text)
        {
            base.SetHeaderProperties(text);

            text.Shadow = false;

			//text.MyFloatColor = ColorHelper.Gray(.85f);
			//text.OutlineColor = ColorHelper.Gray(.05f);
			text.MyFloatColor = ColorHelper.Gray(.925f);
			text.OutlineColor = ColorHelper.Gray(.05f);

			text.Shadow = true;
			text.ShadowColor = new Color(.2f, .2f, .2f, .25f);
			text.ShadowOffset = new Vector2(12, 12);

			text.Scale = FontScale * .9f;
        }

// Whether to make a menu, or a static text with key bindings
#if PC_VERSION
        static bool AsMenu = true;
#else
        static bool AsMenu = true;
#endif

        protected QuadClass LevelCleared;
        Vector2 ShiftAll = new Vector2(-110f, -20f);
        public override void Init()
        {
            base.Init();

            MyPile = new DrawPile();

            //MakeDarkBack();


			MakeDarkBack();

			//QuadClass Backdrop = new QuadClass("Score_Screen", "Backdrop");
			//MyPile.Add(Backdrop);
			QuadClass Backdrop = new QuadClass("Backplate_1230x740", "Backdrop");
			MyPile.Add(Backdrop);
			//MyPile.Add(Backdrop);
			
			EpilepsySafe(.5f);
			

			//LevelCleared = new QuadClass("LevelCleared", "Header");
			//LevelCleared.Scale(.9f);
			//MyPile.Add(LevelCleared);
			//LevelCleared.Pos = new Vector2(10, 655) + ShiftAll;
			var lc = new EzText(Localization.Words.LevelCleared, Resources.Font_Grobold42_2, "LevelCleared");
			SetHeaderProperties(lc);
			lc.Shadow = true;
			lc.ShadowOffset = new Vector2(20, 20);
			lc.ShadowColor = new Color(.36f, .36f, .36f, .86f);
			MyPile.Add(lc);

            MyPile.Add(new QuadClass("Coin_Blue", "Coin"));
            MyPile.Add(new QuadClass("Stopwatch_Black", "Stopwatch"));
            MyPile.Add(new QuadClass("Bob_Dead", "Death"));

            MakeMenu();

            ScoreSound = Tools.SoundWad.FindByName("Coin");
            BonusSound = Tools.SoundWad.FindByName("Coin");
            ScoreSound.MaxInstances = 2;

            SetPos();
        }

        void SetPos()
        {
			//SetHeaderProperties(MyPile.FindEzText("Coins"));
			//SetHeaderProperties(MyPile.FindEzText("Bobs"));
			//SetHeaderProperties(MyPile.FindEzText("Deaths"));



            MenuItem _item;
            _item = MyMenu.FindItemByName("Continue"); if (_item != null) { _item.SetPos = new Vector2(-871.7776f, 516.6667f); _item.MyText.Scale = 0.78f; _item.MySelectedText.Scale = 0.78f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Save"); if (_item != null) { _item.SetPos = new Vector2(-646.8889f, 266.6737f); _item.MyText.Scale = 0.6f; _item.MySelectedText.Scale = 0.6f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Replay"); if (_item != null) { _item.SetPos = new Vector2(-641.3332f, 91.67379f); _item.MyText.Scale = 0.6f; _item.MySelectedText.Scale = 0.6f; _item.SelectIconOffset = new Vector2(0f, 0f); }
            _item = MyMenu.FindItemByName("Back"); if (_item != null) { _item.SetPos = new Vector2(-755.2222f, -75.5462f); _item.MyText.Scale = 0.6f; _item.MySelectedText.Scale = 0.6f; _item.SelectIconOffset = new Vector2(0f, 0f); }

            MyMenu.Pos = new Vector2(902.3811f, -136.1111f);

            EzText _t;
			_t = MyPile.FindEzText("LevelCleared"); if (_t != null) { _t.Pos = new Vector2(-930.5547f, 797.2222f); _t.Scale = 1.195833f; }
            _t = MyPile.FindEzText("Coins"); if (_t != null) { _t.Pos = new Vector2(-719.4445f, 22.22227f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("Blobs"); if (_t != null) { _t.Pos = new Vector2(-661.1107f, -402.7777f); _t.Scale = 1f; }
            _t = MyPile.FindEzText("Deaths"); if (_t != null) { _t.Pos = new Vector2(-655.5553f, 411.1111f); _t.Scale = 1f; }

            QuadClass _q;
            _q = MyPile.FindQuad("Backdrop"); if (_q != null) { _q.Pos = new Vector2(27.77808f, -6.666618f); _q.Size = new Vector2(1509.489f, 943.4307f); }
            _q = MyPile.FindQuad("Backdrop"); if (_q != null) { _q.Pos = new Vector2(27.77808f, -6.666618f); _q.Size = new Vector2(1509.489f, 943.4307f); }
            _q = MyPile.FindQuad("Header"); if (_q != null) { _q.Pos = new Vector2(16.66687f, 615.5555f); _q.Size = new Vector2(937.8f, 147.6f); }
            _q = MyPile.FindQuad("Coin"); if (_q != null) { _q.Pos = new Vector2(-955.5555f, -141.6665f); _q.Size = new Vector2(168.1188f, 168.1188f); }
            _q = MyPile.FindQuad("Stopwatch"); if (_q != null) { _q.Pos = new Vector2(-886.1108f, -513.8888f); _q.Size = new Vector2(180.9532f, 211.6065f); }
            _q = MyPile.FindQuad("Death"); if (_q != null) { _q.Pos = new Vector2(-983.3334f, 230.5556f); _q.Size = new Vector2(321.4366f, 235.7202f); }

            MyPile.Pos = new Vector2(0f, 0f);
        }

        FancyVector2 zoom = new FancyVector2();
        public static bool UseZoomIn = true;

        protected StatGroup MyStatGroup = StatGroup.Level;
        public override void OnAdd()
        {
 	        base.OnAdd();

            if (UseZoomIn)
            {
                SlideIn(0);
                zoom.MultiLerp(6, DrawPile.BubbleScale.Map(v => (v - Vector2.One) * .3f + Vector2.One));
            }

            // Calculate scores
            PlayerManager.CalcScore(MyStatGroup);

            int Coins = PlayerManager.PlayerSum(p => p.GetStats(MyStatGroup).Coins);
            int CoinTotal = PlayerManager.PlayerMax(p => p.GetStats(MyStatGroup).TotalCoins);
            int Blobs = PlayerManager.PlayerSum(p => p.GetStats(MyStatGroup).Blobs);
            int BlobTotal = PlayerManager.PlayerMax(p => p.GetStats(MyStatGroup).TotalBlobs);

			
			EzText text;
			
			text = new EzText(Tools.ScoreString(Coins, CoinTotal), ItemFont, "Coins");
			SetHeaderProperties(text);
            MyPile.Add(text);

            text = new EzText(CoreMath.ShortTime(PlayerManager.Score_Time), ItemFont, "Blobs");
			SetHeaderProperties(text);
			MyPile.Add(text);

            text = new EzText(Tools.ScoreString(PlayerManager.Score_Attempts), ItemFont, "Deaths");
			SetHeaderProperties(text);
			MyPile.Add(text);


            // Prevent menu interactions for a second
            MyMenu.Active = false;

            SetPos();
            MyMenu.SortByHeight();

            MyGame.WaitThenDo(DelayPhsx, () => MyMenu.Active = true);
        }

        protected override void MyDraw()
        {
            if (Core.MyLevel.Replay || Core.MyLevel.Watching) return;

            Vector2 SaveZoom = MyGame.Cam.Zoom;
            Vector2 SaveHoldZoom = MyGame.Cam.HoldZoom;
            Tools.QDrawer.Flush();

            if (zoom != null)
            {
                MyGame.Cam.Zoom = .001f * zoom.Update();
                MyGame.Cam.SetVertexCamera();
                EzText.ZoomWithCamera_Override = true;
            }

            Pos.SetCenter(Core.MyLevel.MainCamera, true);
            Pos.Update();

            base.MyDraw();

            Tools.Render.EndSpriteBatch();

            if (zoom != null)
            {
                MyGame.Cam.Zoom = SaveZoom;
                MyGame.Cam.HoldZoom = SaveHoldZoom;
                MyGame.Cam.SetVertexCamera();
                EzText.ZoomWithCamera_Override = false;
                Tools.QDrawer.Flush();
            }
        }

        protected override void MyPhsxStep()
        {
            Level level = Core.MyLevel;

            if (level != null)
                level.PreventReset = true;

            if (level.Replay || level.Watching)
                return;

            if (Active)
            {
                if (!ShouldSkip())
                {
                    if (AsMenu)
                        base.MyPhsxStep();
                    else
                        GUI_Phsx();
                }
            }
        }

        /// <summary>
        /// Play another level with the same seed
        /// </summary>
        protected void MenuGo_NewLevel(MenuItem item)
        {
            SlideOut(PresetPos.Left);

            Active = false;

            //Tools.SongWad.FadeOut();
            MyGame.EndMusicOnFinish = false;

            MyGame.WaitThenDo(36, () => MyGame.EndGame(true));
            return;
        }

        /// <summary>
        /// Called when 'Continue' is selected from the menu.
        /// The Score Screen slides out and the current game's EndGame function is called.
        /// </summary>
        protected virtual void MenuGo_Continue(MenuItem item)
        {
			SaveGroup.SaveAll();

            SlideOut(PresetPos.Left);

			if (InCampaign)
			{
				StringWorldGameData stringworld = Tools.WorldMap as StringWorldGameData;

				Door door = (ILevelConnector)Tools.CurLevel.FindIObject(LevelConnector.EndOfLevelCode) as Door;
				door.OnOpen = d => GameData.EOL_DoorAction(d);

				if (stringworld != null)
				{
					bool fade = door.MyLevel.MyLevelSeed != null && door.MyLevel.MyLevelSeed.FadeOut;
					if (fade)
						door.OnEnter = stringworld.EOL_StringWorldDoorEndAction_WithFade;
					else
						door.OnEnter = EOL_WaitThenDoEndAction;
								   
					stringworld.EOL_StringWorldDoorAction(door);
				}
			}
			else
			{
				MyGame.WaitThenDo(SlideOutLength + 2, () => MyGame.EndGame(false));
			}
        }

		void EOL_WaitThenDoEndAction(Door door)
		{
			StringWorldGameData stringworld = Tools.WorldMap as StringWorldGameData;

			if (stringworld != null)
			{
				door.Game.WaitThenDo(35, () => stringworld.EOL_StringWorldDoorEndAction(door));
			}
		}

        /// <summary>
        /// Called when 'Exit Freeplay' is selected from the menu.
        /// The Score Screen slides out and the current game's EndGame function is called.
        /// </summary>
        protected virtual void MenuGo_ExitFreeplay(MenuItem item)
        {
            SlideOut(PresetPos.Left);

			//if (MyGame.ParentGame != null)
			//{
			//    CustomLevel_GUI.ExitFreeplay = true;
			//}

            MyGame.WaitThenDo(SlideOutLength + 2, () => MyGame.EndGame(false));
        }

		void MenuGo_ExitCampaign(MenuItem item)
		{
			Tools.CurrentAftermath = new AftermathData();
			Tools.CurrentAftermath.Success = false;
			Tools.CurrentAftermath.EarlyExit = true;

			Tools.CurGameData.EndGame(false);
		}

        protected void MenuGo_Stats(MenuItem item)
        {
            Call(new StatsMenu(MyStatGroup), 19);
        }

        /// <summary>
        /// Called when 'Watch Replay' is selected from the menu.
        /// The level's replay is loaded, with the level's current information saved.
        /// </summary>
        protected void MenuGo_WatchReplay(MenuItem item)
        {
            if (AsMenu)
            {
                Active = false;

                MyGame.WaitThenDo(35, () =>
                    {
                        OnReturnTo(); // Re-activate the Score Screen object
                        Core.MyLevel.WatchReplay(true); // Start the replay
                    });
            }
            else
            {
                Core.MyLevel.WatchReplay(true);
            }
        }

        protected void MenuGo_Save(MenuItem item)
        {
            if (CloudberryKingdomGame.IsDemo)
            {
                Call(new UpSellMenu(Localization.Words.UpSell_SaveLoad, MenuItem.ActivatingPlayer), 0);
                Hide(PresetPos.Left, 0);
            }
            else
            {
#if PC_VERSION
                PlayerData player = MenuItem.GetActivatingPlayerData();
                SaveLoadSeedMenu.MakeSave(this, player)(item);
                Hide(PresetPos.Left);
#elif XBOX
				PlayerData player = MenuItem.GetActivatingPlayerData();
				if (CloudberryKingdomGame.CanSave(player.MyPlayerIndex))
				{
					SaveLoadSeedMenu.MakeSave(this, player)(item);
				}
				else
				{
					CloudberryKingdomGame.ShowError_CanNotSaveNoDevice();
				}
#else
				PlayerData player = MenuItem.GetActivatingPlayerData();
				SaveLoadSeedMenu.MakeSave(this, player)(item);
#endif
            }
        }

        int LastActive;
        bool ShouldSkip()
        {
            if (LastActive + 5 < Tools.TheGame.PhsxCount)
            {
                LastActive = Tools.TheGame.PhsxCount;
                return true;
            }
            else
            {
                LastActive = Tools.TheGame.PhsxCount;
                return false;
            }
        }

        public void GUI_Phsx()
        {
            Level level = Core.MyLevel;

            if (MyGame.MyLevel.ReplayAvailable)
            {
                bool WatchReplay = false;
                if (level.CanWatchReplay && ButtonCheck.State(ControllerButtons.X, -1).Pressed)
                    WatchReplay = true;
#if PC_VERSION
            if (Tools.Keyboard.IsKeyDownCustom(Microsoft.Xna.Framework.Input.Keys.Escape) ||
                Tools.PrevKeyboard.IsKeyDownCustom(Microsoft.Xna.Framework.Input.Keys.Escape))
                WatchReplay = false;
#endif

                if (WatchReplay)
                    MenuGo_WatchReplay(null);
            }

            if (ButtonCheck.State(ControllerButtons.A, -1).Pressed)
                MenuGo_Continue(null);
        }
    }
}