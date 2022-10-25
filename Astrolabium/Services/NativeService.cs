using System;
using System.Collections.Generic;
using Astrolabium.Models;
using FerkopaUtils;
using System.IO;

namespace Astrolabium.Services
{
    public class NativeService:INativeService
    {
        const string nativeDataDir = @".\HoroData";
        public void WriteNative(Native native)
        {
            XmlSerializerUtil.Serialize(native, CreateNativeFileName(native.ID));
        }
        public Native ReadNative(Guid nativeGuid)
        {
            return ReadNative(CreateNativeFileName(nativeGuid));
        }

        private Native ReadNative(String fileName)
        {
            return XmlSerializerUtil.Deserialize<Native>(fileName);
        }
        private string[] GetAndCreateNativeDirectoryName()
        {
            if (! Directory.Exists(nativeDataDir))
            {
                Directory.CreateDirectory(nativeDataDir);
            }
            return Directory.GetFiles(nativeDataDir);
        }
        public List<(Native, string)> GetNativeTupleList(string nativeDirectory = null)
        {
            var list = new List<(Native, string)>();
            foreach (var file in GetAndCreateNativeDirectoryName())
            {         
                var native = ReadNative(file);
                list.Add((native,native.Name));
            }
            return list;
        }

        private string CreateNativeFileName(Guid nativeId)
        {
            return Path.Combine(nativeDataDir, nativeId + ".nxml");
        }
    }
}
