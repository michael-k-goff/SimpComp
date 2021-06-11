# Betti Numbers

This C# program allows the user to input a simplicial complex and calculate the Betti numbers.

### Instructions

This problem is developed and tested on a Mac. Set up Mono, a C# compiler for macOS, and do the following.

- Download the repo and navigate the terminal to the folder containing the files.
- Compilation: `mcs main.cs sc.cs face.cs chain.cs` or `mcs -out:main.exe *.cs`.
- Running: `mono main.exe`.
- Follow the prompts on the command line.

### About

A simplicial complex <img src="https://latex.codecogs.com/gif.latex?\Delta " /> is a generalization of a graph in graph theory. It is a object consisting of the following.

- A finite set of vertices <img src="https://latex.codecogs.com/gif.latex?V " />.
- A set <img src="https://latex.codecogs.com/gif.latex?\mathcal{F} " /> of subsets of <img src="https://latex.codecogs.com/gif.latex?V " /> known as faces.
- If <img src="https://latex.codecogs.com/gif.latex?F \in \mathcal{F} " /> is a face and <img src="https://latex.codecogs.com/gif.latex?F' \subseteq F " />, then also <img src="https://latex.codecogs.com/gif.latex?F' \in \mathcal{F} " />. This implies that unless <img src="https://latex.codecogs.com/gif.latex?\mathcal{F} = \emptyset " />, <img src="https://latex.codecogs.com/gif.latex?\emptyset \in \mathcal{F} " />.

Some definitions also require that for all <img src="https://latex.codecogs.com/gif.latex?v \in V, \lbrace v \rbrace \in \mathcal{F}" />, but that is not required here.

The dimension <img src="https://latex.codecogs.com/gif.latex?d " /> of <img src="https://latex.codecogs.com/gif.latex?\Delta " /> is one less than the size of the largest face of <img src="https://latex.codecogs.com/gif.latex?\Delta " />. Dimension is undefined for <img src="https://latex.codecogs.com/gif.latex?\Delta = \emptyset " />. Thus the dimension of <img src="https://latex.codecogs.com/gif.latex?\lbrace \emptyset \rbrace = -1" />, the dimension of a nonzero set of vertices is 0, the dimension of an ordinary graph with at least one edge is 1, and so on.

Betti numbers <img src="https://latex.codecogs.com/gif.latex?\beta_i(\Delta) " /> are toplogical invariants of a simplicial complex that tell us how many "holes" there are of a given dimension. <img src="https://latex.codecogs.com/gif.latex?\beta_{-1}(\Delta) = 1 " /> if and only if <img src="https://latex.codecogs.com/gif.latex?\Delta = \lbrace \emptyset \rbrace " />, and otherwise <img src="https://latex.codecogs.com/gif.latex?\beta_{-1}(\Delta) = 0 " />. <img src="https://latex.codecogs.com/gif.latex?\beta_{0}(\Delta)" /> is one less than the number of connected components of <img src="https://latex.codecogs.com/gif.latex?\Delta " />. <img src="https://latex.codecogs.com/gif.latex?\beta_1(\Delta) " /> can be regarded as the number of independent 1-dimensional cycles of <img src="https://latex.codecogs.com/gif.latex?\Delta " />, and so on.

The Betti numbers are a topological invariant, in the sense that if two simplicial complexes are homeomorphic, then their Betti numbers are the same (the converse, that having the same Betti numbers implies the complexes are homeomorphic, is not necessarily true). Furthermore, if a simplicial complex is a triangulation of a topological space, it has the same Betti numbers as that space. Since simplicial complexes are finite and calculations of their Betti numbers lend themselves to combinatorial techniques, this is generally easier than calculating the Betti numbers of a topological space.

### Calculating Betti numbers

We calculate Betti numbers in a manner similar as described in [this paper](http://www.cs.jhu.edu/~misha/ReadingSeminar/Papers/Delfinado93.pdf). If the faces of a simplicial complex are ordered <img src="https://latex.codecogs.com/gif.latex?F_1, F_2, \ldots, F_n " /> so that if <img src="https://latex.codecogs.com/gif.latex?F_i \subseteq F_j, i \leq j " />, then we can iteratively apply the Mayer-Vietoris sequence and calculate Betti numbers face by face. Let <img src="https://latex.codecogs.com/gif.latex?\Delta_1 " /> and <img src="https://latex.codecogs.com/gif.latex?\Delta_2 = \Delta_1 \cup F " /> be subcomplexes of <img src="https://latex.codecogs.com/gif.latex?\Delta " /> (`F` is of dimension `d-1`), then the Betti numbers of <img src="https://latex.codecogs.com/gif.latex?\Delta_1 " /> and <img src="https://latex.codecogs.com/gif.latex?\Delta_2 " /> are the same except for the following:

- If F completes a (d-1)-cycle of <img src="https://latex.codecogs.com/gif.latex?\Delta_1 " />, then <img src="https://latex.codecogs.com/gif.latex?\beta_{d-1}(\Delta_2) = \beta_{d-1}(\Delta_1) + 1 " />.
- Otherwise, <img src="https://latex.codecogs.com/gif.latex?\beta_{d-2}(\Delta_2) = \beta_{d-2}(\Delta_1) - 1 " />.

To determine if F completes a (d-1)-cycle, let <img src="https://latex.codecogs.com/gif.latex?F_1, \ldots, F_k " /> be the set of (d-1)-dimensional faces of <img src="https://latex.codecogs.com/gif.latex?\Delta_1" />. Then F completes a (d-1)-cycle if and only if <img src="https://latex.codecogs.com/gif.latex?\partial F " /> can be expressed as a linear combination of <img src="https://latex.codecogs.com/gif.latex?\partial F_1, \ldots, \partial F_k " />, where <img src="https://latex.codecogs.com/gif.latex?\partial" /> is the [boundary operator](https://algebrology.github.io/simplicial-complexes-and-boundary-maps/). This in turn is determined by use of a partial row echelon form.

### Future Directions

Significant improvement to the computational efficiency of the methods herein would be desired. This would allow much larger simplicial complexes.

This work could be extended to calculate the minimal free resolutions of the monomial ideals associated with simplicial complexes (see e.g. [this explanation](https://www.math.ucdavis.edu/~deloera/MISC/LA-BIBLIO/trunk/ReinerVictor/Reiner16.pdf)).

I would have liked to use the [Math.Net](https://numerics.mathdotnet.com/) linear algebra package, but I had trouble getting it to work with this system.