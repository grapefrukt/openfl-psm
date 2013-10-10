using System;
using com.grapefrukt.games.spaceblocks;
using System.Collections.Specialized;
using Sce.PlayStation.Core.Audio;
using System.Collections.Generic;
using com.grapefrukt.utils;

namespace com.grapefrukt.games.spaceblocks.managers
{
	public class SoundPlayer
	{
		
		static private Dictionary<string, SoundTuple> sounds;
		
		static private Bgm bgm;
		static private BgmPlayer bgmPlayer;
		static private SoundTuple enemyAmbience;
		
		public static void init(){
			if(sounds != null) return;
						
			bgm = new Bgm("/Application/assets/music/music.mp3");
			bgmPlayer = bgm.CreatePlayer();
			bgmPlayer.Loop = true;
			
			sounds = new Dictionary<string, SoundTuple>();
		}
		
		public static void preload(string sound){
			//Console.WriteLine("preloading sound: " + sound);
			var st = new SoundTuple();
			try {
				st.sound = new Sound("/Application/assets/sounds/" + sound + ".wav");
			} catch (System.IO.FileNotFoundException){
				st.sound = null;
			}
			
			if(st.sound != null){
				st.player = st.sound.CreatePlayer();
				sounds.Add(sound, st);
			}
			
			if (sound == "flyer_ambience"){
				enemyAmbience = st;
			}
			
		}
		
		public static void toggleMusic(){
			if (bgmPlayer.Status != BgmStatus.Stopped){
				startMusic();
			} else {
				stopMusic();
			}
		}
		
		public static void startMusic(){
			bgmPlayer.Play();
		}
		
		public static void stopMusic(){
			bgmPlayer.Stop();
		}
		
		public static void setAmbienceLevel(double level){
			if (level > 0 && enemyAmbience.player.Status != SoundStatus.Playing){
				enemyAmbience.player.Loop = true;
				enemyAmbience.player.Play();
			} else if ( level <= 0 && enemyAmbience.player.Status == SoundStatus.Playing){
				enemyAmbience.player.Stop();
			}
			
			enemyAmbience.player.Volume = (float) level;
		}
						
		public static void play(string sfx, double volume){
			SoundTuple st;
			if (!sounds.ContainsKey (sfx)) return;
			sounds.TryGetValue(sfx, out st);
			st.player.Volume = (float) volume;
			st.player.Play();
		}
		
		public static void stop(string sfx){
			SoundTuple st;
			if (!sounds.ContainsKey (sfx)) return;
			sounds.TryGetValue(sfx, out st);
			st.player.Stop();
		}
	}
}

struct SoundTuple {
	public Sound sound;
	public SoundPlayer player;
	
	public SoundTuple(Sound sound, SoundPlayer player){
		this.sound = sound;
		this.player = player;
	}
}
