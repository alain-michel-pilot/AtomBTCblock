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
using System.Windows.Forms;

namespace AtomBTCblock
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
