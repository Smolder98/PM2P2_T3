using Plugin.AudioRecorder;
using PM2P2_T3.Model;
using PM2P2_T3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2P2_T3.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        private readonly AudioPlayer audioPlayer = new AudioPlayer();
        public ListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            list.ItemsSource = await new AudioService().GetAudios();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //audioPlayer.Pause();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }


        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            try
            {
                var item = sender as SwipeItem;

                var audio = item.BindingContext as Audio;

                audioPlayer.Play(audio.Path);
            }
            catch (Exception)
            {

                await DisplayAlert("Aviso", "No se pudo reproducir el audio", "OK");
            }
        }

        private async void SwipeItem_Invoked_1(object sender, EventArgs e)
        {
            try
            {
                var item = sender as SwipeItem;

                var audio = item.BindingContext as Audio;

                if (await new AudioService().DeleteAudio(audio))
                {
                    await DisplayAlert("Aviso", "Audio eliminado correctamente !!!", "OK");

                    list.ItemsSource = await new AudioService().GetAudios();
                } 
                else await DisplayAlert("Aviso", "No se pudo eliminar el audio", "OK");

            }
            catch (Exception)
            {

                await DisplayAlert("Aviso", "No se pudo eliminar el audio", "OK");
            }
        }
    }
}