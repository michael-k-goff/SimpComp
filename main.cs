/* Simplicial Complexes */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Program
{
	public static void Main()
	{
		int num_vertices = 0;
		Console.Write("How many vertices?\n");
		num_vertices = int.Parse(Console.ReadLine());
		SimplicialComplex sc = new SimplicialComplex();
		while (true)
		{
			sc.addEdge();
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
			Console.Write("Input the edge: vertex numbers separated by spaces (e.g. 5 7 12).\n");
			string new_face = "";
			new_face = Console.ReadLine();
			string[] vertices_string = new_face.Split(' ');
			
			// Determine how many of the proposed vertices are valid
			Face face = new Face();
			for (int i=0; i<vertices_string.Length; i++)
			{
				int number;
				bool result = int.TryParse(vertices_string[i], out number);
				if (result && number >= 0) {
					face.addVertex(number);
				}
			}
			addFaceToSC(face);
			Console.WriteLine("Number of vertices in the face just added: {0}", face.vertexCount());
			Console.Write("The faces are as follows: \n");
			foreach (var sc_face in faces) {
				sc_face.writeFace();
				Console.Write("\n");
			}
		}
	}
}