﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CalamityMod.DataStructures
{
    public class VerletSimulatedSegment
    {
        public Vector2 position, oldPosition;
        public bool locked;

        public VerletSimulatedSegment(Vector2 _position, bool _locked = false)
        {
            position = _position;
            oldPosition = _position;
            locked = _locked;
        }

        public static List<VerletSimulatedSegment> SimpleSimulation(List<VerletSimulatedSegment> segments, float segmentDistance, int loops = 10, float gravity = 0.3f)
        {
            //https://youtu.be/PGk0rnyTa1U?t=400 verlet integration chains reference here
            foreach (VerletSimulatedSegment segment in segments)
            {
                if (!segment.locked)
                {
                    Vector2 positionBeforeUpdate = segment.position;

                    segment.position += (segment.position - segment.oldPosition); // This adds conservation of energy to the segments. This makes it super bouncy and shouldnt be used but it's really funny
                    segment.position += Vector2.UnitY * gravity; //=> This adds gravity to the segments. 

                    segment.oldPosition = positionBeforeUpdate;
                }
            }

            int segmentCount = segments.Count;

            for (int k = 0; k < loops; k++)
            {
                for (int j = 0; j < segmentCount - 1; j++)
                {
                    VerletSimulatedSegment pointA = segments[j];
                    VerletSimulatedSegment pointB = segments[j + 1];
                    Vector2 segmentCenter = (pointA.position + pointB.position) / 2f;
                    Vector2 segmentDirection = Terraria.Utils.SafeNormalize(pointA.position - pointB.position, Vector2.UnitY);

                    if (!pointA.locked)
                        pointA.position = segmentCenter + segmentDirection * segmentDistance / 2f;

                    if (!pointB.locked)
                        pointB.position = segmentCenter - segmentDirection * segmentDistance / 2f;

                    segments[j] = pointA;
                    segments[j + 1] = pointB;
                }
            }

            return segments;
        }
    }
}
