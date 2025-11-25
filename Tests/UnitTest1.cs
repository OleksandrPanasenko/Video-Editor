using Core.Operations;
using VideoEditor.Core;
using VideoEditor.Core.Operations;
using VideoEditor.Core.Transitions;
using VideoEditor.Infrastructure;
namespace Tests
{
    [TestClass]
    public class MainOperationsTests
    {
        [TestMethod]
        public void AddFragment_ShouldPlaceFragmentAtCorrectIndex()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment);

            lane.AddFragment(placement);

            Assert.AreEqual(1, lane.Fragments.Count);
            Assert.AreEqual(placement, lane.Fragments[0]);
            Assert.AreEqual(lane.Fragments[0].Position, TimeSpan.Zero);
        }
        [TestMethod]
        public void AddFragment_ShouldPlaceFragmentAtSpecifiedPosition()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment);

            lane.AddFragment(placement, TimeSpan.FromSeconds(10));

            Assert.AreEqual(1, lane.Fragments.Count);
            Assert.AreEqual(placement, lane.Fragments[0]);
            Assert.AreEqual(lane.Fragments[0].Position, TimeSpan.FromSeconds(10));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddFragment_ShouldThrow_WhenOverlapOccurs()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment);
            lane.AddFragment(placement);

            fragment = new Fragment("b.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            placement = new FragmentPlacement(fragment);
            lane.AddFragment(placement);

            var newPlacement = new FragmentPlacement(new Fragment("c.mp4", TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5)));

            lane.AddFragment(newPlacement, TimeSpan.FromSeconds(2.5));
        }

        [TestMethod]
        public void TrimFragment_ShouldUpdateStartAndDuration()
        {
            var fragment = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };


            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);
            // Trim 2 seconds from start and 3 seconds from end
            new TrimOperation(project, placement, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(7)).Apply();

            Assert.AreEqual(TimeSpan.FromSeconds(2), fragment.StartTime);
            Assert.AreEqual(TimeSpan.FromSeconds(7), fragment.EndTime);
            Assert.AreEqual(TimeSpan.FromSeconds(5), fragment.Duration);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TrimFragment_ZeroDuration_Should_Causd_Error()
        {
            var fragment = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };


            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);
            // Trim 2 seconds from start and 3 seconds from end
            new TrimOperation(project, placement, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)).Apply();
        }





        [TestMethod]
        public void MovePlacement_ShouldMoveCorrectly()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };
            lane.AddFragment(placement);

            lane.MoveFragment(placement, TimeSpan.FromSeconds(10));

            Assert.AreEqual(TimeSpan.FromSeconds(10), placement.Position);
        }
        [TestMethod]
        public void MovePlacementOperation_ShouldMoveAndUndoCorrectly()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };
            lane.AddFragment(placement);

            var moveOperation = new MoveFragmentOperation(lane, lane, placement.Position, TimeSpan.FromSeconds(10));
            moveOperation.Apply();

            Assert.AreEqual(TimeSpan.FromSeconds(10), placement.Position);

            moveOperation.Undo();

            Assert.AreEqual(TimeSpan.FromSeconds(0), placement.Position);
        }


        [TestMethod]
        public void SplitFragment_ShouldCreateTwoFragments()
        {
            var fragment = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);

            var splitOperation = new SplitOperation(project, project.Lanes[0], placement, TimeSpan.FromSeconds(4));
            splitOperation.Apply();

            Assert.AreEqual(2, project.Lanes[0].Fragments.Count);
            Assert.AreEqual(TimeSpan.Zero, project.Lanes[0].Fragments[0].Position);
            Assert.AreEqual(TimeSpan.FromSeconds(4), project.Lanes[0].Fragments[0].Fragment.Duration);
            Assert.AreEqual(TimeSpan.FromSeconds(4), project.Lanes[0].Fragments[1].Position);
            Assert.AreEqual(TimeSpan.FromSeconds(6), project.Lanes[0].Fragments[1].Fragment.Duration);

            splitOperation.Undo();

            Assert.AreEqual(1, project.Lanes[0].Fragments.Count);
            Assert.AreEqual(TimeSpan.Zero, project.Lanes[0].Fragments[0].Position);
            Assert.AreEqual(TimeSpan.FromSeconds(10), project.Lanes[0].Fragments[0].Fragment.Duration);
        }
        [TestMethod]
        public void DeleteFragment_ShouldRemoveFragment()
        {
            var fragment = new Fragment("a.mp3", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);

            var deleteOperation = new DeleteFragmentOperation(placement, project.Lanes[0]);
            deleteOperation.Apply();

            Assert.AreEqual(0, project.Lanes[0].Fragments.Count);

            deleteOperation.Undo();

            Assert.AreEqual(1, project.Lanes[0].Fragments.Count);
            Assert.AreEqual(placement, project.Lanes[0].Fragments[0]);
        }
        [TestMethod]
        public void Copy_PasteFragment_ShouldDuplicateFragment()
        {
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);

            var copyOperation = new CopyOperation(project, placement);
            copyOperation.Apply();
            var pasteOperation = new PasteOperation(project.SelectionManager.MemoryFragment, project.Lanes[0], TimeSpan.FromSeconds(15));
            pasteOperation.Apply();

            Assert.AreEqual(2, project.Lanes[0].Fragments.Count);
            Assert.AreNotEqual(placement, project.Lanes[0].Fragments[1]);

            Assert.AreEqual(TimeSpan.FromSeconds(15), project.Lanes[0].Fragments[1].Position);
            Assert.AreEqual(fragment.FilePath, project.Lanes[0].Fragments[1].Fragment.FilePath);
            Assert.AreEqual(fragment.Duration, project.Lanes[0].Fragments[1].Fragment.Duration);
        }
        [TestMethod]
        public void Cut_PasteFragment_ShouldMoveFragment()
        {
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placement);

            var cutOperation = new CutOperation(project, project.Lanes[0], placement);
            cutOperation.Apply();
            var pasteOperation = new PasteOperation(project.SelectionManager.MemoryFragment, project.Lanes[0], TimeSpan.FromSeconds(20));
            pasteOperation.Apply();

            Assert.AreEqual(1, project.Lanes[0].Fragments.Count);
            Assert.AreEqual(TimeSpan.FromSeconds(20), project.Lanes[0].Fragments[0].Position);
            Assert.AreEqual(fragment.FilePath, project.Lanes[0].Fragments[0].Fragment.FilePath);
            Assert.AreEqual(fragment.Duration, project.Lanes[0].Fragments[0].Fragment.Duration);
        }
    }
    [TestClass]
    public class TransitionTests
    {
        [TestMethod]
        public void AddTransition_ShouldAdjustFragmentDurations()
        {
            var fragmentA = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placementA = new FragmentPlacement(fragmentA)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var fragmentB = new Fragment("b.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placementB = new FragmentPlacement(fragmentB)
            {
                Position = TimeSpan.FromSeconds(10)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placementA);
            project.Lanes[0].AddFragment(placementB);

            var transition = new CrossFadeTransition
            {
                Duration = TimeSpan.FromSeconds(2)
            };
            var operation = new AddTransitionOperation(project, project.Lanes[0], placementA, placementB, transition);
            operation.Apply();

            // After adding the transition, the end time of fragment A should be reduced by the transition duration
            // and the start time of fragment B should be increased by the transition duration
            Assert.AreEqual(TimeSpan.FromSeconds(8), fragmentA.EndTime);
            Assert.AreEqual(TimeSpan.FromSeconds(2), fragmentB.StartTime);
            Assert.AreEqual(TimeSpan.FromSeconds(8), fragmentB.Duration);
            Assert.IsTrue(project.Lanes[0].Transitions.Contains(transition));
            operation.Undo();
            // After undoing the operation, the original durations should be restored
            Assert.AreEqual(TimeSpan.FromSeconds(10), fragmentA.EndTime);
            Assert.AreEqual(TimeSpan.Zero, fragmentB.StartTime);
            Assert.AreEqual(TimeSpan.FromSeconds(10), fragmentB.Duration);
            Assert.IsFalse(project.Lanes[0].Transitions.Contains(transition));
        }

        [TestMethod]
        public void RemoveTransition_ShouldRestoreFragmentDurations()
        {
            var fragmentA = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placementA = new FragmentPlacement(fragmentA)
            {
                Position = TimeSpan.FromSeconds(0)
            };

            var fragmentB = new Fragment("b.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placementB = new FragmentPlacement(fragmentB)
            {
                Position = TimeSpan.FromSeconds(10)
            };

            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(placementA);
            project.Lanes[0].AddFragment(placementB);

            var transition = new CrossFadeTransition
            {
                Duration = TimeSpan.FromSeconds(2),
                From = placementA,
                To = placementB
            };
            new AddTransitionOperation(project, project.Lanes[0], placementA, placementB, transition).Apply();

            var operation = new RemoveTransitionOperation(project.Lanes[0], transition);
            operation.Apply();

            // After removing the transition, the original durations should be restored
            Assert.AreEqual(TimeSpan.FromSeconds(10), fragmentA.EndTime);
            Assert.AreEqual(TimeSpan.Zero, fragmentB.StartTime);
            Assert.AreEqual(TimeSpan.FromSeconds(10), fragmentB.Duration);
            Assert.IsFalse(project.Lanes[0].Transitions.Contains(transition));
        }
        [TestMethod]
        public void Operations_ShouldThrow_WhenHasTransition()
        {
            var fragment = new Fragment("a.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var aPlacement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };


            var project = new Project();
            project.AddLane();
            project.Lanes[0].AddFragment(aPlacement);
            var bPlacement = new FragmentPlacement(new Fragment("b.jpg", TimeSpan.Zero, TimeSpan.FromSeconds(10)))
            {
                Position = TimeSpan.FromSeconds(10)
            };
            project.Lanes[0].AddFragment(bPlacement);
            var transition = new CrossFadeTransition
            {
                Duration = TimeSpan.FromSeconds(2),
                From = aPlacement,
                To = bPlacement
            };
            project.Lanes[0].Transitions.Add(transition);
            try
            {
                new TrimOperation(project, aPlacement, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(7)).Apply();
                Assert.Fail("Expected InvalidOperationException not thrown for TrimOperation");
            }
            catch (InvalidOperationException)
            {
                //Expected
                try
                {
                    new MoveFragmentOperation(project.Lanes[0], project.Lanes[0], aPlacement.Position, TimeSpan.FromSeconds(15)).Apply();
                    Assert.Fail("Expected InvalidOperationException not thrown for MoveFragmentOperation");
                }
                catch (InvalidOperationException)
                {
                    //Expected

                    try
                    {
                        new DeleteFragmentOperation(aPlacement, project.Lanes[0]).Apply();
                        Assert.Fail("Expected InvalidOperationException not thrown for DeleteFragmentOperation");
                    }
                    catch (InvalidOperationException)
                    {
                        // Expected
                    }
                }
            }
        }
    }
    [TestClass]
    public class LaneTests()
    {
        [TestMethod]  
        public void DoubleLane_ShouldDoubleLane()
        {
            var project = new Project();
            project.AddLane();

            var lane = project.Lanes.Last();
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };
            lane.AddFragment(placement);

            new DoubleLaneOperation(project,lane).Apply();
            var doubledLane = project.Lanes.Last();

            Assert.AreNotEqual(lane, doubledLane);
            Assert.AreEqual(1, doubledLane.Fragments.Count);
            Assert.AreEqual(placement.Fragment.FilePath, doubledLane.Fragments[0].Fragment.FilePath);
            Assert.AreEqual(placement.Position, doubledLane.Fragments[0].Position);
            Assert.AreEqual(placement.Fragment.Duration, doubledLane.Fragments[0].Fragment.Duration);
        }
    }

    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void SaveAndLoadProject_ShouldPreserveData()
        {
            var project = new Project();
            project.AddLane();
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(10));
            var placement = new FragmentPlacement(fragment)
            {
                Position = TimeSpan.FromSeconds(0)
            };
            project.Lanes[0].AddFragment(placement);

            var filePath = "test_project";
            if (!Path.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            ProjectStorage.Save(project, filePath);

            var loadedProject = ProjectStorage.Load(Path.Combine(project.Path,project.Name+".vep"));

            Assert.AreEqual(2, loadedProject.Lanes.Count);//Project started with one empty lane
            Assert.AreEqual(1, loadedProject.Lanes[0].Fragments.Count);
            Assert.AreEqual("a.mp4", loadedProject.Lanes[0].Fragments[0].Fragment.FilePath);
            Assert.AreEqual(TimeSpan.Zero, loadedProject.Lanes[0].Fragments[0].Position);
            Assert.AreEqual(TimeSpan.FromSeconds(10), loadedProject.Lanes[0].Fragments[0].Fragment.Duration);

            // Clean up
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        
    }
}