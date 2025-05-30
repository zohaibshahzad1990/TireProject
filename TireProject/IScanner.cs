﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace TireProject
{
	public interface IScanner
	{
		Task<string> Scan();
        byte[] GenerateBarcode(string content, ZXing.BarcodeFormat format);
        
    }
}

