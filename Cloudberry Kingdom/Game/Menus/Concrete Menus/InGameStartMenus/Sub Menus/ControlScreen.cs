using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CloudberryKingdom
{
    public class ControlScreen : CkBaseMenu
    {
        QuadClass BackgroundQuad;
      
        public ControlScreen(int Control) : base(false)
        {
            EnableBounce();

            this.Control = Control;

            Constructor();
        }

#if PC_VERSION
        QuadClass MakeQuad(Keys key)
        {
            var quad = new QuadClass(ButtonString.KeyToTexture(key), 90);
            MyPile.Add(quad);
            quad.Quad.SetColor(CustomControlsMenu.SecondaryKeyColor);
            return quad;
        }
#endif
        public override void Init()
        {
 	        base.Init();

            PauseGame = true;

            SlideInFrom = SlideOutTo = PresetPos.Left;

            //ReturnToCallerDelay = SlideLength = 0;
            SlideLength = 23;
            DestinationScale *= 1.02f;

            MyPile = new DrawPile();
            EnsureFancy();

            QuadClass Backdrop;
            if (UseBounce)
                Backdrop = new QuadClass("Arcade_BoxLeft", 1500, true);
            else
                Backdrop = new QuadClass("Backplate_1230x740", 1500, true);
            Backdrop.Name = "Backdrop";
            MyPile.Add(Backdrop);

            ReturnToCallerDelay = 7;

            EzText text;

#if PC_VERSION
            text = new EzText(Localization.Words.QuickSpawn, Resources.Font_Grobold42);
            MyPile.Add(text, "quickspawn");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.PowerUpMenu, Resources.Font_Grobold42);
            MyPile.Add(text, "powerups");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.Menu, Resources.Font_Grobold42);
            MyPile.Add(text, "menu");
            text.MyFloatColor = CampaignHelper.DifficultyColor[1].ToVector4();

            text = new EzText(Localization.Words.Accept, Resources.Font_Grobold42);
            MyPile.Add(text, "accept");
            text.MyFloatColor = Menu.DefaultMenuInfo.UnselectedNextColor;
            text.MyFloatColor = Menu.DefaultMenuInfo.SelectedNextColor;

            text = new EzText(Localization.Words.Back, Resources.Font_Grobold42);
            MyPile.Add(text, "back");
            text.MyFloatColor = Menu.DefaultMenuInfo.SelectedBackColor;
            text.MyFloatColor = Menu.DefaultMenuInfo.UnselectedBackColor;

            text = new EzText("b", Resources.Font_Grobold42);
            text.SubstituteText("<");
            MyPile.Add(text, "split");

            QuadClass q;

            q = new QuadClass("Enter_Key"); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "enter");

            q = new QuadClass("Esc_Key"); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "esc");

            q = new QuadClass("Backspace_Key"); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "backspace");

            q = new QuadClass("Space_Key"); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "space");

            SetPos();
#else
            text = new EzText("+", Resources.Font_Grobold42, true);
            MyPile.Add(text, "plus");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.QuickSpawn, Resources.Font_Grobold42, true);
            MyPile.Add(text, "quickspawn");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.Jump, Resources.Font_Grobold42, true);
            MyPile.Add(text, "jump");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.PowerUpMenu, Resources.Font_Grobold42, true);
            MyPile.Add(text, "powerups");
            text.MyFloatColor = ColorHelper.Gray(.955f);

            text = new EzText(Localization.Words.Accept, Resources.Font_Grobold42, true);
            MyPile.Add(text, "accept");
#if XBOX
            text.MyFloatColor = Menu.DefaultMenuInfo.UnselectedNextColor;
            text.MyFloatColor = Menu.DefaultMenuInfo.SelectedNextColor;
#endif

            text = new EzText(Localization.Words.Back, Resources.Font_Grobold42, true);
            MyPile.Add(text, "back");
#if XBOX
            text.MyFloatColor = Menu.DefaultMenuInfo.SelectedBackColor;
            text.MyFloatColor = Menu.DefaultMenuInfo.UnselectedBackColor;
#endif

            QuadClass q;

            q = new QuadClass(ButtonTexture.X); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "x");

            q = new QuadClass("door_castle_1"); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "door");

            q = new QuadClass(ButtonTexture.Y); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "y");

            q = new QuadClass(ButtonTexture.LeftBumper); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "lb");
            q = new QuadClass(ButtonTexture.RightBumper); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "rb");


            q = new QuadClass(ButtonTexture.Go); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "jump");

            q = new QuadClass(ButtonTexture.Go); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "accep");

            q = new QuadClass(ButtonTexture.Back); q.ScaleXToMatchRatio(130);
            MyPile.Add(q, "back");

            SetPos();
#endif
        }

#if PC_VERSION
        void SetPos()
        {
            EzText _t;
            _t = MyPile.FindEzText("quickspawn"); if (_t != null) { _t.Pos = new Vector2(-288.0965f, 435.3178f); _t.Scale = 1.06f; }
            _t = MyPile.FindEzText("powerups"); if (_t != null) { _t.Pos = new Vector2(-267.0644f, 133.7302f); _t.Scale = 1.06f; }
            _t = MyPile.FindEzText("menu"); if (_t != null) { _t.Pos = new Vector2(-280.1582f, 731.7462f); _t.Scale = 1.06f; }
            _t = MyPile.FindEzText("accept"); if (_t != null) { _t.Pos = new Vector2(-286.109f, -156.3493f); _t.Scale = 1.06f; }
            _t = MyPile.FindEzText("back"); if (_t != null) { _t.Pos = new Vector2(-264.2847f, -432.5391f); _t.Scale = 1.06f; }
            _t = MyPile.FindEzText("split"); if (_t != null) { _t.Pos = new Vector2(-536.5085f, 14.28584f); _t.Scale = 1.46f; }

            QuadClass _q;
            _q = MyPile.FindQuad("Backdrop"); if (_q != null) { _q.Pos = new Vector2(0f, 0f); _q.Size = new Vector2(1500f, 902.2556f); }
            _q = MyPile.FindQuad("enter"); if (_q != null) { _q.Pos = new Vector2(-771.4287f, -234.9209f); _q.Size = new Vector2(271.0638f, 130f); }
            _q = MyPile.FindQuad("esc"); if (_q != null) { _q.Pos = new Vector2(-638.8887f, 520.2384f); _q.Size = new Vector2(138.2979f, 130f); }
            _q = MyPile.FindQuad("backspace"); if (_q != null) { _q.Pos = new Vector2(-773.8103f, -603.5712f); _q.Size = new Vector2(271.0638f, 130f); }
            _q = MyPile.FindQuad("space"); if (_q != null) { _q.Pos = new Vector2(-768.6523f, 205.9521f); _q.Size = new Vector2(271.0638f, 130f); }

            MyPile.Pos = new Vector2(0f, 0f);
        }
#else
        void SetPos()
        {
            EzText _t;
            _t = MyPile.FindEzText("plus"); if (_t != null) { _t.Pos = new Vector2(-913.889f, 483.3333f); _t.Scale = 0.5140832f; }
            _t = MyPile.FindEzText("quickspawn"); if (_t != null) { _t.Pos = new Vector2(209.1257f, 546.4289f); _t.Scale = 0.7971667f; }
            _t = MyPile.FindEzText("jump"); if (_t != null) { _t.Pos = new Vector2(211.1111f, 800f); _t.Scale = 0.7969999f; }
            _t = MyPile.FindEzText("powerups"); if (_t != null) { _t.Pos = new Vector2(224.6023f, 292.0634f); _t.Scale = 0.7926666f; }
            _t = MyPile.FindEzText("accept"); if (_t != null) { _t.Pos = new Vector2(-380.5535f, -145.2382f); _t.Scale = 0.7982503f; }
            _t = MyPile.FindEzText("back"); if (_t != null) { _t.Pos = new Vector2(-380.9515f, -407.539f); _t.Scale = 0.7832497f; }

            QuadClass _q;
            _q = MyPile.FindQuad("Backdrop"); if (_q != null) { _q.Pos = new Vector2(0f, 0f); _q.Size = new Vector2(1500f, 902.2556f); }
            _q = MyPile.FindQuad("x"); if (_q != null) { _q.Pos = new Vector2(291.6663f, -577.7776f); _q.Size = new Vector2(91.58332f, 91.58332f); }
            _q = MyPile.FindQuad("door"); if (_q != null) { _q.Pos = new Vector2(758.3333f, -649.9999f); _q.Size = new Vector2(337.3586f, 240.4165f); }
            _q = MyPile.FindQuad("y"); if (_q != null) { _q.Pos = new Vector2(-911.1113f, 141.6666f); _q.Size = new Vector2(97.74995f, 97.74995f); }
            _q = MyPile.FindQuad("lb"); if (_q != null) { _q.Pos = new Vector2(-1141.667f, 397.2222f); _q.Size = new Vector2(175.4162f, 175.4162f); }
            _q = MyPile.FindQuad("rb"); if (_q != null) { _q.Pos = new Vector2(-700.0002f, 411.1111f); _q.Size = new Vector2(179.2496f, 179.2496f); }
            _q = MyPile.FindQuad("jump"); if (_q != null) { _q.Pos = new Vector2(-913.889f, 661.1106f); _q.Size = new Vector2(102.0832f, 102.0832f); }
            _q = MyPile.FindQuad("accep"); if (_q != null) { _q.Pos = new Vector2(-955.5557f, -288.8888f); _q.Size = new Vector2(99.49992f, 99.49992f); }
            _q = MyPile.FindQuad("back"); if (_q != null) { _q.Pos = new Vector2(-949.9999f, -552.7778f); _q.Size = new Vector2(95.41663f, 95.41663f); }

            MyPile.Pos = new Vector2(0f, 0f);
        }
#endif

        protected override void MyPhsxStep()
        {
            base.MyPhsxStep();

            if (!Active) return;

            if (ButtonCheck.State(ControllerButtons.A, -1).Pressed ||
                ButtonCheck.State(ControllerButtons.B, -1).Pressed)
            {
                Active = false;
                ReturnToCaller();
            }
        }
    }
}