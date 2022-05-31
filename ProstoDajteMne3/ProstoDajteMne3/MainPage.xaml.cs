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

        bool on, flick, sos, busy;
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
                Text = "Создать новое сообщение по морзе."
            };
            Button chooseAndPlay = new Button
            {
                Text = "Выбрать сообщение из БД и сыграть."
            };
            morse = new Entry();
            chooseAndPlay.Clicked += ChooseAndPlay_Clicked;
            morseTranslate.Clicked += MorseTranslate_Clicked;
            enableSos.Clicked += EnableSos_Clicked;
            enableSlider.Clicked += EnableSlider_Clicked;
            _switch.Clicked += _switch_Clicked;
            Content = new StackLayout
            {
                Children = { _switch, slider, enableSlider, enableSos, morseTranslate, chooseAndPlay}
            };
        }

        private async void ChooseAndPlay_Clicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            DB database = new DB();
            List<MorseCodeTemplate> items = await database.GetItemsAsync();
            string[] names = new string[items.Count];
            for (int i = 0; i < names.Length; i++)
            {
                pairs.Add((int)items[i].ID, items[i].Name);
                names[i] = items[i].Name;
            }

            string action = await DisplayActionSheet("Выберите сообщение.","Отмена",null, names);
            if (action == "Отмена")
                return;
            int id = pairs.FirstOrDefault(x => x.Value == action).Key;
            playMorse(database.GetItemAsync(id).Result.Morse);
        }

        private async void playMorse(string morseCode)
        {
            busy = true;
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
            busy = false;
        }

        private async void MorseTranslate_Clicked(object sender, EventArgs e)
        {   
            await Navigation.PushAsync(new PopUpMorse());
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
                if (flick)
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
            while (!busy && on)
            {
                playMorse("... --- ...");
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
    public class PopUpMorse : ContentPage
    {
        Label nameL, textL;
        Entry name, text;
        public PopUpMorse()
        {
            nameL = new Label();
            nameL.Text = "Введите пожалуйста название сообщения.(не больше 25 символов)";
            name = new Entry
            {
                MaxLength = 25,
                Placeholder = GetRandomNamePlaceholder() + '.'
            };
            textL = new Label();
            textL.Text = "Введите пожалуйста сообщение.(не больше 50 символов)";
            text = new Entry
            {
                MaxLength = 50,
                Placeholder = GetRandomPlaceholder() + "..."
            };
            Button send = new Button();
            send.Text = "Сохранить!";
            send.Clicked += Send_Clicked;
            Content = new StackLayout
            {
                Children = {nameL, name, text, textL, send}
            };
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(text.Text))
            {
                MorseCode.InitializeDictionary();
                MorseCodeTemplate msg = new MorseCodeTemplate();
                msg.Text = text.Text;
                msg.Name = name.Text;
                msg.Morse = MorseCode.Translate(text.Text);
                DB database = new DB();
                await database.SaveItemAsync(msg);
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ошибка", "Невозможно сохранить пустые название/текст", "OK");
            }
        }

        private string GetRandomPlaceholder()
        {
            string[] placeholders = new string[]
            {
                "5 минут полет нормально",
                "Первый первый, это второй",
                "Cmon boyyy",
                "Фишман встань мид",
                "Нет, друг, я не оправдываюсь",
                "БАУНТИ ХААААНТЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕР",
                "Борщь с капусткой, но не красный",
                "Я ща сяду за руль и ты вылетишь отсюда",
                "Ломай меня полность, я хочу чтобы ты ломал меня"
            };
            Random rnd = new Random();
            return placeholders[rnd.Next(placeholders.Length - 1)];
        }
        private string GetRandomNamePlaceholder()
        {
            string[] names = new string[]
            {
                "Полет космонавта",
                "ВВС китая",
                "Подземелье",
                "Сообщение дотеру",
                "Сообщение дотеру 2",
                "Сообщение дотеру 3",
                "Столовая",
                "Гонки",
                "Странные пожелания"
            };
            Random rnd = new Random();
            return names[rnd.Next(names.Length - 1)];
        }
    }
}
