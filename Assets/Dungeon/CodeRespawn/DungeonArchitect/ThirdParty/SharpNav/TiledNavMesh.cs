// Copyright (c) 2013-2015 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System;
using System.Collections.Generic;

using SharpNav.Collections;
using SharpNav.Geometry;
using SharpNav.Pathfinding;

#if MONOGAME
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
#elif OPENTK
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
#elif SHARPDX
using Vector2 = SharpDX.Vector2;
using Vector3 = SharpDX.Vector3;
#endif

namespace SharpNav
{
	/// <summary>
	/// A TiledNavMesh is a continuous region, which is used for pathfinding. 
	/// </summary>
	public class TiledNavMesh
	{
		/// <summary>
		/// The settings for the TiledNavMesh
		/// </summary>
		public struct TiledNavMeshParams
		{
			public Vector3 Origin;
			public float TileWidth;
			public float TileHeight;
			public int MaxTiles;
			public int MaxPolys;
		}

		private TiledNavMeshParams parameters;
		private Vector3 origin;
		private float tileWidth, tileHeight;
		private int maxTiles;
		private int tileLookupTableSize; //tile hash lookup size
		private int tileLookupTableMask; //tile hash lookup mask

		private MeshTile[] posLookup; //tile hash lookup
		private MeshTile nextFree; //freelist of tiles
		private MeshTile[] tiles; //list of tiles

		private int saltBits; //number of salt bits in ID
		private int tileBits; //number of tile bits in ID
		private int polyBits; //number of poly bits in ID

		/// <summary>
		/// Initializes a new instance of the <see cref="TiledNavMesh"/> class.
		/// </summary>
		/// <param name="data">The Navigation Mesh data</param>
		public TiledNavMesh(NavMeshBuilder data)
		{
			TiledNavMeshParams parameters;
			parameters.Origin = data.Header.Bounds.Min;
			parameters.TileWidth = data.Header.Bounds.Max.X - data.Header.Bounds.Min.X;
			parameters.TileHeight = data.Header.Bounds.Max.Z - data.Header.Bounds.Min.Z;
			parameters.MaxTiles = 1;
			parameters.MaxPolys = data.Header.PolyCount;

			if (!InitTileNavMesh(parameters))
				return;

			int tileRef = 0;
			AddTile(data, 0, ref tileRef);
		}

		/// <summary>
		/// Gets the maximum number of tiles that can be stored
		/// </summary>
		public int TileCount
		{
			get
			{
				return maxTiles;
			}
		}

		/// <summary>
		/// Gets the mesh tile at a specified index.
		/// </summary>
		/// <param name="index">The index referencing a tile.</param>
		/// <returns>The tile at the index.</returns>
		public MeshTile this[int index]
		{
			get
			{
				return tiles[index];
			}
		}

		/// <summary>
		/// Gets or sets user data for this navmesh.
		/// </summary>
		public object Tag { get; set; }

		/// <summary>
		/// Initialize the Tiled Navigation Mesh variables and arrays.
		/// </summary>
		/// <param name="parameters">Tiled Navigation Mesh attributes</param>
		/// <returns>True if initialization is successful</returns>
		public bool InitTileNavMesh(TiledNavMeshParams parameters)
		{
			this.parameters = parameters;
			origin = this.parameters.Origin;
			tileWidth = parameters.TileWidth;
			tileHeight = parameters.TileHeight;

			//init tiles
			maxTiles = parameters.MaxTiles;
			tileLookupTableSize = MathHelper.NextPowerOfTwo(parameters.MaxTiles / 4);
			if (tileLookupTableSize == 0)
				tileLookupTableSize = 1;
			tileLookupTableMask = tileLookupTableSize - 1;

			tiles = new MeshTile[maxTiles];
			posLookup = new MeshTile[tileLookupTableSize];
			for (int i = 0; i < tiles.Length; i++)
				tiles[i] = new MeshTile();
			for (int i = 0; i < posLookup.Length; i++)
				posLookup[i] = null;

			//create a linked list of tiles
			nextFree = null;
			for (int i = maxTiles - 1; i >= 0; i--)
			{
				tiles[i].Salt = 1;
				tiles[i].Next = nextFree;
				nextFree = tiles[i];
			}
			
			//init ID generator values
			tileBits = MathHelper.Log2(MathHelper.NextPowerOfTwo(parameters.MaxTiles));
			polyBits = MathHelper.Log2(MathHelper.NextPowerOfTwo(parameters.MaxPolys));

			//only allow 31 salt bits, since salt mask is calculated using 32-bit int and it will overflow
			saltBits = Math.Min(31, 32 - tileBits - polyBits);
			if (saltBits < 10)
				return false;

			return true;
		}

		/// <summary>
		/// Build a tile and link all the polygons togther, both internally and externally.
		/// Make sure to link off-mesh connections as well.
		/// </summary>
		/// <param name="data">Navigation Mesh data</param>
		/// <param name="lastRef">Last polygon reference</param>
		/// <param name="result">Last tile reference</param>
		public void AddTile(NavMeshBuilder data, int lastRef, ref int result)
		{
			//make sure data is in right format
			PathfindingCommon.NavMeshInfo header = data.Header;

			//make sure location is free
			if (GetTileAt(header.X, header.Y, header.Layer) != null)
				return;

			//allocate a tile
			MeshTile tile = null;
			if (lastRef == 0)
			{
				if (nextFree != null)
				{
					tile = nextFree;
					nextFree = tile.Next;
					tile.Next = null;
				}
			}
			else
			{
				//try to relocate tile to specific index with the same salt
				int tileIndex = DecodePolyIdTile(lastRef);
				if (tileIndex >= maxTiles)
					return;

				//try to find specific tile id from free list
				MeshTile target = tiles[tileIndex];
				MeshTile prev = null;
				tile = nextFree;
				while (tile != null && tile != target)
				{
					prev = tile;
					tile = tile.Next;
				}

				//couldn't find correct location
				if (tile != target)
					return;

				//remove from freelist
				if (prev == null)
					nextFree = tile.Next;
				else
					prev.Next = tile.Next;

				//restore salt
				tile.Salt = DecodePolyIdSalt(lastRef);
			}

			//make sure we could allocate a tile
			if (tile == null)
				return;

			//insert tile into position LookUp Table (lut)
			int h = ComputeTileHash(header.X, header.Y, tileLookupTableMask);
			tile.Next = posLookup[h];
			posLookup[h] = tile;

			if (header.BvNodeCount == 0)
				tile.BVTree = null;

			//patch header
			tile.Verts = data.NavVerts;
			tile.Polys = data.NavPolys;
			tile.DetailMeshes = data.NavDMeshes;
			tile.DetailVerts = data.NavDVerts;
			tile.DetailTris = data.NavDTris;
			tile.BVTree = data.NavBvTree;
			tile.OffMeshConnections = data.OffMeshCons;

			//build links freelist
			tile.LinksFreeList = 0;
			tile.Links = new Link[header.MaxLinkCount];
			for (int i = 0; i < header.MaxLinkCount; i++)
				tile.Links[i] = new Link();

			tile.Links[header.MaxLinkCount - 1].Next = Link.Null;
			for (int i = 0; i < header.MaxLinkCount - 1; i++)
				tile.Links[i].Next = i + 1;

			//init tile
			tile.Header = header;
			tile.Data = data;

			ConnectIntLinks(ref tile);
			BaseOffMeshLinks(ref tile);

			//create connections with neighbor tiles
			MeshTile[] neis = new MeshTile[32];
			int nneis;

			//connect with layers in current tile
			nneis = GetTilesAt(header.X, header.Y, neis);
			for (int j = 0; j < nneis; j++)
			{
				if (neis[j] != tile)
				{
					ConnectExtLinks(ref tile, ref neis[j], BoundarySide.Internal);
					ConnectExtLinks(ref neis[j], ref tile, BoundarySide.Internal);
				}

				ConnectExtOffMeshLinks(ref tile, ref neis[j], BoundarySide.Internal);
				ConnectExtOffMeshLinks(ref neis[j], ref tile, BoundarySide.Internal);
			}

			//connect with neighbour tiles
			for (int i = 0; i < 8; i++)
			{
				BoundarySide b = (BoundarySide)i;
				BoundarySide bo = b.GetOpposite();
				nneis = GetNeighbourTilesAt(header.X, header.Y, b, neis);
				for (int j = 0; j < nneis; j++)
				{
					ConnectExtLinks(ref tile, ref neis[j], b);
					ConnectExtLinks(ref neis[j], ref tile, bo);
					ConnectExtOffMeshLinks(ref tile, ref neis[j], b);
					ConnectExtOffMeshLinks(ref neis[j], ref tile, bo);
				}
			}

			result = GetTileRef(tile);
		}

		/// <summary>
		/// Allocate links for each of the tile's polygons' vertices
		/// </summary>
		/// <param name="tile">A tile contains a set of polygons, which are linked to each other</param>
		public void ConnectIntLinks(ref MeshTile tile)
		{
			if (tile == null)
				return;

			int polyBase = GetPolyRefBase(tile);

			//Iterate through all the polygons
			for (int i = 0; i < tile.Header.PolyCount; i++)
			{
				//The polygon links will end in a null link
				tile.Polys[i].FirstLink = Link.Null;

				//Avoid Off-Mesh Connection polygons
				if (tile.Polys[i].PolyType == PolygonType.OffMeshConnection)
					continue;

				//Build edge links
				for (int j = tile.Polys[i].VertCount - 1; j >= 0; j--)
				{
					//Skip hard and non-internal edges
					if (tile.Polys[i].Neis[j] == 0 || IsExternalLink(tile.Polys[i].Neis[j]))
						continue;

					//Allocate a new link if possible
					int idx = AllocLink(tile);

					//Allocation of link should be successful
					if (IsLinkAllocated(idx))
					{
						//Initialize a new link
						tile.Links[idx].Reference = GetReference(polyBase, tile.Polys[i].Neis[j] - 1);
						tile.Links[idx].Edge = j;
						tile.Links[idx].Side = BoundarySide.Internal;
						tile.Links[idx].BMin = tile.Links[idx].BMax = 0;

						//Add to polygon's links list
						tile.Links[idx].Next = tile.Polys[i].FirstLink;
						tile.Polys[i].FirstLink = idx;
					}
				}
			}
		}

		/// <summary>
		/// Begin creating off-mesh links between the tile polygons.
		/// </summary>
		/// <param name="tile">Current Tile</param>
		public void BaseOffMeshLinks(ref MeshTile tile)
		{
			if (tile == null)
				return;

			int polyBase = GetPolyRefBase(tile);

			//Base off-mesh connection start points
			for (int i = 0; i < tile.Header.OffMeshConCount; i++)
			{
				int con = i;
				int poly = tile.OffMeshConnections[con].Poly;

				Vector3 extents = new Vector3(tile.OffMeshConnections[con].Radius, tile.Header.WalkableClimb, tile.OffMeshConnections[con].Radius);
				
				//Find polygon to connect to
				Vector3 p = tile.OffMeshConnections[con].Pos0;
				Vector3 nearestPt = new Vector3();
				int reference = FindNearestPolyInTile(tile, p, extents, ref nearestPt);
				if (reference == 0)
					continue;

				//Do extra checks
				if ((nearestPt.X - p.X) * (nearestPt.X - p.X) + (nearestPt.Z - p.Z) * (nearestPt.Z - p.Z) >
					tile.OffMeshConnections[con].Radius * tile.OffMeshConnections[con].Radius)
					continue;

				//Make sure location is on current mesh
				tile.Verts[tile.Polys[poly].Verts[0]] = nearestPt;

				//Link off-mesh connection to target poly
				int idx = AllocLink(tile);
				if (IsLinkAllocated(idx))
				{
					//Initialize a new link
					tile.Links[idx].Reference = reference;
					tile.Links[idx].Edge = 0;
					tile.Links[idx].Side = BoundarySide.Internal;
					tile.Links[idx].BMin = tile.Links[idx].BMax = 0;

					//Add to polygon's links list
					tile.Links[idx].Next = tile.Polys[poly].FirstLink;
					tile.Polys[poly].FirstLink = idx;
				}

				//Start end-point always conects back to off-mesh connection
				int tidx = AllocLink(tile);
				if (IsLinkAllocated(tidx))
				{
					//Initialize a new link
					int landPolyIdx = DecodePolyIdPoly(reference);
					tile.Links[idx].Reference = GetReference(polyBase, tile.OffMeshConnections[con].Poly);
					tile.Links[idx].Edge = 0xff;
					tile.Links[idx].Side = BoundarySide.Internal;
					tile.Links[idx].BMin = tile.Links[idx].BMax = 0;

					//Add to polygon's links list
					tile.Links[idx].Next = tile.Polys[landPolyIdx].FirstLink;
					tile.Polys[landPolyIdx].FirstLink = tidx;
				}
			}
		}

		/// <summary>
		/// Connect polygons from two different tiles.
		/// </summary>
		/// <param name="tile">Current Tile</param>
		/// <param name="target">Target Tile</param>
		/// <param name="side">Polygon edge</param>
		public void ConnectExtLinks(ref MeshTile tile, ref MeshTile target, BoundarySide side)
		{
			if (tile == null)
				return;

			//Connect border links
			for (int i = 0; i < tile.Header.PolyCount; i++)
			{
				int numPolyVerts = tile.Polys[i].VertCount;

				for (int j = 0; j < numPolyVerts; j++)
				{
					//Skip non-portal edges
					if ((tile.Polys[i].Neis[j] & Link.External) == 0)
						continue;

					BoundarySide dir = (BoundarySide)(tile.Polys[i].Neis[j] & 0xff);
					if (side != BoundarySide.Internal && dir != side)
						continue;

					//Create new links
					Vector3 va = tile.Verts[tile.Polys[i].Verts[j]];
					Vector3 vb = tile.Verts[tile.Polys[i].Verts[(j + 1) % numPolyVerts]];
					List<int> nei = new List<int>(4);
					List<float> neia = new List<float>(4 * 2);
					FindConnectingPolys(va, vb, target, dir.GetOpposite(), nei, neia);

					//Iterate through neighbors
					for (int k = 0; k < nei.Count; k++)
					{
						//Allocate a new link if possible
						int idx = AllocLink(tile);

						if (IsLinkAllocated(idx))
						{
							tile.Links[idx].Reference = nei[k];
							tile.Links[idx].Edge = j;
							tile.Links[idx].Side = dir;

							tile.Links[idx].Next = tile.Polys[i].FirstLink;
							tile.Polys[i].FirstLink = idx;

							//Compress portal limits to a value
							if (dir == BoundarySide.PlusX || dir == BoundarySide.MinusX)
							{
								float tmin = (neia[k * 2 + 0] - va.Z) / (vb.Z - va.Z);
								float tmax = (neia[k * 2 + 1] - va.Z) / (vb.Z - va.Z);

								if (tmin > tmax)
								{
									float temp = tmin;
									tmin = tmax;
									tmax = temp;
								}

								tile.Links[idx].BMin = (int)(MathHelper.Clamp(tmin, 0.0f, 1.0f) * 255.0f);
								tile.Links[idx].BMax = (int)(MathHelper.Clamp(tmax, 0.0f, 1.0f) * 255.0f);
							}
							else if (dir == BoundarySide.PlusZ || dir == BoundarySide.MinusZ)
							{
								float tmin = (neia[k * 2 + 0] - va.X) / (vb.X - va.X);
								float tmax = (neia[k * 2 + 1] - va.X) / (vb.X - va.X);

								if (tmin > tmax)
								{
									float temp = tmin;
									tmin = tmax;
									tmax = temp;
								}

								tile.Links[idx].BMin = (int)(MathHelper.Clamp(tmin, 0.0f, 1.0f) * 255.0f);
								tile.Links[idx].BMax = (int)(MathHelper.Clamp(tmax, 0.0f, 1.0f) * 255.0f);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Connect Off-Mesh links between polygons from two different tiles.
		/// </summary>
		/// <param name="tile">Current Tile</param>
		/// <param name="target">Target Tile</param>
		/// <param name="side">Polygon edge</param>
		public void ConnectExtOffMeshLinks(ref MeshTile tile, ref MeshTile target, BoundarySide side)
		{
			if (tile == null)
				return;

			//Connect off-mesh links, specifically links which land from target tile to this tile
			BoundarySide oppositeSide = side.GetOpposite();

			//Iterate through all the off-mesh connections of target tile
			for (int i = 0; i < target.Header.OffMeshConCount; i++)
			{
				OffMeshConnection targetCon = target.OffMeshConnections[i];
				if (targetCon.Side != oppositeSide)
					continue;

				Poly targetPoly = target.Polys[targetCon.Poly];

				//Skip off-mesh connections which start location could not be connected at all
				if (!IsLinkAllocated(targetPoly.FirstLink))
					continue;

				Vector3 extents = new Vector3(targetCon.Radius, target.Header.WalkableClimb, targetCon.Radius);

				//Find polygon to connect to
				Vector3 p = targetCon.Pos1;
				Vector3 nearestPt = new Vector3();
				int reference = FindNearestPolyInTile(tile, p, extents, ref nearestPt);
				if (reference == 0)
					continue;

				//Further checks
				if ((nearestPt.X - p.X) * (nearestPt.X - p.X) + (nearestPt.Z - p.Z) * (nearestPt.Z - p.Z) >
					(targetCon.Radius * targetCon.Radius))
					continue;

				//Make sure the location is on the current mesh
				target.Verts[targetPoly.Verts[1]] = nearestPt;

				//Link off-mesh connection to target poly
				int idx = AllocLink(target);
				if (IsLinkAllocated(idx))
				{
					target.Links[idx].Reference = reference;
					target.Links[idx].Edge = i;
					target.Links[idx].Side = oppositeSide;
					target.Links[idx].BMin = target.Links[idx].BMax = 0;

					//add to linked list
					target.Links[idx].Next = target.Polys[i].FirstLink;
					target.Polys[i].FirstLink = idx;
				}

				//link target poly to off-mesh connection
				if ((targetCon.Flags & OffMeshConnectionFlags.Bidirectional) != 0)
				{
					int tidx = AllocLink(tile);
					if (IsLinkAllocated(tidx))
					{
						int landPolyIdx = DecodePolyIdPoly(reference);
						tile.Links[tidx].Reference = GetReference(GetPolyRefBase(target), targetCon.Poly);
						tile.Links[tidx].Edge = 0xff;
						tile.Links[tidx].Side = side;
						tile.Links[tidx].BMin = tile.Links[tidx].BMax = 0;

						//add to linked list
						tile.Links[tidx].Next = tile.Polys[landPolyIdx].FirstLink;
						tile.Polys[landPolyIdx].FirstLink = tidx;
					}
				}
			}
		}

		/// <summary>
		/// Retrieve the endpoints of the offmesh connection at the specified polygon
		/// </summary>
		/// <param name="prevRef">The previous polygon reference</param>
		/// <param name="polyRef">The current polygon reference</param>
		/// <param name="startPos">The starting position</param>
		/// <param name="endPos">The ending position</param>
		/// <returns>True if endpoints found, false if not</returns>
		public bool GetOffMeshConnectionPolyEndPoints(int prevRef, int polyRef, ref Vector3 startPos, ref Vector3 endPos)
		{
			int salt = 0, indexTile = 0, indexPoly = 0;

			if (polyRef == 0)
				return false;

			//get current polygon
			DecodePolyId(polyRef, ref salt, ref indexTile, ref indexPoly);
			if (indexTile >= maxTiles)
				return false;
			if (tiles[indexTile].Salt != salt || tiles[indexTile].Header == null)
				return false;
			MeshTile tile = tiles[indexTile];
			if (indexPoly >= tile.Header.PolyCount)
				return false;
			Poly poly = tile.Polys[indexPoly];

			if (poly.PolyType != PolygonType.OffMeshConnection)
				return false;

			int idx0 = 0, idx1 = 1;

			//find the link that points to the first vertex
			for (int i = poly.FirstLink; i != Link.Null; i = tile.Links[i].Next)
			{
				if (tile.Links[i].Edge == 0)
				{
					if (tile.Links[i].Reference != prevRef)
					{
						idx0 = 1;
						idx1 = 0;
					}

					break;
				}
			}

			startPos = tile.Verts[poly.Verts[idx0]];
			endPos = tile.Verts[poly.Verts[idx1]];

			return true;
		}

		/// <summary>
		/// Search for neighbor polygons in the tile.
		/// </summary>
		/// <param name="va">Vertex A</param>
		/// <param name="vb">Vertex B</param>
		/// <param name="tile">Current tile</param>
		/// <param name="side">Polygon edge</param>
		/// <param name="con">Resulting Connection polygon</param>
		/// <param name="conarea">Resulting Connection area</param>
		public void FindConnectingPolys(Vector3 va, Vector3 vb, MeshTile tile, BoundarySide side, List<int> con, List<float> conarea)
		{
			if (tile == null)
				return;

			Vector2 amin = Vector2.Zero;
			Vector2 amax = Vector2.Zero;
			CalcSlabEndPoints(va, vb, amin, amax, side);
			float apos = GetSlabCoord(va, side);

			//Remove links pointing to 'side' and compact the links array
			Vector2 bmin = Vector2.Zero;
			Vector2 bmax = Vector2.Zero;

			int polyBase = GetPolyRefBase(tile);

			//Iterate through all the tile's polygons
			for (int i = 0; i < tile.Header.PolyCount; i++)
			{
				int numPolyVerts = tile.Polys[i].VertCount;

				//Iterate through all the vertices
				for (int j = 0; j < numPolyVerts; j++)
				{
					//Skip edges which do not point to the right side
					if (tile.Polys[i].Neis[j] != (Link.External | (int)side))
						continue;

					//Grab two adjacent vertices
					Vector3 vc = tile.Verts[tile.Polys[i].Verts[j]];
					Vector3 vd = tile.Verts[tile.Polys[i].Verts[(j + 1) % numPolyVerts]];
					float bpos = GetSlabCoord(vc, side);

					//Segments are not close enough
					if (Math.Abs(apos - bpos) > 0.01f)
						continue;

					//Check if the segments touch
					CalcSlabEndPoints(vc, vd, bmin, bmax, side);

					//Skip if slabs don't overlap
					if (!OverlapSlabs(amin, amax, bmin, bmax, 0.01f, tile.Header.WalkableClimb))
						continue;

					//Add return value
					if (con.Count < con.Capacity)
					{
						conarea.Add(Math.Max(amin.X, bmin.X));
						conarea.Add(Math.Min(amax.X, bmax.X));
						con.Add(GetReference(polyBase, i));
					}

					break;
				}
			}
		}

		/// <summary>
		/// Find the slab endpoints based off of the 'side' value.
		/// </summary>
		/// <param name="va">Vertex A</param>
		/// <param name="vb">Vertex B</param>
		/// <param name="bmin">Minimum bounds</param>
		/// <param name="bmax">Maximum bounds</param>
		/// <param name="side">The side</param>
		public void CalcSlabEndPoints(Vector3 va, Vector3 vb, Vector2 bmin, Vector2 bmax, BoundarySide side)
		{
			if (side == BoundarySide.PlusX || side == BoundarySide.MinusX)
			{
				if (va.Z < vb.Z)
				{
					bmin.X = va.Z;
					bmin.Y = va.Y;
					
					bmax.X = vb.Z;
					bmax.Y = vb.Y;
				}
				else
				{
					bmin.X = vb.Z;
					bmin.Y = vb.Y;
					
					bmax.X = va.Z;
					bmax.Y = va.Y;
				}
			}
			else if (side == BoundarySide.PlusZ || side == BoundarySide.MinusZ)
			{
				if (va.X < vb.X)
				{
					bmin.X = va.X;
					bmin.Y = va.Y;
					
					bmax.X = vb.X;
					bmax.Y = vb.Y;
				}
				else
				{
					bmin.X = vb.X;
					bmin.Y = vb.Y;
					
					bmax.X = va.X;
					bmax.Y = va.Y;
				}
			}
		}

		/// <summary>
		/// Return the proper slab coordinate value depending on the 'side' value.
		/// </summary>
		/// <param name="va">Vertex A</param>
		/// <param name="side">The side</param>
		/// <returns>Slab coordinate value</returns>
		public float GetSlabCoord(Vector3 va, BoundarySide side)
		{
			if (side == BoundarySide.PlusX || side == BoundarySide.MinusX)
				return va.X;
			else if (side == BoundarySide.PlusZ || side == BoundarySide.MinusZ)
				return va.Z;
			
			return 0;
		}

		/// <summary>
		/// Check if two slabs overlap.
		/// </summary>
		/// <param name="amin">Minimum bounds A</param>
		/// <param name="amax">Maximum bounds A</param>
		/// <param name="bmin">Minimum bounds B</param>
		/// <param name="bmax">Maximum bounds B</param>
		/// <param name="px">Point's x</param>
		/// <param name="py">Point's y</param>
		/// <returns>True if slabs overlap</returns>
		public bool OverlapSlabs(Vector2 amin, Vector2 amax, Vector2 bmin, Vector2 bmax, float px, float py)
		{
			//Check for horizontal overlap
			//Segment shrunk a little so that slabs which touch at endpoints aren't connected
			float minX = Math.Max(amin.X + px, bmin.X + px);
			float maxX = Math.Min(amax.X - px, bmax.X - px);
			if (minX > maxX)
				return false;

			//Check vertical overlap
			float leftSlope = (amax.Y - amin.Y) / (amax.X - amin.X);
			float leftConstant = amin.Y - leftSlope * amin.X;
			float rightSlope = (bmax.Y - bmin.Y) / (bmax.X - bmin.X);
			float rightConstant = bmin.Y - rightSlope * bmin.X;
			float leftMinY = leftSlope * minX + leftConstant;
			float leftMaxY = leftSlope * maxX + leftConstant;
			float rightMinY = rightSlope * minX + rightConstant;
			float rightMaxY = rightSlope * maxX + rightConstant;
			float dmin = rightMinY - leftMinY;
			float dmax = rightMaxY - leftMaxY;

			//Crossing segments always overlap
			if (dmin * dmax < 0)
				return true;

			//Check for overlap at endpoints
			float threshold = (py * 2) * (py * 2);
			if (dmin * dmin <= threshold || dmax * dmax <= threshold)
				return true;

			return false;
		}

		/// <summary>
		/// Find the closest polygon possible in the tile under certain constraints.
		/// </summary>
		/// <param name="tile">Current tile</param>
		/// <param name="center">Center starting point</param>
		/// <param name="extents">Range of search</param>
		/// <param name="nearestPt">Resulting nearest point</param>
		/// <returns>Polygon Reference which contains nearest point</returns>
		public int FindNearestPolyInTile(MeshTile tile, Vector3 center, Vector3 extents, ref Vector3 nearestPt)
		{
			BBox3 bounds;
			bounds.Min = center - extents;
			bounds.Max = center + extents;

			//Get nearby polygons from proximity grid
			List<int> polys = new List<int>(128);
			int polyCount = QueryPolygonsInTile(tile, bounds, polys);

			//Find nearest polygon amongst the nearby polygons
			int nearest = 0;
			float nearestDistanceSqr = float.MaxValue;

			//Iterate throuh all the polygons
			for (int i = 0; i < polyCount; i++)
			{
				int reference = polys[i];
				Vector3 closestPtPoly = new Vector3();
				tile.ClosestPointOnPoly(DecodePolyIdPoly(reference), center, ref closestPtPoly);
				float d = (center - closestPtPoly).LengthSquared();
				if (d < nearestDistanceSqr)
				{
					nearestPt = closestPtPoly;
					nearestDistanceSqr = d;
					nearest = reference;
				}
			}

			return nearest;
		}

		/// <summary>
		/// Find all the polygons within a certain bounding box.
		/// </summary>
		/// <param name="tile">Current tile</param>
		/// <param name="qbounds">The bounds</param>
		/// <param name="polys">List of polygons</param>
		/// <returns>Number of polygons found</returns>
		public int QueryPolygonsInTile(MeshTile tile, BBox3 qbounds, List<int> polys)
		{
			if (tile.BVTree.Count != 0)
			{
				int node = 0;
				int end = tile.Header.BvNodeCount;
				Vector3 tbmin = tile.Header.Bounds.Min;
				Vector3 tbmax = tile.Header.Bounds.Max;
				
				//Clamp query box to world box
				Vector3 qbmin = qbounds.Min;
				Vector3 qbmax = qbounds.Max;
				PolyBounds b;
				float bminx = MathHelper.Clamp(qbmin.X, tbmin.X, tbmax.X) - tbmin.X;
				float bminy = MathHelper.Clamp(qbmin.Y, tbmin.Y, tbmax.Y) - tbmin.Y;
				float bminz = MathHelper.Clamp(qbmin.Z, tbmin.Z, tbmax.Z) - tbmin.Z;
				float bmaxx = MathHelper.Clamp(qbmax.X, tbmin.X, tbmax.X) - tbmin.X;
				float bmaxy = MathHelper.Clamp(qbmax.Y, tbmin.Y, tbmax.Y) - tbmin.Y;
				float bmaxz = MathHelper.Clamp(qbmax.Z, tbmin.Z, tbmax.Z) - tbmin.Z;

				const int MinMask = unchecked((int)0xfffffffe);

				b.Min.X = (int)(bminx * tile.Header.BvQuantFactor) & MinMask;
				b.Min.Y = (int)(bminy * tile.Header.BvQuantFactor) & MinMask;
				b.Min.Z = (int)(bminz * tile.Header.BvQuantFactor) & MinMask;
				b.Max.X = (int)(bmaxx * tile.Header.BvQuantFactor + 1) | 1;
				b.Max.Y = (int)(bmaxy * tile.Header.BvQuantFactor + 1) | 1;
				b.Max.Z = (int)(bmaxz * tile.Header.BvQuantFactor + 1) | 1;

				//traverse tree
				int polyBase = GetPolyRefBase(tile);
				
				while (node < end)
				{
					BVTree.Node bvNode = tile.BVTree[node];
					bool overlap = PolyBounds.Overlapping(ref b, ref bvNode.Bounds);
					bool isLeafNode = bvNode.Index >= 0;

					if (isLeafNode && overlap)
					{
						if (polys.Count < polys.Capacity)
							polys.Add(GetReference(polyBase, bvNode.Index));
					}

					if (overlap || isLeafNode)
					{
						node++;
					}
					else
					{
						int escapeIndex = -bvNode.Index;
						node += escapeIndex;
					}
				}

				return polys.Count;
			}
			else
			{
				BBox3 b;
				int polyBase = GetPolyRefBase(tile);

				for (int i = 0; i < tile.Header.PolyCount; i++)
				{
					var poly = tile.Polys[i];

					//don't return off-mesh connection polygons
					if (poly.PolyType == PolygonType.OffMeshConnection)
						continue;

					//calculate polygon bounds
					b.Max = b.Min = tile.Verts[poly.Verts[0]];
					for (int j = 1; j < poly.VertCount; j++)
					{
						Vector3 v = tile.Verts[poly.Verts[j]];
						Vector3Extensions.ComponentMin(ref b.Min, ref v, out b.Min);
						Vector3Extensions.ComponentMax(ref b.Max, ref v, out b.Max);
					}

					if (BBox3.Overlapping(ref qbounds, ref b))
					{
						if (polys.Count < polys.Capacity)
							polys.Add(GetReference(polyBase, i));
					}
				}

				return polys.Count;
			}
		}

		/// <summary>
		/// Allocate a new link if possible.
		/// </summary>
		/// <param name="tile">Current tile</param>
		/// <returns>New link number</returns>
		public int AllocLink(MeshTile tile)
		{
			if (!IsLinkAllocated(tile.LinksFreeList))
				return Link.Null;

			int link = tile.LinksFreeList;
			tile.LinksFreeList = tile.Links[link].Next;
			return link;
		}

		/// <summary>
		/// Get the tile reference
		/// </summary>
		/// <param name="tile">Tile to look for</param>
		/// <returns>Tile reference</returns>
		public int GetTileRef(MeshTile tile)
		{
			if (tile == null)
				return 0;

			int it = 0;
			for (int i = 0; i < tiles.Length; i++)
			{
				if (tiles[i] == tile)
				{
					it = i;
					break;
				}
			}

			return EncodePolyId(tile.Salt, it, 0);
		}

		/// <summary>
		/// Find the tile at a specific location
		/// </summary>
		/// <param name="x">The x-coordinate</param>
		/// <param name="y">The y-coordinate</param>
		/// <param name="layer">The layer number</param>
		/// <returns>The MeshTile at that location</returns>
		public MeshTile GetTileAt(int x, int y, int layer)
		{
			//Find tile based off hash
			int h = ComputeTileHash(x, y, tileLookupTableMask);
			MeshTile tile = posLookup[h];
			
			while (tile != null)
			{
				//Found
				if (tile.Header != null && tile.Header.X == x && tile.Header.Y == y && tile.Header.Layer == layer)
					return tile;

				//Keep searching
				tile = tile.Next;
			}
			
			return null;
		}

		/// <summary>
		/// Find and add a tile if it is found
		/// </summary>
		/// <param name="x">The x-coordinate</param>
		/// <param name="y">The y-coordinate</param>
		/// <param name="tiles">Tile array</param>
		/// <returns>Number of tiles satisfying condition</returns>
		public int GetTilesAt(int x, int y, MeshTile[] tiles)
		{
			int n = 0;

			//Find tile based on hash
			int h = ComputeTileHash(x, y, tileLookupTableMask);
			MeshTile tile = posLookup[h];
			
			while (tile != null)
			{
				//Tile found. 
				//Add to tile array
				if (tile.Header != null && tile.Header.X == x && tile.Header.Y == y)
				{
					if (n < tiles.Length)
						tiles[n++] = tile;
				}

				//Keep searching
				tile = tile.Next;
			}

			return n;
		}

		/// <summary>
		/// Gets the neighboring tile at that position
		/// </summary>
		/// <param name="x">The x-coordinate</param>
		/// <param name="y">The y-coordinate</param>
		/// <param name="side">The side value</param>
		/// <param name="tiles">An array of MeshTiles</param>
		/// <returns>The number of tiles satisfying the condition</returns>
		public int GetNeighbourTilesAt(int x, int y, BoundarySide side, MeshTile[] tiles)
		{
			int nx = x, ny = y;
			switch (side)
			{
				case BoundarySide.PlusX:
					nx++;
					break;

				case BoundarySide.PlusXPlusZ:
					nx++;
					ny++;
					break;

				case BoundarySide.PlusZ:
					ny++;
					break;

				case BoundarySide.MinusXPlusZ:
					nx--;
					ny++;
					break;

				case BoundarySide.MinusX:
					nx--;
					break;

				case BoundarySide.MinusXMinusZ:
					nx--;
					ny--;
					break;

				case BoundarySide.MinusZ:
					ny--;
					break;

				case BoundarySide.PlusXMinusZ:
					nx++;
					ny--;
					break;
			}

			return GetTilesAt(nx, ny, tiles);
		}

		/// <summary>
		/// Computes the tile hash code, which can be used in a hash table for quick lookup.
		/// </summary>
		/// <param name="x">The x-coordinate</param>
		/// <param name="y">The y-coordinate</param>
		/// <param name="mask">The mask</param>
		/// <returns>Tile hash code</returns>
		public int ComputeTileHash(int x, int y, int mask)
		{
			//choose large multiplicative constants which are primes
			uint h1 = 0x8da6b343;
			uint h2 = 0xd8163841;
			uint n = (uint)(h1 * x + h2 * y);
			return (int)(n & mask);
		}

		/// <summary>
		/// Get the actual polygon reference
		/// </summary>
		/// <param name="polyBase">The base value</param>
		/// <param name="poly">The offset</param>
		/// <returns>The polygon reference</returns>
		public int GetReference(int polyBase, int poly)
		{
			return polyBase | poly;
		}

		/// <summary>
		/// Determines whether a link exists for that index
		/// </summary>
		/// <param name="index">The index</param>
		/// <returns>True if allocated, false if not</returns>
		public bool IsLinkAllocated(int index)
		{
			return index != Link.Null;
		}

		/// <summary>
		/// Determines whether the two polygons are externally linked or not
		/// </summary>
		/// <param name="neighbor">The neighboring polygon</param>
		/// <returns>True if externally linked, false if not</returns>
		public bool IsExternalLink(int neighbor)
		{
			return (neighbor & Link.External) != 0;
		}
		
		/// <summary>
		/// Get the base reference for the polygons in a tile.
		/// </summary>
		/// <param name="tile">Current Tile</param>
		/// <returns>Base poly reference</returns>
		public int GetPolyRefBase(MeshTile tile)
		{
			if (tile == null)
				return 0;

			int it = 0;
			for (int i = 0; i < tiles.Length; i++)
			{
				if (tiles[i] == tile)
				{
					it = i;
					break;
				}
			}

			return EncodePolyId(tile.Salt, it, 0);
		}

		/// <summary>
		/// Retrieve the tile and poly based off of a polygon reference
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <param name="tile">Resulting tile</param>
		/// <param name="poly">Resulting poly</param>
		/// <returns>True if tile and poly successfully retrieved</returns>
		public bool TryGetTileAndPolyByRef(int reference, out MeshTile tile, out Poly poly)
		{
			tile = null;
			poly = null;

			if (reference == 0)
				return false;

			//Get tile and poly indices
			int salt = 0, indexTile = 0, indexPoly = 0;
			DecodePolyId(reference, ref salt, ref indexTile, ref indexPoly);
			
			//Make sure indices are valid
			if (indexTile >= maxTiles)
				return false;

			if (tiles[indexTile].Salt != salt || tiles[indexTile].Header == null)
				return false;

			if (indexPoly >= tiles[indexTile].Header.PolyCount)
				return false;

			//Retrieve tile and poly
			tile = tiles[indexTile];
			poly = tiles[indexTile].Polys[indexPoly];
			return true;
		}

		/// <summary>
		/// Only use this function if it is known that the provided polygon reference is valid.
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <param name="tile">Resulting tile</param>
		/// <param name="poly">Resulting poly</param>
		public void TryGetTileAndPolyByRefUnsafe(int reference, out MeshTile tile, out Poly poly)
		{
			int salt = 0, indexTile = 0, indexPoly = 0;
			DecodePolyId(reference, ref salt, ref indexTile, ref indexPoly);
			tile = tiles[indexTile];
			poly = tiles[indexTile].Polys[indexPoly];
		}

		/// <summary>
		/// Check if polygon reference is valid.
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <returns>True if valid</returns>
		public bool IsValidPolyRef(int reference)
		{
			if (reference == 0)
				return false;

			int salt = 0, indexTile = 0, indexPoly = 0;
			DecodePolyId(reference, ref salt, ref indexTile, ref indexPoly);

			if (indexTile >= maxTiles)
				return false;

			if (tiles[indexTile].Salt != salt || tiles[indexTile].Header == null)
				return false;

			if (indexPoly >= tiles[indexTile].Header.PolyCount)
				return false;

			return true;
		}

		/// <summary>
		/// Decode a standard polygon reference 
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <param name="salt">Resulting salt value</param>
		/// <param name="indexTile">Resulting tile index</param>
		/// <param name="indexPoly">Resulting poly index</param>
		public void DecodePolyId(int reference, ref int salt, ref int indexTile, ref int indexPoly)
		{
			int saltMask = (1 << saltBits) - 1;
			int tileMask = (1 << tileBits) - 1;
			int polyMask = (1 << polyBits) - 1;
			salt = (reference >> (polyBits + tileBits)) & saltMask;
			indexTile = (reference >> polyBits) & tileMask;
			indexPoly = reference & polyMask;
		}

		/// <summary>
		/// Extract a tile's salt value from the specified polygon reference 
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <returns>Salt value</returns>
		public int DecodePolyIdSalt(int reference)
		{
			int saltMask = (1 << saltBits) - 1;
			return (reference >> (polyBits + tileBits)) & saltMask;
		}

		/// <summary>
		/// Extract a tile's index from the specified polygon reference
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <returns>Tile index</returns>
		public int DecodePolyIdTile(int reference)
		{
			int tileMask = (1 << tileBits) - 1;
			return (reference >> polyBits) & tileMask;
		}

		/// <summary>
		/// Extract a polygon's index (within its tile) from the specified polygon reference 
		/// </summary>
		/// <param name="reference">Polygon reference</param>
		/// <returns>Poly index</returns>
		public int DecodePolyIdPoly(int reference)
		{
			int polyMask = (1 << polyBits) - 1;
			return reference & polyMask;
		}

		/// <summary>
		/// Derive a standard polygon reference, which compresses salt, tile index, and poly index together
		/// </summary>
		/// <param name="salt">Salt value</param>
		/// <param name="indexTile">Tile index</param>
		/// <param name="indexPoly">Poly index</param>
		/// <returns>Polygon reference</returns>
		public int EncodePolyId(int salt, int indexTile, int indexPoly)
		{
			return (salt << (int)(polyBits + tileBits)) | (indexTile << (int)polyBits) | indexPoly;
		}

		/// <summary>
		/// Calculates the tile location.
		/// </summary>
		/// <param name="pos">The position</param>
		/// <param name="tx">The tile's x-coordinate</param>
		/// <param name="ty">The tile's y-coordinate</param>
		public void CalcTileLoc(ref Vector3 pos, out int tx, out int ty)
		{
			tx = (int)Math.Floor((pos.X - origin.X) / tileWidth);
			ty = (int)Math.Floor((pos.Z - origin.Z) / tileHeight);
		}

		/*/// <summary>
		/// Serializes the navigation mesh into a JSON format and writes the 
		/// serialized data to a file. 
		/// </summary>
		/// <param name="filename">Path to file to be written</param>
		/// <returns>True if JSON data read, false otherwise</returns>
		public bool SaveJson(string filename)
		{
			string data = this.JSONObject.ToString();
			File.WriteAllText(filename, data);
			return true;
		}

		/// <summary>
		/// Reads the JSON data from a file, deserializes it and updates the current
		/// TiledNavMesh instance to reflect the deserialized data. 
		/// </summary>
		/// <param name="filename">Path to file to be read</param>
		/// <returns>True if file exists and was read successfully, false otherwise</returns>
		public static TiledNavMesh LoadJson(string filename)
		{
			if (!File.Exists(filename))    
				return null; 

			string data = File.ReadAllText(filename); 
			return (TiledNavMesh) JsonConvert.DeserializeObject<TiledNavMesh>(data);
		}*/
	}
}
