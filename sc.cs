using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
	public class SimplicialComplex
	{
		List<Face> faces = new List<Face>();
		int num_vertices = 10;
	
		public void setNumVertices()
		{
			Console.Write("How many vertices? There are {0} now.\n",num_vertices);
			int entered_number = int.Parse(Console.ReadLine());
			foreach (var scface in faces) {
				int last_vertex = scface.lastVertex();
				if (entered_number < last_vertex) {
					Console.Write("The number of vertices you entered is less than the number in some faces. Cannot complete this operation.\n");
					Console.Write("Enter anything to continue.\n");
					Console.ReadLine();
					return;
				}
			}
			num_vertices = entered_number;
			Console.Write("Resizing the number of vertices.\n");
			Console.Write("Enter anything to continue.\n");
			Console.ReadLine();
		}
	
		public void displayNumVertices()
		{
			Console.Write("There are {0} vertices.\n",num_vertices);
		}
	
		// Add a face to the simplicial complex if it is not already present. Just a single face, not all subfaces.
		// Return true iff a new face is actually added.
		private bool addToSC(Face new_face)
		{
			new_face.sortFace();
			if (!faces.Contains(new_face)) {
				faces.Add(new_face);
				return true;
			}
			return false;
		}
	
		// Add a face and all its subfaces to a simplicial complex.
		private void addFaceToSC(Face new_face)
		{
			// Count the number of faces added
			int num_added = 0;
			new_face.sortFace();
			List<Face> subfaces = new_face.getSubfaces();
			foreach (var sc_face in subfaces) {
				if (addToSC(sc_face)) {
					num_added += 1;
					Console.Write("Adding face ");
					sc_face.writeFace();
					Console.Write("\n");
				}
			}
			Console.Write("\nAdded {0} new faces.\n",num_added );
		}
	
		public void addEdge()
		{
			Console.Write("Input the face: vertex numbers separated by spaces (e.g. 5 7 12).\n");
			string new_face = "";
			new_face = Console.ReadLine();
			string[] vertices_string = new_face.Split(' ');
		
			// Determine how many of the proposed vertices are valid
			Face face = new Face();
			for (int i=0; i<vertices_string.Length; i++)
			{
				int number;
				bool result = int.TryParse(vertices_string[i], out number);
				if (!result) {
					Console.Write("{0} is not a number, so ignoring it.\n",vertices_string[i]);
				}
				else if (number < 1) {
					Console.Write("Cannot have non-positive vertex numbers, so ignoring {0}.\n",vertices_string[i]);
				}
				else if (number > num_vertices) {
					Console.Write("{0} exceeds the number of vertices for this simplicial complex.\n",vertices_string[i]);
				}
				if (result && number >= 1 && number <= num_vertices) {
					face.addVertex(number);
				}
			}
			face.sortFace();
			Console.Write("The modified face added is as follows: ");
			face.writeFace();
			Console.Write("\n");
			addFaceToSC(face);
			Console.Write("Enter anything to continue.\n");
			Console.ReadLine();
		}
	
		public void displayFaces()
		{
			Console.Write("There are {0} faces as follows: \n",faces.Count);
			foreach (var sc_face in faces) {
				sc_face.writeFace();
				Console.Write("\n");
			}
			Console.Write("Enter anything to return to the main menu.\n");
			Console.ReadLine();
		}
	
		public void removeFaceFromSC(Face old_face)
		{
			// Count the number of faces removed.
			int num_removed = 0;
			int i = 0;
			while (i < faces.Count) {
				Face sc_face = faces[i];
				bool contains_face = sc_face.contains(old_face);
				if (contains_face) {
					Console.Write("Removing face ");
					sc_face.writeFace();
					Console.Write("\n");
					faces.RemoveAt(i);
					i--;
					num_removed++;
				}
				i++;
			}
			Console.Write("\nRemoved {0} faces.\n",num_removed);
		}
	
		public void deleteFace()
		{
			Console.Write("Input the face: vertex numbers separated by spaces (e.g. 5 7 12).\n");
			string new_face = "";
			new_face = Console.ReadLine();
			string[] vertices_string = new_face.Split(' ');
		
			// Determine how many of the proposed vertices are valid
			Face face = new Face();
			for (int i=0; i<vertices_string.Length; i++)
			{
				int number;
				bool result = int.TryParse(vertices_string[i], out number);
				if (!result) {
					Console.Write("{0} is not a number, so ignoring it.\n",vertices_string[i]);
				}
				else if (number < 1) {
					Console.Write("Cannot have non-positive vertex numbers, so ignoring {0}.\n",vertices_string[i]);
				}
				else if (number > num_vertices) {
					Console.Write("{0} exceeds the number of vertices for this simplicial complex.\n",vertices_string[i]);
				}
				if (result && number >= 1 && number <= num_vertices) {
					face.addVertex(number);
				}
			}
			face.sortFace();
			Console.Write("The modified face removed is as follows: ");
			face.writeFace();
			Console.Write("\n");
			removeFaceFromSC(face);
			Console.Write("Enter anything to continue.\n");
			Console.ReadLine();
		}
	}
}