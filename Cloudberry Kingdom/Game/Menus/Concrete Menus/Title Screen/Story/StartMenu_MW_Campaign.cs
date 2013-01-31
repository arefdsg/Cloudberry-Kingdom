using System.Collections.Generic;

using Microsoft.Xna.Framework;

using CloudberryKingdom.Stats;

namespace CloudberryKingdom
{
    class CampaignChapterItem : MenuItem
    {
        public int Chapter = 0;
        public bool Locked = false;

        public CampaignChapterItem(EzText Text, int Chapter)
            : base(Text)
        {
            this.Chapter = Chapter;

            UpdateLock();
        }

        public void UpdateLock()
        {
            Locked = false;
            if (!CloudberryKingdomGame.Unlock_Levels)
            {
				int level = PlayerManager.MaxPlayerTotalCampaignLevel();
				if (CampaignSequence.Instance.ChapterEnd.ContainsKey(Chapter - 1))
					Locked = level < CampaignSequence.Instance.ChapterEnd[Chapter - 1];
            }
        }
    }

    public class StartMenu_MW_Campaign : StartMenu
    {
        public override void OnReturnTo()
        {
            base.OnReturnTo();

            Update();
        }

        void Update()
        {
			EzText _t;

            // Update level text
            int Level = PlayerManager.MaxPlayerTotalCampaignIndex();
			bool ShowLevel = Level > 0;
			//bool ShowLevel = false;

			//string template_level = Localization.WordString(Localization.Words.Continue) + ", " +
			//              Localization.WordString(Localization.Words.Level) + " {0}";
			string template_level = Localization.WordString(Localization.Words.Continue);


			MenuItem __item = MyMenu.FindItemByName("Continue");
			if (__item != null)
			{
				if (Level < 1)
				{
					Level = 1;
					__item.Selectable = false;
					__item.Show = false;
					MyMenu.SelectItem(1);
				}
				else
				{
					__item.Selectable = true;
					__item.Show = true;
				}

				__item.MyText.SubstituteText(string.Format(template_level, Level));
				__item.MySelectedText.SubstituteText(string.Format(template_level, Level));
			}

			if (ShowLevel)
            {
                MyPile.FindEzText("Level").Show = true;
                MyPile.FindQuad("BoxLeft").Show = true;

                _t = MyPile.FindEzText("LevelNum");
                _t.Show = true;
                _t.SubstituteText(Level.ToString());
            }
            else
            {
                MyPile.FindEzText("Level").Show = false;
                MyPile.FindEzText("LevelNum").Show = false;
                MyPile.FindQuad("BoxLeft").Show = false;
            }


            // Update menu items (faded if locked)
            foreach (MenuItem _item in MyMenu.Items)
            {
                CampaignChapterItem item = _item as CampaignChapterItem;
                if (null != item)
                {
                    item.UpdateLock();

                    if (item.Locked)
                    {
                        item.MyText.Alpha = .4f;
                        item.MySelectedText.Alpha = .4f;
                    }
                    else
                    {
                        item.MyText.Alpha = 1f;
                        item.MySelectedText.Alpha = 1f;
                    }
                }
            }
        }

        public TitleGameData_MW Title;
        public StartMenu_MW_Campaign(TitleGameData_MW Title)
            : base()
        {
            this.Title = Title;
        }

        public override void SlideIn(int Frames)
        {
            Title.BackPanel.SetState(StartMenu_MW_Backpanel.State.Scene_Princess);

            base.SlideIn(0);
        }

        public override void SlideOut(PresetPos Preset, int Frames)
        {
            base.SlideOut(Preset, 0);
        }

        protected void SetText(EzText text)
        {
            text.MyFloatColor = new Color(34, 214, 47).ToVector4();
            text.OutlineColor = new Color(0, 0, 0, 0).ToVector4();
        }

        protected override void SetItemProperties(MenuItem item)
        {
            base.SetItemProperties(item);

            SetText(item.MyText);

            item.MySelectedText.Shadow = item.MyText.Shadow = false;
            item.MySelectedText.MyFloatColor = new Color(73, 255, 86).ToVector4(); 
            item.MySelectedText.OutlineColor = new Color(0, 0, 0, 0).ToVector4();

            //item.MyOscillateParams.Set(1f, 1.01f, .005f);
        }

        public override void OnAdd()
        {
            base.OnAdd();
        }

        public override void Init()
        {
 	        base.Init();

            CallDelay = ReturnToCallerDelay = 0;
            MyMenu.OnB = MenuReturnToCaller;

            MyMenu.ClearList();

            MakeHeader();

            CreateMenu();

            Update();
        }

        protected virtual void CreateMenu()
        {
            MenuItem item;

			//string template_chapter = Localization.WordString(Localization.Words.Chapter);
			string template_chapter = Localization.WordString(Localization.Words.Chapter) + ", The {1}";

			// Continue
			int level = PlayerManager.MaxPlayerTotalCampaignIndex();
			item = new CampaignChapterItem(new EzText("xxx", ItemFont), -1);
			item.Name = "Continue";
			item.Go = Go;
			AddItem(item);

            // Chapter 1
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 1), ItemFont), 1);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 1, Localization.Words.Sea), ItemFont), 1);
            item.Name = "Chapter1";
            item.Go = Go;
            AddItem(item);

            // Chapter 2
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 2), ItemFont), 2);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 2, Localization.Words.Hills), ItemFont), 2);
            item.Name = "Chapter2";
            item.Go = Go;
            AddItem(item);

            // Chapter 3
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 3), ItemFont), 3);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 3, Localization.Words.Forest), ItemFont), 3);
            item.Name = "Chapter3";
            item.Go = Go;
            AddItem(item);

            // Chapter 4
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 4), ItemFont), 4);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 4, Localization.Words.Cloud), ItemFont), 4);
            item.Name = "Chapter4";
            item.Go = Go;
            AddItem(item);

            // Chapter 5
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 5), ItemFont), 5);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 5, Localization.Words.Cave), ItemFont), 5);
            item.Name = "Chapter5";
            item.Go = Go;
            AddItem(item);

			// Chapter 6
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 6), ItemFont), 6);
			item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 6, Localization.Words.Castle), ItemFont), 6);
			item.Name = "Chapter6";
			item.Go = Go;
			AddItem(item);

			// Chapter 7
			//item = new CampaignChapterItem(new EzText(string.Format(template_chapter, 7), ItemFont), 7);
			item = new CampaignChapterItem(new EzText(Localization.Words.TheMasochist, ItemFont), 7);
			item.Name = "Chapter7";
			item.Go = Go;
			AddItem(item);


            //// Cinematics
            //item = new MenuItem(new EzText("Cinematics", ItemFont));
            //item.Name = "Cine";
            //item.Go = null;
            //AddItem(item);
            //item.MyText.MyFloatColor = new Color(241, 32, 117).ToVector4();
            //item.MySelectedText.MyFloatColor = new Color(251, 52, 137).ToVector4();

            MyMenu.SelectItem(0);


            // Level
            var TextBack = new QuadClass("Arcade_BoxLeft", 100, true);
            TextBack.Alpha = 1f;
            TextBack.Degrees = 90;
            MyPile.Add(TextBack, "BoxLeft");

            var LevelText = new EzText(Localization.Words.Level, Resources.Font_Grobold42);
            LevelText.Scale *= .72f;
            StartMenu.SetText_Green(LevelText, true);
            MyPile.Add(LevelText, "Level");
            LevelText.Show = false;

            var LevelNum = new EzText("Garbage", Resources.Font_Grobold42);
            LevelNum.Scale *= 1.1f;
            StartMenu.SetText_Green(LevelNum, true);
            MyPile.Add(LevelNum, "LevelNum");
            LevelNum.Show = false;

            //SetPos_NoCinematic();
            SetPos_WithCinematic();
        }

        protected void MakeHeader()
        {
            var Header = new EzText(Localization.Words.StoryMode, ItemFont);
            Header.Name = "Header";
            Header.Scale *= 1.3f;
            SetText(Header);
            Header.OutlineColor = Color.Black.ToVector4();
            MyPile.Add(Header);
            
            Header.Pos = new Vector2(-800.0029f, 863.8889f);
        }

        void Go(MenuItem item)
        {
            if (CloudberryKingdomGame.LockCampaign) return;

            CampaignChapterItem c_item = item as CampaignChapterItem;
            if (null == c_item) return;

            if (c_item.Locked) return;

            Go(c_item.Chapter);
        }

        int _StartLevel;
        void Go(int StartLevel)
        {
            Tools.SongWad.FadeOut();
            MyGame.FadeToBlack(.0225f, 20);
            Active = false;

            _StartLevel = StartLevel;
            MyGame.WaitThenDo(75, _Go);
        }

        private void _Go()
        {
            Active = true;
            MyGame.FadeIn(.05f);
            CampaignSequence.Instance.Start(_StartLevel);
            MyGame.WaitThenDo(0, OnReturnFromGame);
        }

        public void OnReturnFromGame()
        {
            Update();
            SaveGroup.SaveAll();

			Tools.PlayHappyMusic(MyGame);
        }

        void SetPos_NoCinematic()
        {
            MenuItem _item;
            _item = MyMenu.FindItemByName("Chapter1"); if (_item != null) { _item.SetPos = new Vector2(686.4453f, 191.6667f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, 186.1112f)); }
            _item = MyMenu.FindItemByName("Chapter2"); if (_item != null) { _item.SetPos = new Vector2(708.665f, -36.44455f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
            _item = MyMenu.FindItemByName("Chapter3"); if (_item != null) { _item.SetPos = new Vector2(711.4443f, -239.5557f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
            _item = MyMenu.FindItemByName("Chapter4"); if (_item != null) { _item.SetPos = new Vector2(714.2227f, -437.111f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
            _item = MyMenu.FindItemByName("Chapter5"); if (_item != null) { _item.SetPos = new Vector2(730.8906f, -656.889f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }

            MyMenu.Pos = new Vector2(-783.3339f, 227.7778f);
        }

        void SetPos_WithCinematic()
        {
			MenuItem _item;
			_item = MyMenu.FindItemByName("Continue"); if (_item != null) { _item.SetPos = new Vector2(740f, 251.6668f); _item.MyText.Scale = 0.5006671f; _item.MySelectedText.Scale = 0.5006671f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter1"); if (_item != null) { _item.SetPos = new Vector2(740f, 58.33334f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter2"); if (_item != null) { _item.SetPos = new Vector2(740f, -92.00005f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter3"); if (_item != null) { _item.SetPos = new Vector2(742.7776f, -247.8891f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter4"); if (_item != null) { _item.SetPos = new Vector2(745.5554f, -400.9998f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter5"); if (_item != null) { _item.SetPos = new Vector2(740f, -548.5557f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter6"); if (_item != null) { _item.SetPos = new Vector2(750.3334f, -700.3333f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }
			_item = MyMenu.FindItemByName("Chapter7"); if (_item != null) { _item.SetPos = new Vector2(750.3334f, -850.6672f); _item.MyText.Scale = 0.5883336f; _item.MySelectedText.Scale = 0.5883336f; _item.SelectIconOffset = new Vector2(0f, 0f); }

			MyMenu.Pos = new Vector2(-708.3339f, 216.6667f);

			EzText _t;
			_t = MyPile.FindEzText("Header"); if (_t != null) { _t.Pos = new Vector2(-800.0029f, 863.8889f); _t.Scale = 1.3f; }
			_t = MyPile.FindEzText("Level"); if (_t != null) { _t.Pos = new Vector2(-1241.667f, -577.7778f); _t.Scale = 0.7490832f; }
			_t = MyPile.FindEzText("LevelNum"); if (_t != null) { _t.Pos = new Vector2(-775.0001f, -513.8888f); _t.Scale = 1.001751f; }

			QuadClass _q;
			_q = MyPile.FindQuad("BoxLeft"); if (_q != null) { _q.Pos = new Vector2(-755.5557f, -702.7777f); _q.Size = new Vector2(172.6158f, 503.8864f); }

			MyPile.Pos = new Vector2(0f, 0f);

			//MenuItem _item;
			//_item = MyMenu.FindItemByName("Chapter1"); if (_item != null) { _item.SetPos = new Vector2(686.4453f, 191.6667f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, 186.1112f)); }
			//_item = MyMenu.FindItemByName("Chapter2"); if (_item != null) { _item.SetPos = new Vector2(708.665f, -36.44455f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
			//_item = MyMenu.FindItemByName("Chapter3"); if (_item != null) { _item.SetPos = new Vector2(711.4443f, -239.5557f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
			//_item = MyMenu.FindItemByName("Chapter4"); if (_item != null) { _item.SetPos = new Vector2(714.2227f, -437.111f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }
			//_item = MyMenu.FindItemByName("Chapter5"); if (_item != null) { _item.SetPos = new Vector2(730.8906f, -656.889f); _item.MyText.Scale = 0.8f; _item.MySelectedText.Scale = 0.8f; _item.SelectIconOffset = new Vector2(0f, 0f); _item.SetSelectedPos(new Vector2(622.5566f, -1f)); }

			//MyMenu.Pos = new Vector2(-783.3339f, 227.7778f);

			//EzText _t;
			//_t = MyPile.FindEzText("Header"); if (_t != null) { _t.Pos = new Vector2(-800.0029f, 863.8889f); _t.Scale = 1.3f; }
			//_t = MyPile.FindEzText("Level"); if (_t != null) { _t.Pos = new Vector2(-1241.667f, -577.7778f); _t.Scale = 0.7490832f; }
			//_t = MyPile.FindEzText("LevelNum"); if (_t != null) { _t.Pos = new Vector2(-775.0001f, -513.8888f); _t.Scale = 1.001751f; }

			//QuadClass _q;
			//_q = MyPile.FindQuad("BoxLeft"); if (_q != null) { _q.Pos = new Vector2(-755.5557f, -702.7777f); _q.Size = new Vector2(172.6158f, 503.8864f); }

			//MyPile.Pos = new Vector2(0f, 0f);
        }
    }
}