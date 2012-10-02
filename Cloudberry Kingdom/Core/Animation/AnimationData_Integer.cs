﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.IO;
using CloudberryKingdom;

namespace Drawing
{
    public struct FrameData
    {
        public EzTexture texture; public Vector2 uv_bl, uv_tr;
        public FrameData(EzTexture texture)
        {
            this.texture = texture;
            uv_bl = Vector2.Zero;
            uv_tr = Vector2.One;
        }
    }
    public struct OneAnim_Texture
    {
        public FrameData[] Data;
        public float Speed;
    }
    public class AnimationData_Texture
    {
        public OneAnim_Texture[] Anims;

        public AnimationData_Texture()
        {
            Anims = null;
        }

        public AnimationData_Texture(string TextureName)
        {
            Anims = new OneAnim_Texture[1];

            AddFrame(Tools.Texture(TextureName), 0);

            var FirstFrameTexture = Anims[0].Data[0].texture;
            Width = FirstFrameTexture.Tex.Width;
            Height = FirstFrameTexture.Tex.Height;
        }
        public AnimationData_Texture(string TextureRoot, int StartFrame, int EndFrame)
        {
            Anims = new OneAnim_Texture[1];

            // Determine format of texture names.
            string format = "";
            int length = 1;
            if (Tools.Texture(TextureRoot + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
                format = "";
            else if (Tools.Texture(TextureRoot + "_" + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
                format = "_";
            else if (Tools.Texture(TextureRoot + "_0" + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
            {
                format = "_";
                length = 2;
            }
            else if (Tools.Texture(TextureRoot + "_00" + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
            {
                format = "_";
                length = 3;
            }
            else if (Tools.Texture(TextureRoot + "_000" + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
            {
                format = "_";
                length = 4;
            }
            else if (Tools.Texture(TextureRoot + "_0000" + StartFrame.ToString()) != Tools.TextureWad.DefaultTexture)
            {
                format = "_";
                length = 5;
            }

            string[] pad = { "", "0", "00", "000", "0000" };

            if (EndFrame >= StartFrame)
            {
                for (int i = StartFrame; i <= EndFrame; i++)
                {
                    int len = i == 0 ? 1 : (int)Math.Log10(i) + 1;
                    string padding = len <= length ? pad[length - len] : "";
                    AddFrame(Tools.Texture(TextureRoot + format + padding + i.ToString()), 0);
                }
            }
            else
            {
                for (int i = EndFrame; i <= StartFrame; i--)
                    AddFrame(Tools.Texture(TextureRoot + format + i.ToString()), 0);
            }

            var FirstFrameTexture = Anims[0].Data[0].texture;
            Width = FirstFrameTexture.Tex.Width;
            Height = FirstFrameTexture.Tex.Height;
        }

        public AnimationData_Texture(EzTexture texture)
        {
            if (texture == null)
            {
                Anims = null;
            }
            else
            {
                Anims = new OneAnim_Texture[1];
                AddFrame(texture, 0);
            }
        }

        public int Width, Height;

        public void Release()
        {
            if (Anims != null)
                for (int i = 0; i < Anims.Length; i++)
                    Anims[i].Data = null;
            Anims = null;
        }

        public void Write(BinaryWriter writer)
        {
            if (Anims == null) writer.Write(-1);
            else
            {
                writer.Write(Anims.Length);
                for (int i = 0; i < Anims.Length; i++)
                    WriteReadTools.WriteOneAnim(writer, Anims[i]);
            }
        }

        public void Read(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length == -1) Anims = null;
            else
            {
                Anims = new OneAnim_Texture[length];
                for (int i = 0; i < length; i++)
                    WriteReadTools.ReadOneAnim(reader, ref Anims[i]);
            }
        }

        public AnimationData_Texture(AnimationData_Texture data)
        {
            Anims = new OneAnim_Texture[data.Anims.Length];
            for (int i = 0; i < data.Anims.Length; i++)
                CopyAnim(data, i);
        }

        public void CopyAnim(AnimationData_Texture data, int Anim)
        {
            if (data.Anims[Anim].Data != null)
            {
                Anims[Anim].Speed = data.Anims[Anim].Speed;
                Anims[Anim].Data = new FrameData[data.Anims[Anim].Data.Length];
                data.Anims[Anim].Data.CopyTo(Anims[Anim].Data, 0);
            }
            else
                Anims[Anim].Data = null;
        }

        public void Init()
        {
            Anims = new OneAnim_Texture[] { new OneAnim_Texture() };
        }

        public void InsertFrame(int anim, int frame)
        {
            if (anim >= Anims.Length) return;
            if (Anims[anim].Data == null) return;
            if (frame >= Anims[anim].Data.Length) return;

            OneAnim_Texture NewAnim = new OneAnim_Texture();
            NewAnim.Data = new FrameData[Anims[anim].Data.Length + 1];
            for (int i = 0; i < frame; i++) NewAnim.Data[i] = Anims[anim].Data[i];
            NewAnim.Data[frame] = Anims[anim].Data[frame];
            for (int i = frame + 1; i < Anims[anim].Data.Length + 1; i++) NewAnim.Data[i] = Anims[anim].Data[i-1];
            Anims[anim] = NewAnim;
        }

        /// <summary>
        /// Clears a given animation back to 0 frames.
        /// </summary>
        public void ClearAnim(int anim)
        {
            if (anim >= Anims.Length) return;
            if (Anims[anim].Data == null) return;

            OneAnim_Texture NewAnim = new OneAnim_Texture();
            NewAnim.Data = new FrameData[0];
            Anims[anim] = NewAnim;
        }

        public void DeleteFrame(int anim, int frame)
        {
            if (anim >= Anims.Length) return;
            if (Anims[anim].Data == null) return;
            if (frame >= Anims[anim].Data.Length) return;

            if (Anims[anim].Data.Length > 1)
            {
                OneAnim_Texture NewAnim = new OneAnim_Texture();
                NewAnim.Data = new FrameData[Anims[anim].Data.Length - 1];
                for (int i = 0; i < frame; i++) NewAnim.Data[i] = Anims[anim].Data[i];
                for (int i = frame + 1; i < Anims[anim].Data.Length; i++) NewAnim.Data[i - 1] = Anims[anim].Data[i];
                Anims[anim] = NewAnim;
            }
        }

        public void AddFrame(EzTexture val, int anim) { AddFrame(val, anim, Vector2.Zero, Vector2.One); }
        public void AddFrame(EzTexture val, int anim, Vector2 uv, Vector2 uv_size)
        {
            int frame = 0;
            if (anim >= Anims.Length) frame = 0;
            else if (Anims[anim].Data == null) frame = 0;
            else frame = Anims[anim].Data.Length;

            Set(val, anim, frame, uv, uv_size);
        }
        public void Set(EzTexture val, int anim, int frame, Vector2 uv, Vector2 uv_size)
        {
            FrameData Default = new FrameData(null);
            if (Anims[0].Data != null)
            {
                Default = Anims[0].Data[0];
            }

            if (anim >= Anims.Length)
            {
                OneAnim_Texture[] NewAnims = new OneAnim_Texture[anim + 1];
                Anims.CopyTo(NewAnims, 0);
                Anims = NewAnims;
            }

            if (Anims[anim].Data == null)
            {
                Anims[anim].Data = new FrameData[] { new FrameData(null) };
            }
            else
                if (frame > 0)
                    Default = Get(anim, frame - 1);

            if (frame >= Anims[anim].Data.Length && !(val == Default.texture && uv == Default.uv_bl && Anims[anim].Data.Length <= 1))
            {
                FrameData[] NewData = new FrameData[frame + 1];
                for (int i = 0; i < frame + 1; i++)
                {
                    NewData[i] = new FrameData(null);
                    NewData[i].texture = Default.texture;
                }
                Anims[anim].Data.CopyTo(NewData, 0);
                Anims[anim].Data = NewData;
            }

            if (frame < Anims[anim].Data.Length)
            {
                Anims[anim].Data[frame].texture = val;
                Anims[anim].Data[frame].uv_bl = uv;
                Anims[anim].Data[frame].uv_tr = uv + uv_size;
            }
        }

        public FrameData Get(int anim, int frame)
        {
            if (Anims[0].Data == null)
                return new FrameData(null);

            FrameData Default = Anims[0].Data[0];

            if (anim >= Anims.Length)
                return Default;

            if (Anims[anim].Data == null)
                return Default;
            else
            {
                int Length = Anims[anim].Data.Length;
                if (Length > 0)
                    Default = Anims[anim].Data[0];
                else
                    return Default;
            }
            if (frame >= Anims[anim].Data.Length || frame < 0)
                return Default;

            return Anims[anim].Data[frame];
        }

        public int LastSetFrame = 0, LastSetAnim = 0;
        public FrameData NextKeyFrame()
        {
            LastSetFrame++;
            if (LastSetFrame >= Anims[LastSetAnim].Data.Length)
                LastSetFrame = 0;

            return Anims[LastSetAnim].Data[LastSetFrame];
        }

        public FrameData Calc(int anim, float t, int Length, bool Loop)
        {
            int i = (int)Math.Floor(t);
            LastSetFrame = i;
            LastSetAnim = anim;
            return Get(anim, i);
        }

        public FrameData Calc(int anim, float t)
        {
            return Calc(anim, t, Anims[anim].Data.Length, true);
        }
    }
}