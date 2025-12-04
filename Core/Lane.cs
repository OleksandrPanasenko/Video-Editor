using VideoEditor.Core.Transitions;

namespace VideoEditor.Core
{
    public class Lane
    {
        //Lane contains fragments and transitions between them
        public string Name { get; set; }
        // List of fragments in the lane
        public List<FragmentPlacement> Fragments { get; set; } = new List<FragmentPlacement>();
        // List of transitions in the lane
        public List<ITransition> Transitions { get; set; } = new List<ITransition>();
        public TimeSpan? LaneStart { get { return Fragments.Count == 0 ? null : Fragments[0].Position; } }
        public TimeSpan? LaneEnd { get { return Fragments.Count == 0 ? null : Fragments[Fragments.Count-1].EndPosition; } }
        public Lane(string name)
        {
            Name = name;
        }
        //Add fragment at the end
        public void AddFragment(FragmentPlacement NewPlacement)
        {
            Fragments.Add(NewPlacement);
            if (Fragments.Count > 1)
            {
                NewPlacement.Position = Fragments[Fragments.Count - 2].EndPosition;
            }
        }
        //Add fragment at specific position, adjusting position if overlapping
        public void AddFragment(FragmentPlacement NewPlacement, TimeSpan position)
        {
            if (this[position,position+ NewPlacement.Fragment.Duration] != null)
            {
                if(NewPlacement.Fragment==null) throw new ArgumentNullException(nameof(NewPlacement));
                List <FragmentPlacement> existing = this[position,position+NewPlacement.Fragment.Duration];
                var First = existing[0];
                var Last=existing.Last()!=NewPlacement?existing.Last():existing[existing.Count()-2];
                if (position+NewPlacement.Fragment.Duration/2 >= (First.Position+Last.EndPosition) / 2)
                {
                    //Try to append after
                    if (this[Last.EndPosition,Last.EndPosition+NewPlacement.Fragment.Duration] == null)
                    {
                        position = Last.EndPosition;
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot add fragment: Time position overlaps with an existing fragment.");
                    }
                }
                else
                {
                    //Try to append before
                    if (this[First.Position - NewPlacement.Fragment.Duration, First.Position] == null)
                    {
                        if (First.Position - NewPlacement.Fragment.Duration < TimeSpan.Zero)
                        {
                            throw new InvalidOperationException("Cannot add fragment: Time position is before the start of the timeline.");
                        }
                        position = First.Position - NewPlacement.Fragment.Duration;
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot add fragment: Time position overlaps with an existing fragment.");
                    }
                }
                
            }
            NewPlacement.Position = position;
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
        // Indexer to get fragment at specific time
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
        // Indexer to get fragments overlapping a time range
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
        // Create a deep copy of the lane
        public Lane DeepCopy()
        {
            Lane NewLane = new Lane(Name + "_copy");
            foreach (var placement in Fragments)
            {
                var newPlacement = placement.DeepCopy();
                NewLane.AddFragment(newPlacement,newPlacement.Position);
            }
            return NewLane;
        }
        public bool IsMovable(TimeSpan position)
        {
            var fragment = this[position];
            if(Transitions.Any(t=>t.From==fragment || t.To==fragment))
            {
                return false;
            }
            return true;
        }
        // Get transition at specific time
        public ITransition GetTransitionFromTime(TimeSpan time)
        {
            foreach(var transition in Transitions)
            {
                if(transition.From.EndPosition <= time && transition.To.Position >= time)
                {
                    return transition;
                }
            }
            return null;
        }
        public bool HasEndTransition(FragmentPlacement fragment)
        {
            return Transitions.Any(Transitions => Transitions.From == fragment);
        }
        public bool HasStartTransition(FragmentPlacement fragment)
        {
            return Transitions.Any(Transitions => Transitions.To == fragment);
        }
        public bool HasTransition(FragmentPlacement fragment)
        {
            return HasStartTransition(fragment) || HasEndTransition(fragment);
        }
    }
}