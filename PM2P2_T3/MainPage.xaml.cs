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

        private  AudioRecorderService audioRecorderService = new AudioRecorderService() { 
            StopRecordingOnSilence = false,
            StopRecordingAfterTimeout = false
        };

        private  AudioPlayer audioPlayer = new AudioPlayer();

        private bool reproducir = false;

        public MainPage()
        {
            InitializeComponent();

            if (App.DBase != null) { }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           // audioPlayer.Pause();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                var status = await Permissions.RequestAsync<Permissions.Microphone>();
                var status2 = await Permissions.RequestAsync<Permissions.StorageRead>();
                var status3 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted & status2 != PermissionStatus.Granted & status3 != PermissionStatus.Granted)
                {
                    return; // si no tiene los permisos no avanza
                }

                if (audioRecorderService.IsRecording)
                {
                    await audioRecorderService.StopRecording();


                    audioPlayer.Play(audioRecorderService.GetAudioFilePath());

                    txtMessage.Text = "NO esta grabando";


                    btnGrabar.Text = "Grabar audio";

                    reproducir = true;
                }
                else
                {
                    await audioRecorderService.StartRecording();


                    txtMessage.Text = "Esta grabando";

                    btnGrabar.Text = "Dejar de Grabar";

                    //reproducir = false;
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

                    //var mStream = new MemoryStream(File.ReadAllBytes(audioRecorderService.GetAudioFilePath()));

                    //var mStream = (MemoryStream)audioFile;
                   // Byte[] bytes = mStream.ToArray();


                    var folderPath = "/storage/emulated/0/Android/data/com.companyname.pm2p2_t3/files/Audio";

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var nameAudio = DateTime.Now.ToString("MMddyyyyhhmmss") + ".wav";
                    var fullPath = folderPath + "/" + nameAudio;



                    var stream = audioRecorderService.GetAudioFileStream();


                    using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }

                    //AudioPath = fileName;


                    //File.WriteAllBytes( fullPath, bytes);

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
