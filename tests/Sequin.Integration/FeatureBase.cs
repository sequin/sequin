namespace Sequin.Integration
{
    using Fakes;
    using Xbehave;

    public abstract class FeatureBase
    {
        [Background]
        public void Background()
        {
            TrackedCommandHandler.Reset();
        }
    }
}