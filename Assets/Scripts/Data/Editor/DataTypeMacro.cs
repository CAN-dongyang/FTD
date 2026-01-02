#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

class DataTypeMacro
{
	string _filePath = "./Assets/Scripts/Data/DataType.cs";
	
	int __indent = 0x200; // 1,000(oct), 512(dec)
	int __indent_asset = 0x40000; // 1,000,000(oct), 262,144(dec)

	string[] _assetNames = {
		"Character",
		"Guild"
	};
	KeyValuePair<int, string>[] _instanceNames = {
		new(0, "Student"),
		new(0, "Professor"),
		new(1, "School"),
		new(1, "Guild"),
	};
	
	[MenuItem("Macro/Create DataType Enum")]
	static public void CreateDataTypeEnum()
	{
		new DataTypeMacro().Run();
		GC.Collect();
	}
	public void Run()
	{
		var fs = File.CreateText(_filePath);

		fs.Write(_comments);
		fs.Write(_firstLine);

		Dictionary<int,int> asset_counts = new();

		fs.WriteLine("\t// Assets");
		for(int i=0; i<_assetNames.Length; i++)
		{
			fs.WriteLine($"\tA_{_assetNames[i]} = 0x{__indent_asset:X} * {i+1},");
			asset_counts.Add(i, 1);
		}
		fs.Write('\n');

		fs.WriteLine("\t// Instances");
		for(int i=0; i<_instanceNames.Length; i++)
		{
			int assetKey = _instanceNames[i].Key;

			fs.Write($"\tI_{_instanceNames[i].Value} = "
				+ $"A_{_assetNames[assetKey]}"
				+ $" + 0x{__indent:X} * {asset_counts[assetKey]++}");
			
			if(i+1 < _instanceNames.Length) fs.Write(",");
			fs.Write("\n");
		}
		fs.Write('}');
		
		fs.Close();
		fs.Dispose();
		
		AssetDatabase.Refresh();
	}

	string _comments = "/// <summary>\n"
		+ "/// 데이터등록번호(int)에 대한 타입 감별자.<br/>\n"
		+ "/// Asset의 경우 <c>A_</c><br/>\n"
		+ "/// Instance의 경우 <c>I_</c> 접두사를 가진다\n"
		+ "/// \n"
		+ "/// <para>ttt ttt 000 (8,oct)<br/>\n"
		+ "/// <c>DataID</c>와 직접 비교가 가능하다.\n"
		+ "/// </para>\n"
		+ "/// </summary>\n";
	string _firstLine = "public enum DataType : int\n{"
		+ "\n\tNone = 0, // Invalid\n";
}
#endif