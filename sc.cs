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
		
		// Determine if there is a solution by using partial row echelon form.
		// Though not directly related to simplicial complexes, I am putting the function here for simplicity.
		public static bool RowEchelon(List<List<int>> system_of_equations) {
			if (system_of_equations.Count == 0 || system_of_equations[0].Count == 0) {
				return true; // A pathological case. Deal with this separately if needed.
			}
			int num_cols = system_of_equations[0].Count;
			int num_rows = system_of_equations.Count;
			int cur_col = 0; // The current column for which we are finding a nonzero leading coefficient.
			int cur_row = 0; // Number of rows that have been moved to the top.
			while (cur_col < num_cols-1) {
				// Find a nonzero leading coefficient if there is one.
				
				// Swap this row to the highest allowable point
				for (int j=cur_row; j<num_rows; j++) {
					if (system_of_equations[j][cur_col] != 0) { // Found a nonzero leading coefficient
						int temp = 0;
						for (int i=0; i<num_cols; i++) {
							temp = system_of_equations[cur_row][i];
							system_of_equations[cur_row][i] = system_of_equations[j][i];
							system_of_equations[j][i] = temp;
						}
						j = num_rows;
						cur_row++;
					}
				}
				
				// Clear out leading coefficients below it.
				// To fully do row echelon form, we should also clear out the leading coefficients above the one we found.
				// However, since we only need to determine of there is a solution (not actually find it), we can skip that step.
				for (int j=cur_row+1; j<num_rows; j++) {
					if (system_of_equations[j][cur_col] != 0) {
						for (int i=0; i<num_cols; i++) {
							system_of_equations[j][i] = system_of_equations[cur_row][cur_col]*system_of_equations[j][i] - (system_of_equations[cur_row][i]*system_of_equations[j][cur_col]);
						}
					}
				}
				
				cur_col++;
			}
			
			// Find if there are any rows there the constant is nonzero but all variable coefficients are zero.
			// There is a solution iff there is no such row.
			for (int i=0; i<num_rows; i++) {
				bool any_nonzero = false;
				for (int j=0; j<num_cols-1; j++) {
					if (system_of_equations[i][j] != 0) {
						any_nonzero = true;
					}
				}
				if (any_nonzero == false && system_of_equations[i][num_cols-1] != 0) {
					return false;
				}
			}
			return true;
		}
		
		// Determine if face face_num can be written as a linear combination of the previous faces in the chain.
		// If yes, then we add one to beta_d, where d is the dimension of the face.
		// If no, then we subtract one from beta_(d-1)
		public bool determine_betti(List<Chain> chains_dim, int face_num)
		{
			// Make a copy so we can modify the chain without worry
			List<Chain> temp_chains = new List<Chain>();
			List<List<int>> system_of_equations = new List<List<int>>(); // Another way of storing the same data
			for (int i=0; i<chains_dim.Count; i++) {
				temp_chains.Add(chains_dim[i].copyChain());
			}
			for (int i=0; i<=faces.Count; i++) {
				system_of_equations.Add(new List<int>());
				for (int j=0; j<=face_num; j++) {
					system_of_equations[i].Add(temp_chains[j].getCoeff(i));
				}
			}
			// The goal here will be to determine of the (face_num) chain can be written as a linear combination of the previous chains.
			// Iff yet, then return true.
			// We will do this by row echelon form.
			
			bool is_solution = RowEchelon(system_of_equations);
			
			// Some code for displaying and testing, displaying output only at a certain point.
			int display_dim = -1;
			int display_face_num = -1; // Values set to prevent display
			if (face_num == display_face_num && chains_dim[0].dimension() == display_dim) {
				Console.Write("************************\nRow Echelon Form\n");
				for (int i=0; i<system_of_equations.Count; i++) {
					for (int j=0; j<system_of_equations[i].Count; j++) {
						Console.Write("{0} ",system_of_equations[i][j]);
					}
					Console.Write("\n");
				}
				Console.Write("************************\n");
			}
			return is_solution;
		}
		
		public void betti()
		{
			Console.Write("Following are the faces and their boundaries, followed by Betti numbers.\n");
			
			List<List<Chain>> chains_by_dimension = new List<List<Chain>>();
			List<List<int>> face_nums_by_dimension = new List<List<int>>();
			List<int> betti_numbers = new List<int>();
			int max_d = -1; // Maximum value of d found. -1 means no value found yet.
			
			for (int i=0; i<faces.Count; i++) {
				int dimension = faces[i].vertices.Count;
				if (faces[i].vertices.Count > max_d) {
					max_d++;
					chains_by_dimension.Add(new List<Chain>());
					face_nums_by_dimension.Add(new List<int>());
					betti_numbers.Add(0);
				}
				Chain chain = boundary(i);
				chains_by_dimension[dimension].Add(chain);
				face_nums_by_dimension[dimension].Add(i);
			}
			for (int i=0; i<chains_by_dimension.Count; i++) {
				Console.Write("Dimension {0}\n===============\n",i-1);
				for (int j=0; j<chains_by_dimension[i].Count; j++) {
					Console.Write("The face: ");
					faces[face_nums_by_dimension[i][j]].writeFace();
					Console.Write("\n");
					Console.Write("The boundary: ");
					chains_by_dimension[i][j].display();
					Console.Write("\n\n");
					bool in_span = determine_betti(chains_by_dimension[i],j);
					if (in_span) {
						betti_numbers[i]++;
					}
					else if (i>0) { // The case i=0 shouldn't occur
						betti_numbers[i-1]--;
					}
				}
			}
			// Display Betti numbers
			Console.Write("Betti numbers:\n");
			for (int i=0; i<betti_numbers.Count; i++) {
				Console.Write("beta_{0}: {1}\n",i-1,betti_numbers[i]);
			}
			Console.Write("Enter anything to continue.\n");
			Console.ReadLine();
		}
		
		// Get the number in the list that a given face is.
		// This is probably inefficient, so think of another way.
		public int getFaceNum(Face face)
		{
			for (int i=0; i<faces.Count; i++) {
				if (faces[i].isEqual(face)) {
					return i;
				}
			}
			Console.Write("Error: getFaceNum (sc.cs) is not finding a face as expected.\n");
			Console.ReadLine();
			return -1; // Should not reach this case.
		}
		
		public Chain boundary(int face_num)
		{		
			if (face_num >= faces.Count) {
				Console.Write("Face number out of range (should not get to this code).\n");
				Console.ReadLine();
				return new Chain();
			}
			Face face = faces[face_num];
			Chain chain = new Chain();
			int coefficient = 1;
			for (int i=face.vertices.Count-1; i>=0; i--) {
				Face smallerFace = face.removeVertex(i);
				chain.addTerm(getFaceNum(smallerFace),coefficient);
				if (coefficient == 1) {
					coefficient = -1;
				}
				else {
					coefficient = 1;
				}
			}
			return chain;
		}
	
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
		// The private helper of addFace(), where input and output has already been taken care of.
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
	
		// The public version of adding a face, which also handles text display and the input of a face to add.
		public void addFace()
		{
			Console.Write("Input the face: vertex numbers separated by spaces (e.g. 5 7 12).\n");
			Console.Write("At most 10 vertices are allowed, as integers from 1 to the number of vertices.\n");
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
				else if (i >= 10) {
					Console.Write("Too many vertices. The limit is {0} in a face (dimension {1}).\n",10,9);
				}
				if (result && number >= 1 && number <= num_vertices && i<10) {
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
	
		// The private helper of deleteFace, where input and output is already taken care of.
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
	
		// The public version of deleting a face. Also does input and output.
		public void deleteFace()
		{
			Console.Write("Input the face: vertex numbers separated by spaces (e.g. 5 7 12).\n");
			Console.Write("A face without valid vertices will be interpreted as the empty face and cause everything to be removed.\n");
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
		
		// Initialize to one of several preset simplicial complexes.
		public void loadComplex()
		{
			Console.Write("\nEnter the type of complex, followed by parameters.\n\n");
			Console.Write("Simplex on n vertices: S n (1 <= n <= 10.)\n");
			Console.Write("Boundary simplex on n vertices: B n (1 <= n <= 10.)\n");
			Console.Write("Complete complex of dimension d-1 on n vertices: C n d (1 <= d <= n <= 10.)\n");
			Console.Write("All other inputs: do nothing.\n");
			string input = Console.ReadLine();
			String[] input_blocks = input.Split(' ');
			if (input_blocks.Length < 1) {
				return;
			}
			if ((input_blocks[0] == "S" || input_blocks[0] == "s") && input_blocks.Length >= 2) {
				int param; 
				bool result = int.TryParse(input_blocks[1], out param);
				if (result && param >= 1 && param <= 10) {
					Console.Write("One simplex coming right up.\n");
					Console.ReadLine();
					faces = new List<Face>();
					num_vertices = param;
					
					Face new_face = new Face();
					for (int i=1; i<=param; i++) {
						new_face.addVertex(i);
					}
					addFaceToSC(new_face);
					return;
				}
			}
			if ((input_blocks[0] == "B" || input_blocks[0] == "b") && input_blocks.Length >= 2) {
				int param; 
				bool result = int.TryParse(input_blocks[1], out param);
				if (result && param >= 1 && param <= 10) {
					Console.Write("Reticulating splines (and the boundary).\n");
					Console.ReadLine();
					faces = new List<Face>();
					num_vertices = param;
					
					Face new_face = new Face();
					for (int i=1; i<=param; i++) {
						new_face.addVertex(i);
					}
					addFaceToSC(new_face);
					removeFaceFromSC(new_face);
					return;
				}
			}
			if ((input_blocks[0] == "C" || input_blocks[0] == "c") && input_blocks.Length >= 3) {
				int param1;
				int param2;
				bool result1 = int.TryParse(input_blocks[1], out param1);
				bool result2 = int.TryParse(input_blocks[2], out param2);
				if (result1 && result2 && param1 <= 10 && param2 >= 1 && param1 >= param2) {
					Console.Write("Building the complex.\n");
					Console.ReadLine();
					faces = new List<Face>();
					num_vertices = param1;
					
					Face new_face = new Face();
					for (int i=1; i<=param2; i++) {
						new_face.addVertex(i);
					}
					bool result = true;
					while(result) {
						addFaceToSC(new_face);
						result = new_face.advance(num_vertices);
					}					
					return;
				}
			}
			// Should only get here is no valid input is found
			Console.Write("Does not appear to be valid input, so doing nothing. Enter anything to continue.\n");
			Console.ReadLine();
		}
	}
}