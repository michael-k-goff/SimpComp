using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
	public class Face
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
	
		public int lastVertex()
		{
			if (vertices.Count() == 0) {
				return -1;
			}
			return (vertices[vertices.Count()-1]);
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
			int check_pointer = 0;
			int face_pointer = 0;
			while (check_pointer < face.vertices.Count) {
				if (face_pointer >= vertices.Count) {
					return false;
				}
				if (vertices[face_pointer] < face.vertices[check_pointer]) {
					face_pointer++;
				}
				else if (vertices[face_pointer] == face.vertices[check_pointer]) {
					face_pointer++;
					check_pointer++;
				}
				else if (vertices[face_pointer] > face.vertices[check_pointer]) {
					return false;
				}
			}
			return true;
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
}