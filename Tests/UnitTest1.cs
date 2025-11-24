using VideoEditor.Core;
namespace Tests
{
    [TestClass]
    public class LaneTests
    {
        [Fact]
        public void AddFragment_ShouldPlaceFragmentAtCorrectIndex()
        {
            var lane = new Lane("Name");
            var fragment = new Fragment("a.mp4", TimeSpan.Zero, TimeSpan.FromSeconds(5));
            var placement = new FragmentPlacement(fragment);

            lane.AddFragment(placement);

            Assert.Single(lane.Fragments);
            Assert.Equal(placement, lane.Fragments[0]);
        }
    }
}