namespace VideoEditor.Core
{
    public class Lane
    {
        public string Name { get; set; }
        public List<FragmentPlacement> Fragments { get; set; } = new List<FragmentPlacement>();
        public Lane(string name)
        {
            Name = name;
        }
        public void AddFragment(FragmentPlacement NewPlacement)
        {
            Fragments.Add(NewPlacement);
            if (Fragments.Count > 1)
            {
                NewPlacement.Position = Fragments[Fragments.Count - 2].EndPosition;
            }
        }
        public void AddFragment(FragmentPlacement NewPlacement, TimeSpan position)
        {
            if (this[position,position+ NewPlacement.Fragment.Duration] != null)
            {
                throw new InvalidOperationException("Cannot add fragment: Time position overlaps with an existing fragment.");
            }
            Fragments.Add(NewPlacement);
            Fragments = Fragments.OrderBy(f => f.Position).ToList();
        }
        public void RemoveFragment(FragmentPlacement placement)
        {
            if (Fragments.Contains(placement))
            {
                Fragments.Remove(placement);
            }
        }
        public void MoveFragment(FragmentPlacement placement, TimeSpan newPosition)
        {
            if (!Fragments.Contains(placement))
            {
                throw new InvalidOperationException("Cannot move fragment: Fragment not found in the lane.");
            }
            if (this[newPosition, newPosition + placement.Fragment.Duration] != null && this[newPosition, newPosition + placement.Fragment.Duration] != new List<FragmentPlacement>{placement})
            {
                throw new InvalidOperationException("Cannot move fragment: New time position overlaps with an existing fragment.");
            }
            placement.Position = newPosition;
            Fragments = Fragments.OrderBy(f => f.Position).ToList();
        }
        public FragmentPlacement this[TimeSpan time]
        {
            get
            {
                foreach (var placement in Fragments)
                {
                    if (time >= placement.Position && time < placement.EndPosition)
                    {
                        return placement;
                    }
                }
                return null; // No fragment found at the specified time
            }
        }
        public List<FragmentPlacement> this[TimeSpan timeStart, TimeSpan timeEnd]
        {
            get
            {
                List<FragmentPlacement> overlappingFragments = new List<FragmentPlacement>();
                foreach (var placement in Fragments)
                {
                    if (timeStart < placement.EndPosition && timeEnd > placement.Position)
                    {
                        overlappingFragments.Add(placement);
                    }
                }
                if(overlappingFragments.Count>0) return overlappingFragments;
                else return null; // No fragment found in the specified time range
            }
        }
    }
}