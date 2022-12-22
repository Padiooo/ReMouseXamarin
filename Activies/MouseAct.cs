
using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using RemoseNetwork.Packets;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using ReMouse.Utils.Mouse.Display;
using ReMouse.Utils.Mouse.Working;

namespace ReMouse.Activies
{
    [Activity(Label = "MouseAct")]
    public class MouseAct : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LayoutDisplayTracker.SetLayout(this, Resource.Layout.mouse, Resource.Id.mouse);
            // Create your application here

            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.mouse);

            // build mouse
            new MouseWorkingButton(layout);

            foreach (DisplayablePropertiesBase properties in DisplayablePropertiesBase.GetEnabled())
            {
                ImageView imageView = new ImageView(this);
                layout.AddView(imageView);
                imageView.RequestLayout();
                imageView = ButtonFactory.SetupImageView(imageView, properties);
                if (properties.Name == ButtonType.Keyboard_Input.ToString())
                {
                    EditText editText = new EditText(this);
                    layout.AddView(editText);
                    editText.RequestLayout();
                    editText.SetX(-500);
                    editText.SetY(-500);
                    editText.LayoutParameters.Height = 10;
                    editText.LayoutParameters.Width = 10;
                    editText.RequestLayout();
                    editText.InputType = InputTypes.TextVariationWebEmailAddress;
                    new KeyboardInputWorkingButton(imageView, editText, properties.IsInBox);
                }
                else if (properties.Name == "Box")
                {
                    new BoxWorkingButton(imageView);
                }
                else
                {
                    ButtonFactory.CreateWorkingButton(imageView, properties);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var button in WorkingButtonBase.activeButtons.ToArray())
            {
                button.Delete();
            }
        }
    }
}