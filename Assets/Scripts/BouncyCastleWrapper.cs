//------------------------------------------------------------------------------
// <copyright file="BouncyCastleWrapper.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// BouncyCastle Camellia Wapper
// </summary>
//------------------------------------------------------------------------------
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
namespace Com.FurtherSystems.vQL.Client
{
    class BouncyCastleWrapper
    {
        PaddedBufferedBlockCipher cipher;
        public BouncyCastleWrapper(IBlockCipher cipherType, IBlockCipherPadding padding = null)
        {
            if (padding != null)
            {
                cipher = new PaddedBufferedBlockCipher(cipherType, padding);
            }
            else
            {
                cipher = new PaddedBufferedBlockCipher(new CamelliaEngine());
            }
        }

        public byte[] Encrypt(byte[] plain, string key)
        {
            return Crypto(true, plain, UTF8Encoding.UTF8.GetBytes(key));
        }

        public byte[] Decrypt(byte[] cipher, string key)
        {
            return Crypto(false, cipher, UTF8Encoding.UTF8.GetBytes(key));
        }

        byte[] Crypto(bool useEncrypt, byte[] data, byte[] key)
        {
            cipher.Init(useEncrypt, new ParametersWithIV(new KeyParameter(key), key));
            return cipher.DoFinal(data);
        }
    }
}