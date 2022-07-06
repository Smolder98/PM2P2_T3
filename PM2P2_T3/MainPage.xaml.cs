using Plugin.AudioRecorder;
using PM2P2_T3.Model;
using PM2P2_T3.Service;
using PM2P2_T3.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2P2_T3
{
    public partial class MainPage : ContentPage
    {

        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService
        {
            StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
            StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
            TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
        };

        private readonly AudioPlayer audioPlayer = new AudioPlayer();

        private bool reproducir = false;

        public MainPage()
        {
            InitializeComponent();

            if (App.DBase != null) { }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                var status = await Permissions.RequestAsync<Permissions.Microphone>();


                if (status != PermissionStatus.Granted)
                    return;

                if (audioRecorderService.IsRecording)
                {
                    await audioRecorderService.StopRecording();

                    txtMessage.Text = "NO esta grabando";


                    reproducir = true;
                }
                else
                {
                    await audioRecorderService.StartRecording();


                    txtMessage.Text = "Esta grabando";
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Alerta", ex.Message, "OK");
            }

        }

        private  async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (reproducir)
            {
                audioPlayer.Play(audioRecorderService.GetAudioFilePath());
            }
            else
            {
                await DisplayAlert("Alerta","No ha grabado ningun audio", "OK");
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                if (reproducir)
                {


                    if (string.IsNullOrEmpty(txtDescription.Text))
                    {
                        await DisplayAlert("Alerta", "Debe escribir una descripcion", "OK");
                        return;
                    }

                    //Stream audioFile =  audioRecorderService.GetAudioFileStream();

                    var mStream = new MemoryStream(File.ReadAllBytes(audioRecorderService.GetAudioFilePath()));

                    //var mStream = (MemoryStream)audioFile;
                    Byte[] bytes = mStream.ToArray();


                    var folderPath = "/storage/emulated/0/Android/data/com.companyname.pm2p2_t3/files/Audio";

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var nameAudio = DateTime.Now.ToString("MMddyyyyhhmmss") + ".wav";
                    var fullPath = folderPath + "/" + nameAudio;

                    File.WriteAllBytes( fullPath, bytes);

                    var audio = new Audio()
                    {
                        Id = 0,
                        Descripcion = txtDescription.Text,
                        Path = fullPath,
                        DateCreation = DateTime.Now
                    };





                    if (await new AudioService().saveAudios(audio))
                        await DisplayAlert("Alerta", "Audio guardado correctamente !!!", "OK");
                    else 
                        await DisplayAlert("Alerta", "El audio  no se pudo guardar correctamente !!!", "OK");




                    reproducir = false;
                    txtDescription.Text = "";
                }
                else
                {
                    await DisplayAlert("Alerta", "No ha grabado ningun audio", "OK");
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Alerta", ex.Message, "OK");
            }
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ListPage());
        }
    }
}
