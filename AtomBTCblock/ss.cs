/*
 * AtomBTCblock - tool created to rebuild and verify BTC block in the way it is used 
 * by Stratum pool and other miner software/hardware, powered by AtomMiner
 *
 * Copyright 2015-2017 AtomMiner <info@atomminer.com>
 *
 * BTC donation: 3LwsJAzPd8weD1FypVWmkDFMwA7rgjPSif
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option)
 * any later version.  See COPYING for more details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AtomBTCblock
{
	internal class ss
	{
		public static string arrToHex(byte[] bytes)
		{
			return string.Concat(Array.ConvertAll<byte, string>(bytes, (byte b) => b.ToString("x2")));
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.ToArray<byte>(Enumerable.Select<int, byte>(Enumerable.Where<int>(Enumerable.Range(0, hex.Length), (int x) => x % 2 == 0), (int x) => Convert.ToByte(hex.Substring(x, 2), 16)));
		}

		public static double getDiff(uint bits)
		{
			double num = Math.Log(65535.0);
			double num2 = Math.Log(256.0);
			return Math.Exp(num - Math.Log(bits & 16777215u) + num2 * (29u - ((bits & 4278190080u) >> 24)));
		}

		public static uint[] getTarget(uint bits)
		{
			uint[] array = new uint[8];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 4294967295u;
			}
			double num = ss.getDiff(bits);
			int num2 = 6;
			while (num2 > 0 && num > 1.0)
			{
				num /= 4294967296.0;
				num2--;
			}
			ulong num3 = (ulong)(4294901760.0 / num);
			uint[] result;
			if (num3 == 0uL && num2 == 6)
			{
				result = array;
			}
			else
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0u;
				}
				array[num2] = (uint)num3;
				array[num2 + 1] = (uint)(num3 >> 32);
				result = array;
			}
			return result;
		}

		public static string getTargetStr(uint bits)
		{
			IEnumerable<uint> enumerable = Enumerable.Reverse<uint>(ss.getTarget(bits));
			string text = "";
			foreach (uint current in enumerable)
			{
				text += current.ToString("x8");
			}
			return text;
		}

		public static string strHash(string text)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			SHA256Managed sHA256Managed = new SHA256Managed();
			byte[] array = sHA256Managed.ComputeHash(bytes);
			string text2 = string.Empty;
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				text2 += string.Format("{0:x2}", b);
			}
			return text2;
		}

		public static byte[] sha256(byte[] bytes)
		{
			return new SHA256Managed().ComputeHash(bytes);
		}

		public static byte[] sha256(string hex)
		{
			byte[] bytes = ss.StringToByteArray(hex);
			return ss.sha256(bytes);
		}

		public static byte[] sha256d(byte[] bytes)
		{
			return new SHA256Managed().ComputeHash(new SHA256Managed().ComputeHash(bytes));
		}

		public static byte[] sha256d(string hex)
		{
			byte[] bytes = ss.StringToByteArray(hex);
			return ss.sha256d(bytes);
		}

		public static string hexHash(string text)
		{
			return ss.arrToHex(ss.sha256(text));
		}

		public static string hexHashRev(byte[] bytes)
		{
			return ss.arrToHex(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ss.sha256(bytes))));
		}

		public static string hexHashRev(string text)
		{
			return ss.arrToHex(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ss.sha256(text))));
		}

		public static string hexHashD(string text)
		{
			return ss.arrToHex(ss.sha256d(text));
		}

		public static string hexHashDRev(byte[] bytes)
		{
			return ss.arrToHex(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ss.sha256d(bytes))));
		}

		public static string hexHashDRev(string text)
		{
			return ss.arrToHex(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ss.sha256d(text))));
		}
	}
}
