using Android.Graphics;
using Android.Widget;
using RemoseNetwork.Packets;
using ReMouse.Data.Holders;
using ReMouse.Utils.Mouse.Working;
using System;

namespace ReMouse.Utils.Mouse.Display
{
    public static class ButtonFactory
    {
        public static void CreateWorkingButton(ImageView view, DisplayablePropertiesBase properties)
        {
            ButtonType buttonType = Enum.Parse<ButtonType>(properties.Name);

            if (!properties.IsEnable && buttonType != ButtonType.MoveMouse) return;

            switch (buttonType)
            {
                case ButtonType.Button_0:
                case ButtonType.Button_1:
                case ButtonType.Button_2:
                case ButtonType.Button_3:
                case ButtonType.Button_4:
                case ButtonType.Button_5:
                case ButtonType.Button_6:
                case ButtonType.Button_7:
                case ButtonType.Button_8:
                case ButtonType.Button_9:
                case ButtonType.Copy:
                case ButtonType.Paste:
                case ButtonType.System_Volume_Mute:
                case ButtonType.System_Volume_Down:
                case ButtonType.System_Volume_Up:
                case ButtonType.Player_Control_Forward:
                case ButtonType.Player_Control_Backward:
                case ButtonType.Player_Control_PlayPause:
                case ButtonType.Player_Control_Volume_Up:
                case ButtonType.Player_Control_Volume_Down:
                case ButtonType.Right:
                    new DefaultWorkingButton(view, buttonType, properties.IsInBox);
                    break;

                case ButtonType.Middle_Click:
                case ButtonType.Middle_Down:
                case ButtonType.Middle_Up:
                    new MiddleWorkingButton(view, buttonType, properties.IsInBox);
                    break;

                case ButtonType.Settings:
                    break;
                case ButtonType.MoveMouse:
                    break;
                case ButtonType.Left:
                    new LeftWorkingButton(view, properties.IsInBox);
                    break;
                default:
                    break;
            }
        }

        public static ImageView SetupImageView(ImageView view, DisplayablePropertiesBase properties)
        {
            view.SetImageDrawable(properties.Drawable);
            view.SetX(properties.PosX);
            view.SetY(properties.PosY);
            LayoutDisplayTracker.SetupView(view, properties.Size, Color.ParseColor(properties.Color));

            return view;
        }
    }
}