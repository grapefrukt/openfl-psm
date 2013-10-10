using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace com.grapefrukt.utils {
	
	public class SaveData {
		
		const string EMPTY = 		"";
		const int FIELD_NAME_W = 	30;
		const int FIELD_SIZE_W = 	9;
		const int FIELD_W = 		FIELD_NAME_W + FIELD_SIZE_W * 2 + 1;
		static char[] PADDING = 	{ ' ' };
		const string SAVEFILE = 	"/Documents/rymdkapsel.dat";
		
		private Dictionary<string, string> dictionary;
		
		public SaveData() {
			dictionary = new Dictionary<string, string>();
		}
		
		public void load() {
			//Console.WriteLine("SaveData.load");
			
			if ( !System.IO.File.Exists(SAVEFILE) ) {
				Console.WriteLine("SaveData load failed, file does not exist");
				return;
			}
			
			using (FileStream hStream = System.IO.File.Open(SAVEFILE, FileMode.Open)) {
				if (hStream == null) {
					Console.WriteLine("SaveData load failed, could not open stream");
					return;
				}
				
				long size = hStream.Length;
				byte[] saveData = new byte[size];
				hStream.Read(saveData, 0, (int)size);
				hStream.Close();
				
				if (saveData.Length < FIELD_W) {
					Console.WriteLine("SaveData load failed, savefile shorter than FIELD_W (" + FIELD_W + ")");
					return;
				}
				
				string numFieldsStr = System.Text.Encoding.ASCII.GetString(saveData, 0, FIELD_W);
				numFieldsStr = numFieldsStr.Trim(PADDING);
				
				int numFields;
				if (!int.TryParse(numFieldsStr, out numFields)){
					Console.WriteLine("SaveData load failed, file has bogus contents");
					return;
				}
				
				Console.WriteLine("Loaded " + numFields + " data fields");
				
				for (int i = 1; i <= numFields; i++) {
					string field = 	System.Text.Encoding.ASCII.GetString(saveData, FIELD_W * i, FIELD_NAME_W).TrimEnd(PADDING);
					string offset = System.Text.Encoding.ASCII.GetString(saveData, FIELD_W * i + FIELD_NAME_W, FIELD_SIZE_W).TrimEnd(PADDING);
					string fsize = 	System.Text.Encoding.ASCII.GetString(saveData, FIELD_W * i + FIELD_NAME_W + FIELD_SIZE_W, FIELD_SIZE_W).TrimEnd(PADDING);
					
					//Console.WriteLine(field + " starts at " + offset + " and is " + fsize);
												
					dictionary.Add(field, System.Text.Encoding.ASCII.GetString(saveData, FIELD_W * (numFields + 1) + int.Parse(offset), int.Parse(fsize)));
												
				}
				
			}
			
			Console.WriteLine("SaveData loaded successfully");
		}
		
		public void save() {
			string saveString = EMPTY.PadRight(FIELD_NAME_W) + EMPTY.PadRight(FIELD_SIZE_W) + dictionary.Count.ToString().PadRight(FIELD_SIZE_W) + "\n";
			
			int offset = 0;
			foreach(KeyValuePair<String,String> entry in dictionary) {
			    saveString += entry.Key.PadRight(FIELD_NAME_W);
				saveString += offset.ToString().PadRight(FIELD_SIZE_W);
				saveString += entry.Value.Length.ToString().PadRight(FIELD_SIZE_W);
				saveString += "\n";
				
				offset += entry.Value.Length;
			}
						
			foreach(KeyValuePair<String,String> entry in dictionary) {
			    saveString += entry.Value;
			}
			
			write(ref saveString);
		}
		
		private static void write(ref string saveString){
			//Console.WriteLine("Getting save bytes...");
			
			byte[] saveData = System.Text.Encoding.ASCII.GetBytes(saveString);
			
			//Console.WriteLine("Opening file handle...");
		
			using (System.IO.FileStream hStream = System.IO.File.Open(@SAVEFILE, FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.Write)) {
				//Console.WriteLine("Writing to file...");
				hStream.Write(saveData, 0, saveData.Length);
				//Console.WriteLine("Closing file...");
				hStream.Close();
				//Console.WriteLine("Wrote " + saveData.Length + " bytes to disk");
			}
		}
		
		public void set(string field, string data, haxe.lang.Null<bool> compress) {
			if(dictionary.ContainsKey(field)) dictionary.Remove(field);
			dictionary.Add(field, data);
		}
		
		public string get(string field, haxe.lang.Null<bool> decompress) {
			string val;
			if (!dictionary.TryGetValue(field, out val)) return "";
			return val;
		}
	}
}