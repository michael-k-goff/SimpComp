/* Simplicial Complexes */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Program
{
	public static void Main()
	{
		SimplicialComplex sc = new SimplicialComplex();
		bool result = true;
		while (result)
		{
			result = sc.performOperation();
		}
	}
	
	class Face
	{
		List<int> vertices = new List<int>();
		
		public void addVertex(int new_vertex)
		{
			if (vertices.Contains(new_vertex) == false) {
				vertices.Add(new_vertex);
			}
		}
		
		public void sortFace()
		{
			vertices.Sort();
		}
		
		public int vertexCount()
		{
			return vertices.Count;
		}
		
		public void writeFace()
		{
			if (vertices.Count == 0) {
				Console.Write("(Empty Face)");
				return;
			}
			foreach (int j in vertices) {
				Console.Write(j+ " ");
			}
		}
		
		public void setVertices(List<int> vertex_list)
		{
			vertices = vertex_list;
		}
		
		public List<Face> getSubfaces()
		{
			 List<Face> result = new List<Face>();
			 if (vertices.Count == 0) {
				 Face null_face = new Face();
				 result.Add(null_face);
				 return result;
			 }
			 else {
				 // Think of a faster way to do this
				 List<int> smaller_face = new List<int>();
				 smaller_face.AddRange(vertices);
				 smaller_face.RemoveAt(vertices.Count-1);
				 Face smallerFace = new Face();
				 smallerFace.setVertices(smaller_face);
				 
				 List<Face> result1 = smallerFace.getSubfaces();
				 List<Face> result2 = smallerFace.getSubfaces();
				 foreach (var sc_face in result2) {
					 sc_face.addVertex(vertices[vertices.Count-1]);
				 }
				 return result1.Concat(result2).ToList();
			 }
		}
		
		public bool contains(Face face) {
			return true; // Override this
		}
		
		public override bool Equals(object obj)
		{
		    return Equals(obj as Face);
		}
		
		public bool Equals(Face other)
		{
			if (other == null) {
				return false;
			}
			if (vertices.Count != other.vertices.Count) {
				return (false);
			}
			if (vertices.Count == other.vertices.Count) {
				for (int i=0; i<vertices.Count; i++) {
					if (vertices[i] != other.vertices[i]) {
						return false;
					}
				}
			}
		    return (true);
		}
		
		public override int GetHashCode()
		{
		    return HashCode.Combine(vertices.Count);
		}
	}
	
	class SimplicialComplex
	{
		List<Face> faces = new List<Face>();
		int num_vertices = 10;
		
		public void setNumVertices()
		{
			Console.Write("How many vertices?\n");
			num_vertices = int.Parse(Console.ReadLine());
		}
		
		public void displayNumVertices()
		{
			Console.Write("There are {0} vertices.\n",num_vertices);
		}
		
		// Add a face to the simplicial complex if it is not already present. Just a single face, not all subfaces.
		private void addToSC(Face new_face)
		{
			new_face.sortFace();
			if (!faces.Contains(new_face)) {
				faces.Add(new_face);
			}
		}
		
		// Add a face and all its subfaces to a simplicial complex.
		private void addFaceToSC(Face new_face)
		{
			new_face.sortFace();
			List<Face> subfaces = new_face.getSubfaces();
			foreach (var sc_face in subfaces) {
				addToSC(sc_face);
			}
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
					Console.Write("{0} exceeds the number of vertices for his simplicial complex.\n",vertices_string[i]);
				}
				if (result && number >= 1 && number <= num_vertices) {
					face.addVertex(number);
				}
			}
			addFaceToSC(face);
		}
		
		public void displayFaces()
		{
			Console.Write("The faces are as follows: \n");
			foreach (var sc_face in faces) {
				sc_face.writeFace();
				Console.Write("\n");
			}
			Console.Write("Enter anything to return to the main menu.\n");
			Console.ReadLine();
		}
		
		public void removeFaceFromSC(Face old_face)
		{
			foreach (var sc_face in faces) {
				if (sc_face.contains(old_face)) {
					Console.Write("asdf"); // Add in proper deletion
				}
			}
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
					Console.Write("{0} exceeds the number of vertices for his simplicial complex.\n",vertices_string[i]);
				}
				if (result && number >= 1 && number <= num_vertices) {
					face.addVertex(number);
				}
			}
			removeFaceFromSC(face);
		}
		
		public bool performOperation()
		{
			Console.Clear();
			displayNumVertices();
			Console.Write("What would you like to do?\n\n");
			Console.Write("(A)dd a Face (adds all subfaces too).\n");
			Console.Write("(D)elete a Face (deletes all superfaces too).\n");
			Console.Write("(C)hange number of vertices.\n");
			Console.Write("(V)iew faces.\n");
			Console.Write("(Q)uit.\n");
			string selection = Console.ReadLine();
			if (selection.Length > 0 && (selection[0] == 'a' || selection[0] == 'A')) {
				addEdge();
			}
			if (selection.Length > 0 && (selection[0] == 'd' || selection[0] == 'D')) {
				deleteFace();
			}
			if (selection.Length > 0 && (selection[0] == 'c' || selection[0] == 'C')) {
				setNumVertices();
			}
			if (selection.Length > 0 && (selection[0] == 'v' || selection[0] == 'V')) {
				displayFaces();
			}
			if (selection.Length > 0 && (selection[0] == 'q' || selection[0] == 'Q')) {
				return false;
			}
			return true;
		}
	}
}