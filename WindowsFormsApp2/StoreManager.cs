using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class StoreManager
    {
        public struct Location
        {
            public StoreLocation Name;
            public StoreName[] StoreNames;
        }
        private X509Store store;
        public List<Location> Locations;
        
        public StoreManager()
        {
            FillLocations();
        }
        private void FillLocations()
        {

            StoreLocation[] storeLocations = (StoreLocation[])Enum.GetValues(typeof(StoreLocation));
            Locations = new List<Location>();
            foreach (StoreLocation storeLocation in storeLocations)
            {
                Location loc;
                loc.Name = storeLocation;
                loc.StoreNames = (StoreName[]) Enum.GetValues(typeof(StoreName));
                Locations.Add(loc);
            }
        }

        public void getCertificates(StoreLocation storeLocation, StoreName storeName)
        {
            store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly);

                X509Certificate2Collection certificates = store.Certificates;
                Console.WriteLine("Yes    {0,4}  {1}, {2}", certificates.Count, store.Name, store.Location);
                X509Certificate2Collection fcollection = (X509Certificate2Collection)certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.MultiSelection);
                Console.WriteLine("Number of certificates: {0}{1}", scollection.Count, Environment.NewLine);

                foreach (X509Certificate2 x509 in scollection)
                {
                    try
                    {
                        byte[] rawdata = x509.RawData;
                        Console.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
                        Console.WriteLine("Friendly Name: {0}{1}", x509.FriendlyName, Environment.NewLine);
                        Console.WriteLine("Certificate Verified?: {0}{1}", x509.Verify(), Environment.NewLine);
                        Console.WriteLine("Simple Name: {0}{1}", x509.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
                        Console.WriteLine("Signature Algorithm: {0}{1}", x509.SignatureAlgorithm.FriendlyName, Environment.NewLine);
                        //Console.WriteLine("Private Key: {0}{1}", x509.PrivateKey.ToXmlString(false), Environment.NewLine);
                        Console.WriteLine("Public Key: {0}{1}", x509.PublicKey.Key.ToXmlString(false), Environment.NewLine);
                        Console.WriteLine("Certificate Archived?: {0}{1}", x509.Archived, Environment.NewLine);
                        Console.WriteLine("Length of Raw Data: {0}{1}", x509.RawData.Length, Environment.NewLine);
                        X509Certificate2UI.DisplayCertificate(x509);
                        x509.Reset();
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("Information could not be written out for this certificate.");
                    }
                }
                //Close the store.
                store.Close();
            }
            catch (CryptographicException)
            {
                Console.WriteLine("No {0}, {1}", store.Name, store.Location);
            }
        }


    }
}
