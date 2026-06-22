using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace GDPSMaker.Services
{
    public static class cs
    {
        public static void df(string kp, string pw, string al)
        {
            var kg = new RsaKeyPairGenerator();
            kg.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            var kr = kg.GenerateKeyPair();

            var dn = new X509Name("CN=Android,O=AppSigner,C=EU");
            var cg = new X509V3CertificateGenerator();
            var sr = BigInteger.ProbablePrime(120, new Random());

            cg.SetSerialNumber(sr);
            cg.SetIssuerDN(dn);
            cg.SetSubjectDN(dn);
            cg.SetNotBefore(DateTime.UtcNow);
            cg.SetNotAfter(DateTime.UtcNow.AddYears(25));
#pragma warning disable CS0618
            cg.SetSignatureAlgorithm("SHA256WITHRSA");
            cg.SetPublicKey(kr.Public);
            var ct = cg.Generate(kr.Private);
#pragma warning restore CS0618

            var st = new Pkcs12Store();
            st.SetKeyEntry(
                al,
                new AsymmetricKeyEntry(kr.Private),
                new X509CertificateEntry[] { new X509CertificateEntry(ct) });

            using var fs = new FileStream(kp, FileMode.Create, FileAccess.Write);
            st.Save(fs, pw.ToCharArray(), new SecureRandom());
        }
    }
}
