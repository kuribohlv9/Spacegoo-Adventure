using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Runtime.InteropServices;

namespace Elias
{
	public class UTF8MarshallingHelpers
	{
		public static IntPtr ConvertToNativeUTF8(string inputString)
		{
			//TODO: This check seems reasonable to start with, but perhaps I should instead use a valid pointer?
			if (inputString == "")
				return IntPtr.Zero;
			//The +1 is to add the null terminator.
			int length = Encoding.UTF8.GetByteCount (inputString) + 1;
			byte[] tempByteBuffer = new byte[length];
			Encoding.UTF8.GetBytes (inputString, 0, inputString.Length, tempByteBuffer, 0);
			IntPtr outPtr = Marshal.AllocHGlobal (tempByteBuffer.Length);
			Marshal.Copy(tempByteBuffer, 0, outPtr, tempByteBuffer.Length);
			return outPtr;
		}

		public static IntPtr CreateOutputStringPointer(int maxLengthInBytes)
		{
			return Marshal.AllocHGlobal (maxLengthInBytes);
		}

		public static void FreeStringPointer(IntPtr inputPointer)
		{
			Marshal.FreeHGlobal (inputPointer);
		}

		public static string ConvertFromNativeUTF8(IntPtr inputPointer)
		{
			int length = 0;
			//We need to know how many bytes the string is. So we look for the null terminator.
			while (Marshal.ReadByte (inputPointer, length) != 0)
				length++;
			byte[] tempByteBuffer = new Byte[length];
			Marshal.Copy (inputPointer, tempByteBuffer, 0, length);
			return Encoding.UTF8.GetString (tempByteBuffer);
		}
	}
}