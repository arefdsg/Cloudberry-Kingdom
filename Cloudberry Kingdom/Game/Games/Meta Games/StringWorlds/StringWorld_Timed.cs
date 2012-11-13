﻿using System.Collections.Generic;

using CloudberryKingdom.Levels;

using System;
using System.Threading;

namespace CloudberryKingdom
{
    public class StringWorldTimed : StringWorldGameData
    {
        GUI_Timer MyGUI_Timer;
        TimerWarning Warning;
        public GUI_Score MyGUI_Score;
        public GUI_Level MyGUI_Level;
        CoinScoreMultiplierObject MyCoinScoreMultiplier;

        public StringWorldTimed(Func<int, LevelSeedData> GetSeed, GUI_Timer Timer)
            : base(GetSeed)
        {
            MyGUI_Timer = Timer;

            Warning = new TimerWarning();
            Warning.MyTimer = Timer;

            Timer.OnTimeExpired += (timer) => MyGUI_Score.SlideOut(GUI_Panel.PresetPos.Top);
            Timer.OnTimeExpired += (timer) => MyGUI_Level.SlideOut(GUI_Panel.PresetPos.Top);

            MyGUI_Score = new GUI_Score();
            MyGUI_Level = new GUI_Level();

            // Coin score multiplier
            MyCoinScoreMultiplier = new CoinScoreMultiplierObject();

            // Add 'Perfect' watcher
            OnSwapToFirstLevel.Add(new OnSwapLambda(this));
        }

        class OnSwapLambda : Lambda_1<LevelSeedData>
        {
            StringWorldTimed ch;
            public OnSwapLambda(StringWorldTimed ch)
            {
                this.ch = ch;
            }

            public void Apply(LevelSeedData data)
            {
                data.MyGame.AddGameObject(ch.MyGUI_Timer, ch.Warning, ch.MyGUI_Score, ch.MyGUI_Level,
                    ch.MyCoinScoreMultiplier, new PerfectScoreObject(false, true));
            }
        }

        public override void Release()
        {
            base.Release();

            MyGUI_Timer.ForceRelease();
        }

        public override void AdditionalSwapToLevelProcessing(GameData game)
        {
            base.AdditionalSwapToLevelProcessing(game);

            MyGUI_Level.SetLevel(CurLevelIndex + 1);

            game.TakeOnce = true;
        }
    }
}