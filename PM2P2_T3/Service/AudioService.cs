using PM2P2_T3.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2P2_T3.Service
{
    public class AudioService
    {

         public async Task<bool> saveAudios(Audio Audio)
        {
            var result = await App.DBase.insertUpdateAudio(Audio);

            return (result > 0);

        }


        public async Task<List<Audio>> GetAudios()
        {
            return await App.DBase.getListAudio();
        }

        public async Task<bool> DeleteAudio(Audio audio)
        {
            var result = await App.DBase.deleteAudio(audio);
            return (result > 0);
        }
    }
}
