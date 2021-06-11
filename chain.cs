using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Program
{	
	// A chain is a linear combination of simplicial complexes.
	// Info about chains and boundary maps here: https://algebrology.github.io/simplicial-complexes-and-boundary-maps/
	public class Chain 
	{
		List<int> face_nums;
		List<int> coeffs;
		
		// Constructor
		public Chain() {
			face_nums = new List<int>();
			coeffs = new List<int>();
		}
		
		public void addTerm(int face_num, int coeff) {
			face_nums.Add(face_num);
			coeffs.Add(coeff);
		}
		
		// Both lists should be the same length
		public void display() {
			if (face_nums.Count == 0) {
				Console.Write("(Boundary is empty)");
			}
			for (int i=0; i<face_nums.Count; i++) {
				if (coeffs[i] < 0) {
					Console.Write("{0}x{1} ",coeffs[i], face_nums[i]);
				}
				else
				{
					Console.Write("+{0}x{1} ",coeffs[i], face_nums[i]);
				}
			}
		}
		
		public Chain copyChain() {
			Chain new_chain = new Chain();
			for (int i=0; i<face_nums.Count; i++) {
				new_chain.face_nums.Add(face_nums[i]);
				new_chain.coeffs.Add(coeffs[i]);
			}
			return new_chain;
		}
		
		public int dimension() {
			return (face_nums.Count - 1);
		}
		
		public void displayCoeffs(int num_faces) {
			int current_coeff = 0;
			for (int i=0; i<num_faces; i++) {
				int display_coeff = 0;
				if (current_coeff < coeffs.Count && i==face_nums[current_coeff]) {
					display_coeff = coeffs[current_coeff];
					current_coeff++;
				}
				Console.Write("{0} ",display_coeff);
			}
			Console.Write("\n");
		}
		
		public int getCoeff(int face_num) {
			for (int i=0; i<face_nums.Count; i++) {
				if (face_nums[i] == face_num) {
					return (coeffs[i]);
				}
			}
			return 0;
		}
	}
}