using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ProstoDajteMne3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //InitializeComponent();
            Label title = new Label
            {
                Text = "Добро пожаловать в мое приложение для фонарика!",
                FontSize = 32
            };
            Button btn = new Button
            {
                Text = "Начать!"
            };
            btn.Clicked += Btn_Clicked;
            Content = new StackLayout
            {
                Children = { title, btn }
            };
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FlashlightPage());
        }
    }
    public class FlashlightPage : ContentPage
    {
        
        bool on, flick, sos;
        Slider slider;
        Entry morse;
        public FlashlightPage()
        {
            Button _switch = new Button
            {
                Text = "Включить фонарик!"
            };
            slider = new Slider
            {
                Maximum = 1000.0,
                Minimum = 1.0,
            };
            Button enableSlider = new Button
            {
                Text = "Включить мерцание."
            };
            Button enableSos = new Button
            {
                Text = "Включить SOS сигнал."
            };
            Button morseTranslate = new Button
            {
                Text = "Перевести введенный текст в код морзе."
            };
            morse = new Entry();
            morseTranslate.Clicked += MorseTranslate_Clicked;
            enableSos.Clicked += EnableSos_Clicked;
            enableSlider.Clicked += EnableSlider_Clicked;
            _switch.Clicked += _switch_Clicked;
            Content = new StackLayout
            {
                Children = { _switch, slider, enableSlider, enableSos, morseTranslate, morse}
            };
        }

        private async void MorseTranslate_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(morse.Text))
            {
                MorseCode.InitializeDictionary();
                string morseCode = MorseCode.Translate(morse.Text);
                for (int i = 0; i < morseCode.Length; i++)
                {
                    if (morseCode[i] == '.')
                    {
                        await Flashlight.TurnOnAsync();
                        await Task.Delay(300);
                        await Flashlight.TurnOffAsync();
                    }
                    else if (morseCode[i] == '-')
                    {
                        await Flashlight.TurnOnAsync();
                        await Task.Delay(400);
                        await Flashlight.TurnOffAsync();
                    }
                    else
                        await Task.Delay(600);
                }
            }
        }

        private async void EnableSos_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (on)
            {
                sos = !sos;
                if (sos)
                {
                    btn.Text = "Выключить SOS.";
                    await SOS();
                }
                else
                {
                    btn.Text = "Включить SOS сигнал.";
                }

            }
        }

        private async void EnableSlider_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (on)
            {
                flick = !flick;
                if(flick)
                {
                    btn.Text = "Выключить мерцание.";
                    await FLICK();
                }
                else
                {
                    btn.Text = "Включить мерцание.";
                }
                    
            }
        }

        private async Task FLICK()
        {
            while (flick && on)
            {
                await Flashlight.TurnOnAsync();
                await Task.Delay((int)Math.Round(3000 / slider.Value, 0));
                await Flashlight.TurnOffAsync();
                await Task.Delay((int)Math.Round(3000 / slider.Value, 0));
            };
        }

        private async Task SOS()
        {
            while (sos && on)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!on || !sos)
                        return;
                    await Flashlight.TurnOnAsync();
                    await Task.Delay(600);
                    await Flashlight.TurnOffAsync();
                    await Task.Delay(100);
                }
                for (int i = 0; i < 3; i++)
                {
                    if (!on || !sos)
                        return;
                    await Flashlight.TurnOnAsync();
                    await Task.Delay(1200);
                    await Flashlight.TurnOffAsync();
                    await Task.Delay(300);
                }
            };
        }

        private async void _switch_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (!on)
            {
                btn.Text = "Выключить фонарик!";
                await Flashlight.TurnOnAsync();
            }
            else
            {
                btn.Text = "Включить фонарик!";
                await Flashlight.TurnOffAsync();
            }
                
            flick = !flick;
            on = !on;
        }
    }
}
