namespace Tests.Mocks
{
    public static class MockRegistryExtensions
    {
        public static MockRegistry SetLicenceKey(this MockRegistry mockRegistry, string licenceKey)
        {
            return mockRegistry.SetValue("SOFTWARE\\Audition\\Audition", "LicenceKey", licenceKey);
        }
    }
}