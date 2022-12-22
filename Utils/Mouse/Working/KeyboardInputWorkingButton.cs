using Android.Text;
using Android.Views;
using Android.Widget;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReMouse.Utils.Mouse.Working
{
    public class KeyboardInputWorkingButton : WorkingButtonBase
    {
        private readonly EditText _text;
        private readonly string _resetText = "_";
        private readonly ImageView _image;

        private readonly Dictionary<string, int> stringToId = new Dictionary<string, int>();

        private readonly KeyboardInputPacket erase = new KeyboardInputPacket(ButtonBehaviour.KEY_BACK);
        private readonly KeyboardInputPacket enter = new KeyboardInputPacket(ButtonBehaviour.KEY_RETURN);
        private readonly KeyboardInputPacket input = new KeyboardInputPacket(ButtonBehaviour.KEY_CHAR);

        public KeyboardInputWorkingButton(ImageView image, EditText text, bool isHideable) : base(image, ButtonType.Keyboard_Input, isHideable)
        {
            _image = image;
            _text = text;
            _text.InputType = InputTypes.TextVariationPassword;

            _text.TextChanged += TextChanged;
            ResetText("erase");
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = e.Text.Count();
            string s = e.Text.ToString().Replace(_resetText, "");

            if (length == 0)
            {
                Send(erase);
                ResetText("erase");
            }
            else if (s.Contains("\n"))
            {
                Send(enter);
                ResetText("erase");
            }
            else if (length == 2)
            {
                input.Input = s;
                Send(input);
                ResetText(s);
            }
        }

        private void ResetText(string s)
        {
            _text.TextChanged -= TextChanged;
            _text.Text = _resetText;
            _text.TextChanged += TextChanged;
            _text.SetSelection(1);

            if (!stringToId.TryGetValue(s, out int id))
            {
                try
                {
                    if (char.IsDigit(s[0]))
                    {
                        id = (int)typeof(Resource.Drawable).GetField("button_" + s).GetValue(null);
                    }
                    else
                    {
                        id = (int)typeof(Resource.Drawable).GetField(s.ToLower()).GetValue(null);
                    }
                    stringToId.Add(s, id);
                }
                catch
                {
                    id = Resource.Drawable.erase;
                }
            }

            _image.SetImageResource(id);
            _image.RefreshDrawableState();
        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            _text.RequestFocus();
            MainActivity.ShowKeyboard(_text);
        }

        public override void OnLongClickEvent(object sender, View.LongClickEventArgs e)
        {
            e.Handled = true;
            Send(enter);
            return;
        }

        public override void Delete()
        {
            base.Delete();

            _text.TextChanged -= TextChanged;
        }
    }
}