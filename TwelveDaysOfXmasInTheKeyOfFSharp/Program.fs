// --------------------------------------------------------------------------------------
// F# and games in celebration times
// --------------------------------------------------------------------------------------
// (c) Andrea Magnorsky 2014 {@silverspoon}
// Distributed under the open-source MS-PL license
// --------------------------------------------------------------------------------------
#if INTERACTIVE
#r @"C:\source\TwelveDaysOfXmasInTheKeyOfFSharp\packages\OpenTKWithOpenAL.1.1.1589.5942\lib\NET40\OpenTK.dll"
#load @"C:\source\TwelveDaysOfXmasInTheKeyOfFSharp\packages\FSharp.Charting.0.90.9\FSharp.Charting.fsx"
#endif

open System
open OpenTK.Audio
open OpenTK.Audio.OpenAL    
open FSharp.Charting

    let toFrequency (note:string) =
        match note.ToUpper() with
        | "C" ->  261.626f
        | "C#" -> 277.183f
        | "D"-> 293.665f
        |"D#"-> 311.127f
        |"E"-> 329.628f
        |"F"-> 349.228f
        |"F#"-> 369.994f
        |"G"-> 391.995f
        |"G#"-> 415.305f
        |"A"-> 440.0f
        |"A#"-> 466.164f
        |"B"-> 493.883f
        |_ ->  369.994f (*Defaults to F# because xMas ;) *)

    let samplingFrequency = 44100.0
    
    let generateNote (freq:float32) =
        let noteLength = 0.5
        let seqLength = int  (samplingFrequency * noteLength)
        Seq.init seqLength (fun i ->                                 
                        (2.0 * Math.PI * float freq) / samplingFrequency * float i
                        |> Math.Sin                        
                        |> ( * )  (float Int16.MaxValue)
                        |> int16
                        )

    let playSong (song: string) =
        let notes = song.Split ' '

        use audioContext = new AudioContext()
        let buffer = AL.GenBuffer()
        let audioSourceIndex = AL.GenSource()
         
        let freqToWaves = toFrequency >>  generateNote
        let data = notes                        
                        |> Seq.collect freqToWaves                        
                        |> Array.ofSeq
                        
               
        AL.BufferData (buffer, ALFormat.Mono16, data, data.Length * 2, int samplingFrequency)
        AL.Source(audioSourceIndex, ALSourcei.Buffer, buffer)
        AL.SourcePlay(audioSourceIndex)

        
        Console.WriteLine("Errors: {0}", AL.GetError())
        Console.ReadLine() |> ignore

        // Clean up after ourselves 
        AL.DeleteBuffer buffer
        AL.DeleteSource audioSourceIndex

        ()

let mysterySong = "F# F# G# F# B A#"

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    let silentNight = "F# F# G# F# F# D# D# D# F# F# G# F# F# D# D# D#" 
    playSong silentNight    
    0 // return an integer exit code
