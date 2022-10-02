using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToText
{
    internal class Program
    {
        async static Task Main (string[] args)
        {
            //await RecognizeSpeech();
            //onsole.WriteLine("Finished");
            var speechConfig = SpeechConfig.FromSubscription("2e29cf83ea0b49759bcecc3aa379fa17", "germanywestcentral");
            await FromMic(speechConfig);

            speechConfig.SpeechSynthesisVoiceName = "de-DE-ConradNeural";

            string path = @"C:\Temp\Example.txt";
            if (!File.Exists(path))
            {
                Console.WriteLine("No Text File");
                using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
                {
                    // Get text from the console and synthesize to the default speaker.
                    Console.WriteLine("Enter some text that you want to speak >");
                    string text = Console.ReadLine();

                    var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                    OutputSpeechSynthesisResult(speechSynthesisResult, text);
                }
            }
            else if (File.Exists(path))
            {
                string text_file = File.ReadAllText(path);
                using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
                {
                    // Get text from the console and synthesize to the default speaker.
                    //Console.WriteLine("Enter some text that you want to speak >");
                    string text = text_file;

                    var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                    OutputSpeechSynthesisResult(speechSynthesisResult, text);
                }

            }

            //speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
            

           

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        async static Task FromMic(SpeechConfig speechConfig)
        {
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            Console.WriteLine("Speak into your microphone.");
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);
            

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                //Console.WriteLine(result.Text);
                string path = @"C:\Temp\Example.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path);
                    tw.WriteLine(result.Text);
                    tw.Close();
                }
                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path);
                    tw.WriteLine(result.Text);
                    tw.Close();
                }

            }


            Console.WriteLine($"RECOGNIZED: Text={result.Text}");
        }

        static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }



        private static async Task RecognizeSpeech()
        {
            
            var configuration = SpeechConfig.FromSubscription("2e29cf83ea0b49759bcecc3aa379fa17", "germanywestcentral");
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            
            using (var recog = new SpeechRecognizer(configuration, audioConfig))
            {
                Console.WriteLine("Speak something....");
                
                var result = await recog.RecognizeOnceAsync();
                
                if(result.Reason == ResultReason.RecognizedSpeech)
                {
                    //Console.WriteLine(result.Text);
                    string path = @"C:\Temp\Example.txt";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                        TextWriter tw = new StreamWriter(path);
                        tw.WriteLine(result.Text);
                        tw.Close();
                    }
                    else if (File.Exists(path))
                    {
                        TextWriter tw = new StreamWriter(path);
                        tw.WriteLine(result.Text);
                        tw.Close();
                    }
                    
                }


                
            }
        }
    }
}
