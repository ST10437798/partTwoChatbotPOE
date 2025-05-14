namespace partOneChatbot
{
    //import the system.media
    using System.Media;
    using System;
    using System.IO;

    public class playNow
    {

        //constructor
        public playNow()
        {
            //now get where the project is
            string project_location = AppDomain.CurrentDomain.BaseDirectory;

            //check if it is getting the Directory
            Console.WriteLine(project_location);

            //now lets replace the bin\debug so it can get the audio
            string updated_path = project_location.Replace("bin\\Debug", "");
            //lets combine the wav name as Voice Greeting.wav with the updated path
            string full_path = Path.Combine(updated_path, "Voice Greeting-enhanced.wav");

            //now lets pass it to the method play_wav
            Play_wav(full_path);


        }//end of constructor

        //method to play the sound
        private void Play_wav(string full_path)
        {
            //try and catch 
            try
            {
                //or play the sound
                using (SoundPlayer player = new SoundPlayer(full_path))
                {
                    //to play the sound till end use this
                    player.PlaySync();
                }

            }
            catch (Exception error)
            {
                //then show error message
                Console.WriteLine(error.Message);
            }
        }
    }//end of class

}//end of namespace