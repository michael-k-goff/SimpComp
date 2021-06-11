/* Simplicial Complexes */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Program {
	class Program
	{
		public static void Main()
		{
			SimplicialComplex sc = new SimplicialComplex();
			bool result = true;
			while (result)
			{
				result = performOperation(sc);
			}
		}
	
		public static bool performOperation(SimplicialComplex sc)
		{
			Console.Clear();
			sc.displayNumVertices();
			Console.Write("What would you like to do?\n\n");
			Console.Write("(A)dd a Face (adds all subfaces too).\n");
			Console.Write("(D)elete a Face (deletes all superfaces too).\n");
			Console.Write("(C)hange number of vertices.\n");
			Console.Write("(V)iew faces.\n");
			Console.Write("(L)oad a pre-existing complex.\n");
			Console.Write("(B)etti Numbers.\n");
			Console.Write("(Q)uit.\n");
			string selection = Console.ReadLine();
			if (selection.Length > 0 && (selection[0] == 'a' || selection[0] == 'A')) {
				sc.addFace();
			}
			if (selection.Length > 0 && (selection[0] == 'd' || selection[0] == 'D')) {
				sc.deleteFace();
			}
			if (selection.Length > 0 && (selection[0] == 'c' || selection[0] == 'C')) {
				sc.setNumVertices();
			}
			if (selection.Length > 0 && (selection[0] == 'v' || selection[0] == 'V')) {
				sc.displayFaces();
			}
			if (selection.Length > 0 && (selection[0] == 'l' || selection[0] == 'L')) {
				sc.loadComplex();
			}
			if (selection.Length > 0 && (selection[0] == 'b' || selection[0] == 'B')) {
				sc.betti();
			}
			if (selection.Length > 0 && (selection[0] == 'q' || selection[0] == 'Q')) {
				return false;
			}
			return true;
		}
	}
}