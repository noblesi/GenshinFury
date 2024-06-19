// Copyright (c) 2014 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System.Collections.Generic;

using SharpNav.Geometry;

namespace SharpNav
{
	//TODO right now this is basically an alias for TiledNavMesh. Fix this in the future.

	/// <summary>
	/// A TiledNavMesh generated from a collection of triangles and some settings
	/// </summary>
	public class NavMesh : TiledNavMesh
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NavMesh" /> class.
		/// </summary>
		/// <param name="builder">The NavMeshBuilder data</param>
		public NavMesh(NavMeshBuilder builder)
			: base(builder)
		{
		}

		/// <summary>
		/// Generates a <see cref="NavMesh"/> given a collection of triangles and some settings.
		/// </summary>
		/// <param name="triangles">The triangles that form the level.</param>
		/// <param name="settings">The settings to generate with.</param>
		/// <returns>A <see cref="NavMesh"/>.</returns>
		public static NavMesh Generate(IEnumerable<Triangle3> triangles, NavMeshGenerationSettings settings, out PolyMesh polyMesh, out PolyMeshDetail polyMeshDetail)
		{
			BBox3 bounds = triangles.GetBoundingBox(settings.CellSize);
			var hf = new Heightfield(bounds, settings);
			hf.RasterizeTriangles(triangles);
			hf.FilterLedgeSpans(settings.VoxelAgentHeight, settings.VoxelMaxClimb);
			hf.FilterLowHangingWalkableObstacles(settings.VoxelMaxClimb);
			hf.FilterWalkableLowHeightSpans(settings.VoxelAgentHeight);
			var chf = new CompactHeightfield(hf, settings);
			chf.Erode(settings.VoxelAgentRadius);
			chf.BuildDistanceField();
			chf.BuildRegions(2, settings.MinRegionSize, settings.MergedRegionSize);
			var cont = chf.BuildContourSet(settings);

			polyMesh = new PolyMesh(cont, settings);

			polyMeshDetail = new PolyMeshDetail(polyMesh, chf, settings);
			var buildData = new NavMeshBuilder(polyMesh, polyMeshDetail, new Pathfinding.OffMeshConnection[0], settings);

			var navMesh = new NavMesh(buildData);
			return navMesh;
		}
	}
}
